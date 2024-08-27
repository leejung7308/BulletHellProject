using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void Update()
    {
        transform.position = transform.parent.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
            transform.parent.transform.position = 
                Vector2.MoveTowards(transform.parent.gameObject.transform.position, GameObject.FindWithTag("Player").transform.position, 0.5f);
        }
    }
}
