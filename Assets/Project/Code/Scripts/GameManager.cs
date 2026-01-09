using MoreMountains.Feedbacks;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private GameManager() { }

    [Header("Score")]
    [SerializeField] private int playerPoint = 0;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private MMF_Player playerPointFeedBack;
    [SerializeField] private MMF_Player enemyPointFeedBack;

    public delegate void OnPlayerWin();
    public OnPlayerWin onPlayerWin;

    public delegate void OnPlayerLost();
    public OnPlayerWin onPlayerLost;


    public bool infinitMod = false;


    private void Awake()
    {
        if(_instance == null) _instance = this;
    }

    public void NewPoint(int amount, bool forPlayer)
    {
        if (forPlayer)
        {
            playerPoint += amount;
            enemyPoint -= amount;
            playerPointFeedBack?.PlayFeedbacks();
        }
        else
        {
            playerPoint -= amount;
            enemyPoint += amount;
            enemyPointFeedBack?.PlayFeedbacks();
        }


        playerPoint = Mathf.Clamp(playerPoint, 0, 3);
        enemyPoint = Mathf.Clamp(enemyPoint, 0, 3);

        if (playerPoint >= 3) Win();
        else if (enemyPoint >= 3) Lost();
        CanvasManager.Instance.UpdateScore(playerPoint, enemyPoint);
    }

    private void Win() => onPlayerWin?.Invoke();
    private void Lost() => onPlayerLost?.Invoke();
    
}
