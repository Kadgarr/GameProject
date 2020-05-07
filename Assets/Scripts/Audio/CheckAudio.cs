using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CheckAudio : MonoBehaviour
{
    private string path;
    private string soundOnOff;
    private string textFile;

    public bool check1;
    public bool check2;

    public GameObject soundObj_1;
    public GameObject soundObj_2;
    AudioSource sound1;
    AudioSource sound2;

    private string On_Off = "Music = true\nSound = true";

    public AudioActivate Перевіряє
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        sound1 = soundObj_1.GetComponent<AudioSource>();
        try
        {
            sound2 = soundObj_2.GetComponent<AudioSource>();
        }
        catch 
        {

        }
        
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "level_completed");
#else
        path = Path.Combine(Application.dataPath, "options.txt");
#endif

        if (!File.Exists(path))
        {
            using (StreamWriter fstream = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                fstream.WriteLine(On_Off);
            }
        }


        if (File.Exists(path))
        {

            using (StreamReader sr = new StreamReader(path))
            {
                textFile = sr.ReadLine();
            }

            soundOnOff = textFile.Substring(8);


            if (soundOnOff == "false")
            {
                GetComponent<AudioSource>().Stop();
            }

            if (soundOnOff == "true")
            {
                GetComponent<AudioSource>().Play();
            }

            textFile = "";
            soundOnOff = "";
        }
        

        if (File.Exists(path))
        {

                using (StreamReader sr = new StreamReader(path))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        textFile = sr.ReadLine();
                    }
                }

                soundOnOff = textFile.Substring(8);


                if (soundOnOff == "false")
                {
                    sound1.enabled = false;
                    try
                    {
                        sound2.enabled = false;
                    }
                    catch
                    {

                    }
                }

                if (soundOnOff == "true")
                {
                    sound1.enabled = true;
                    
                    try
                    {
                        sound2.enabled = true;
                    }
                    catch
                    {

                    }
                }

                textFile = "";
                soundOnOff = "";
            
        }
        
    }
}
