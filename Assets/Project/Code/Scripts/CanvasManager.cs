using MoreMountains.Feedbacks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    // ---- Instance ----
    private static CanvasManager _instance;
    public static CanvasManager Instance { get { return _instance; } }
    private CanvasManager() { }


    [Header("Score")]
    [SerializeField] private TMP_Text playerTextScore;
    [SerializeField] private TMP_Text enemyTextScore;

    [Header("Result")]
    [SerializeField] private GameObject resultMenu;
    [SerializeField] private TMP_Text resultText;

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.onPlayerWin += OnPlayerWin;
        GameManager.Instance.onPlayerLost += OnPlayerLost;
    }

    private void OnDisable()
    {
        GameManager.Instance.onPlayerWin -= OnPlayerWin;
        GameManager.Instance.onPlayerLost -= OnPlayerLost;
    }

    private void OnPlayerWin()
    {
        resultText.text = "Win !!!";
        resultText.color = Color.green;
        ShowResultMenu();
    }

    private void OnPlayerLost()
    {
        resultText.text = "Lost...";
        resultText.color = Color.red;
        ShowResultMenu();
    }

    public void ShowResultMenu()
    {
        resultMenu.SetActive(true);
    }

    public void UpdateScore(float playerScore, float enemyScore)
    {

        playerTextScore.text = $"{playerScore}";
        enemyTextScore.text = $"{enemyScore}";
    }

    #region Buttons events
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        //SceneManager.LoadScene("To determine");
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
