using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;


public class AudioActivate : MonoBehaviour
{
    // Start is called before the first frame update
    private string path;

    private string On_Off= "Music = true\nSound = true";

    private string textFile;

    private string soundOnOff="";

    public GameObject audioBackSource;

    public GameObject audioClick;
    AudioSource Click;


    public GameObject Button;
    public Sprite MelodyOn;
    public Sprite MelodyOff;
    Image butSwitch;

    public GameObject Button2;
    public Sprite DynamicOn;
    public Sprite DynamicOff;
    Image butSwitch2;

    AudioSource background;

    private void Start()
    {
        BinaryFormatter binformat = new BinaryFormatter();
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        path = Path.Combine(Application.dataPath, "options.txt");
#endif
        if (!File.Exists(path))
        {
            using (FileStream fstream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                binformat.Serialize(fstream, On_Off);
            }
        }  

        butSwitch = Button.GetComponent<Image>();
        butSwitch2 = Button2.GetComponent<Image>();
        background = audioBackSource.GetComponent<AudioSource>();
        Click = audioClick.GetComponent<AudioSource>();

        if (File.Exists(path))
        {
            
            using (StreamReader sr = new StreamReader(path))
            {
                textFile = sr.ReadLine(); 
            }
           
            soundOnOff = textFile.Substring(8);
            

            if (soundOnOff == "false")
            {
                butSwitch.sprite = MelodyOff;
                background.Stop();
            }
                
            if (soundOnOff == "true")
            {
                
                butSwitch.sprite = MelodyOn;
                
            }

            textFile = "";
            soundOnOff = "";

            using (StreamReader str = new StreamReader(path))
            {
                for (int i = 0; i < 2; i++)
                {
                    textFile = str.ReadLine();
                }
            }

            soundOnOff = textFile.Substring(8);


            if (soundOnOff == "false")
            {
                butSwitch2.sprite = DynamicOff;
            }

            if (soundOnOff == "true")
            {
                butSwitch2.sprite = DynamicOn;
            }

            textFile = "";
            soundOnOff = "";

            using (StreamReader str = new StreamReader(path))
            {
                for (int i = 0; i < 2; i++)
                {
                    textFile = str.ReadLine();
                }
            }

            textFile = "";
            soundOnOff = "";
        }

        
    }

    public void ClickOnOfAudio()
    {
        Click.Play();
        using (StreamReader sr = new StreamReader(path))
        {
            textFile = sr.ReadLine(); 
        }
        //print(textFile);
        soundOnOff = textFile.Substring(8);
        


        //ПОЧЕМУ ТО НА 2 СИМВОЛА ПОКАЗЫВАЕТ БОЛЬШЕ ЧЕМ НУЖНО

        if (soundOnOff=="false")
        {
            string[] readText = File.ReadAllLines(path);

            readText[0] = "Music = true";

            File.WriteAllLines(path, readText);

            butSwitch.sprite = MelodyOn;
            background.Play();
        }

        else

        if (soundOnOff == "true")
        {
            string[] readText = File.ReadAllLines(path);

            readText[0] = "Music = false";

            File.WriteAllLines(path, readText);

            butSwitch.sprite = MelodyOff;
            background.Stop();
        }
        textFile = "";
    }

    public void ClickSound()
    {
        Click.Play();
        using (StreamReader str = new StreamReader(path))
        {
            for (int i = 0; i < 2; i++)
            {
                 textFile = str.ReadLine();
            }
        }

        soundOnOff = textFile.Substring(8);
        

        //ПОЧЕМУ ТО НА 2 СИМВОЛА ПОКАЗЫВАЕТ БОЛЬШЕ ЧЕМ НУЖНО

        if (soundOnOff == "false")
        {
            string[] readText = File.ReadAllLines(path);

            readText[1] = "Sound = true";

            File.WriteAllLines(path, readText);
           
            Click.enabled = true;
            Click.Play();
            butSwitch2.sprite = DynamicOn;
        
        }

        else

        if (soundOnOff == "true")
        {

            string[] readText = File.ReadAllLines(path);

            readText[1] = "Sound = false";

            File.WriteAllLines(path, readText);
            
            Click.enabled = false;
            butSwitch2.sprite = DynamicOff;
         
        }
        
        textFile = "";

    }
}