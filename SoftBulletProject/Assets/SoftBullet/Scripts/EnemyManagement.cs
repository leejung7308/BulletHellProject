using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MoveElements
{
    public bool usingHp = false;
    public float startHp;
    public float nextMoveDelay = 0f;
    public bool isRotate;
    public float moveSpeed;
    public Vector2 nextPosition;
    public int nextIndex;
    public int repeatCount;
}
public class EnemyManagement : MonoBehaviour
{
    Animator animator;
    private GameObject player;

    public bool hasRotate;
    public GameObject rotateObject;
    //public bool isRotate;
    public Vector2 rotateCenter;
    public float rotateRadius;
    public bool powerDrop;
    public PowerObject powerObject;
    public float health = 10f;
    public float score = 300f;
    //public bool isRepeat;
    //public bool isBoss = false;
    //public int repeatCount = 0;
    
    public List<MoveElements> moveElements;
    public GameObject flame;
    public GameObject patternManager;

    private AudioSource audioSource;

    public AudioClip hitClip;
    public AudioClip destroyClip;

    //private bool patternTrigger = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //StartCoroutine(EnemyMovement());
        StartCoroutine(EnemyMove());
    }
    private void FixedUpdate()
    {
        gameObject.transform.up = GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position;
        if (health <= 0)
        {
            
            StartExplosion();
        }
        if (hasRotate)
        {
            rotateObject.transform.position = transform.position;
            rotateObject.transform.Rotate(new Vector3(0, 0, 2));
        }
    }
    /*
    IEnumerator EnemyMovement()
    {
        if (isRepeat)
        {
            yield return new WaitForSeconds(moveElements[0].nextMoveDelay);
            gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[0].nextPosition.normalized * moveElements[0].moveSpeed;
            yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[0].nextPosition) / moveElements[0].moveSpeed);
            gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
            if (repeatCount == 0)
            {
                while (true)
                {
                    for (int i = 1; i < moveElements.Count; i++)
                    {
                        yield return new WaitForSeconds(moveElements[i].nextMoveDelay);
                        gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[i].nextPosition.normalized * moveElements[i].moveSpeed;
                        yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[i].nextPosition) / moveElements[i].moveSpeed);
                        gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
                    }
                }
            }
            else
            {
                for(int i = 0; i < repeatCount; i++)
                {
                    for (int j = 1; j < moveElements.Count-1; j++)
                    {
                        yield return new WaitForSeconds(moveElements[j].nextMoveDelay);
                        gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[j].nextPosition.normalized * moveElements[j].moveSpeed;
                        yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[j].nextPosition) / moveElements[j].moveSpeed);
                        gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
                    }
                }
                yield return new WaitForSeconds(moveElements[moveElements.Count-1].nextMoveDelay);
                gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[moveElements.Count - 1].nextPosition.normalized * moveElements[moveElements.Count - 1].moveSpeed;
                yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[moveElements.Count - 1].nextPosition) / moveElements[moveElements.Count - 1].moveSpeed);
                gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
            }
        }
        else
        {
            for (int i = 0; i < moveElements.Count; i++)
            {
                yield return new WaitForSeconds(moveElements[i].nextMoveDelay);
                gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[i].nextPosition.normalized * moveElements[i].moveSpeed;
                yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[i].nextPosition) / moveElements[i].moveSpeed);
                gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
            }
            if (isRotate)
            {
                StartCoroutine(CirCularMove());
            }
        }
    }
    */
    IEnumerator EnemyMove()
    {
        int cur = 0;
        while(moveElements[cur].nextIndex != -1)
        {
            if (moveElements[cur].usingHp)
            {
                yield return new WaitUntil(() => (health <= moveElements[cur].startHp));
                patternManager.GetComponent<PatternManager>().SkipPattern();
            }
            else
            {
                yield return new WaitForSeconds(moveElements[cur].nextMoveDelay);
            }

            if(moveElements[cur].isRotate)
            {
                while(health > moveElements[cur].startHp)
                {
                    yield return StartCoroutine(CirCularMove());
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = moveElements[cur].nextPosition.normalized * moveElements[cur].moveSpeed;
                yield return new WaitForSeconds(Vector2.Distance(Vector2.zero, moveElements[cur].nextPosition) / moveElements[cur].moveSpeed);
                gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
            }

            if (moveElements[cur].repeatCount == -1) cur = moveElements[cur].nextIndex;
            else if(moveElements[cur].repeatCount > 0)
            {
                moveElements[cur].repeatCount--;
                cur = moveElements[cur].nextIndex;
            }
            else
            {
                cur++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            health -= player.GetComponent<PlayerController>().power;
            audioSource.PlayOneShot(hitClip);
            StartCoroutine(IsHit());
        }
        if(collision.gameObject.tag == "EnemyDelete")
        {
            gameObject.SetActive(false);
        }
    }
    public void StartExplosion()
    {
        flame.SetActive(false);
        patternManager.SetActive(false);
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("expl", true);
    }
    public void EnemyDestroy()
    {
        gameObject.SetActive(false);
        player.GetComponent<PlayerController>().score += score;
        player.GetComponent<PlayerController>().scoreCount += score;
    }
    public void PowerDrop()
    {
        if (powerDrop)
        {
            GameObject powerUP = Instantiate(powerObject.gameObject, transform.position, Quaternion.identity);
        }
    }
    private IEnumerator IsHit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    private IEnumerator CirCularMove()
    {        
        for (int th = 0; th < 1440; th++)
        {
            var rad = Mathf.Deg2Rad * th/4;
            var x = rotateRadius * Mathf.Sin(rad);
            var y = rotateRadius * Mathf.Cos(rad);
            this.transform.position = new Vector2(x, y) + rotateCenter;
            yield return new WaitForSeconds(0.004f);
        }
    }
    public void PlayDestroySound()
    {
        audioSource.PlayOneShot(destroyClip);
    }

    /*
    public void SkipPattern()
    {
        patternTrigger = true;
    }
    */
}
