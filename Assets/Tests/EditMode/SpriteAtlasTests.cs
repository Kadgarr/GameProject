using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace DarknessInside.Tests
{
    /// <summary>
    /// Атласы этапа 6: каждый спрайт уровней и интерфейса упакован в атлас (иначе — лишний draw call),
    /// у атласов выключены mipmaps и для Android задан ASTC.
    /// Полноэкранные фоны (от 1000 px) намеренно без атласа.
    /// </summary>
    public class SpriteAtlasTests
    {
        private const int BackgroundMinSize = 1000;

        private static IEnumerable<string> AtlasPaths()
        {
            foreach (string guid in AssetDatabase.FindAssets("t:SpriteAtlas", new[] { "Assets/Art/Atlases" }))
            {
                yield return AssetDatabase.GUIDToAssetPath(guid);
            }
        }

        [Test]
        public void Atlases_Exist()
        {
            Assert.That(new List<string>(AtlasPaths()), Has.Count.GreaterThanOrEqualTo(2));
        }

        [TestCaseSource(nameof(AtlasPaths))]
        public void Atlas_HasMobileSettings(string path)
        {
            var importer = (SpriteAtlasImporter)AssetImporter.GetAtPath(path);
            Assert.That(importer.includeInBuild, Is.True, "Атлас не попадёт в билд.");
            Assert.That(importer.textureSettings.generateMipMaps, Is.False, "Mipmaps у 2D-атласа — лишняя память.");
            Assert.That(importer.packingSettings.enableRotation, Is.False, "Поворот ломает спрайты в UI Image.");

            TextureImporterPlatformSettings android = importer.GetPlatformSettings("Android");
            Assert.That(android.overridden, Is.True, "Нет настроек для Android.");
            Assert.That(android.format.ToString(), Does.StartWith("ASTC"));
        }

        [Test]
        public void AllGameplayAndUiSprites_ArePacked()
        {
            var atlases = new List<SpriteAtlas>();
            foreach (string path in AtlasPaths())
            {
                atlases.Add(AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path));
            }

            var roots = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    roots.Add(scene.path);
                }
            }

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources" }))
            {
                roots.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            var notPacked = new List<string>();
            foreach (string dependency in AssetDatabase.GetDependencies(roots.ToArray(), true))
            {
                if (!dependency.StartsWith("Assets/") || dependency.StartsWith("Assets/_Unused"))
                {
                    continue;
                }

                if (!(AssetImporter.GetAtPath(dependency) is TextureImporter importer) || importer.textureType != TextureImporterType.Sprite)
                {
                    continue;
                }

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(dependency);
                if (texture == null || texture.width >= BackgroundMinSize || texture.height >= BackgroundMinSize)
                {
                    continue;
                }

                foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(dependency))
                {
                    if (asset is Sprite sprite && !atlases.Exists(a => a.CanBindTo(sprite)))
                    {
                        notPacked.Add(dependency);
                        break;
                    }
                }
            }

            Assert.That(notPacked, Is.Empty, "Спрайты вне атласов (добавьте в Assets/Art/Atlases): " + string.Join(", ", notPacked));
        }
    }
}
