using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        var newPos = followTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position,newPos,0.5f);
    }
}
