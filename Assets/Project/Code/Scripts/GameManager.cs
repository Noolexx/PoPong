using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private GameManager() { }


    [Header("Score")]
    [SerializeField] private int playerPoint = 0;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private int scoreToWin = 3;
    [SerializeField] private MMF_Player playerPointFeedBack;
    [SerializeField] private MMF_Player enemyPointFeedBack;

    public delegate void OnPlayerWin();
    public OnPlayerWin onPlayerWin;

    public delegate void OnPlayerLost();
    public OnPlayerWin onPlayerLost;


    [Header("Debug")]
    [SerializeField] private InputActionReference debugInput;
    [SerializeField] private GameObject debugMenu;
    [SerializeField] private MMF_Player debugMenuFeedback;
    bool inDebug = false;
    public bool unableScore = false;
    public bool infinitMode = false;


    private void Awake()
    {
        if(_instance == null) _instance = this;

        // Debug Menu
        debugMenu.SetActive(inDebug);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        debugInput.action.started += ToggleDebugMode;
#endif
    }


    private void OnDisable()
    {
#if UNITY_EDITOR
        debugInput.action.started -= ToggleDebugMode;
#endif
    }

    private void ToggleDebugMode(InputAction.CallbackContext context)
    {
        inDebug = !inDebug;

        debugMenuFeedback.Initialization();
        
        if (inDebug)
        {
            debugMenu.SetActive(true);
            debugMenuFeedback.PlayFeedbacks();
        }
        else
        {
            debugMenuFeedback.Events.OnComplete.AddListener(OnDebugMenuClosed);
            debugMenuFeedback.PlayFeedbacksInReverse();
        }

    }

    private void OnDebugMenuClosed()
    {
        debugMenuFeedback.Events.OnComplete.RemoveListener(OnDebugMenuClosed);
        debugMenuFeedback.SetDirection(MMFeedbacks.Directions.TopToBottom);
        debugMenu.SetActive(false);
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


        if (!infinitMode)
        {
            playerPoint = Mathf.Clamp(playerPoint, 0, scoreToWin);
            enemyPoint = Mathf.Clamp(enemyPoint, 0, scoreToWin);

            if (playerPoint >= scoreToWin) Win(); // Player Win
            else if (enemyPoint >= scoreToWin) Lost(); // Enemy Win
        }
        else
        {
            playerPoint = Mathf.Clamp(playerPoint, 0, 999);
            enemyPoint = Mathf.Clamp(enemyPoint, 0, 999);
        }

            CanvasManager.Instance.UpdateScore(playerPoint, enemyPoint);
    }

    private void Win() => onPlayerWin?.Invoke();
    private void Lost() => onPlayerLost?.Invoke();

    #region Debug

    public void OnToggleUnableScore(bool value)
    {
        unableScore = value;
    }

    public void OnToggleInfinitMode(bool value)
    {
        infinitMode = value;
    }

    #endregion
}
