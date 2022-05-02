using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private bool suddenMove;
    [SerializeField] private Transform[] nodes;


    // [SerializeField] private float moveDuration = 2f;
    // [SerializeField] private AnimationCurve moveCurve;
    // private bool _isMovingForward = true;


    // private void OnEnable()
    // {
    //     Move();
    // }

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
    
    // DOTWEEN

    // private async void Move()
    // {
    //     while (gameObject.activeInHierarchy)
    //     {
    //         var startPosition = (_isMovingForward ? nodes[0] : nodes[1]).position;
    //         var endPosition = (_isMovingForward ? nodes[1] : nodes[0]).position;
    //         var passedTime = 0f;
    //         transform.position = startPosition;
    //         while (passedTime < moveDuration)
    //         {
    //             startPosition = (_isMovingForward ? nodes[0] : nodes[1]).position;
    //             endPosition = (_isMovingForward ? nodes[1] : nodes[0]).position;
    //             var t = moveCurve.Evaluate(passedTime / moveDuration);
    //             transform.position = Vector3.Lerp(startPosition, endPosition, t);
    //             passedTime += Time.deltaTime;
    //             if (passedTime > moveDuration)
    //             {
    //                 _isMovingForward = !_isMovingForward;
    //                 break;
    //             }
    //
    //             await Task.Yield();
    //         }
    //     }
    // }
}