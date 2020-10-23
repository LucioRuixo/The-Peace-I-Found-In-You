using System;
using UnityEngine;

public class Blink : MonoBehaviour
{
    static public event Action OnBlinkOpenCompleted;

    public void CompleteBlinkOpen()
    {
        OnBlinkOpenCompleted?.Invoke();
    }
}