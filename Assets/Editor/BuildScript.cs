using UnityEditor;
using UnityEngine;
using System;

namespace KoksalBaba.Editor
{
    /// <summary>
    /// Unity Editor build script for command-line builds.
    /// Usage: Unity.exe -executeMethod BuildScript.BuildIOS
    /// </summary>
    public class BuildScript
    {
        private static readonly string[] Scenes = new[]
        {
            "Assets/Scenes/Bootstrap.unity",
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Game.unity",
            "Assets/Scenes/Results.unity"
        };

        [MenuItem("Build/iOS (Debug)")]
        public static void BuildIOSDebug()
        {
            BuildIOS(BuildOptions.Development);
        }

        [MenuItem("Build/iOS (Release)")]
        public static void BuildIOSRelease()
        {
            BuildIOS(BuildOptions.None);
        }

        public static void BuildIOS()
        {
            BuildIOS(BuildOptions.None);
        }

        private static void BuildIOS(BuildOptions options)
        {
            Debug.Log("=== Starting iOS Build ===");

            // Configure iOS build settings
            PlayerSettings.iOS.targetOSVersionString = "14.0";
            PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneOnly;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.iOS.buildNumber = DateTime.Now.ToString("yyyyMMddHHmm");

            // Configure quality settings
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            // Build
            string buildPath = "Build/iOS";
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = buildPath,
                target = BuildTarget.iOS,
                options = options
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded! Size: {report.summary.totalSize} bytes");
                Debug.Log($"Output: {buildPath}");
            }
            else
            {
                Debug.LogError($"Build failed! Result: {report.summary.result}");
                EditorApplication.Exit(1);
            }
        }

        [MenuItem("Build/Android (Debug)")]
        public static void BuildAndroidDebug()
        {
            BuildAndroid(BuildOptions.Development);
        }

        [MenuItem("Build/Android (Release)")]
        public static void BuildAndroidRelease()
        {
            BuildAndroid(BuildOptions.None);
        }

        private static void BuildAndroid(BuildOptions options)
        {
            Debug.Log("=== Starting Android Build ===");

            // Configure Android build settings
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

            // Build
            string buildPath = "Build/Android/KoksalBaba.apk";
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = buildPath,
                target = BuildTarget.Android,
                options = options
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded! Size: {report.summary.totalSize} bytes");
                Debug.Log($"Output: {buildPath}");
            }
            else
            {
                Debug.LogError($"Build failed! Result: {report.summary.result}");
                EditorApplication.Exit(1);
            }
        }
    }
}
