using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera gameplayCam;
    public CinemachineCamera altCam;

    private void Start()
    {
        // Make sure only gameplay cam is active at start
        gameplayCam.Priority = 10;
        altCam.Priority = 0;
    }

    public void SwitchToAltCam()
    {
        altCam.Priority = 11;       // Higher priority becomes active
        gameplayCam.Priority = 5;
    }

    public void SwitchToGameplayCam()
    {
        gameplayCam.Priority = 11;
        altCam.Priority = 5;
    }
}
