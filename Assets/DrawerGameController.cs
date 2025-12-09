using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DrawerGameController : MonoBehaviour
{
    [System.Serializable]
    public class DrawerItemData
    {
        public string itemName;
        public string emoji;
        public bool isIdCard;
    }

    [Header("UI References")]
    public GameObject drawerContainer;
    public GameObject itemPrefab;
    public TextMeshProUGUI clicksText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI foundText;
    public TextMeshProUGUI messageText;
    public Button newGameButton;

    [Header("Game Settings")]
    public List<DrawerItemData> items = new List<DrawerItemData>
    {
        new DrawerItemData { itemName = "Phone", emoji = "📱", isIdCard = false },
        new DrawerItemData { itemName = "Keys", emoji = "🔑", isIdCard = false },
        new DrawerItemData { itemName = "Credit Card", emoji = "💳", isIdCard = false },
        new DrawerItemData { itemName = "Note", emoji = "📝", isIdCard = false },
        new DrawerItemData { itemName = "Pen", emoji = "✏️", isIdCard = false },
        new DrawerItemData { itemName = "Paperclip", emoji = "📎", isIdCard = false },
        new DrawerItemData { itemName = "Pin", emoji = "📌", isIdCard = false },
        new DrawerItemData { itemName = "Safety Pin", emoji = "🧷", isIdCard = false },
        new DrawerItemData { itemName = "Paper", emoji = "📄", isIdCard = false },
        new DrawerItemData { itemName = "Clipboard", emoji = "📋", isIdCard = false },
        new DrawerItemData { itemName = "Business Card", emoji = "📇", isIdCard = false },
        new DrawerItemData { itemName = "Ticket", emoji = "🎫", isIdCard = false },
        new DrawerItemData { itemName = "Pill", emoji = "💊", isIdCard = false },
        new DrawerItemData { itemName = "Coin", emoji = "🪙", isIdCard = false },
        new DrawerItemData { itemName = "Receipt", emoji = "🧾", isIdCard = false },
        new DrawerItemData { itemName = "ID Card", emoji = "🆔", isIdCard = true }
    };

    private int clicks = 0;
    private float startTime;
    private bool gameActive = false;
    private bool idCardFound = false;
    private List<GameObject> spawnedItems = new List<GameObject>();

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
        if (gameActive && !idCardFound)
        {
            float elapsed = Time.time - startTime;
            if (timeText != null)
            {
                timeText.text = Mathf.FloorToInt(elapsed) + "s";
            }
        }
    }

    public void StartNewGame()
    {
        // Clear existing items
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();

        // Reset game state
        clicks = 0;
        gameActive = true;
        idCardFound = false;
        startTime = Time.time;

        // Update UI
        if (clicksText != null) clicksText.text = "0";
        if (timeText != null) timeText.text = "0s";
        if (foundText != null) foundText.text = "0/1";
        if (messageText != null)
        {
            messageText.text = "";
            messageText.color = Color.white;
        }

        // Shuffle items
        List<DrawerItemData> shuffledItems = new List<DrawerItemData>(items);
        Shuffle(shuffledItems);

        // Spawn items
        if (drawerContainer != null && itemPrefab != null)
        {
            RectTransform drawerRect = drawerContainer.GetComponent<RectTransform>();
            if (drawerRect != null)
            {
                foreach (DrawerItemData itemData in shuffledItems)
                {
                    GameObject itemObj = Instantiate(itemPrefab, drawerContainer.transform);
                    DrawerItem drawerItem = itemObj.GetComponent<DrawerItem>();
                    if (drawerItem != null)
                    {
                        drawerItem.Initialize(itemData, this);
                    }

                    // Random position within drawer
                    RectTransform itemRect = itemObj.GetComponent<RectTransform>();
                    if (itemRect != null)
                    {
                        float x = Random.Range(50f, drawerRect.rect.width - 50f);
                        float y = Random.Range(50f, drawerRect.rect.height - 50f);
                        itemRect.anchoredPosition = new Vector2(x, -y);
                        itemRect.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    }

                    spawnedItems.Add(itemObj);
                }
            }
        }
    }

    public void OnItemClicked(DrawerItemData itemData)
    {
        if (!gameActive || idCardFound) return;

        clicks++;
        if (clicksText != null)
        {
            clicksText.text = clicks.ToString();
        }

        if (itemData.isIdCard)
        {
            idCardFound = true;
            gameActive = false;
            if (foundText != null) foundText.text = "1/1";
            if (messageText != null)
            {
                float elapsed = Time.time - startTime;
                messageText.text = $"🎉 Congratulations! You found the ID card in {clicks} clicks and {Mathf.FloorToInt(elapsed)} seconds!";
                messageText.color = Color.green;
            }
        }
        else
        {
            if (messageText != null)
            {
                messageText.text = $"That's a {itemData.itemName}, keep looking!";
                messageText.color = Color.white;
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}


