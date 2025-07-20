using UnityEngine;

public class GlobalAudioScript : MonoBehaviour
{
    public AudioClip[] natureClips;
    public AudioClip[] horrorClips;
    public AudioClip[] indoorClips;
    private AudioSource audioSource;
    void Awake()
{
    DontDestroyOnLoad(gameObject.transform.parent);
}
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = natureClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySongByCategory(string category, int clipIndex = -1)
    {
        audioSource.Stop();

        switch (category)
        {
            case "Nature":
                if (clipIndex < 0)
                {
                    audioSource.clip = natureClips[Random.Range(0, (int)natureClips.Length)];
                }
                else {
                    audioSource.clip = natureClips[0];
                }
                break;
            case "Horror":
                if (clipIndex < 0)
                {
                    audioSource.clip = horrorClips[Random.Range(0, (int)horrorClips.Length)];
                }
                else {
                    audioSource.clip = horrorClips[0];
                }
                break;
            case "Indoor":
                if (clipIndex < 0)
                {
                    audioSource.clip = indoorClips[Random.Range(0, (int)indoorClips.Length)];
                }
                else {
                    audioSource.clip = indoorClips[0];
                }
                break;
            default: Debug.LogError("Category " + category + " does not exist"); break;
        }
        
        audioSource.Play();
    }

}
