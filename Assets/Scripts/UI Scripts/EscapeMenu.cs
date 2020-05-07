using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EscapeMenu : MonoBehaviour
{
    bool GameIsPaused = false;
    public GameObject pauseMenuObject;
    public Button ButtonUp;
    public Button ButtonLeft;
    public Button ButtonRight;
    public Button ButtonPause;

    public GameObject hintCanv;

    public PlayerController Активує
    {
        get => default;
        set
        {
        }
    }

    public StartMenu Повертає_да
    {
        get => default;
        set
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
     
          /*  if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }*/
        
    }
   /* void Resume()
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }*/

    public void Pause()
    {
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        GetComponent<AudioSource>().Play();
        ButtonUp.GetComponent<Button>().enabled = false;
        ButtonLeft.GetComponent<Button>().enabled = false;
        ButtonRight.GetComponent<Button>().enabled = false;
        ButtonPause.GetComponent<Button>().enabled = false;

        this.GetComponentInParent<AudioSource>().Play();
        ButtonUp.GetComponent<EventTrigger>().enabled = false;
        ButtonLeft.GetComponent<EventTrigger>().enabled = false;
        ButtonRight.GetComponent<EventTrigger>().enabled = false;
    }

    public void Continue()
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        this.GetComponentInParent<AudioSource>().Play();
        ButtonUp.GetComponent<Button>().enabled = true;
        ButtonLeft.GetComponent<Button>().enabled = true;
        ButtonRight.GetComponent<Button>().enabled = true;
        ButtonPause.GetComponent<Button>().enabled = true;


        ButtonUp.GetComponent<EventTrigger>().enabled = true;
        ButtonLeft.GetComponent<EventTrigger>().enabled = true;
        ButtonRight.GetComponent<EventTrigger>().enabled = true;
    }

    public void Menu()
    {
        this.GetComponentInParent<AudioSource>().Play();
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene("SceneMenu");
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void HintQuestionButton()
    {
        hintCanv.gameObject.SetActive(true);
    }

    public void HintExitButton()
    {
        hintCanv.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

  
}
