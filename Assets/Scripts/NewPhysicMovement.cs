using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class NewPhysicMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerActions;
    [SerializeField] private TrailRenderer _trailRenderer;

    public CharacterController2D controller;

    public float knockbackForce = 6f;
    public int health = 5;
    public float speed = 1;
    public float dashingSpeed = 4f;
    public float dashingDuration = 0.3f;
    public float dashingCooldown = 0.5f;
    public float iFramesLength = 1f;
    public event Action OnDeath;
    public float damage = 1.2f;

    protected bool canDash = true;
    protected bool isDashing = false;
    protected bool jump = false;
    protected bool isCrouching = false;

    private float horizontalMove = 0f;
    private Rigidbody2D rb2d;
    private bool extendIFrames = false;
    public bool isBeingHit = false;
    public bool isPickingUp = false;

    private void Awake()
    {
        _playerActions = new PlayerInput();
        _playerActions.Player_map.Enable();

        _playerActions.Player_map.Dash.performed += ctx => OnDash();
        _playerActions.Player_map.Jump.performed += ctx => jump = true;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        var playerInput = _playerActions.Player_map.Move.ReadValue<Vector2>();
        horizontalMove = playerInput.x;
        UnityEngine.Debug.Log(playerInput);
    }
    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }
       controller.Move(horizontalMove*Time.fixedDeltaTime * speed,isCrouching,jump);
       jump = false;
    }
    private IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = direction.normalized * dashingSpeed;
        _trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingDuration);
        isDashing = false;
        _trailRenderer.emitting = false;
        rb2d.velocity = new Vector2(0, 0);
        rb2d.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void OnDash()
    {
        if (horizontalMove != 0 && canDash)
        {
            Vector2 moveAlongGround = new Vector2(horizontalMove,0f);
            StartCoroutine(Dash(moveAlongGround));
            StartCoroutine(GetIFrames(dashingDuration));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = GetEnemyData(collision.gameObject);
            if (enemy.collisionDamage)
            {
                Vector2 direction = transform.position - collision.transform.position;
                direction.Normalize();

                GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce / 2, ForceMode2D.Impulse);

                //Add getting  hit
                if (!isBeingHit)
                {
                    StartCoroutine(GetHit());
                    isBeingHit = true;
                }
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy bullets"))
        {
            if (!isBeingHit)
            {
                StartCoroutine(GetHit());
                isBeingHit = true;
            }
        }
    }
    public static Enemy GetEnemyData(GameObject gameObject)
    {
        var components = gameObject.GetComponents<MonoBehaviour>();

        foreach (var component in components)
        {
            var componentType = component.GetType();

            var fields = componentType.GetFields();

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Enemy))
                {
                    var enemyData = (Enemy)field.GetValue(component);
                    return enemyData;
                }
            }
        }

        return null; 
    }
    public IEnumerator GetHit()
    {
        health--;
        if(health <= 0)
        {
            StopAllCoroutines();
            isBeingHit=false;
            Destroy(gameObject);
            OnDeath.Invoke();
        }
        StartCoroutine(GetIFrames(iFramesLength));
        Material material = GetComponent<SpriteRenderer>().material;
        Color color = material.color;
        for (int i = 0; i < iFramesLength * 5; i++)
        {
            color.a = 0.2f;
            material.color = color;
            yield return new WaitForSeconds(0.2f);
            color.a = 1f;
            material.color = color;
            yield return new WaitForSeconds(0.2f);
        }

        isBeingHit = false;
    }
    private IEnumerator GetIFrames(float length)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10, true);

        yield return new WaitForSeconds(length);

        BoxCollider2D triggerCollider = GetComponents<BoxCollider2D>()[1];

        if (!triggerCollider.isTrigger)
        {

            Physics2D.IgnoreLayerCollision(gameObject.layer, 9, false);
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, false);
        }
        else
        {
            yield return new WaitForSeconds(length);
            Physics2D.IgnoreLayerCollision(gameObject.layer, 9, false);
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, false);
        }
    }
}