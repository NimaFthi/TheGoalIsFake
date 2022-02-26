using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private bool suddenMove;
    [SerializeField] private Transform[] nodes;

    private int index;

    private void Update()
    {
        Vector3 moveDirection = nodes[index].position - transform.position;
        if (suddenMove)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime,Space.World);
        }
        else
        {
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime,Space.World);
        }
        
        if (Vector3.Distance(transform.position,nodes[index].position) > 1f) return;

        index++;
        transform.eulerAngles += new Vector3(0f, 180f, 0);

        if (index != nodes.Length) return;
        
        index = 0;
    }
}