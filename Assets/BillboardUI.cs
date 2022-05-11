using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private void Update()
    {
        var rotVector =transform.position- LevelManager.instance.camPivot.position ;
        var rot=Quaternion.LookRotation(rotVector.normalized);
        transform.rotation = Quaternion.Euler(0,rot.y+45,rot.z);
    }
}