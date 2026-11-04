using UnityEngine;

namespace DarknessInside.Core.Audio
{
    /// <summary>Какая музыка играет в этой сцене. Сам звук воспроизводит AudioService в Core.</summary>
    public sealed class SceneMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;

        private void Start()
        {
            if (AudioService.Instance != null)
            {
                AudioService.Instance.PlayMusic(clip, volume);
            }
            else
            {
                Debug.LogWarning("SceneMusic: AudioService is missing (Core prefab not loaded).", this);
            }
        }
    }
}
