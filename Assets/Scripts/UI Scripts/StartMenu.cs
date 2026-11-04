using DarknessInside.Core.Levels;
using DarknessInside.Core.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Главное меню: переключение экранов и запуск уровней. Прогресс берётся из SaveService,
/// список уровней — из LevelDatabase. Кнопки уровней вызывают LoadLevel(номер) — номер задан в инспекторе.
/// </summary>
public class StartMenu : MonoBehaviour
{
    [SerializeField] private LevelDatabase levels;

    public GameObject butNewGame;
    public GameObject butContinueGame;

    public GameObject canvLevels;
    public GameObject canImage1;
    public GameObject canImage2;
    public GameObject mainCanv;
    public GameObject optionsCanv;
    public GameObject InfoCanv;

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
        PlayClick();
        SetActive(optionsCanv, true);
        SetActive(canImage1, false);
        SetActive(canImage2, false);
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LevelScene()
    {
        PlayClick();
        SetActive(canvLevels, true);
        SetActive(canImage1, false);
        SetActive(canImage2, false);
        gameObject.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        PlayClick();
        SetActive(canImage1, true);
        SetActive(canImage2, true);
        SetActive(mainCanv, true);
        gameObject.SetActive(false);
    }

    public void ShowUpInfo()
    {
        SetActive(InfoCanv, true);
        PlayClick();
    }

    public void ShowDownInfo()
    {
        SetActive(InfoCanv, false);
        PlayClick();
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
