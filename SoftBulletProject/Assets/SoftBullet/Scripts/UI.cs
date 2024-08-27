using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shapes;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [HideInInspector]
    public List<Image> lifeImageList;
    private GameObject player;
    private int playerPower;
    private float playerScore;
    private int playerLife;

    public Image powerImage;
    public Image lifeImage;
    public Text powerText;
    public Text scoreText;
    public Text gameClearScoreText;
    public Texture2D cursorImage;
    public GameObject pauseSet;
    public GameObject gameOverSet;
    public GameObject clearSet;
    public GameObject powerGauge;
    public GameObject boss;
    public static bool gamePaused = false; 

    private void Start()
    {
        pauseSet.SetActive(false);
        gameOverSet.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        Cursor.SetCursor(cursorImage, new Vector2(cursorImage.width/2,cursorImage.height/2), CursorMode.ForceSoftware);
    }
    void Update()
    {
        Sprite powerSprite = player.GetComponent<PlayerController>().GetCurrentSprite();
        playerPower = player.GetComponent<PlayerController>().power;
        playerScore = player.GetComponent<PlayerController>().score;
        playerLife = player.GetComponent<PlayerController>().life;
        if (playerPower / 10 == 0)
        {
            powerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(powerSprite.rect.width * 0.7f, powerSprite.rect.height * 0.7f);
        }
        else if(playerPower / 10 == 5) 
        {
            powerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(powerSprite.rect.width * 1.5f, powerSprite.rect.height * 1.5f);
        }
        else
        {
            powerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(powerSprite.rect.width, powerSprite.rect.height);
        }
        powerImage.sprite = powerSprite;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        UpdateLifeIcon();
        UpdatePowerTXT();
        UpdatePowerGauge();
        UpdateScoreTXT();
        if (boss.GetComponent<EnemyManagement>().health <= 0)
            Clear();
    }
    public void UpdateLifeIcon()
    {
        if (lifeImageList.Count < playerLife)
        {
            int tmp = playerLife - lifeImageList.Count;
            for (int i = 0; i < tmp; i++)
            {
                Image instImage = Instantiate(lifeImage);
                instImage.transform.SetParent(gameObject.transform, false);
                lifeImageList.Add(instImage);
            }
        }
        for (int i = 0; i < lifeImageList.Count; i++)
        {
            lifeImageList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playerLife; i++)
        {
            lifeImageList[i].gameObject.SetActive(true);
            lifeImageList[i].rectTransform.position = (Vector2)lifeImage.transform.position + new Vector2(50 * i, 0);
        }
    }
    public void GameOver()
    {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
        gameOverSet.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void UpdatePowerGauge()
    {
        powerGauge.GetComponent<Rectangle>().Width = 20.8f * playerPower / 50f;
        //powerGauge.GetComponent<Rectangle>().Color = new Color(playerPower / 50f, 0, (50f - playerPower) / 50f, 0.7f);
    }
    void UpdatePowerTXT()
    {
        if (playerPower == 50)
        {
            powerText.text = "Max";
        }
        else
        {
            powerText.text = playerPower.ToString();    
        }
    }
    public void UpdateScoreTXT()
    {
        scoreText.text = playerScore.ToString();
    }
    public void Resume()
    {
        pauseSet.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }
    void Pause()
    {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
        pauseSet.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    void Clear()
    {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
        player.GetComponent<PlayerController>().invincible = true;
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        player.GetComponent<PlayerController>().collideArea.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;
        clearSet.SetActive(true);
        player.transform.up = new Vector3(1, 0, 0);
        gameClearScoreText.text = player.GetComponent<PlayerController>().score.ToString();
        Vector2 target = player.transform.position;
        target += new Vector2(400, 0);
        player.transform.position = Vector2.MoveTowards(player.transform.position, target, 70f*Time.deltaTime);
    }
}
