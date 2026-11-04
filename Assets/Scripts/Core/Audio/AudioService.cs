using DarknessInside.Core.Save;
using UnityEngine;
using UnityEngine.Audio;

namespace DarknessInside.Core.Audio
{
    /// <summary>
    /// Музыка и громкость для всей игры. Музыка играет из одного источника в Core и не прерывается
    /// при смене сцен с той же музыкой. «Музыка/звук вкл/выкл» — громкость групп Music и SFX в AudioMixer,
    /// поэтому источникам звука в сценах ничего знать о настройках не нужно: достаточно вывода в группу SFX.
    /// </summary>
    public sealed class AudioService : MonoBehaviour
    {
        private const float MutedDb = -80f;

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private string musicVolumeParameter = "MusicVolume";
        [SerializeField] private string sfxVolumeParameter = "SfxVolume";
        [SerializeField, Range(-80f, 0f)] private float musicVolumeDb;
        [SerializeField, Range(-80f, 0f)] private float sfxVolumeDb;

        private bool _paused;

        public static AudioService Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            SaveService.Instance.MusicChanged += ApplyMusic;
            SaveService.Instance.SoundChanged += ApplySound;
        }

        private void Start()
        {
            // AudioMixer.SetFloat не срабатывает в Awake, поэтому применяем настройки здесь.
            ApplyMusic(SaveService.Instance.MusicOn);
            ApplySound(SaveService.Instance.SoundOn);
        }

        private void OnDisable()
        {
            SaveService.Instance.MusicChanged -= ApplyMusic;
            SaveService.Instance.SoundChanged -= ApplySound;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>Включить музыку сцены. Если уже играет этот же трек — он продолжается без перезапуска.</summary>
        public void PlayMusic(AudioClip clip, float volume)
        {
            if (clip == null)
            {
                StopMusic();
                return;
            }

            musicSource.volume = volume;
            if (musicSource.clip == clip && (musicSource.isPlaying || _paused))
            {
                return;
            }

            musicSource.clip = clip;
            _paused = false;
            if (SaveService.Instance.MusicOn)
            {
                musicSource.Play();
            }
        }

        public void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
            _paused = false;
        }

        private void ApplyMusic(bool on)
        {
            mixer.SetFloat(musicVolumeParameter, on ? musicVolumeDb : MutedDb);

            // Выключенная музыка ставится на паузу, чтобы не тратить время на декодирование потока.
            if (!on)
            {
                if (musicSource.isPlaying)
                {
                    musicSource.Pause();
                    _paused = true;
                }

                return;
            }

            if (musicSource.clip == null)
            {
                return;
            }

            if (_paused)
            {
                musicSource.UnPause();
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }

            _paused = false;
        }

        private void ApplySound(bool on)
        {
            mixer.SetFloat(sfxVolumeParameter, on ? sfxVolumeDb : MutedDb);
        }
    }
}
