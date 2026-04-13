using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private GameObject _buttonPrefab;

    /*
    private void Start()
    {
        GenerateButtons();
    }
    */

    private void OnEnable()
    {
        foreach (Transform child in _buttonContainer)
        {
            Destroy(child.gameObject);
        }

        GenerateButtons();
    }

    private void GenerateButtons()
    {
        string[] levels = LevelMgr.Instance.LevelSceneNames;
        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 0);

        for (int i = 0; i < levels.Length; i++)
        {
            int index = i;

            GameObject buttonObj = Instantiate(_buttonPrefab, _buttonContainer);

            // Set button text
            var text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            var button = buttonObj.GetComponent<Button>();

            // Check if level unlocked
            if (i <= highestUnlocked)
            {
                // Unlocked level
                string key = $"BestScore_Level_{index}";
                float bestScore = PlayerPrefs.GetFloat(key, 0);

                text.text = $"Level {index + 1}\nBest: {Mathf.RoundToInt(bestScore)}";
                button.interactable = true; // Make sure the button can be clicked

                button.onClick.AddListener(() =>
                {
                    OnLevelSelected(index);
                });
            }
            else
            {
                // Locked level
                text.text = $"Level {index + 1}\nLocked";
                button.interactable = false; // This greys out the button automatically
            }
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