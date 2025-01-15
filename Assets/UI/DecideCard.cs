using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DecideCard : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;
    private GameObject activeCard;
    private Image[] cardImages;
    private Image activeImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Cache Image components for each card
        cardImages = new Image[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            cardImages[i] = cards[i].GetComponentInChildren<Image>();

            if (cardImages[i] == null)
            {
                Debug.LogWarning($"No Image component found in children of {cards[i].name}");
            }

            cards[i].SetActive(false); // Disable all cards initially
        }
    }

    public void Decide(int cardIndex)
    {
        print(cardIndex);
        // Ensure the index is within bounds
        if (cardIndex < 0 || cardIndex >= cards.Length)
        {
            Debug.LogError($"Invalid cardIndex: {cardIndex}");
            return;
        }

        // Fade out and disable the previously active card (if any)
        if (activeCard != null)
        {
            FadeOutAndDisable(activeCard, activeImage);
        }

        // Enable the selected card and update references
        activeCard = cards[cardIndex];
        activeImage = cardImages[cardIndex];
        activeCard.SetActive(true);
    }

    private void FadeOutAndDisable(GameObject card, Image image)
    {
        int index = System.Array.IndexOf(cards, card);
        if (index < 0 || image == null)
        {
            card.SetActive(false);
            return;
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.transform.DOShakeRotation(0.3f,new Vector3(0,0,10), 20, 90, true, ShakeRandomnessMode.Harmonic));
        sequence.Join(image.DOFade(0f, 0.5f));

        // Fade the image to 0 alpha over 0.5 seconds
        sequence.OnComplete(() =>
        {   
            card.SetActive(false); // Disable after fade-out
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f); // Reset alpha
        });
    }

    // Hides the currently active card with fade-out effect
    public void HideCurrent()
    {
        if (activeCard != null && activeImage != null)
        {
            FadeOutAndDisable(activeCard, activeImage); // Fade and disable active card
            activeCard = null;
            activeImage = null;
        }
    }

    // Utility to instantly hide all cards (no animation)
    public void HideAll()
    {
        foreach (var card in cards)
        {
            card.SetActive(false);
        }
    }
}
