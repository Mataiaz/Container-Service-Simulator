using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    public Transform[] destinations;
    public string customMessage;
    public string category = null;
    public int doorUses = -1;
    private Transform destination;
    private AudioSource audioSource;
    private string teleported = "true";

    public int loadScene = -1;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        destination = destinations[0];
    }

    void SetDestination()
    {
        teleported = "true";

        if (destination == destinations[0])
        {
            destination = destinations[1];
        }
        else
        {
            destination = destinations[0];
        }
    }

    public string TeleportPlayer(GameObject player)
    {

        if (loadScene > -1 && customMessage != null && customMessage != "")
        {
            if (loadScene == 2)
            {
                if (!GameObject.FindGameObjectWithTag("Scanner").GetComponent<ScannerScript>().hasBeenPickedUp)
                {
                    teleported = customMessage;
                }
                else
                {
                    teleported = "true";
                }
            }

        }

        if (doorUses == 0)
        {
            teleported = "false";
        }
        else
        {
            HandleTeleportion(player);
        }

        return teleported;
    }

    private void HandleTeleportion(GameObject player)
    {
        if (loadScene > -1 && teleported != customMessage)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().LoadScene(loadScene);
        }
        else if (teleported != customMessage)
        {
            Vector3 destinationPos = destination.position;

            if (doorUses > 0)
            {
                doorUses = doorUses - 1;
            }

            SetDestination();

            player.GetComponent<Rigidbody>().MovePosition(destinationPos);

            audioSource.pitch = Random.Range(0.7f, 0.9f);
            audioSource.Play();

            if (category != null & category != "")
            {
                GameObject.FindGameObjectWithTag("GlobalAudio").GetComponent<GlobalAudioScript>().PlaySongByCategory(category, 0);
            }

        }
    }
}
