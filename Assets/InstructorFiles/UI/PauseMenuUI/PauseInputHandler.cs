using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInputHandler : MonoBehaviour
{
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        GameMgr.Instance.PauseGameToggle();
    }
}