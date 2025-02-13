using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    private CanvasGroup loadingCanvasGroup;

    private void Awake()
    {
        loadingCanvasGroup = loadingCanvas.GetComponent<CanvasGroup>();
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene (string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingCanvasGroup.alpha = 0f;
        loadingCanvas.SetActive(true);

        await loadingCanvasGroup.DOFade(1, 1f).AsyncWaitForCompletion();
       
        while(!scene.isDone)
        {
            await Task.Delay(500);
            scene.allowSceneActivation = true;

        }

        await Task.Delay(100);

        await loadingCanvasGroup.DOFade(0, .5f).AsyncWaitForCompletion();

        loadingCanvas.SetActive(false);

    }
}
/* var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingCanvasGroup.alpha = 0f;
        loadingCanvas.SetActive(true);

        if (loadingCanvasGroup != null)
        {
            await loadingCanvasGroup.DOFade(0, 2f).AsyncWaitForCompletion();
        }

        while (!scene.isDone) 
        {
            await Task.Delay(1000);
          
        } 

        loadingCanvas.SetActive(false);*/