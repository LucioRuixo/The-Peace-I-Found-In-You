using System;
using UnityEngine;

public class Blink : MonoBehaviour
{
    Animator animator;

    static public event Action OnBlinkOpenCompleted;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CompleteBlinkOpen()
    {
        OnBlinkOpenCompleted?.Invoke();
    }
}