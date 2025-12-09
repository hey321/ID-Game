using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References (Optional - for direct UI management)")]
    [SerializeField] private TextMeshProUGUI clicksText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI foundText;
    [SerializeField] private TextMeshProUGUI messageText;

    private int currentClicks = 0;
    private float currentTime = 0f;
    private bool foundStatus = false;

    public void UpdateClicks(int clicks)
    {
        currentClicks = clicks;
        if (clicksText != null)
        {
            clicksText.text = clicks.ToString();
        }
    }

    public void UpdateTimer(float elapsedTime)
    {
        currentTime = elapsedTime;
        if (timeText != null)
        {
            timeText.text = Mathf.FloorToInt(elapsedTime) + "s";
        }
    }

    public void UpdateFoundStatus(bool found)
    {
        foundStatus = found;
        if (foundText != null)
        {
            foundText.text = found ? "1/1" : "0/1";
        }
    }

    public void ShowMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
        }
    }

    public void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            messageText.color = Color.white;
        }
    }

    public void ResetUI()
    {
        UpdateClicks(0);
        UpdateTimer(0f);
        UpdateFoundStatus(false);
        ClearMessage();
    }

    public int GetCurrentClicks()
    {
        return currentClicks;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool GetFoundStatus()
    {
        return foundStatus;
    }
}

