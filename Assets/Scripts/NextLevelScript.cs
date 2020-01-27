using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
public class NextLevelScript : MonoBehaviour
{
    //public int IndexOfLevel;
    public void OnTriggerEnter2D(Collider2D Other)
    {
        List<int> ListCompletedScene = new List<int>();

        if (Other.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            // добавляем в массив текущий индекс сцены
            ListCompletedScene.Add(SceneManager.GetActiveScene().buildIndex);
        }

        //сохраняем текущий индекс сцены
        
        BinaryFormatter binformat = new BinaryFormatter();
        using (FileStream fstream = new FileStream("level_completed", FileMode.OpenOrCreate, FileAccess.Write))
        {
            binformat.Serialize(fstream, ListCompletedScene);
        }
        
    }
}
