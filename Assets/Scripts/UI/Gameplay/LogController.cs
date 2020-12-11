using UnityEngine;
using TMPro;

public class LogController : MonoBehaviour
{
    string lastCharacterToSpeak = "";

    [SerializeField] TextMeshProUGUI log = null;

    public void AddSentence(string characterName, string sentence)
    {
        if (characterName != lastCharacterToSpeak)
        {
            lastCharacterToSpeak = characterName;

            if (log.text != "") log.text += "\n\n";

            log.text += lastCharacterToSpeak + ": ";
        }

        log.text += "\n" + sentence;
    }

    public void SetLastCharacterToSpeak(string _lastCharacterToSpeak)
    {
        lastCharacterToSpeak = _lastCharacterToSpeak;
    }

    public string GetLastCharacterToSpeak()
    {
        return lastCharacterToSpeak;
    }

    public void SetLogText(string logText)
    {
        log.text = logText;
    }

    public string GetLogText()
    {
        return log.text;
    }
}