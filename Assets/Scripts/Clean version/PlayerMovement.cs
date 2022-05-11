using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    [SerializeField] private Joystick joystick;
    private Rigidbody rb;

    //move stats
    [SerializeField] private float moveSpeed = 600f;

    private Vector3 input;


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
        var horizontal = joystick.Horizontal;
        var vertical = joystick.Vertical;
        input = new Vector3(horizontal, 0, vertical).ToIso().normalized;
    }

    private void Move()
    {
        if (PlayerManager.instance.isDead || !PlayerManager.instance.canMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = input * moveSpeed * Time.deltaTime;
    }
}