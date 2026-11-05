using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DarknessInside.Editor.Build
{
    /// <summary>
    /// Сборка WebGL для itch.io. Настройки подобраны под хостинг без своих HTTP-заголовков:
    /// Gzip + Decompression Fallback (браузер распакует сам), кэш данных в браузере.
    /// Игра определяет устройство сама: на компьютере — клавиатура, на телефоне — экранные кнопки.
    /// После сборки активная платформа возвращается на прежнюю (обычно Android).
    /// Готовую папку нужно заархивировать в ZIP и загрузить на itch.io как HTML-игру.
    /// </summary>
    public static class WebGLBuild
    {
        public const string OutputPath = "Builds/WebGL/GrayToBright";

        [MenuItem("Darkness Inside/Build/WebGL (itch.io)", priority = 2)]
        private static void BuildFromMenu()
        {
            Build();
        }

        public static BuildReport Build()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.WebGL, BuildTarget.WebGL))
            {
                Debug.LogError("[Build] WebGL Build Support не установлен (Unity Hub → Installs → Add modules).");
                return null;
            }

            BuildTarget previousTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup previousGroup = BuildPipeline.GetBuildTargetGroup(previousTarget);

            ApplySettings();
            Directory.CreateDirectory(OutputPath);

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                locationPathName = OutputPath,
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            Debug.Log($"[Build] WebGL: {report.summary.result}, {report.summary.totalSize / (1024f * 1024f):0.0} MB, {report.summary.totalTime.TotalSeconds:0} s → {OutputPath}");

            if (previousTarget != BuildTarget.WebGL)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(previousGroup, previousTarget);
            }

            return report;
        }

        private static void ApplySettings()
        {
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.WebGL.dataCaching = true;
            PlayerSettings.WebGL.nameFilesAsHashes = false;
            PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
            PlayerSettings.WebGL.showDiagnostics = false;
            // Свой шаблон: холст заполняет окно iframe, размер окна задаётся на странице itch.io (Embed options).
            PlayerSettings.WebGL.template = "PROJECT:GrayToBright";
            PlayerSettings.defaultWebScreenWidth = 960;
            PlayerSettings.defaultWebScreenHeight = 480;
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.Medium);
            PlayerSettings.stripEngineCode = true;
        }
    }
}
