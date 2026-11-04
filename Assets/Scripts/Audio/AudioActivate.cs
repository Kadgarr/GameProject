using DarknessInside.Core.Save;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Кнопки «музыка» и «звук» в настройках меню. Переключают настройку в SaveService;
/// громкость применяет AudioService, здесь только иконки.
/// </summary>
public class AudioActivate : MonoBehaviour
{
    public GameObject audioClick;

    public GameObject Button;
    public Sprite MelodyOn;
    public Sprite MelodyOff;

    public GameObject Button2;
    public Sprite DynamicOn;
    public Sprite DynamicOff;

    private AudioSource _click;
    private Image _musicIcon;
    private Image _soundIcon;

    private void Awake()
    {
        _click = audioClick != null ? audioClick.GetComponent<AudioSource>() : null;
        _musicIcon = Button != null ? Button.GetComponent<Image>() : null;
        _soundIcon = Button2 != null ? Button2.GetComponent<Image>() : null;
    }

    private void OnEnable()
    {
        SaveService save = SaveService.Instance;
        SetMusicIcon(save.MusicOn);
        SetSoundIcon(save.SoundOn);
        save.MusicChanged += SetMusicIcon;
        save.SoundChanged += SetSoundIcon;
    }

    private void OnDisable()
    {
        SaveService.Instance.MusicChanged -= SetMusicIcon;
        SaveService.Instance.SoundChanged -= SetSoundIcon;
    }

    public void ClickOnOfAudio()
    {
        PlayClick();
        SaveService.Instance.SetMusicOn(!SaveService.Instance.MusicOn);
    }

    public void ClickSound()
    {
        SaveService.Instance.SetSoundOn(!SaveService.Instance.SoundOn);
        PlayClick();
    }

    private void SetMusicIcon(bool on)
    {
        if (_musicIcon != null)
        {
            _musicIcon.sprite = on ? MelodyOn : MelodyOff;
        }
    }

    private void SetSoundIcon(bool on)
    {
        if (_soundIcon != null)
        {
            _soundIcon.sprite = on ? DynamicOn : DynamicOff;
        }
    }

    private void PlayClick()
    {
        if (_click != null && _click.isActiveAndEnabled)
        {
            _click.Play();
        }
    }
}
