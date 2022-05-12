using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private enum JoyStickDirection
    {
        Horizontal,
        Vertical,
        Both
    }

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    [SerializeField] private JoyStickDirection direction = JoyStickDirection.Both;
    [Range(0, 2f)] [SerializeField] private float handleLimit = 1f;

    private Vector2 input = Vector2.zero;
    private Vector2 joyPos = Vector2.zero;

    //output
    public float horizontal => input.x;
    public float vertical => input.y;

    public void OnDrag(PointerEventData eventData)
    {
        var joystickDirection = eventData.position - joyPos;
        input = joystickDirection.magnitude > (background.sizeDelta.x / 2f)
            ? joystickDirection.normalized
            : joystickDirection / (background.sizeDelta.x / 2f);

        if (direction == JoyStickDirection.Horizontal)
        {
            input = new Vector2(input.x, 0f);
        }

        if (direction == JoyStickDirection.Vertical)
        {
            input = new Vector2(0f, input.y);
        }

        handle.anchoredPosition = input * (background.sizeDelta.x / 2f) * handleLimit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        joyPos = eventData.position;
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }
}