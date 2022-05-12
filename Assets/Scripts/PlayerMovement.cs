using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //components
    [SerializeField] private FloatingJoyStick joystick;
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
        var horizontal = joystick.horizontal;
        var vertical = joystick.vertical;
        
        input = new Vector3(horizontal, 0, vertical).ToIso().normalized;

        // var forward = Quaternion.AngleAxis(-45f, transform.up) * transform.right;
        // var right = Quaternion.AngleAxis(-90f, transform.up) * forward;
        // input = forward * horizontal + right * vertical;
        // input.y = 0;
        // input.Normalize();
    }

    private void Move()
    {
        if (PlayerManager.instance.isDead || !PlayerManager.instance.canMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = input * moveSpeed * Time.fixedDeltaTime;
    }
}