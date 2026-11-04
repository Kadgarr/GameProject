using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessInside.Core.Levels
{
    /// <summary>
    /// Список уровней игры по порядку: номер уровня = позиция в списке (с 1).
    /// Порядок и переходы задаются здесь, а не арифметикой с build index.
    /// </summary>
    [CreateAssetMenu(menuName = "Darkness Inside/Level Database", fileName = "LevelDatabase")]
    public sealed class LevelDatabase : ScriptableObject
    {
        [Serializable]
        public sealed class LevelEntry
        {
            [Tooltip("Имя сцены уровня (сцена должна быть в Build Settings).")]
            public string sceneName;
        }

        [SerializeField] private string menuScene = "SceneMenu";
        [SerializeField] private string endScene = "EndScene";
        [SerializeField] private List<LevelEntry> levels = new List<LevelEntry>();

        public string MenuScene => menuScene;

        public string EndScene => endScene;

        public int Count => levels.Count;

        /// <summary>Имя сцены уровня по номеру (1..Count) или null.</summary>
        public string GetSceneName(int levelNumber)
        {
            return levelNumber >= 1 && levelNumber <= levels.Count ? levels[levelNumber - 1].sceneName : null;
        }

        /// <summary>Номер уровня по имени сцены, 0 — сцена не уровень.</summary>
        public int GetLevelNumber(string sceneName)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].sceneName == sceneName)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        /// <summary>Сцена после уровня: следующий уровень или финальная сцена после последнего.</summary>
        public string GetNextScene(int levelNumber)
        {
            return levelNumber >= 0 && levelNumber < levels.Count ? levels[levelNumber].sceneName : endScene;
        }
    }
}
