using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
public class JenkinsSupport : MonoBehaviour
{
    [MenuItem("Build/BuildAndroid")]
    public static void BuildPlayer()
    {
        var arg = System.Environment.GetCommandLineArgs();
        bool isDev = bool.Parse(arg[arg.Length - 1]);
        string version = arg[arg.Length - 2];
        PlayerSettings.bundleVersion = version;
        Debug.Log(arg);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = @"C:\Users\wuzhj\source\repos\TestUnity\ExportApk\Test.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        if (isDev)  buildPlayerOptions.options |= BuildOptions.Development;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        Debug.Log("this is a branch test build");
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }
        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }


}
