using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
public class NextLevelScript : MonoBehaviour
{
    private string path;
    List<IndexScene> ListCompletedScene = new List<IndexScene>();

 

    //public int IndexOfLevel;

    public void OnTriggerEnter2D(Collider2D Other)
    {  
        
        if (Other.tag == "Player")
        {
            // добавляем в массив текущий индекс сцены
           
            ListCompletedScene.Add(new IndexScene(SceneManager.GetActiveScene().buildIndex));

           
        }
        // SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
#if UNITY_ANDROID && !UNITY_EDITOR
            path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        path = Path.Combine(Application.dataPath, "level_completed");

#endif      //сохраняем текущий индекс сцены
        BinaryFormatter binformat = new BinaryFormatter();

        if (File.Exists(path))
        {
            using (FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                binformat.Serialize(fstream, ListCompletedScene);
            }
        }
        else
        {
            using (FileStream fstream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                binformat.Serialize(fstream, ListCompletedScene);
            }
        }
       

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}


[System.Serializable]
public class IndexScene
{
   public int Index;
    public IndexScene(int Index)
    {
        this.Index = Index;
    }
    
}
