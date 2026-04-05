
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

    public string[] LevelSceneNames => _levelSceneNames;

    private int _currentLevelIndex;
    public bool IsLevelLoaded { get; private set; }

    public void SetCurrentLevel(int currentLevelIndex)
    {
        _currentLevelIndex = currentLevelIndex;
    }

    public void LoadCurrentLevel()
    {
        IsLevelLoaded = false;
        StartCoroutine(LoadLevelRoutine());
    }

    public void LoadNextLevel()
    {
        if (_currentLevelIndex >= _levelSceneNames.Length)
        {
            Debug.Log("No more levels");
            return;
        }
        StartCoroutine(LoadNextLevelRoutine());
    }

    public void NextLevel()
    {
        _currentLevelIndex++;
    }

    private IEnumerator LoadNextLevelRoutine()
    {
        // Unload current level
        string currentLevel = _levelSceneNames[_currentLevelIndex - 1];
        yield return SceneManager.UnloadSceneAsync(currentLevel);
        // Load next level
        yield return LoadLevelRoutine();
    }

    private IEnumerator LoadLevelRoutine()
    {
        GameMgr.Instance.ResetScore();

        string levelName = _levelSceneNames[_currentLevelIndex];

        Debug.Log($"LevelMgr: Loading {levelName} additively");

        var asyncOperation =
            SceneManager.LoadSceneAsync(
                levelName, LoadSceneMode.Additive);

        while (asyncOperation is {isDone: false}) yield return null;

        Debug.Log("LevelMgr: Level loaded");

        IsLevelLoaded = true;
    }

    public void ResetLevels()
    {
        _currentLevelIndex = 0;
    }
}