using UnityEngine;
using DG.Tweening;
public class BarsManager : MonoBehaviour
{
    [SerializeField] private Transform upperLine;
    [SerializeField] private Transform lowerLine;


    private void Awake()
    {
        upperLine.gameObject.SetActive(false);
        lowerLine.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AnimateLower();
        AnimateUpper();
    }

    private void AnimateUpper()
    {
        if (upperLine == null) return;
        upperLine.gameObject.SetActive(true);

        float screenWidth = Screen.width;
        // Reset position far to the right
        Vector3 startPosition = new Vector3(screenWidth * -.7f, upperLine.transform.position.y, 0);
        Vector3 stopPosition = new Vector3(screenWidth * 0.7f, upperLine.transform.position.y, 0); // Centered
        Vector3 slowMoveLeft = new Vector3(screenWidth * 0.711f, upperLine.transform.position.y, 0); // Slight left movement
        Vector3 resetRight = new Vector3(screenWidth * -.7f, upperLine.transform.position.y, 0); // Far right again

        upperLine.transform.position = startPosition; // Start from the right
        Sequence warningSequence = DOTween.Sequence();

        warningSequence.Append(upperLine.transform.DOMoveX(stopPosition.x, 1f).SetEase(Ease.OutQuad)) // Move fast to center
                       .Append(upperLine.transform.DOMoveX(slowMoveLeft.x, 1.2f).SetEase(Ease.Linear)) // Small slow movement
                       .Append(upperLine.transform.DOMoveX(resetRight.x, .5f).SetEase(Ease.InQuad)) // Fast reset
                       .OnComplete(() => upperLine.gameObject.SetActive(false)); // Disable after animation
    }

    private void AnimateLower()
    {
        if (lowerLine == null) return;
        lowerLine.gameObject.SetActive(true);
        float screenWidth = Screen.width;
        // Reset position far to the right
        Vector3 startPosition = new Vector3(screenWidth * 1f, lowerLine.transform.position.y, 0);
        Vector3 stopPosition = new Vector3(screenWidth * 0.75f, lowerLine.transform.position.y, 0); // Centered
        Vector3 slowMoveLeft = new Vector3(screenWidth * 0.741f, lowerLine.transform.position.y, 0); // Slight left movement
        Vector3 resetRight = new Vector3(Screen.width * 1f, lowerLine.transform.position.y, 0); // Far right again

        lowerLine.transform.position = startPosition; // Start from the right

        // DOTween Animation Sequence
        Sequence warningSequence = DOTween.Sequence();

        warningSequence.Append(lowerLine.transform.DOMoveX(stopPosition.x, 1f).SetEase(Ease.OutQuad)) // Move fast to center
                       .Append(lowerLine.transform.DOMoveX(slowMoveLeft.x, 1.2f).SetEase(Ease.Linear)) // Small slow movement
                       .Append(lowerLine.transform.DOMoveX(resetRight.x, .5f).SetEase(Ease.InQuad)) // Fast reset
                       .OnComplete(() => lowerLine.gameObject.SetActive(false)); // Disable after animation

    }
}
