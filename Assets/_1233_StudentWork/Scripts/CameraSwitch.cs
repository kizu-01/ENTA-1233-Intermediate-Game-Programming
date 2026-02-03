using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera camToActivate;
    public CinemachineCamera camToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camToActivate.Priority = 20;
            camToDeactivate.Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camToActivate.Priority = 10;
            camToDeactivate.Priority = 20;
        }
    }
}