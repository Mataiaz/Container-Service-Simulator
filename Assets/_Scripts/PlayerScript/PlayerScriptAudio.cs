using System.Collections;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
  public AudioSource movementAudio;
  float clipLength = 0;
  public AudioClip[] soundClips;

  public void InitAudio()
  {
    SetMovementAudioStats();
  }

  void SetMovementAudioStats()
  {
    clipLength = movementAudio.clip.length;

  }
  void PlayMovementAudio(int movementX, int movementY)
  {
    if (!movementAudio.isPlaying && IsGrounded && (movementX != 0 || movementY != 0))
    {
      movementAudio.pitch = Random.Range(1.2f, 1.5f);
      movementAudio.Play();
    }
  }

  void SetMovementAudioClip(GroundType type, bool playOnSwitch)
  {
    if (type == GroundType.Void) return;
    AudioClip selectedClip = soundClips[(int)type];

    if (movementAudio.clip != selectedClip)
    {
      movementAudio.clip = selectedClip;
    }

    if (playOnSwitch)
    {
      movementAudio.Play();
    }

  }

}