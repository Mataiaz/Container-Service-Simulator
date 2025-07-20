using UnityEngine;

public partial class ScannerScript : MonoBehaviour
{
  private AudioSource audioSource;
  public AudioClip[] clips;

  public void PlaySound(int soundIndex)
  {
    if (!scannerMesh.activeSelf) return;
  
    if (audioSource.isPlaying)
    {
      audioSource.Stop();
    }
    audioSource.clip = clips[soundIndex];
    audioSource.Play();
  }
}