using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class IDCardGameManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string name;
        public string emoji;
     
        public bool isIdCard;
    }

    [Header("UI References")]
    [SerializeField] private RectTransform drawerContainer;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TextMeshProUGUI clicksText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI foundText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button newGameButton;

    [Header("Game Settings")]
    [SerializeField] private List<ItemData> gameItems = new List<ItemData>
    {
        new ItemData { name = "Phone", emoji = "📱", isIdCard = false },
        new ItemData { name = "Keys", emoji = "🔑", isIdCard = false },
        new ItemData { name = "Credit Card", emoji = "💳", isIdCard = false },
        new ItemData { name = "Note", emoji= "📝", isIdCard = false },
        new ItemData { name = "Pen", emoji= "✏️", isIdCard = false },
        new ItemData { name = "Paperclip", emoji = "📎", isIdCard = false },
        new ItemData { name = "Pin", emoji = "📌", isIdCard = false },
        new ItemData { name = "Safety Pin", emoji = "🧷", isIdCard = false },
        new ItemData { name = "Paper", emoji = "📄", isIdCard = false },
        new ItemData { name = "Clipboard", emoji = "📋", isIdCard = false },
        new ItemData { name = "Business Card", emoji = "📇", isIdCard = false },
        new ItemData { name = "Ticket", emoji = "🎫", isIdCard = false },
        new ItemData { name = "Pill", emoji = "💊", isIdCard = false },
        new ItemData { name = "Coin", emoji = "🪙", isIdCard = false },
        new ItemData { name = "Receipt", emoji = "🧾", isIdCard = false },
        new ItemData { name = "ID Card", emoji = "🆔", isIdCard = true }
    };

    private int clickCount = 0;
    private float gameStartTime;
    private bool isGameActive = false;
    private bool hasWon = false;
    private List<GameObject> spawnedItems = new List<GameObject>();
    private GameUIManager uiManager;

    void Awake()
    {
        uiManager = GetComponent<GameUIManager>();
        if (uiManager == null)
        {
            uiManager = gameObject.AddComponent<GameUIManager>();
        }
    }

    void Start()
    {
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(StartNewGame);
        }
        StartNewGame();
    }

    void Update()
    {
        if (isGameActive && !hasWon)
        {
            float elapsedTime = Time.time - gameStartTime;
            UpdateTimer(elapsedTime);
        }
    }

    public void StartNewGame()
    {
        ClearExistingItems();
        ResetGameState();
        SpawnItems();
        UpdateUI();
    }

    private void ClearExistingItems()
    {
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();
    }

    private void ResetGameState()
    {
        clickCount = 0;
        isGameActive = true;
        hasWon = false;
        gameStartTime = Time.time;
    }

    private void SpawnItems()
    {
        if (drawerContainer == null || itemPrefab == null)
        {
            Debug.LogWarning("Drawer container or item prefab not assigned!");
            return;
        }

        List<ItemData> shuffledItems = new List<ItemData>(gameItems);
        ShuffleList(shuffledItems);

        Rect drawerRect = drawerContainer.rect;
        float padding = 50f;
        
        // Get the usable area (accounting for padding and item size)
        float itemSize = 80f; // Size of items from DrawerItemController
        float halfItemSize = itemSize * 0.5f;
        float minX = -drawerRect.width * 0.5f + padding + halfItemSize;
        float maxX = drawerRect.width * 0.5f - padding - halfItemSize;
        float minY = -drawerRect.height * 0.5f + padding + halfItemSize;
        float maxY = drawerRect.height * 0.5f - padding - halfItemSize;

        foreach (ItemData itemData in shuffledItems)
        {
            GameObject itemObj = Instantiate(itemPrefab, drawerContainer);
            DrawerItemController itemController = itemObj.GetComponent<DrawerItemController>();
            
            if (itemController == null)
            {
                itemController = itemObj.AddComponent<DrawerItemController>();
            }

            RectTransform itemRect = itemObj.GetComponent<RectTransform>();
            if (itemRect != null)
            {
                // Set anchor to center for easier positioning
                itemRect.anchorMin = new Vector2(0.5f, 0.5f);
                itemRect.anchorMax = new Vector2(0.5f, 0.5f);
                itemRect.pivot = new Vector2(0.5f, 0.5f);
            }

            itemController.Initialize(itemData, this);

            if (itemRect != null)
            {
                // Position relative to center of drawer container
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                itemRect.anchoredPosition = new Vector2(randomX, randomY);
                itemRect.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            }

            spawnedItems.Add(itemObj);
        }
    }

    public void OnItemClicked(ItemData itemData)
    {
        if (!isGameActive || hasWon) return;

        clickCount++;
        UpdateClickCounter();

        if (itemData.isIdCard)
        {
            HandleWin();
        }
        else
        {
            ShowMessage($"That's a {itemData.name}, keep looking!", Color.white);
        }
    }

    private void HandleWin()
    {
        hasWon = true;
        isGameActive = false;
        float elapsedTime = Time.time - gameStartTime;
        string winMessage = $"🎉 Congratulations! You found the ID card in {clickCount} clicks and {Mathf.FloorToInt(elapsedTime)} seconds!";
        ShowMessage(winMessage, Color.green);
        UpdateFoundStatus(true);
    }

    private void UpdateUI()
    {
        UpdateClickCounter();
        UpdateTimer(0f);
        UpdateFoundStatus(false);
        ShowMessage("", Color.white);
    }

    private void UpdateClickCounter()
    {
        if (clicksText != null)
        {
            clicksText.text = clickCount.ToString();
        }
        if (uiManager != null)
        {
            uiManager.UpdateClicks(clickCount);
        }
    }

    private void UpdateTimer(float elapsedTime)
    {
        if (timeText != null)
        {
            timeText.text = Mathf.FloorToInt(elapsedTime) + "s";
        }
        if (uiManager != null)
        {
            uiManager.UpdateTimer(elapsedTime);
        }
    }

    private void UpdateFoundStatus(bool found)
    {
        if (foundText != null)
        {
            foundText.text = found ? "1/1" : "0/1";
        }
        if (uiManager != null)
        {
            uiManager.UpdateFoundStatus(found);
        }
    }

    private void ShowMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
        }
        if (uiManager != null)
        {
            uiManager.ShowMessage(message, color);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    public bool HasWon()
    {
        return hasWon;
    }
}

