using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ScannerScript : MonoBehaviour
{
    public GameObject scannerSensor;
    public Renderer sensorRender;
    public float scannerActivePosX = -25f;
    public float scannerDeactivePosX = -70f;
    private bool isScanning = false;
    private bool canProcessScan = true;
    Collider sensorCollider;
    List<GameObject> gameObjectsToScan = new List<GameObject>();
    public TMPro.TMP_Text scannerText;
    private int objectsToScan = 0;
    private int scannedCount = 0;
    private string emissiveColorText = "_EmissiveColor";
    private GameObject scannerMesh;
    public bool hasBeenPickedUp = false;

    void Start()
    {
        sensorCollider = GetComponent<Collider>();
        sensorCollider.enabled = false;
        audioSource = GetComponent<AudioSource>();
        scannerMesh = transform.GetChild(0).gameObject;
    }

    public void ShowScanner(bool show)
    {
        if (!hasBeenPickedUp) return;
        scannerMesh.SetActive(show);
    }

    public void UpdateScanText(bool scannedObject = false, int objectsToScanCount = -1)
    {
        if (scannedObject)
        {
            scannedCount++;
        }

        if (objectsToScanCount != -1)
        {
            objectsToScan = objectsToScanCount;
            scannerText.text = "CCU Scanned\n" + scannedCount + "/" + objectsToScan;
        }
        else
        {
            scannerText.text = "CCU Scanned\n" + scannedCount + "/" + objectsToScan;
        }
    }

    public void SetScanPos(bool scan)
    {
        if (!hasBeenPickedUp) return;

        if (scan && !isScanning)
        {
            gameObject.transform.localEulerAngles = new Vector3(scannerDeactivePosX, -180, 0);
            isScanning = true;
        }
        else if (!scan)
        {
            isScanning = false;
            gameObject.transform.localEulerAngles = new Vector3(scannerActivePosX, -180, 0);
            sensorCollider.enabled = false;
        }

        if (isScanning && canProcessScan)
        {
            canProcessScan = false;
            StartCoroutine(ScanTimer(1f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isScanning)
        {
            gameObjectsToScan.Add(other.gameObject);
        }
    }

    IEnumerator ScanTimer(float time)
    {
        PlaySound(0);
        yield return new WaitForSeconds(time);
        sensorCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        sensorCollider.enabled = false;
        StartCoroutine(ScanResult(2f, gameObjectsToScan));
    }

    IEnumerator ScanResult(float time, List<GameObject> tagsToValidate)
    {
        GameObject unscannedObject = null;
        
        foreach (GameObject unvalidatedObject in gameObjectsToScan)
        {
            if (unvalidatedObject.tag == "CcuUnscanned")
            {
                unscannedObject = unvalidatedObject;
            }
        }

        Color color = Color.red * Mathf.Pow(2, 10);

        tagsToValidate.Clear();

        if (unscannedObject)
        {
            unscannedObject.tag = "CcuScanned";
            UpdateScanText(true);
            PlaySound(1);
            color = Color.green * Mathf.Pow(2, 10);
        }
        else
        {
            PlaySound(2);
        }

        if (isScanning)
        {
            sensorRender.material.SetColor(emissiveColorText, color);
        }

        yield return new WaitForSeconds(time);

        sensorRender.material.SetColor(emissiveColorText, Color.black * Mathf.Pow(2, 10));
        SetScanPos(false);
        canProcessScan = true;
    }
}
