using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    private AsyncOperation preloadOp;

    void Start()
    {
        StartCoroutine(PreloadScene(1));
    }

    IEnumerator PreloadScene(int sceneIndex)
    {
        preloadOp = SceneManager.LoadSceneAsync(sceneIndex);
        preloadOp.allowSceneActivation = false;

        while (preloadOp.progress < 0.9f)
        {
            Debug.Log("Loading: " + (preloadOp.progress * 100f) + "%");
            yield return null;
        }

        Debug.Log("Scene ready. Waiting for player to start...");
    }

    public void StartGame()
    {
        if (preloadOp != null && preloadOp.progress >= 0.9f)
        {
            Debug.Log("Activating preloaded scene...");
            preloadOp.allowSceneActivation = true;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
