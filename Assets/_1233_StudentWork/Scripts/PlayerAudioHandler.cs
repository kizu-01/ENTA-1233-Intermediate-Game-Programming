using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource landSource;
    [SerializeField] private AudioSource attackSource;

    public void PlayFootstep()
    {
        if (footstepSource != null)
            footstepSource.Play();
    }

    public void PlayJump()   // called by Jump animation event
    {
        if (jumpSource != null)
            jumpSource.Play();
    }

    public void PlayLand()   // called by Land animation event
    {
        if (landSource != null)
            landSource.Play();
    }

    public void PlayAttack()   // called by Attack animation event
    {
        if (attackSource != null)
            attackSource.Play();
    }
}