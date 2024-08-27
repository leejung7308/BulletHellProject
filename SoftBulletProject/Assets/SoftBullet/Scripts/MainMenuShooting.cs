using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuShooting : MonoBehaviour
{
    public GameObject pool;

    private void Start()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject bullet = pool.GetComponent<ObjectPooling>().Enable();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 20;
            bullet.transform.up = new Vector2(1, 0);
        }
    }
}
