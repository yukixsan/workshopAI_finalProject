using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NUnit.Framework;

public class CardUI : MonoBehaviour
{
    public RectTransform[] cards; // Assign the Card UI RectTransforms in the Inspector
    public float moveDistance = 20f; // Distance to move up
    public float moveDuration = 0.2f; // Time for animation

    private PlayerInput playerInput;
    private InputAction skillInput;

    private Vector2[] originalPos;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        skillInput = playerInput.actions["Skill"];
        originalPos = new Vector2[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            originalPos[i] = cards[i].anchoredPosition;
        }
    }

    private void OnEnable()
    {
        skillInput.performed += OnSkillInput;
    }

    private void OnDisable()
    {
        skillInput.performed -= OnSkillInput;
    }

    public void OnSkillInput(InputAction.CallbackContext context)
    {
        int index = int.Parse(context.control.name)- 1; // Adjusting for 0-based index
        AnimateCard(index);
        print(index);
    }

    private void AnimateCard(int index)
    {
        if (index < 0 || index >= cards.Length) return;

        // Move up
        cards[index].DOAnchorPosY(cards[index].anchoredPosition.y + moveDistance, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Move back down
                cards[index].DOAnchorPosY(originalPos[index].y, 0f);
            });
    }
}
