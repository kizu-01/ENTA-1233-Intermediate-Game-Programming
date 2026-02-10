using UnityEngine;

public class StationaryMover : MonoBehaviour, IMover
{
    public Vector3 Velocity => Vector3.zero;
    public float RemainingDistance => 0f;
    public bool IsAtDestination => true;

    public void SetDestination(Vector3 destination)
    {
        /*Do nothing*/
    }

    public void Stop()
    {
        /*Do nothing*/
    }

    public void Resume()
    {
        /*Do nothing*/
    }

    public void SetEnabled(bool enabled)
    {
        /*Do nothing*/
    }
}
