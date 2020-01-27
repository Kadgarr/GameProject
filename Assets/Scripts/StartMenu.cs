using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level_1");
        return;
    }
    public void ExitGame()
    {
        Application.Quit();
        return;
    }
    public void LevelScene()
    {
        SceneManager.LoadScene("SceneLevelsMenu");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("SceneMenu");
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level_3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level_4");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("Level_5");
    }

    public void LoadLevel6()
    {
        SceneManager.LoadScene("Level_6");
    }

    public void LoadLevel7()
    {
        SceneManager.LoadScene("Level_7");
    }
}
