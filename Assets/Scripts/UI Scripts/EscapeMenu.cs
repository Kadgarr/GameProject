using DarknessInside.Core;
using DarknessInside.Gameplay.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Пауза, выход в меню и подсказка на уровне. На паузе экранное управление выключается
/// через CanvasGroup — без перебора отдельных кнопок.
/// На компьютере Esc закрывает подсказку, а если её нет — ставит или снимает паузу.
/// На телефоне клавиатуру не опрашиваем: компонент выключается, кнопки работают как прежде.
/// </summary>
public class EscapeMenu : MonoBehaviour
{
    private const string MenuScene = "SceneMenu";

    public GameObject pauseMenuObject;
    public GameObject hintCanv;

    [Tooltip("CanvasGroup канваса управления: на паузе его кнопки не нажимаются.")]
    [SerializeField] private CanvasGroup controls;
    [SerializeField] private LevelFlow levelFlow;
    [Tooltip("Звук нажатия. Пусто — AudioSource на этом объекте или выше.")]
    [SerializeField] private AudioSource clickSound;

    public bool GameIsPaused => GamePause.IsPaused;

    private void Awake()
    {
        if (clickSound == null)
        {
            clickSound = GetComponentInParent<AudioSource>();
        }

        // Update нужен только для клавиши Esc; публичные методы работают и у выключенного компонента.
        enabled = !Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        if (hintCanv != null && hintCanv.activeSelf)
        {
            HintExitButton();
        }
        else if (GamePause.IsPaused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        SetActive(pauseMenuObject, true);
        GamePause.Set(true);
        PlayClick();
        SetControlsEnabled(false);
    }

    public void Continue()
    {
        SetActive(pauseMenuObject, false);
        GamePause.Set(false);
        PlayClick();
        SetControlsEnabled(true);
    }

    public void Menu()
    {
        PlayClick();
        GamePause.Set(false);
        if (levelFlow != null)
        {
            levelFlow.ExitToMenu();
        }
        else
        {
            SceneManager.LoadScene(MenuScene);
        }
    }

    public void HintQuestionButton()
    {
        SetActive(hintCanv, true);
        LevelFlow.ReportHintOpened();
    }

    public void HintExitButton()
    {
        SetActive(hintCanv, false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void PlayClick()
    {
        if (clickSound != null && clickSound.isActiveAndEnabled)
        {
            clickSound.Play();
        }
    }

    private void SetControlsEnabled(bool enabled)
    {
        if (controls == null)
        {
            return;
        }

        controls.interactable = enabled;
        controls.blocksRaycasts = enabled;
    }

    private static void SetActive(GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }
}
