﻿using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProjectBuilder
{
    const string buildPath = "Build/";
    static readonly string[] scenes = { "Assets/Main Scene.unity" };

    [MenuItem("Tools/Build")]
    public static void BuildProject()
    {
        Directory.CreateDirectory(buildPath);

        // do builds
        PerformBuild("macOS", BuildTarget.StandaloneOSX, "UniversityDomination.app");
        PerformBuild("win32", BuildTarget.StandaloneWindows, "win32/UniversityDomination32.exe");
        PerformBuild("win64", BuildTarget.StandaloneWindows64, "win64/UniversityDomination64.exe");
        PerformBuild("android", BuildTarget.Android, "UniversityDomination.apk");
        PerformBuild("linux-universal", BuildTarget.StandaloneLinuxUniversal, "linuxUniversal/UniversityDomination");
        PerformBuild("linux32", BuildTarget.StandaloneLinux, "linux32/UniversityDomination");
        PerformBuild("linux64", BuildTarget.StandaloneLinux64, "linux64/UniversityDomination");
    }

    static void PerformBuild(string kind, BuildTarget target, string name)
    {
        Debug.Log(string.Format("=@= Building {0} =@=", kind));
        BuildPipeline.BuildPlayer(scenes, buildPath + name, target, BuildOptions.None);
        Debug.Log("=@= Build complete! =@=");
    }
}