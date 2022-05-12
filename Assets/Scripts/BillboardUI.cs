using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private void Update()
    {
        var rotVector =transform.position- LevelManager.instance.mainCam.transform.position ;
        var rot=Quaternion.LookRotation(rotVector.normalized);
        transform.rotation = Quaternion.Euler(0f,rot.y+45f,rot.z);
    }
}