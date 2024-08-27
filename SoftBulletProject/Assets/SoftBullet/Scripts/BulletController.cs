using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    public bool changeSpeed;
    public float newSpeed;
    public float speedChangeTime;
    public bool changeDirection;
    public Vector2 newDirection;
    public float directionChangeTime;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;

        if (changeSpeed)
        {
            Invoke("UpdateSpeed", speedChangeTime);
        }

        if (changeDirection)
        {
            Invoke("UpdateDirection", directionChangeTime);
        }
    }

    private void UpdateSpeed()
    {
        if (changeSpeed)
        {
            rb.velocity = rb.velocity.normalized * newSpeed;
        }
    }

    private void UpdateDirection()
    {
        if (changeDirection)
        {
            rb.velocity = newDirection.normalized * rb.velocity.magnitude;
        }
    }
}
