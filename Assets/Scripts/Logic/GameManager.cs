using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Use this for initialization
    void Start()
    {
        inGame = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!inGame && Input.GetKeyDown("space"))
            StartGame();
        else if (inGame)
        {
            if (Input.GetKeyDown("space"))
                Time.timeScale = 0f; // Pause Game
            else
                Time.timeScale = 1f;

            playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);

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
    }
    void NextLevel() {
        timer = 100.0f;
    }
    private void GameOver()
    {
        score = 0;
    }
}
