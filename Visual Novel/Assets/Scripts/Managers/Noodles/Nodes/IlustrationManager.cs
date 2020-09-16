using System;
using UnityEngine;
using nullbloq.Noodles;

public class IlustrationManager : MonoBehaviour
{
    public enum Ilustration
    {

    }

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnIlutration += Begin;
    }

    void OnDisable()
    {
        NoodleManager.OnIlutration -= Begin;
    }

    void Begin(CustomIlustrationNode node)
    {

    }

    void End()
    {

    }
}