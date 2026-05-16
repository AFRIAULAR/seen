#if UNITY_EDITOR && UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
public class IOSBuild
{
	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			{
				string plistPath = pathToBuiltProject + "/Info.plist";
				PlistDocument plist = new PlistDocument();
				plist.ReadFromString(File.ReadAllText(plistPath));
				PlistElementDict rootDict = plist.root;
				rootDict.SetBoolean("UIViewControllerBasedStatusBarAppearance", false);
				File.WriteAllText(plistPath, plist.WriteToString());
			}
			{
				string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
				PBXProject pbxProject = new PBXProject();
				pbxProject.ReadFromFile(projectPath);
				string target = pbxProject.GetUnityMainTargetGuid();
				pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
				target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
				pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
				target = pbxProject.GetUnityFrameworkTargetGuid();
				pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
				pbxProject.WriteToFile(projectPath);
			}
		}
	}
}
#endif