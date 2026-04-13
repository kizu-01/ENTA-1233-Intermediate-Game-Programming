using UnityEngine;

public class EnemyAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource hurtSource;
    [SerializeField] private AudioSource deathSource;

    public void PlayAttack()
    {
        attackSource?.Play();
    }

    public void PlayHurt()
    {
        hurtSource?.Play();
    }

    public void PlayDeath()
    {
        deathSource?.Play();
    }
}