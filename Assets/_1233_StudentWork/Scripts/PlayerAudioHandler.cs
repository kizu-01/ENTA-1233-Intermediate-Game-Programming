using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _footstepSource;

    public void PlayFootstep()
    {
        _footstepSource.Play();
    }
}
