using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f,0f,360f);

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}