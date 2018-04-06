using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [HideInInspector]
    public float playerHealth;
    public float maxHealth;

    [HideInInspector]
    public int score;
    private int highscore;

    private bool inGame;

    private float timer = 100.0f;
    private bool created = false;
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake: " + this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        inGame = false;
    }


    // Update is called once per frame
    void Update()
    {
        if ((SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) && Input.GetKeyDown("space"))
            StartGame();
        else if (inGame)
        {
            /*
            if (Input.GetKeyDown("space"))
                Time.timeScale = 0f; // Pause Game
            else
                Time.timeScale = 1f;
            */
            playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
            if (Input.GetKeyDown("k")) NextLevel();
        }
        else if (!inGame && Input.GetKeyDown("space"))
        {
            LoadMenu();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void FixedUpdate() {

        timer -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(timer);
        if (inGame)
        {
            Text t = gameObject.transform.Find("Interface").transform.Find("ScoreText").GetComponentInChildren<Text>();
            t.text = "SCORE: " + score;
            if (score > highscore)
                highscore = score;


            t = gameObject.transform.Find("Interface").transform.Find("HighScoreText").GetComponentInChildren<Text>();
            t.text = "HIGH SCORE: " + highscore;

            t = gameObject.transform.Find("Interface").transform.Find("TimerText").GetComponentInChildren<Text>();
            t.text = "" + seconds;
            UpdateHealthBar();

            if (playerHealth <= 0)
                GameOver();
            if (timer <= 0)
                NextLevel();
        }
    }

    void UpdateHealthBar()
    {
        //Debug.Log(playerHealth);
        float ratio = playerHealth / maxHealth;
        //Debug.Log(ratio);
        Image hb = gameObject.transform.Find("Interface").transform.Find("HealthBar").transform.Find("CurrentHealthBar").GetComponent<Image>();
        //Debug.Log(hb.rectTransform.localScale);
        hb.rectTransform.localScale = new Vector3 (ratio,1,1);
    }

    void StartGame()
    {
        inGame = true;
        gameObject.transform.Find("Prompt").gameObject.SetActive(false);
        gameObject.transform.Find("Interface").gameObject.SetActive(true);
        Reset();
        SceneManager.LoadScene(1);
    }
    void NextLevel() {
        timer = 100.0f;
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
            GameOver();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void GameOver()
    {
        inGame = false;
        gameObject.transform.Find("Prompt").gameObject.SetActive(true);
        gameObject.transform.Find("Interface").gameObject.SetActive(false);
        Text t = gameObject.transform.Find("Prompt").transform.Find("Title").GetComponentInChildren<Text>();
        t.text = "GAMEOVER";
        t = gameObject.transform.Find("Prompt").transform.Find("Text").GetComponentInChildren<Text>();
        t.text = "Final Score:" + score;
    }
    private void Reset()
    {
        playerHealth = maxHealth;
        timer = 100.0f;
        score = 0;
    }
    private void LoadMenu() {
        Text t = gameObject.transform.Find("Prompt").transform.Find("Title").GetComponentInChildren<Text>();
        t.text = "MALWARE";
        t = gameObject.transform.Find("Prompt").transform.Find("Text").GetComponentInChildren<Text>();
        t.text = "Press SPACE to Start Game";
    }
}
