using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayerMask;
    
    private int spikesUpAnimationID;

    private void Start()
    {
        spikesUpAnimationID = Animator.StringToHash("SpikesUp");
    }


    private void Update()
    {
        anim.SetBool(spikesUpAnimationID,false);
        if(!Physics.CheckBox(transform.position + transform.up * 1.2f,new Vector3(0.5f,1.8f,0.5f),transform.rotation,playerLayerMask)) return;
        
        anim.SetBool(spikesUpAnimationID,true);
    }
}
