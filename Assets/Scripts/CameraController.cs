using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private Transform playerTransform;
    [SerializeField] private float scrollSpeed = 100f;
    //[SerializeField] private Vector3 cameraOffset;

    private Camera cam;
    

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        //transform.position = playerTransform.position + cameraOffset;
        
        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 10f, 20f);
    }
}