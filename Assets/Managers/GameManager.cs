using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject player;
    [SerializeField] private ParticleSystem playerExplosion;
    [SerializeField] private AudioClip deathSound;
    private PlayerHealth playerHealth;


    [Header("Bosses")]
    [SerializeField] private GameObject firstBoss;  // Assign the first boss in the Inspector
    [SerializeField] private GameObject secondBoss; // Assign the second boss in the Inspector

    private EnemyHealth firstBossHealth;
    private EnemyHealth secondBossHealth;

    [SerializeField] private ParticleSystem explosion;

    [SerializeField] private float bossDeathDelay = .5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource bgmSource; // Assign an AudioSource in the Inspector
    [SerializeField] private AudioClip firstBossBgm;
    [SerializeField] private AudioClip secondBossBgm;

    [Header("UI")]
    [SerializeField] private Texture2D cursorTex;
    private Vector2 cursorHotspot;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private CanvasGroup winCanvas;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private GameObject dieScreen;
    private bool winGame;
    private bool loseGame;
    private bool pauseGame;

    [Header("Warning Text")]
    [SerializeField] private TextMeshProUGUI warningText;
    private BarsManager bars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.OnDie += () => StartCoroutine(KillPlayer());
        playerInput.SwitchCurrentActionMap("Player");

        cursorHotspot = new Vector2(cursorTex.width / 2, cursorTex.height / 2);
        Cursor.SetCursor(cursorTex, cursorHotspot, CursorMode.Auto);

        winGame = false;
        loseGame = false;
        winScreen.gameObject.SetActive(false);
        winCanvas.alpha = 0f;

        pauseScreen.gameObject.SetActive(false);
        pauseGame = false;

        helpScreen.gameObject.SetActive(false);

        bars = GetComponent<BarsManager>();
        bars.enabled = false;

        firstBossHealth = firstBoss.GetComponent<EnemyHealth>();
        secondBossHealth = secondBoss.GetComponent<EnemyHealth>();

        // Set up death event listeners
        firstBossHealth.OnDie += () => StartCoroutine(HandleBossDeath(firstBoss, secondBoss, secondBossBgm));
        secondBossHealth.OnDie += () => StartCoroutine(WinGame(secondBoss));

        // Enable only the first boss at the start
        firstBoss.SetActive(false);
        secondBoss.SetActive(false);

        StartCoroutine(HandleBossDeath(null, firstBoss, firstBossBgm));


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !winGame && !loseGame)
        {
            TogglePause();
        }
    }

    private IEnumerator HandleBossDeath(GameObject currentBoss, GameObject nextBoss, AudioClip nextBgm)
    {
        if (currentBoss != null)
        {
            Vector3 bossPosition = currentBoss.transform.position;
            explosion.transform.position = new Vector3(bossPosition.x, 4, bossPosition.z);
            if (explosion != null)
            {
                explosion.gameObject.SetActive(true);
                explosion.Play();
            }
            yield return StartCoroutine(FadeOutBgm());

        }

        yield return StartCoroutine(ShowBossWarning(nextBoss));
        // Wait for the death animation or delay
        yield return new WaitForSeconds(bossDeathDelay);

        if (nextBoss != null)
        {
            // Enable the next boss
            nextBoss.SetActive(true);
            //Debug.Log($"Boss {nextBoss.name} is now active!");

            PlayBgm(nextBgm);
        }

    }

    private IEnumerator ShowBossWarning(GameObject nextBoss)
    {
        if (warningText == null) yield break;
        warningText.gameObject.SetActive(true);

        // Set the warning text based on the next boss
        if (nextBoss == firstBoss)
        {
            warningText.text = "WARNING!\nROULETTE";
        }
        else if (nextBoss == secondBoss)
        {
            warningText.text = "WARNING!\nDICE CUP";
        }

        bars.enabled = true;

        float screenWidth = Screen.width;
        // Reset position far to the right
        Vector3 startPosition = new Vector3(screenWidth * 1.2f, warningText.transform.position.y, 0);
        Vector3 stopPosition = new Vector3(screenWidth * 0.7f, warningText.transform.position.y, 0); // Centered
        Vector3 slowMoveLeft = new Vector3(screenWidth * 0.675f, warningText.transform.position.y, 0); // Slight left movement
        Vector3 resetRight = new Vector3(Screen.width * 1.2f, warningText.transform.position.y, 0); // Far right again

        warningText.transform.position = startPosition; // Start from the right

        // DOTween Animation Sequence
        Sequence warningSequence = DOTween.Sequence();

        warningSequence.Append(warningText.transform.DOMoveX(stopPosition.x, 1f).SetEase(Ease.OutQuad)) // Move fast to center
                       .Append(warningText.transform.DOMoveX(slowMoveLeft.x, 1.2f).SetEase(Ease.Linear)) // Small slow movement
                       .Append(warningText.transform.DOMoveX(resetRight.x, .5f).SetEase(Ease.InQuad)) // Fast reset
                       .OnComplete(() => warningText.gameObject.SetActive(false)); // Disable after animation

        yield return warningSequence.WaitForCompletion(); // Wait for animation to complete
        bars.enabled = false;
    }

    private IEnumerator WinGame(GameObject currentBoss)
    {
        winGame = true;
        Vector3 bossPosition = currentBoss.transform.position;
        explosion.transform.position = new Vector3(bossPosition.x, 4, bossPosition.z);
        if (explosion != null)
        {
            explosion.gameObject.SetActive(true);
            explosion.Play();
        }
        yield return new WaitForSecondsRealtime(2f);

        if (!winGame) yield break;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0f;
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            winCanvas.DOFade(1, 1f).SetUpdate(true);
        }

        StartCoroutine(FadeOutBgm());
    }

    private IEnumerator KillPlayer()
    {
        loseGame = true;
        Vector3 playerPos = player.transform.position;
        playerExplosion.transform.position = playerPos;
        if (playerExplosion != null)
        {
            playerExplosion.gameObject.SetActive(true);
            playerExplosion.Play();
        }
        playerInput.SwitchCurrentActionMap("UI");

        yield return new WaitForSeconds(.5f);

        SoundManager.Instance.PlaySFX(deathSound);


        dieScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0f;

    }

    private void PlayBgm(AudioClip bgmClip)
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.volume = .5f; // Reset volume to full before playing
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    private IEnumerator FadeOutBgm()
    {
        if (bgmSource == null) yield break;

        float startVolume = bgmSource.volume;

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / 1);
            yield return null;
        }

        bgmSource.volume = 0;
        bgmSource.Stop();
    }

    private void TogglePause()
    {
        pauseGame = !pauseGame; // Toggle pause state

        if (pauseGame)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");

        }
        else
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");

        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        dieScreen.SetActive(false);
        LevelManager.Instance.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ToMenu()
    {
        pauseScreen.SetActive(false);
        dieScreen.SetActive(false);

        Time.timeScale = 1f;
        LevelManager.Instance.LoadScene("MainMenu");

    }

    public void RestartKill()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        dieScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuKill()
    {
        pauseScreen.SetActive(false);
        dieScreen.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseGame = false;
        pauseScreen.SetActive(false);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void OpenHelp()
    {
        helpScreen.gameObject.SetActive(true);
    }

    public void CloseHelp()
    {
        helpScreen.gameObject.SetActive(false);

    }


}


