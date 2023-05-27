using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;
    [SerializeField] private PlayerInput _playerActions;

    private Vector2 _direction;
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

        //Debug.Log(_direction);
        //_movement.Move(new Vector2(-_direction.y,_direction.x));
        _direction = _playerActions.Player_map.Move.ReadValue<Vector2>();
        if ( _direction.x != 0 )
            _movement.Move(_direction);
    }
    public void OnMove()
    {
       

    }
}
