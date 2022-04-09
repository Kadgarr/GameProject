using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Assets.Scripts.Audio;

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
        path = Path.Combine(Application.persistentDataPath, "options.json");
#else
        path = Path.Combine(Application.dataPath, "options.json");
#endif
       
        if (!File.Exists(path))
        {
            OptionMusSound options = new OptionMusSound();
            options.musicOp = true;
            options.soundOp = true;
            string soundOnOff = JsonUtility.ToJson(options);
            File.WriteAllText(path, soundOnOff);

        }


        if (File.Exists(path))
        {
            soundOnOff = File.ReadAllText(path);
            var options = JsonUtility.FromJson<OptionMusSound>(soundOnOff);
            
            if (options.musicOp == false)
            {
                GetComponent<AudioSource>().Stop();
            }

            if (options.musicOp == true)
            {
                GetComponent<AudioSource>().Play();
            }

        }
        

        if (File.Exists(path))
        {

            soundOnOff = File.ReadAllText(path);
            var options = JsonUtility.FromJson<OptionMusSound>(soundOnOff);

            if (options.soundOp == false)
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

                if (options.soundOp == true)
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

            
        }
        
    }
}
