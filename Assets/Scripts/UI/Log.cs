using UnityEngine;
using TMPro;

public class Log : MonoBehaviour
{
    bool firstSentenceAdded = false;

    string lastCharacterName = "";

    [SerializeField] TextMeshProUGUI logText = null;

    void Start()
    {
        logText.text = "";

        gameObject.SetActive(false);
    }

    public void AddSentence(string characterName, string sentence)
    {
        if (characterName != lastCharacterName)
        {
            lastCharacterName = characterName;

            if (firstSentenceAdded) logText.text += "\n\n";
            else firstSentenceAdded = true;

            logText.text += lastCharacterName + ": ";
        }

        logText.text += "\n" + sentence;
    }
}