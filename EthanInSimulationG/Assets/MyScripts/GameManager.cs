using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private float health = 100;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOverText;
    public GameObject background;
    public GameObject background2;
    public bool isGameActive = false;
    private SpawnManager spawnManager;
    //public FixedButton SceneButton;

    void Start()
    {
        // scoreText.text = "Score: " + score;
        // healthText.text = "Health: " + health;
        background.SetActive(true);
    }

    void Update()
    {

    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth(float damageToAdd)
    {
        health -= damageToAdd;
        healthText.text = "Health: " + health;

        if (health == 0)
            GameOver();
    }

    public void GameOver()
    {
        background.SetActive(true);
        //gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        background2.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;

        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        spawnManager.SpawnEnemyWave(spawnManager.waveNumber * difficulty);


        score = 0;
        health = 100;
        //spawn ekle
        background.SetActive(false);
    }
}