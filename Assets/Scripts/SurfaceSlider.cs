using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    private Vector2 _normal;

    public Vector2 Project(Vector2 movement)
    {
        return movement - Vector2.Dot(movement, _normal) * _normal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _normal = collision.contacts[0].normal;
    }
}
