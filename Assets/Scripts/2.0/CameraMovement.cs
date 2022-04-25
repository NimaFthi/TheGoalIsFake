using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothness;

    private void FixedUpdate()
    {
        var newPos = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, newPos, smoothness);
    }
}
