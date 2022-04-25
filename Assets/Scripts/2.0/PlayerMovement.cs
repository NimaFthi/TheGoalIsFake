using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    private Rigidbody rb;
    private Vector3 input;
    [SerializeField] private Joystick joystick;

    [Header("Stats")] 
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        input = new Vector3(horizontal, 0, vertical);
    }
    
    private void Move()
    {
        var relative = input.ToIso();
        
        rb.velocity = new Vector3(relative.x, 0, relative.z) * (moveSpeed * Time.fixedDeltaTime);
    }
}
