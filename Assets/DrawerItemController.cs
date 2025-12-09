using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class DrawerItemController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private IDCardGameManager.ItemData itemData;
    private IDCardGameManager gameManager;
    private TextMeshProUGUI emojiText;
    private Image backgroundImage;
    private RectTransform rectTransform;
    private bool isAnimating = false;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    public void Initialize(IDCardGameManager.ItemData data, IDCardGameManager manager)
    {
        itemData = data;
        gameManager = manager;

        SetupComponents();
        SetupAppearance();
        StoreOriginalTransform();
    }

    private void SetupComponents()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
        }

        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
        {
            backgroundImage = gameObject.AddComponent<Image>();
        }

        emojiText = GetComponentInChildren<TextMeshProUGUI>();
        if (emojiText == null)
        {
            CreateEmojiText();
        }

        Button button = GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        button.transition = Selectable.Transition.None;
    }

    private void CreateEmojiText()
    {
        GameObject textObj = new GameObject("EmojiText");
        textObj.transform.SetParent(transform);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        emojiText = textObj.AddComponent<TextMeshProUGUI>();
        emojiText.alignment = TextAlignmentOptions.Center;
        emojiText.fontSize = 48;
        emojiText.color = Color.white;
    }

    private void SetupAppearance()
    {
        if (emojiText != null)
        {
            emojiText.text = itemData.emoji;
        }

        if (itemData.isIdCard)
        {
            SetupIdCardAppearance();
        }
        else
        {
            SetupRegularItemAppearance();
        }

        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(80, 80);
        }
    }

    private void SetupIdCardAppearance()
    {
        backgroundImage.color = new Color(0.4f, 0.5f, 0.9f, 0.9f);
        
        if (emojiText != null)
        {
            Outline outline = emojiText.GetComponent<Outline>();
            if (outline == null)
            {
                outline = emojiText.gameObject.AddComponent<Outline>();
            }
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(2, 2);
        }
    }

    private void SetupRegularItemAppearance()
    {
        backgroundImage.color = new Color(1f, 1f, 1f, 0.1f);
    }

    private void StoreOriginalTransform()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating || !gameManager.IsGameActive() || gameManager.HasWon())
        {
            return;
        }

        gameManager.OnItemClicked(itemData);

        if (itemData.isIdCard)
        {
            StartCoroutine(CelebrateAnimation());
        }
        else
        {
            StartCoroutine(ShakeAnimation());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isAnimating)
        {
            transform.localScale = originalScale * 1.1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isAnimating)
        {
            transform.localScale = originalScale;
        }
    }

    private IEnumerator ShakeAnimation()
    {
        isAnimating = true;
        float duration = 0.5f;
        float shakeIntensity = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            
            float rotation = Random.Range(-5f, 5f);
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, rotation);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        isAnimating = false;
    }

    private IEnumerator CelebrateAnimation()
    {
        isAnimating = true;
        float duration = 1.5f;
        float elapsed = 0f;
        float rotationSpeed = 720f;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float scale = 1f + Mathf.Sin(progress * Mathf.PI * 4) * 0.3f;
            transform.localScale = originalScale * scale;
            
            float rotation = rotationSpeed * elapsed;
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, rotation);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
        isAnimating = false;
    }
}                                                                                     