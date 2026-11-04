using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DarknessInside.Editor.Build
{
    /// <summary>
    /// Настройки одного варианта Android-сборки (тестовая / релизная).
    /// Ассеты лежат в Assets/Settings/Build, правятся в инспекторе; там же кнопки «Применить» и «Собрать».
    /// </summary>
    [CreateAssetMenu(fileName = "AndroidBuildConfig", menuName = "Darkness Inside/Android Build Config")]
    public sealed class AndroidBuildConfig : ScriptableObject
    {
        [Header("Сборка")]
        [SerializeField] private bool developmentBuild = true;
        [Tooltip("Сразу подключать Profiler при запуске (только для Development Build).")]
        [SerializeField] private bool connectProfiler = true;
        [Tooltip("AAB для Google Play вместо APK.")]
        [SerializeField] private bool buildAppBundle;
        [Tooltip("Путь относительно папки проекта. Расширение .apk/.aab подставится автоматически.")]
        [SerializeField] private string outputPath = "Builds/Android/DarknessInside_Test";

        [Header("IL2CPP")]
        [SerializeField] private AndroidArchitecture architectures = AndroidArchitecture.ARM64;
        [Tooltip("Debug собирается быстрее, но работает медленнее — для замеров производительности используйте Release.")]
        [SerializeField] private Il2CppCompilerConfiguration compilerConfiguration = Il2CppCompilerConfiguration.Release;
        [Tooltip("OptimizeSize = «Faster (smaller) builds»: быстрее сборка, меньше вес.")]
        [SerializeField] private Il2CppCodeGeneration codeGeneration = Il2CppCodeGeneration.OptimizeSize;
        [Tooltip("Удаление неиспользуемого кода. Medium безопасен: код игры не использует рефлексию и BinaryFormatter.")]
        [SerializeField] private ManagedStrippingLevel managedStrippingLevel = ManagedStrippingLevel.Medium;
        [Tooltip("Не включать в сборку неиспользуемые модули движка.")]
        [SerializeField] private bool stripEngineCode = true;

        [Header("Подпись")]
        [Tooltip("Подписывать своим keystore. Пароли в ассете не хранятся — введите их в Player Settings → Publishing Settings перед сборкой.")]
        [SerializeField] private bool useCustomKeystore;

        private static readonly NamedBuildTarget Target = NamedBuildTarget.Android;

        public string FullOutputPath => Path.ChangeExtension(outputPath, buildAppBundle ? "aab" : "apk");

        /// <summary>Записывает настройки конфига в Player Settings и Build Settings.</summary>
        public void Apply()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }

            PlayerSettings.SetScriptingBackend(Target, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetIl2CppCompilerConfiguration(Target, compilerConfiguration);
            PlayerSettings.SetIl2CppCodeGeneration(Target, codeGeneration);
            PlayerSettings.SetManagedStrippingLevel(Target, managedStrippingLevel);
            PlayerSettings.stripEngineCode = stripEngineCode;
            PlayerSettings.Android.targetArchitectures = architectures;
            PlayerSettings.Android.useCustomKeystore = useCustomKeystore;

            EditorUserBuildSettings.development = developmentBuild;
            EditorUserBuildSettings.connectProfiler = developmentBuild && connectProfiler;
            EditorUserBuildSettings.buildAppBundle = buildAppBundle;

            AssetDatabase.SaveAssets();
            Debug.Log($"[Build] Применён конфиг «{name}».");
        }

        /// <summary>Применяет настройки и собирает игру. Возвращает null, если сборка не запускалась.</summary>
        public BuildReport Build()
        {
            if (useCustomKeystore && !HasKeystorePasswords())
            {
                Debug.LogError($"[Build] «{name}»: не введены пароли keystore (Player Settings → Publishing Settings). Сборка отменена.");
                return null;
            }

            Apply();

            string path = FullOutputPath;
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            BuildOptions options = BuildOptions.None;
            if (developmentBuild)
            {
                options |= BuildOptions.Development;
                if (connectProfiler)
                {
                    options |= BuildOptions.ConnectWithProfiler;
                }
            }

            var buildOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                locationPathName = path,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = options
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;
            string message = $"[Build] «{name}»: {summary.result}, {summary.totalSize / (1024f * 1024f):0.0} МБ, " +
                             $"{summary.totalTime.TotalMinutes:0.0} мин, ошибок {summary.totalErrors} → {path}";

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log(message);
            }
            else
            {
                Debug.LogError(message);
            }

            return report;
        }

        private static bool HasKeystorePasswords()
        {
            return !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass)
                   && !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass);
        }
    }
}
