using DarknessInside.Core.Save;
using UnityEngine;

/// <summary>
/// Применяет настройки «музыка/звук» из SaveService к музыке на этом объекте
/// и к источникам звуковых эффектов сцены. Реагирует на изменения через события.
/// Временное решение до AudioService в Core-сцене (шаг 3.4).
/// </summary>
public class CheckAudio : MonoBehaviour
{
    public GameObject soundObj_1;
    public GameObject soundObj_2;

    private AudioSource _music;
    private AudioSource _sfx1;
    private AudioSource _sfx2;

    private void Awake()
    {
        _music = GetComponent<AudioSource>();
        _sfx1 = soundObj_1 != null ? soundObj_1.GetComponent<AudioSource>() : null;
        _sfx2 = soundObj_2 != null ? soundObj_2.GetComponent<AudioSource>() : null;
    }

    private void OnEnable()
    {
        SaveService save = SaveService.Instance;
        ApplyMusic(save.MusicOn);
        ApplySound(save.SoundOn);
        save.MusicChanged += ApplyMusic;
        save.SoundChanged += ApplySound;
    }

    private void OnDisable()
    {
        SaveService.Instance.MusicChanged -= ApplyMusic;
        SaveService.Instance.SoundChanged -= ApplySound;
    }

    private void ApplyMusic(bool on)
    {
        if (_music == null)
        {
            return;
        }

        if (on && !_music.isPlaying)
        {
            _music.Play();
        }
        else if (!on)
        {
            _music.Stop();
        }
    }

    private void ApplySound(bool on)
    {
        if (_sfx1 != null)
        {
            _sfx1.enabled = on;
        }

        if (_sfx2 != null)
        {
            _sfx2.enabled = on;
        }
    }
}
