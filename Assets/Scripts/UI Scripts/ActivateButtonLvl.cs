using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class ActivateButtonLvl : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button button10;
    public Button button11;
    public Button button12;
    public Button button13;
    public Button button14;
    public Button button15;

    private string path;

   
    List<IndexScene> ListCompletedScene = new List<IndexScene>();

    void Start()
    {
        
#if     UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        path = Path.Combine(Application.dataPath, "level_completed");
#endif
        BinaryFormatter binformat = new BinaryFormatter();
      
        
            using (FileStream fstream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                ListCompletedScene = (List<IndexScene>)binformat.Deserialize(fstream);
            }
      
       
        
        // извлекается индекс последней пройденной сцены и загружается сцена (+1), на которой игрок остановился в последний раз
        foreach (IndexScene b in ListCompletedScene)
        {
            for(int i = 1; i <= b.Index+1; i++)
            {
                print(b.Index);
                switch (i)
                {
                    case 1: button1.GetComponent<Button>().interactable = true; break;
                    case 2: button2.GetComponent<Button>().interactable = true; break;
                    case 3: button3.GetComponent<Button>().interactable = true; break;
                    case 4: button4.GetComponent<Button>().interactable = true; break;
                    case 5: button5.GetComponent<Button>().interactable = true; break;
                    case 6: button6.GetComponent<Button>().interactable = true; break;
                    case 7: button7.GetComponent<Button>().interactable = true; break;
                    case 8: button8.GetComponent<Button>().interactable = true; break;
                    case 9: button9.GetComponent<Button>().interactable = true; break;
                    case 10: button10.GetComponent<Button>().interactable = true; break;
                    case 11: button11.GetComponent<Button>().interactable = true; break;
                    case 12: button12.GetComponent<Button>().interactable = true; break;
                    case 13: button13.GetComponent<Button>().interactable = true; break;
                    case 14: button14.GetComponent<Button>().interactable = true; break;
                    case 15: button15.GetComponent<Button>().interactable = true; break;
                }
            }
            
        }

    }

   

}
