using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeObject : MonoBehaviour
{
    public int lifeIncrease = 1;
    public float lifeTime = 10.0f;

    private void OnEnable()
    {
        StartCoroutine(Timedestroy());
        RandomMove();
    }
    private void FixedUpdate()
    {
        if (gameObject.transform.position.x <= -70 || gameObject.transform.position.x >= 170)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity *= new Vector2(-1, 1);
        }
        if (gameObject.transform.position.y <= -40 || gameObject.transform.position.y >= 40)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity *= new Vector2(1, -1);
        }
    }
    IEnumerator Timedestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    private void RandomMove()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-90, 90), Random.Range(-90, 90)).normalized * 5f;
    }
}
