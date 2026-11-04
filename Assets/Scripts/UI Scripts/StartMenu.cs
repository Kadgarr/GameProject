using DarknessInside.Core.Levels;
using DarknessInside.Core.Save;
using DarknessInside.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Главное меню: запуск уровней и команды кнопок. Переключение экранов делегируется MenuNavigator.
/// Прогресс берётся из SaveService, список уровней — из LevelDatabase.
/// Кнопки уровней вызывают LoadLevel(номер) — номер задан в инспекторе.
/// </summary>
public class StartMenu : MonoBehaviour
{
    [SerializeField] private LevelDatabase levels;
    [SerializeField] private MenuNavigator navigator;

    public GameObject butNewGame;
    public GameObject butContinueGame;

    public GameObject soundClick;

    private AudioSource _click;

    private void Awake()
    {
        _click = soundClick != null ? soundClick.GetComponent<AudioSource>() : null;
    }

    private void Start()
    {
        bool hasProgress = SaveService.Instance.HasProgress;
        SetActive(butNewGame, !hasProgress);
        SetActive(butContinueGame, hasProgress);
    }

    public void Play()
    {
        LoadLevel(1);
    }

    public void Continue()
    {
        PlayClick();
        SceneManager.LoadScene(levels.GetNextScene(SaveService.Instance.LastCompletedLevel));
    }

    /// <summary>Запуск уровня по номеру (1..N). Вызывается кнопками меню.</summary>
    public void LoadLevel(int levelNumber)
    {
        PlayClick();
        string scene = levels != null ? levels.GetSceneName(levelNumber) : null;
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogError("StartMenu: no level " + levelNumber + " in LevelDatabase.", this);
            return;
        }

        SceneManager.LoadScene(scene);
    }

    public void Options()
    {
        Open(MenuScreenId.Options);
    }

    public void LevelScene()
    {
        Open(MenuScreenId.Levels);
    }

    public void ExitToMainMenu()
    {
        Open(MenuScreenId.Main);
    }

    public void ShowUpInfo()
    {
        PlayClick();
        if (navigator != null)
        {
            navigator.SetInfoOpen(true);
        }
    }

    public void ShowDownInfo()
    {
        PlayClick();
        if (navigator != null)
        {
            navigator.SetInfoOpen(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Open(MenuScreenId id)
    {
        PlayClick();
        if (navigator != null)
        {
            navigator.Show(id);
        }
    }

    private void PlayClick()
    {
        if (_click != null && _click.isActiveAndEnabled)
        {
            _click.Play();
        }
    }

    private static void SetActive(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }
}
