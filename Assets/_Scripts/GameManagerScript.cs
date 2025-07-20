using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject sun;
    private int objectsToScan = 0;
    private ScannerScript scannerScript;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitScripts();
    }

    public void LoadScene(int scene)
    {
        StartCoroutine(LoadSceneAndInit(scene));
    }

    private IEnumerator LoadSceneAndInit(int scene)
    {
        SceneManager.LoadScene(scene);
        yield return null; // Wait one frame until scene is fully loaded

        InitScripts(); // Safe to call now
    }
    void InitScripts()
    {
        scannerScript = GameObject.FindGameObjectWithTag("Scanner").GetComponent<ScannerScript>();
        objectsToScan = GameObject.FindGameObjectsWithTag("CcuUnscanned").Length;
        scannerScript.UpdateScanText(false, objectsToScan);
    }

    public void SetGlobalLight(bool isOn)
    {
        if (isOn)
        {
            sun.SetActive(true);
        }
        else
        {
            sun.SetActive(false);
        }
    }

}
