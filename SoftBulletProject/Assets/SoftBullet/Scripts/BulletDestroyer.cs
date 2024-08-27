using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class BulletDestroyer : MonoBehaviour
{
    public string poolName;
    public GameObject OP;

    public float lifeTime = 5f;
    public int index = 0;

    private GameObject player;
    private void OnEnable()
    {
        //OP = GameObject.FindGameObjectWithTag("Pool");
        OP = GameObject.Find(poolName);
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Timedestroy());
    }
    
    IEnumerator Timedestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        if (player.GetComponent<PlayerController>().isPooling) OP.GetComponent<ObjectPooling>().Disable(index);
        else Destroy(this.gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BulletDelete")
        {
            if (player.GetComponent<PlayerController>().isPooling) OP.GetComponent<ObjectPooling>().Disable(index);
            else Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Player")
        {
            if (player.GetComponent<PlayerController>().isPooling) OP.GetComponent<ObjectPooling>().Disable(index);
            else Destroy(this.gameObject);
        }
    }
}
