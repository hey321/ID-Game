using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DrawerItem : MonoBehaviour, IPointerClickHandler
{
    private DrawerGameController.DrawerItemData itemData;
    private DrawerGameController gameController;
    private TextMeshProUGUI emojiText;
    private TextMeshProUGUI nameText;
    private Image backgroundImage;
    private bool isClicked = false;

    public void Initialize(DrawerGameController.DrawerItemData data, DrawerGameController controller)
    {
        itemData = data;
        gameController = controller;

        // Get or create UI components
        emojiText = GetComponentInChildren<TextMeshProUGUI>();
        if (emojiText == null)
        {
            GameObject emojiObj = new GameObject("EmojiText");
            emojiObj.transform.SetParent(transform);
            RectTransform emojiRect = emojiObj.AddComponent<RectTransform>();
            emojiRect.anchorMin = Vector2.zero;
            emojiRect.anchorMax = Vector2.one;
            emojiRect.sizeDelta = Vector2.zero;
            emojiRect.anchoredPosition = Vector2.zero;
            emojiText = emojiObj.AddComponent<TextMeshProUGUI>();
            emojiText.alignment = TextAlignmentOptions.Center;
            emojiText.fontSize = 48;
        }

        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
        {
            backgroundImage = gameObject.AddComponent<Image>();
        }

        // Set up appearance
        if (itemData.isIdCard)
        {
            emojiText.text = itemData.emoji;
            backgroundImage.color = new Color(0.4f, 0.5f, 0.9f, 0.9f);
            // Add glow effect for ID card
            var outline = emojiText.GetComponent<UnityEngine.UI.Outline>();
            if (outline == null)
            {
                outline = emojiText.gameObject.AddComponent<UnityEngine.UI.Outline>();
            }
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(2, 2);
        }
        else
        {
            emojiText.text = itemData.emoji;
            backgroundImage.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // Set size
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.sizeDelta = new Vector2(80, 80);
        }

        // Add button component for click detection
        Button button = GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        button.transition = Selectable.Transition.None;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return;

        isClicked = true;
        gameController.OnItemClicked(itemData);

        // Shake animation
        StartCoroutine(ShakeAnimation());

        // If it's the ID card, celebrate
        if (itemData.isIdCard)
        {
            StartCoroutine(CelebrateAnimation());
        }
    }

    private System.Collections.IEnumerator ShakeAnimation()
    {
        Vector3 originalPos = transform.localPosition;
        float shakeDuration = 0.5f;
        float shakeAmount = 10f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);
            transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        isClicked = false;
    }

    private System.Collections.IEnumerator CelebrateAnimation()
    {
        Vector3 originalScale = transform.localScale;
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float scale = 1f + Mathf.Sin(elapsed * Mathf.PI * 2) * 0.3f;
            transform.localScale = originalScale * scale;
            transform.Rotate(0, 0, 360 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        transform.rotation = Quaternion.identity;
    }
}


