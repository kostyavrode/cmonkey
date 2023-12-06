using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static Action onGameStarted;
    public static Action onGameEnded;
    public static GameManager instance;
    public CameraFollower cameraFollower;
    public GameObject losePanel;
    public GameObject inGameUI;
    private bool isGameStarted;
    private int score;
    public TMP_Text scoreBar;
    public TMP_Text bestScoreBar;
    private void Awake()
    {
        instance = this;
    }
    private void OnDisable()
    {
    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            score++;
            scoreBar.text ="Score: "+ score.ToString();
        }
    }
    public void EndGame()
    {
        inGameUI.SetActive(false);
        losePanel.SetActive(true);
        onGameEnded?.Invoke();
        CheckBestScore();
        isGameStarted = false;
    }
    private void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (PlayerPrefs.GetInt("BestScore")<score)
            {
                bestScoreBar.text =score.ToString();
                PlayerPrefs.SetInt("BestScore", score);
            }
            else
            {
                bestScoreBar.text = PlayerPrefs.GetInt("BestScore").ToString();
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreBar.text =  score.ToString();
        }
    }    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsGameStarted()
    {
        return isGameStarted;
    }
    public void StartGame()
    {
        isGameStarted = true;
        onGameStarted?.Invoke();
        
        cameraFollower.enabled = true;
        cameraFollower.StartShow();
    }
    public IEnumerator ShowLosePanel()
    {
        yield return new WaitForSeconds(1f);
        EndGame();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
