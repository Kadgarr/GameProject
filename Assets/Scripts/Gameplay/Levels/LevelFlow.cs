using System;
using System.Collections;
using DarknessInside.Core;
using DarknessInside.Core.Levels;
using DarknessInside.Core.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarknessInside.Gameplay.Levels
{
    /// <summary>
    /// Жизненный цикл уровня: старт, смерть игрока → перезапуск, выход → сохранение и следующий уровень.
    /// Единственное место, которое грузит сцены во время игры. Статические события — для систем,
    /// которые живут дольше уровня (аналитика, музыка).
    /// </summary>
    public sealed class LevelFlow : MonoBehaviour
    {
        [SerializeField] private LevelDatabase levels;
        [SerializeField] private DeadPlayer player;

        private bool _transitioning;

        /// <summary>Уровень начался. Аргумент — номер уровня (0 — сцена не из списка уровней).</summary>
        public static event Action<int> LevelStarted;

        /// <summary>Игрок погиб на уровне.</summary>
        public static event Action<int> PlayerDied;

        /// <summary>Уровень пройден.</summary>
        public static event Action<int> LevelCompleted;

        public int LevelNumber { get; private set; }

        private void Awake()
        {
            // Уровень всегда начинается без паузы (например, после выхода в меню с паузы).
            GamePause.Set(false);
            LevelNumber = levels != null ? levels.GetLevelNumber(gameObject.scene.name) : 0;
        }

        private void OnEnable()
        {
            if (player != null)
            {
                player.Died += OnPlayerDied;
            }
        }

        private void OnDisable()
        {
            if (player != null)
            {
                player.Died -= OnPlayerDied;
            }
        }

        private void Start()
        {
            LevelStarted?.Invoke(LevelNumber);
        }

        public void Restart(float delay)
        {
            LoadScene(gameObject.scene.name, delay);
        }

        public void CompleteLevel()
        {
            if (_transitioning)
            {
                return;
            }

            if (LevelNumber > 0)
            {
                SaveService.Instance.MarkLevelCompleted(LevelNumber);
            }

            LevelCompleted?.Invoke(LevelNumber);
            LoadScene(levels.GetNextScene(LevelNumber), 0f);
        }

        public void ExitToMenu()
        {
            LoadScene(levels.MenuScene, 0f);
        }

        private void OnPlayerDied(float restartDelay)
        {
            PlayerDied?.Invoke(LevelNumber);
            Restart(restartDelay);
        }

        private void LoadScene(string sceneName, float delay)
        {
            if (_transitioning)
            {
                return;
            }

            _transitioning = true;
            StartCoroutine(LoadAfter(sceneName, delay));
        }

        private static IEnumerator LoadAfter(string sceneName, float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            SceneManager.LoadScene(sceneName);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            LevelStarted = null;
            PlayerDied = null;
            LevelCompleted = null;
        }
    }
}
