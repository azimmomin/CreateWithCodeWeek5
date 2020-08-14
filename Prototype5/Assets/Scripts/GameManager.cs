using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool IsGameActive { get; private set; }

    [SerializeField] private List<GameObject> targets;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI gameOverText = null;
    [SerializeField] private Button restartButton = null;
    [SerializeField] private GameObject titleScreen = null;

    private int score = 0;

    private void Awake()
    {
        Target.OnTargedClicked += UpdateScore;
        Target.OnBadTargetCollidedWithSensor += SetGameOver;
        DifficultyButton.OnDifficultySet += StartGame;
    }

    private void UpdateScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    private void SetGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        IsGameActive = false;
    }

    private void Start()
    {
        IsGameActive = false;
    }

    private void StartGame(float difficulty)
    {
        titleScreen.SetActive(false);
        scoreText.gameObject.SetActive(true);

        IsGameActive = true;
        ResetScore();

        spawnRate /= difficulty;
        if (targets.Count > 0 && spawnRate > 0f)
            StartCoroutine(SpawnTarget());
    }

    private void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    private IEnumerator SpawnTarget()
    {
        while (IsGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = UnityEngine.Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void RestartGame()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        Target.OnTargedClicked -= UpdateScore;
        Target.OnBadTargetCollidedWithSensor -= SetGameOver;
        DifficultyButton.OnDifficultySet -= StartGame;
    }
}
