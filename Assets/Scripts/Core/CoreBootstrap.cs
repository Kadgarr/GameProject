using UnityEngine;

namespace DarknessInside.Core
{
    /// <summary>
    /// Создаёт общие сервисы игры (префаб Resources/Core) до загрузки первой сцены и сохраняет их
    /// на всю сессию. Работает при запуске с любой сцены, в том числе прямо с уровня в редакторе.
    /// Содержимое Core правится в префабе: Assets/Resources/Core.prefab.
    /// </summary>
    public static class CoreBootstrap
    {
        private const string PrefabPath = "Core";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateCore()
        {
            var prefab = Resources.Load<GameObject>(PrefabPath);
            if (prefab == null)
            {
                Debug.LogError("CoreBootstrap: prefab Resources/" + PrefabPath + " not found.");
                return;
            }

            GameObject core = Object.Instantiate(prefab);
            core.name = prefab.name;
            Object.DontDestroyOnLoad(core);
        }
    }
}
