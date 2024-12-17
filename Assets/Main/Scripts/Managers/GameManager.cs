using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerStats playerStats;

    bool isGameStart = true;
    [SerializeField] AudioSource gameBgm;

    public static Action<int> OnUpdateTimer;

    int timer;
    [SerializeField] TMP_Text timeText;

    int enemyKilled, enemyLived;
    [SerializeField] TMP_Text enemyKilledText, enemyLivedText;

    [Header("Win Lose UI")]
    [SerializeField] Image endGameBackground;
    [SerializeField] GameObject statsPanel;
    [SerializeField] TMP_Text winLoseText;
    [SerializeField] TMP_Text enemyKilledCountText, goldCountText;

    private void OnEnable()
    {
        WaveManager.OnEnemySpawnedE += OnEnemySpawnedHandler;
        WaveManager.OnEnemyDieE += OnEnemyDieHandler;

        PlayerStats.OnPlayerDieE += OnLose;

    }

    private void OnDisable()
    {
        WaveManager.OnEnemySpawnedE -= OnEnemySpawnedHandler;
        WaveManager.OnEnemyDieE -= OnEnemyDieHandler;

        PlayerStats.OnPlayerDieE -= OnLose;
    }

    private void Start()
    {
        gameBgm.Play();

        Time.timeScale = 1;
        playerStats = FindObjectOfType<PlayerStats>();

        timer = 0;
        enemyKilled = 0;
        enemyLived = 0;

        enemyKilledText.text = enemyKilled.ToString();
        enemyLivedText.text = enemyLived.ToString();

        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (isGameStart)
        {
            TimeSpan t = TimeSpan.FromSeconds(timer);
            timeText.text = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);

            OnUpdateTimer?.Invoke(timer);

            OnWinChecker(timer);

            yield return new WaitForSeconds(1);

            timer++;
        }
    }

    public void OnEnemyDieHandler(Enemy enemy)
    {
        enemyKilled++;
        enemyLived--;

        enemyKilledText.text = enemyKilled.ToString();
        enemyLivedText.text = enemyLived.ToString();
    }

    public void OnEnemySpawnedHandler()
    {
        enemyLived++;
        enemyLivedText.text = enemyLived.ToString();
    }

    public void OnWinChecker(int time)
    {
        if (time >= 180)        // 3 minutes
        {
            OnWin();
        }
    }

    public void OnWin()
    {
        Time.timeScale = 0;
        endGameBackground.gameObject.SetActive(true);
        endGameBackground.DOFade(0.75f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            statsPanel.SetActive(true);
            winLoseText.text = "Stage Clear";
            enemyKilledCountText.text = enemyKilled.ToString();
            goldCountText.text = playerStats.Gold.ToString();
        });
    }

    public void OnLose()
    {
        Time.timeScale = 0;
        endGameBackground.gameObject.SetActive(true);
        endGameBackground.DOFade(0.75f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            statsPanel.SetActive(true);
            winLoseText.text = "You Lose";
            enemyKilledCountText.text = enemyKilled.ToString();
            goldCountText.text = playerStats.Gold.ToString();
        });
    }
}
