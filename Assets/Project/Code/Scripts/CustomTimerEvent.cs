using System;
using UnityEngine;
using UnityEngine.Events;

public class CustomTimerEvent : MonoBehaviour
{

    [SerializeField] private bool onAwake;
    [SerializeField] private float second;
    public UnityEvent onTimerFinished;

    private float currentTimer = 0;
    bool finished = false;

    private void Start()
    {
        if (onAwake)
        {
            StartTimer(second);
        }
    }

    public void StartTimer(float timer)
    {
        currentTimer = timer;
        finished = false;
    }

    private void Update()
    {
        if(currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
            finished = false;
        }
        else
        {
            if (!finished)
            {
                onTimerFinished?.Invoke();
                currentTimer = 0f;
                finished = true;
            }
        }
    }
}
