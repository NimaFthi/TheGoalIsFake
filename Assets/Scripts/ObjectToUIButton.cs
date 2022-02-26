using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectToUIButton : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent = new UnityEvent();
    [SerializeField] private Camera mainCam;
    private GameObject button;
    private void Start()
    {
        button = gameObject;
    }
    
    private void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == button)
            {
                unityEvent.Invoke();
            }
        }
    }
}
