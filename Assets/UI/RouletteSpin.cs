using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.InputSystem;

public class RouletteSpin : MonoBehaviour
{
    public Transform rouletteParent; // Parent object that rotates
    public RectTransform[] cards; // Individual card UI elements
    public float rotationStep = 120f; // Angle to rotate per tick
    public float tickDuration = 0.5f; // Time between ticks
    public float popUpScale = 1.2f; // Scale factor for pop-up
    public float popUpDuration = 0.2f; // Duration of pop-up animation

    public delegate void OnColorSelected(string color); // Updated to use string
    public static event OnColorSelected ColorSelected; // Event for broadcasting the selected color

    private bool isSpinning = false;
    private float currentRotation = 0f;
    private int currentIndex = 0; // Keeps track of the highlighted card

    [SerializeField] private AudioClip spinSfx;


    public void OnSpin(InputAction.CallbackContext context)
    {
        if (context.performed) // Use Space to toggle spinning
        {
            if (!isSpinning)
                StartRoulette();
            else
                StopRoulette();
        }
    }


    private void StartRoulette()
    {
        isSpinning = true;
        
        StartCoroutine(SpinCoroutine());
    }

    private void StopRoulette()
    {
        isSpinning = false;
        HighlightCard(currentIndex); // Ensure the last card is highlighted
        if (ColorSelected != null)
        {
            
            cards[currentIndex].transform.DOPunchRotation(new Vector3(0, 0, 10), popUpDuration, 20);
            ColorSelected.Invoke(cards[currentIndex].name); // Use card name for simplicity
            
            Debug.Log($"Selected Color: {cards[currentIndex].name}");
        }
        }

    private IEnumerator SpinCoroutine()
    {
        while (isSpinning)
        {
            // Reset scale of the current highlighted card
            cards[currentIndex].DOScale(1f, popUpDuration);

            // Calculate the next index
            currentIndex = (currentIndex + 1) % cards.Length;

            // Rotate the parent
            currentRotation += rotationStep;
            rouletteParent.DORotate(new Vector3(0, 0, currentRotation), tickDuration, RotateMode.Fast)
                          .SetEase(Ease.OutQuad);

            // Pop-up effect for the next card
            HighlightCard(currentIndex);
            SoundManager.Instance.PlaySFX(spinSfx);
            // Wait for the next tick
            yield return new WaitForSeconds(tickDuration);
        }
    }

    private void HighlightCard(int index)
    {
        // Scale up the current card for the pop-up effect
        cards[index].DOScale(popUpScale, popUpDuration)
                    .SetEase(Ease.OutQuad);
    }
}
