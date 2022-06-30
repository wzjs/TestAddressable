using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using UnityEditor.iOS.Xcode;
public class JenkinsSupport : MonoBehaviour
{
    [MenuItem("Build/Build")]
    public static void BuildPlayer()
    {
        Debug.Log("Branch Change");
        Debug.Log("start build player");
        var arg = System.Environment.GetCommandLineArgs();
        bool isDev = bool.Parse(arg[arg.Length - 1]);
        string version = arg[arg.Length - 2];
        string platform = arg[arg.Length - 3];
        string exportPath = arg[arg.Length - 4];
        PlayerSettings.bundleVersion = version;
        Debug.Log(arg);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = exportPath;
        buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
        buildPlayerOptions.options = BuildOptions.None;
        if (isDev) buildPlayerOptions.options |= BuildOptions.Development;
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
