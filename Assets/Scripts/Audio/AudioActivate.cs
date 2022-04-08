using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Audio;

public class AudioActivate : MonoBehaviour
{
    // Start is called before the first frame update
    private string path;

    //private string On_Off= "Music = true\nSound = true";

    //private string textFile;

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
        OptionMusSound options = new OptionMusSound();
        BinaryFormatter binformat = new BinaryFormatter();
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "options.json");
#else
        path = Path.Combine(Application.dataPath, "options.json");
#endif
        if (!File.Exists(path))
        {
            options.musicOp = true;
            options.musicOp = true;
           string soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);
        }  

        butSwitch = Button.GetComponent<Image>();
        butSwitch2 = Button2.GetComponent<Image>();
        background = audioBackSource.GetComponent<AudioSource>();
        Click = audioClick.GetComponent<AudioSource>();

        if (File.Exists(path))
        {
           soundOnOff= File.ReadAllText(path);
            options =JsonUtility.FromJson<OptionMusSound>(soundOnOff);


            if (options.musicOp==false)
            {
                butSwitch.sprite = MelodyOff;
                background.Stop();
            }
                
            if (options.musicOp == true)
            {
                
                butSwitch.sprite = MelodyOn;
                
            }

            
            //soundOnOff = "";

            //using (StreamReader str = new StreamReader(path))
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
            //        textFile = str.ReadLine();
            //    }
            //}

            //soundOnOff = textFile.Substring(8);


            if (options.soundOp == false)
            {
                butSwitch2.sprite = DynamicOff;
            }

            if (options.soundOp == true)
            {
                butSwitch2.sprite = DynamicOn;
            }

            //textFile = "";
            //soundOnOff = "";

            //using (StreamReader str = new StreamReader(path))
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
            //        textFile = str.ReadLine();
            //    }
            //}

            //textFile = "";
            //soundOnOff = "";
        }

        
    }

    public void ClickOnOfAudio()
    {
        //OptionMusSound options = new OptionMusSound();
        Click.Play();
        soundOnOff = File.ReadAllText(path);
        var options = JsonUtility.FromJson<OptionMusSound>(soundOnOff);
        
        //print(textFile);
        //soundOnOff = textFile.Substring(8);
        


        //ПОЧЕМУ ТО НА 2 СИМВОЛА ПОКАЗЫВАЕТ БОЛЬШЕ ЧЕМ НУЖНО

        if (options.musicOp==false)
        {
            //string[] readText = File.ReadAllLines(path);

            //readText[0] = "Music = true";
            options.musicOp = true;
            soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);
            butSwitch.sprite = MelodyOn;
            background.Play();
        }
        else
        if (options.musicOp == true)
        {
            //string[] readText = File.ReadAllLines(path);

            //readText[0] = "Music = false";

            options.musicOp = false;
            soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);
            butSwitch.sprite = MelodyOff;
            background.Stop();
        }
        soundOnOff = "";
    }

    public void ClickSound()
    {
        
        Click.Play();
        soundOnOff = File.ReadAllText(path);
        var options = JsonUtility.FromJson<OptionMusSound>(soundOnOff);
        //using (StreamReader str = new StreamReader(path))
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        textFile = str.ReadLine();
        //    }
        //}

        //soundOnOff = textFile.Substring(8);


        //ПОЧЕМУ ТО НА 2 СИМВОЛА ПОКАЗЫВАЕТ БОЛЬШЕ ЧЕМ НУЖНО

        if (options.soundOp == false)
        {
            //string[] readText = File.ReadAllLines(path);

            //readText[1] = "Sound = true";

            options.soundOp = true;
            soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);
            Click.enabled = true;
            Click.Play();
            butSwitch2.sprite = DynamicOn;

        }

        else

        if (options.soundOp==true)
        {

            //string[] readText = File.ReadAllLines(path);

            //readText[1] = "Sound = false";

            options.soundOp = false;
            soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);
            Click.enabled = false;
            butSwitch2.sprite = DynamicOff;

        }

        soundOnOff = "";

    }
}