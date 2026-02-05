
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manager to apply level based data to the game state before the game loop begins
/// Might contain a list of difficulties, levels, etc.
/// </summary>
public class LevelMgr : Singleton<LevelMgr>
{
    [SerializeField] private string[] _levelSceneNames;

    private int _currentLevelIndex;
    public bool IsLevelLoaded { get; private set; }

    public void LoadCurrentLevel()
    {
        IsLevelLoaded = false;
        StartCoroutine(LoadLevelRoutine());
    }

    private IEnumerator LoadLevelRoutine()
    {
        string levelName = _levelSceneNames[_currentLevelIndex];

        Debug.Log($"LevelMgr: Loading {levelName} additively");

        var asyncOperation =
            SceneManager.LoadSceneAsync(
                levelName, LoadSceneMode.Additive);

        while (asyncOperation is {isDone: false}) yield return null;

        Debug.Log("LevelMgr: Level loaded");

        IsLevelLoaded = true;
    }
}