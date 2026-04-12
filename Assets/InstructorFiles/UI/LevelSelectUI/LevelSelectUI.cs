using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private GameObject _buttonPrefab;

    private void Start()
    {
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        string[] levels = LevelMgr.Instance.LevelSceneNames;

        for (int i = 0; i < levels.Length; i++)
        {
            int index = i;

            GameObject buttonObj = Instantiate(_buttonPrefab, _buttonContainer);

            // Set button text
            var text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            string key = $"BestScore_Level_{index}";
            float bestScore = PlayerPrefs.GetFloat(key, 0);

            text.text = $"Level {index + 1}\nBest: {Mathf.RoundToInt(bestScore)}";

            // Set button click
            var button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                OnLevelSelected(index);
            });
        }
    }

    private void OnLevelSelected(int levelIndex)
    {
        // Set which level to load
        LevelMgr.Instance.SetCurrentLevel(levelIndex);

        // Load gameplay scene (your system handles the rest)
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }
}