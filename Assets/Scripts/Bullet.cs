using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask layerToAffect;

    private Vector2 _startingPosition;
    
    private Vector2 _direction;
    public void GetDirection(Vector2 direction)
    {
        _direction = direction;
        _startingPosition = direction;
    }
    private void Update()
    {
        transform.position += (Vector3)_direction * speed;
        if((transform.position - (Vector3)_startingPosition).magnitude > 50)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int tempLayerValue = (int)Mathf.Log((int)layerToAffect,2);
        if (collision.gameObject.layer == tempLayerValue && tempLayerValue == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Hits");
            Enemy enemy = NewPhysicMovement.GetEnemyData(collision.gameObject);
            var script = GameObject.Find("Player").GetComponent<NewPhysicMovement>();
            var damage = script.damage;
            enemy.hp -= damage;
            if (enemy.hp <= 0)
            {
                Destroy(collision.gameObject);
                var gameController = GameObject.Find("Game controller").GetComponent<GameControls>();
                gameController.score += 100;
            }
        }
            GameObject.Destroy(transform.gameObject);
    }
}
