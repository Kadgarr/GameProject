using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EscapeMenu : MonoBehaviour
{
    public bool GameIsPaused { get; private set; }
    public GameObject pauseMenuObject;
    public Button ButtonUp;
    public Button ButtonLeft;
    public Button ButtonRight;
    public Button ButtonPause;

    public GameObject hintCanv;



    public void Pause()
    {
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayClick();
        SetControlsEnabled(false);
    }

    public void Continue()
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayClick();
        SetControlsEnabled(true);
    }

    private void PlayClick()
    {
        var click = GetComponentInParent<AudioSource>();
        if (click != null)
        {
            click.Play();
        }
    }

    /// <summary>
    /// Включает/выключает обработку нажатий на кнопках управления (Button, EventTrigger, ButtonClick и т.п.).
    /// Пустые ссылки пропускаются — на некоторых уровнях часть кнопок не задана.
    /// </summary>
    private void SetControlsEnabled(bool enabled)
    {
        SetButtonEnabled(ButtonUp, enabled);
        SetButtonEnabled(ButtonLeft, enabled);
        SetButtonEnabled(ButtonRight, enabled);
        SetButtonEnabled(ButtonPause, enabled);
    }

    private static void SetButtonEnabled(Button button, bool enabled)
    {
        if (button == null)
        {
            return;
        }

        foreach (Behaviour behaviour in button.GetComponents<Behaviour>())
        {
            if (behaviour is Selectable || behaviour is IEventSystemHandler)
            {
                behaviour.enabled = enabled;
            }
        }
    }

    public void Menu()
    {
        PlayClick();
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene("SceneMenu");
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void HintQuestionButton()
    {
        if (hintCanv != null)
        {
            hintCanv.SetActive(true);
        }
    }

    public void HintExitButton()
    {
        if (hintCanv != null)
        {
            hintCanv.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

  
}
