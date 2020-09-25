using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    bool timerActive = false;

    float time;

    Quaternion targetRotation;

    public RectTransform needlePivot;

    public static event Action<bool> OnTimeUp;

    void Awake()
    {
        Quaternion addedRotation = Quaternion.Euler(0f, 0f, -360f);
        targetRotation = needlePivot.rotation * addedRotation;
    }

    void Update()
    {
        if (!timerActive) return;

        needlePivot.Rotate(Vector3.forward, (Time.deltaTime / time) * -360f);
        if (needlePivot.rotation == targetRotation)
        {
            OnTimeUp?.Invoke(false);
            timerActive = false;
        }
    }

    public void StartTimer(float _time)
    {
        time = _time;
        timerActive = true;
    }
}