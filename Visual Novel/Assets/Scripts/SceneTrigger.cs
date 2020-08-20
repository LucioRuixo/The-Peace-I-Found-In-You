using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public SceneBitSO initialBit;

    void Start()
    {
        initialBit.Execute();
    }
}