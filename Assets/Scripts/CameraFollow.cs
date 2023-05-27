using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = .3f;
    public Vector3 offSet;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offSet;
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
