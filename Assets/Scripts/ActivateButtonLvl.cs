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

    // Start is called before the first frame update
    void Start()
    {

        List<int> ListCompletedScene = new List<int>();

        BinaryFormatter binformat = new BinaryFormatter();

        try
        {
            using (FileStream fstream = new FileStream("level_completed", FileMode.Open, FileAccess.Read))
            {
                ListCompletedScene = (List<int>)binformat.Deserialize(fstream);
            }
        }
        catch
        {
            
        }

        foreach ( int x in ListCompletedScene)
        {
      
            switch (x)
            {
                case 1: button1.GetComponent<Button>().interactable = true; break;
                case 2: button1.GetComponent<Button>().interactable = true; break;
                case 3: button1.GetComponent<Button>().interactable = true; break;
                case 4: button1.GetComponent<Button>().interactable = true; break;
                case 5: button1.GetComponent<Button>().interactable = true; break;
                case 6: button1.GetComponent<Button>().interactable = true; break;
                case 7: button1.GetComponent<Button>().interactable = true; break;
            }
        }

    }
    
}
