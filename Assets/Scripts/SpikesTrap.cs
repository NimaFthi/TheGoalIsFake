using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] bool isFake;

    private int fakeAnimationID;
    private int normalAnimationID;

    private void Start()
    {
        fakeAnimationID = Animator.StringToHash("FakeAnimation");
        normalAnimationID = Animator.StringToHash("NormalAnimation");
    }


    private void Update()
    {
        anim.SetBool(normalAnimationID,false);
        if(!Physics.CheckBox(transform.position + transform.up * 1.2f,new Vector3(0.5f,1.8f,0.5f),transform.rotation,playerLayerMask)) return;
        
        if (isFake)
        {
            anim.SetTrigger(fakeAnimationID);
        }
        else
        {
            anim.SetBool(normalAnimationID,true);
        }
    }
}
