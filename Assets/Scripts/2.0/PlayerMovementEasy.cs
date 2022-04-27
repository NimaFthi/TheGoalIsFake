using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementEasy : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 input;

    [SerializeField] private float moveSpeed;

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
        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;

        input = new Vector3(ver, 0f, hor);
    }

    private void Move()
    {
        input.ToIso();
        rb.velocity = input * moveSpeed * Time.fixedDeltaTime;
    }
}