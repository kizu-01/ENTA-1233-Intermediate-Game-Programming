using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manages the gameplay, start, end, score, etc.
/// Works with the <see cref="GameLoopManager"/>
/// This has the responsibility of managing non-loop related elements,
/// such as score, states, etc. that may need to be available in other scenes
/// </summary>
public class GameMgr : Singleton<GameMgr> 
{
    /*
    public override void Awake() {
        base.Awake();
    }*/
    
    /// <summary>
    /// Are we actively in the gameplay state.
    /// Should the game loop be looping
    /// </summary>
    public bool IsGameRunning { get; private set; }
    
    /// <summary>
    /// Example of GameMgr responsibility.
    /// Score may need to survive the game loop for Game Over screen
    /// </summary>
    public float Score { get; private set; }
    
    /// <summary>
    /// Reset the score, assumes starting at zero
    /// </summary>
    public void ResetScore()
    {
        Score = 0;
    }
    
    /// <summary>
    /// Gain score from a source
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(float value)
    {
        Score += value;
    }

    /// <summary>
    ///  Subtract score, don't allow lower than zero
    /// </summary>
    /// <param name="value"></param>
    public void SubtractScore(float value)
    {
        Score = Mathf.Max(0, Score - value);
    }

    /// <summary>
    /// Begin the game and start the game loop
    /// This should only happen after the game scene is fully loaded
    /// and any-pre loop functionality has resolved
    /// </summary>
    public void StartGame()
    {
        ResetScore();
        IsGameRunning = true;
    }

    /// <summary>
    /// Handle the result of the game ending
    /// </summary>
    public void GameOver()
    {
        IsGameRunning = false;
        SceneMgr.Instance.LoadScene(GameScenes.GameOver, GameMenus.GameOverMenu);
    }

    public void NextLevel()
    {
        throw new NotImplementedException("No next level logic");
    }

    public void LevelComplete()
    {
        IsGameRunning = false;
        // Open Level Complete menu

        SceneMgr.Instance.LoadScene(GameScenes.LevelComplete, GameMenus.LevelCompleteMenu);
    }

    /// <summary>
    /// Toggle the game state
    /// </summary>
    public void PauseGameToggle()
    {
        var player = PlayerMgr.Instance?.PlayerObject;

        if (IsGameRunning)
        {
            // --- PAUSE ---
            IsGameRunning = false;
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player != null)
            {
                var input = player.GetComponent<PlayerInput>();
                if (input != null)
                    input.enabled = false;

                // Freeze player movement without messing up horizontal velocity
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                // Animator stays where it is; no extra triggers
            }

            UIMgr.Instance.HideMenu(GameMenus.InGameUI);
            UIMgr.Instance.ShowMenu(GameMenus.PauseMenu);
        }
        else
        {
            // --- RESUME ---
            IsGameRunning = true;
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (player != null)
            {
                var input = player.GetComponent<PlayerInput>();
                if (input != null)
                    input.enabled = true;

                // Reset vertical velocity only (horizontal movement stays)
                var controller = player.GetComponent<PlayerController>();
                if (controller != null)
                    controller.ResetVerticalVelocity();

                // Make sure Rigidbody doesn't carry any residual rotation/velocity
                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.angularVelocity = Vector3.zero;
                }

                // Reset Animator to Locomotion without triggering Land/Jump
                var animator = player.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Locomotion", 0, 0f); // start at normalizedTime = 0
                }
            }

            // Handle UI only here
            UIMgr.Instance.HideMenu(GameMenus.PauseMenu);
            UIMgr.Instance.ShowMenu(GameMenus.InGameUI);
        }
    }

    public void ResetGameState()
    {
        IsGameRunning = false;
        ResetScore();
        Time.timeScale = 1f;

        var player = PlayerMgr.Instance?.PlayerObject;
        if (player != null)
        {
            var input = player.GetComponent<PlayerInput>();
            if (input != null)
                input.enabled = true;

            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            var animator = player.GetComponent<Animator>();
            if (animator != null)
                animator.Play("Locomotion", 0);

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.ResetVerticalVelocity();
        }

        UIMgr.Instance.CloseAllMenus();
    }
}