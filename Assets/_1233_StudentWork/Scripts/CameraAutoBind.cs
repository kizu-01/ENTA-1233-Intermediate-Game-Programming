using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraAutoBind : MonoBehaviour
{
    public CinemachineCamera followCam;
    public CinemachineCamera altCam;
    public CinemachineCamera altCam2;
    public CinemachineCamera altCam3;

    private void Start()
    {
        StartCoroutine(BindCamerasToPlayer());
    }

    private IEnumerator BindCamerasToPlayer()
    {
        GameObject player = null;

        // Wait until a GameObject with the "Player" tag exists
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null; // wait 1 frame
        }

        // Bind the cameras
        if (followCam != null)
        {
            followCam.Follow = player.transform;
            followCam.LookAt = player.transform;
        }

        if (altCam != null)
        {
            altCam.Follow = player.transform;
            altCam.LookAt = player.transform;
        }

        if (altCam2 != null)
        {
            altCam.Follow = player.transform;
            altCam.LookAt = player.transform;
        }

        if (altCam3 != null)
        {
            altCam.Follow = player.transform;
            altCam.LookAt = player.transform;
        }
    }
}