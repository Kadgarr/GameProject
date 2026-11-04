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

        /// <summary>Игрок погиб на уровне. Аргументы — номер уровня и причина смерти.</summary>
        public static event Action<int, string> PlayerDied;

        /// <summary>Уровень пройден.</summary>
        public static event Action<int> LevelCompleted;

        /// <summary>Игрок вышел с уровня в меню.</summary>
        public static event Action<int> LevelQuit;

        /// <summary>Попытка решить загадку: номер уровня и успех.</summary>
        public static event Action<int, bool> RiddleAttempted;

        /// <summary>Подобран предмет: номер уровня и id предмета.</summary>
        public static event Action<int, string> ItemCollected;

        /// <summary>Открыта подсказка уровня.</summary>
        public static event Action<int> HintOpened;

        private static LevelFlow _current;

        /// <summary>Номер текущего уровня (0 — не уровень).</summary>
        public static int CurrentLevel => _current != null ? _current.LevelNumber : 0;

        public int LevelNumber { get; private set; }

        private void Awake()
        {
            // Уровень всегда начинается без паузы (например, после выхода в меню с паузы).
            GamePause.Set(false);
            LevelNumber = levels != null ? levels.GetLevelNumber(gameObject.scene.name) : 0;
            _current = this;
        }

        private void OnDestroy()
        {
            if (_current == this)
            {
                _current = null;
            }
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
            if (!_transitioning)
            {
                LevelQuit?.Invoke(LevelNumber);
            }

            LoadScene(levels.MenuScene, 0f);
        }

        /// <summary>Сообщить о попытке решить загадку на текущем уровне.</summary>
        public static void ReportRiddleAttempt(bool success)
        {
            RiddleAttempted?.Invoke(CurrentLevel, success);
        }

        /// <summary>Сообщить о подобранном предмете на текущем уровне.</summary>
        public static void ReportItemCollected(string itemId)
        {
            ItemCollected?.Invoke(CurrentLevel, itemId);
        }

        /// <summary>Сообщить, что игрок открыл подсказку.</summary>
        public static void ReportHintOpened()
        {
            HintOpened?.Invoke(CurrentLevel);
        }

        private void OnPlayerDied(float restartDelay)
        {
            PlayerDied?.Invoke(LevelNumber, player != null ? player.Cause : DeadPlayer.UnknownCause);
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
            LevelQuit = null;
            RiddleAttempted = null;
            ItemCollected = null;
            HintOpened = null;
            _current = null;
        }
    }
}
