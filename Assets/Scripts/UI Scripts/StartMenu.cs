using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class StartMenu : MonoBehaviour
{
    List<IndexScene> ListCompletedScene = new List<IndexScene>();
    private string path;

    public GameObject butNewGame;
    public GameObject butContinueGame;

    public GameObject canvLevels;
    public GameObject canImage1;
    public GameObject canImage2;
    public GameObject mainCanv;
    public GameObject optionsCanv;
    public GameObject InfoCanv;

    public GameObject soundClick;
    AudioSource Click;
    int indexForContinue;

   

    private void Start()
    {
        Click = soundClick.GetComponent<AudioSource>();
#if       UNITY_ANDROID && !UNITY_EDITOR
       path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        path = Path.Combine(Application.dataPath, "level_completed");
#endif
        BinaryFormatter binformat = new BinaryFormatter();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 0)
        {
            Destroy(objs[0]);
        }
        try
        {
            using (FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                ListCompletedScene = (List<IndexScene>)binformat.Deserialize(fstream);
            }
        }
        catch
        {
            Debug.Log("Пройденный уровней нет!!");
        }
        foreach (IndexScene b in ListCompletedScene)
        {
            if (b.Index >= 1)
            {
                butNewGame.gameObject.SetActive(false);
                butContinueGame.gameObject.SetActive(true);
                indexForContinue = b.Index;
            }
            
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Level_1");
        GetComponentInChildren<AudioSource>().Play();
        return;
    }

    public void Continue()
    {
        Click.Play();
        Application.LoadLevel(indexForContinue+1);
        
    }

    public void Options()
    {
        Click.Play();
        optionsCanv.gameObject.SetActive(true);
        canImage1.gameObject.SetActive(false);
        canImage2.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
        return;
    }

    public void LevelScene()
    {
        Click.Play();
        
         canvLevels.gameObject.SetActive(true);
         canImage1.gameObject.SetActive(false);
         canImage2.gameObject.SetActive(false);
         this.gameObject.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        Click.Play();

        canImage1.gameObject.SetActive(true);
        canImage2.gameObject.SetActive(true);
        mainCanv.gameObject.SetActive(true);
        gameObject.SetActive(false);
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowUpInfo()
    {
        InfoCanv.gameObject.SetActive(true);
        Click.Play();
    }

    public void ShowDownInfo()
    {
        InfoCanv.gameObject.SetActive(false);
        Click.Play();
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_1");
        Click.Play();
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_2");
        Click.Play();
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level_3");
        Click.Play();
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level_4");
        Click.Play();
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("Level_5");
        Click.Play();
    }

    public void LoadLevel6()
    {
        SceneManager.LoadScene("Level_6");
        Click.Play();
    }

    public void LoadLevel7()
    {
        SceneManager.LoadScene("Level_7");
        Click.Play();
    }

    public void LoadLevel8()
    {
        SceneManager.LoadScene("Level_8");
        Click.Play();
    }

    public void LoadLevel9()
    {
        SceneManager.LoadScene("Level_9");
        Click.Play();
    }

    public void LoadLevel10()
    {
        SceneManager.LoadScene("Level_10");
        Click.Play();
    }

    public void LoadLevel11()
    {
        SceneManager.LoadScene("Level_11");
        Click.Play();
    }

    public void LoadLevel12()
    {
        SceneManager.LoadScene("Level_12");
        Click.Play();
    }

    public void LoadLevel13()
    {
        SceneManager.LoadScene("Level_13");
        Click.Play();
    }

    public void LoadLevel14()
    {
        SceneManager.LoadScene("Level_14");
        Click.Play();
    }

    public void LoadLevel15()
    {
        SceneManager.LoadScene("Level_15");
        Click.Play();
    }
}
