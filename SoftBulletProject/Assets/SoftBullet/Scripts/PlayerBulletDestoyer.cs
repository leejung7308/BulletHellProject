using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDestoyer : MonoBehaviour
{
    Animator animator;
    GameObject player;
    public float lifeTime = 5f;
    private void OnEnable()
    {
        StartCoroutine(Timedestroy());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator Timedestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
    public void StartHitAnimation()
    {
        float[] arr = new float[6] { 3f, 2f, 2f, 1.5f, 2f, 1f };
        int playerPower = player.GetComponent<PlayerController>().power;
        Vector2 hitPosition = gameObject.GetComponent<Rigidbody2D>().velocity.normalized * (new Vector2(arr[playerPower / 10], arr[playerPower / 10]));
        gameObject.transform.position += new Vector3(hitPosition.x, hitPosition.y, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("hit", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            StartHitAnimation();
        }
    }
    public void DestroyPlayerBullet()
    {
        Destroy(this.gameObject);
    }
}
