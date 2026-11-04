using System.Collections.Generic;
using DarknessInside.Gameplay.Hazards;
using DarknessInside.Gameplay.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DarknessInside.Editor.Physics
{
    /// <summary>
    /// Раскладывает объекты уровней по физическим слоям по их компонентам и тегам
    /// и настраивает матрицу столкновений. Запускать после добавления новых объектов в уровни:
    /// меню Darkness Inside → Physics.
    /// Правила: PlayerMotor → Player; ловушка (Hazard) → Hazard; твёрдый коллайдер с PlatformMove → Platform,
    /// с тегом Ground → Ground, остальные твёрдые → Wall; только триггеры → Trigger. Слой UI не трогаем.
    /// </summary>
    public static class PhysicsLayerAssigner
    {
        public const string Player = "Player";
        public const string Ground = "Ground";
        public const string Wall = "Wall";
        public const string Platform = "Platform";
        public const string Hazard = "Hazard";
        public const string Trigger = "Trigger";

        private const string GroundTag = "Ground";

        private static readonly string[] Layers = { Player, Ground, Wall, Platform, Hazard, Trigger };

        [MenuItem("Darkness Inside/Physics/Assign Layers In Open Scenes", priority = 200)]
        private static void AssignOpenScenes()
        {
            EnsureLayers();
            int changed = 0;
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    changed += AssignHierarchy(root, true);
                }

                EditorSceneManager.MarkSceneDirty(scene);
            }

            Debug.Log("[Physics] Layers assigned in open scenes, changed objects: " + changed);
        }

        [MenuItem("Darkness Inside/Physics/Assign Layers In All Levels And Prefabs", priority = 201)]
        public static void AssignAll()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            EnsureLayers();
            SetupCollisionMatrix();
            int changed = 0;

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains("/Resources/"))
                {
                    continue;
                }

                GameObject root = PrefabUtility.LoadPrefabContents(path);
                int count = AssignHierarchy(root, false);
                if (count > 0)
                {
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                    changed += count;
                }

                PrefabUtility.UnloadPrefabContents(root);
            }

            foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
            {
                if (!buildScene.enabled)
                {
                    continue;
                }

                var scene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Single);
                int count = 0;
                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    count += AssignHierarchy(root, true);
                }

                if (count > 0)
                {
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                    changed += count;
                }
            }

            Debug.Log("[Physics] Layers assigned in all levels and prefabs, changed objects: " + changed);
        }

        /// <summary>Триггеры сталкиваются только с игроком; остальные пары — как раньше.</summary>
        [MenuItem("Darkness Inside/Physics/Setup Collision Matrix", priority = 202)]
        public static void SetupCollisionMatrix()
        {
            EnsureLayers();
            int trigger = LayerMask.NameToLayer(Trigger);
            int player = LayerMask.NameToLayer(Player);
            for (int layer = 0; layer < 32; layer++)
            {
                if (string.IsNullOrEmpty(LayerMask.LayerToName(layer)))
                {
                    continue;
                }

                Physics2D.IgnoreLayerCollision(trigger, layer, layer != player);
            }

            AssetDatabase.SaveAssets();
        }

        /// <summary>Какой слой должен быть у объекта; null — не трогать.</summary>
        public static string LayerFor(GameObject go)
        {
            if (go.layer == LayerMask.NameToLayer("UI"))
            {
                return null;
            }

            if (go.GetComponent<PlayerMotor>() != null)
            {
                return Player;
            }

            var colliders = go.GetComponents<Collider2D>();
            if (colliders.Length == 0)
            {
                return null;
            }

            if (go.GetComponent<Hazard>() != null)
            {
                return Hazard;
            }

            bool hasSolid = false;
            foreach (Collider2D collider in colliders)
            {
                hasSolid |= !collider.isTrigger;
            }

            if (!hasSolid)
            {
                return Trigger;
            }

            if (go.GetComponent<PlatformMove>() != null)
            {
                return Platform;
            }

            return go.CompareTag(GroundTag) ? Ground : Wall;
        }

        private static int AssignHierarchy(GameObject root, bool recordOverrides)
        {
            int changed = 0;
            foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
            {
                GameObject go = transform.gameObject;
                string layerName = LayerFor(go);
                if (layerName == null)
                {
                    continue;
                }

                int layer = LayerMask.NameToLayer(layerName);
                if (go.layer == layer)
                {
                    continue;
                }

                go.layer = layer;
                if (recordOverrides && PrefabUtility.IsPartOfPrefabInstance(go))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(go);
                }

                changed++;
            }

            return changed;
        }

        /// <summary>Создаёт недостающие слои в Tags and Layers (свободные пользовательские слоты с 8).</summary>
        public static void EnsureLayers()
        {
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            var existing = new HashSet<string>();
            for (int i = 0; i < layers.arraySize; i++)
            {
                existing.Add(layers.GetArrayElementAtIndex(i).stringValue);
            }

            int slot = 8;
            foreach (string name in Layers)
            {
                if (existing.Contains(name))
                {
                    continue;
                }

                while (slot < layers.arraySize && !string.IsNullOrEmpty(layers.GetArrayElementAtIndex(slot).stringValue))
                {
                    slot++;
                }

                if (slot >= layers.arraySize)
                {
                    Debug.LogError("[Physics] No free layer slot for " + name);
                    return;
                }

                layers.GetArrayElementAtIndex(slot).stringValue = name;
                existing.Add(name);
            }

            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
