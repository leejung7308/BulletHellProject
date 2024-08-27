using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes;

[System.Serializable]
public class PowerPattern
{
    public float powerInterval;
    public GameObject bulletPattern;
}
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public float scoreCount=0f;
    [HideInInspector]
    public float score = 0f;
    public GameObject lifeUpPrefab;
    public GameObject powerUpPrefab;
    public bool invincible;
    public float invincibleTime = 1f;
    public UI ui;
    public float originalSpeed = 5f;
    public float slowModeSpeed = 5f;
    private float moveSpeed = 5f;
    public List<PowerPattern> powerPatterns;
    public int power = 1;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    public int life = 2;
    public GameObject bulletSpawnPoint;
    public GameObject collideArea;
    public GameObject bulletDistroyer;
    private float nextFire;
    private Camera mainCamera;
    private Transform playerTransform;

    public bool isPooling = true;
    public bool isOPooling = false;

    private AudioSource audioSource;

    public AudioClip shotClip;
    public AudioClip upClip;
    public AudioClip hitClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        moveSpeed = originalSpeed;
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        nextFire = Time.time;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = slowModeSpeed;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            collideArea.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = originalSpeed;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            collideArea.gameObject.SetActive(false);
        }
        
    }
    void FixedUpdate()
    {
        
        Move();
        Shoot();
        Lookat();
        if (scoreCount > 3000)
        {
            DropLifeUP();
        }
        if (life <= 0)
        {
            ui.GameOver();
        }
        
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        playerTransform.position += (Vector3)moveDirection * moveSpeed * Time.fixedDeltaTime;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Vector2 shootingDirection = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            shootingDirection.Normalize();

            //if (audioSource.clip != shotClip) audioSource.clip = shotClip;
            audioSource.PlayOneShot(shotClip);
            GameObject currentPattern = GetCurrentPattern();
            GameObject bullet = Instantiate(currentPattern, bulletSpawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * bulletSpeed;

            float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg - 90f;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    void Lookat()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position;
        transform.up = dirVec.normalized;
    }

    GameObject GetCurrentPattern()
    {
        GameObject currentPattern = powerPatterns[0].bulletPattern;

        foreach (PowerPattern pattern in powerPatterns)
        {
            if (power >= pattern.powerInterval)
            {
                currentPattern = pattern.bulletPattern;
            }
        }

        return currentPattern;
    }
    void DropLifeUP()
    {
        float x = Random.Range(transform.position.x - 30, transform.position.x + 30);
        float y = Random.Range(transform.position.y - 30, transform.position.y + 30);
        if (x < -70) x = -70;
        if (x > 170) x = 170;
        if (y < -30) y = -30;
        if (y > 40) y = 40;
        Vector2 pos = new Vector2(x, y);
        scoreCount -= 3000;
        GameObject lifeUP = Instantiate(lifeUpPrefab, pos, Quaternion.identity);
    }
    public void DropPowerUP()
    {
        for (int i = 0; i < power / 2; i++)
        {
            float x = Random.Range(transform.position.x - 30, transform.position.x + 30);
            float y = Random.Range(transform.position.y - 30, transform.position.y + 30);
            if (x < -70) x = -70;
            if (x > 170) x = 170;
            if (y < -30) y = -30;
            if (y > 40) y = 40;
            Vector2 pos = new Vector2(x, y);
            GameObject powerUP = Instantiate(powerUpPrefab, pos, Quaternion.identity);
        }
        power /= 2;
        if (power == 0) power = 1;
    }
    public Sprite GetCurrentSprite()
    {
        Sprite currentSprite = powerPatterns[power/10].bulletPattern.GetComponent<SpriteRenderer>().sprite;
        return currentSprite;
    }

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
        {
            if (!invincible)
            {
                //if (audioSource.clip != hitClip) audioSource.clip = hitClip;
                audioSource.PlayOneShot(hitClip);
                life--;
                DropPowerUP();
                StartCoroutine(EnableInvincible(invincibleTime));
                StartCoroutine(EnableBulletDistoryer(0.1f));
            }
        }
        if (collision.CompareTag("LifeUp") && life<30)
        {
            //if (audioSource.clip != upClip) audioSource.clip = upClip;
            audioSource.PlayOneShot(upClip);
            life += collision.gameObject.GetComponent<LifeObject>().lifeIncrease;
        }
        if (collision.CompareTag("PowerUp") && power<50)
        {
            if (audioSource.clip != upClip) audioSource.clip = upClip;
            audioSource.Play();
            power += collision.gameObject.GetComponent<PowerObject>().powerIncrease;
            if(power>50) power = 50;
        }
    }
    private IEnumerator EnableInvincible(float sec)
    {
        invincible = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
        yield return new WaitForSeconds(sec);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        invincible = false;
    }

    private IEnumerator EnableBulletDistoryer(float r)
    {
        bulletDistroyer.SetActive(true);
        float curTime = 0;
        while(curTime < 1f)
        {
            curTime += Time.deltaTime;
            bulletDistroyer.GetComponent<CircleCollider2D>().radius += r;
            bulletDistroyer.GetComponent<Disc>().Radius += r;

            yield return null;
        }

        bulletDistroyer.GetComponent<CircleCollider2D>().radius = 0.01f;
        bulletDistroyer.GetComponent<Disc>().Radius = 0.01f;
        bulletDistroyer.SetActive(false);
    }
}

// Todo
/*
 * 파워에 따른 탄막 패턴 설정
 * 플레이어 탄막 오브젝트 풀링
 * 적 탄막 피격 구현
 * 피격 후 리스폰 구현
 * 에너미 그룹 이동 구현
 * 스테이지 진행에 따른 에너미 스폰
 * 카메라 무브
 * 플레이어 탄막 피격 판정
 * 사운드 매니저, 애니메이션
 * 플레이어 좌표 클렘프
 * 더 다양한 탄막 패턴
 */