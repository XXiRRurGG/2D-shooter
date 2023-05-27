using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerActions;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _shootingSpeed;

    private float _timeBetweenBullets;
    public void OnShoot(InputAction.CallbackContext context)
    {
        //Debug.Log(_playerActions.Player_map.Shoot.ReadValue<Vector2>());
        //var bullet = Instantiate(_bullet,transform.position,Quaternion.identity);
        //bullet.transform.position += (Vector3)playerActions.Player_map.Shoot.ReadValue<Vector2>() * 3;
        //var script = bullet.GetComponent<Bullet>();
        //script.GetDirection(playerActions.Player_map.Shoot.ReadValue<Vector2>());
    }
    private void Awake()
    {
        _playerActions = new PlayerInput();
        _playerActions.Player_map.Enable();
    }
    private void OnEnable()
    {
        _playerActions.Player_map.Enable();
    }
    private void OnDisable()
    {
        _playerActions.Player_map.Disable();
    }
    private void Update()
    {
        var direction = _playerActions.Player_map.Shoot.ReadValue<Vector2>();
        if(direction != Vector2.zero && _timeBetweenBullets == 0)
        {
            var bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
            bullet.transform.position += (Vector3)_playerActions.Player_map.Shoot.ReadValue<Vector2>();
            var script = bullet.GetComponent<Bullet>();
            script.GetDirection(_playerActions.Player_map.Shoot.ReadValue<Vector2>());
            var coroutine = StartCoroutine(Countdown());
        }
    }
    private IEnumerator Countdown()
    {
        while (true)
        {
            _timeBetweenBullets += Time.deltaTime;
            if (_timeBetweenBullets > _shootingSpeed)
            {
                _timeBetweenBullets = 0;
                yield break;
            }
            yield return null;
        }
    }
}
