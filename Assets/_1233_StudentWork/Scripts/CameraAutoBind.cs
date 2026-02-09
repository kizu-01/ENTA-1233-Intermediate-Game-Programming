using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraAutoBind : MonoBehaviour
{
    public CinemachineCamera followCam;
    public CinemachineCamera altCam;

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
            Debug.Log("FollowCamera bound to player.");
        }

        if (altCam != null)
        {
            altCam.Follow = player.transform;
            altCam.LookAt = player.transform;
            Debug.Log("AltCamera bound to player.");
        }
    }
}