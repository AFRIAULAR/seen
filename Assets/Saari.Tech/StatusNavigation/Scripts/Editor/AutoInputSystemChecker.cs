#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace SaariTech
{
	[InitializeOnLoad]
	public static class AutoInputSystemChecker
	{
		static AutoInputSystemChecker()
		{
			CheckAndGenerate();
		}
		private static void CheckAndGenerate()
		{
			if (Application.isPlaying) return;
			string inputClass = "UnityEngine.InputSystem.InputAction";
			string define = "USE_INPUTSYSTEM";
			bool exists = Type.GetType(inputClass) != null;
			if (!exists)
			{
				foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (asm.GetType(inputClass) != null)
					{
						exists = true;
						break;
					}
				}
			}
			PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, out string[] defines);
			bool defined = false;
			for (int i = 0; i < defines.Length; i++)
			{
				if (defines[i] == define)
				{
					defined = true;
					break;
				}
			}
			if (exists && !defined)
			{
				List<string> symbols = new List<string>();
				symbols.AddRange(defines);
				symbols.Add(define);
				PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, symbols.ToArray());
				Debug.Log($"[MSANB] Added: {define}");
			}
			else if (!exists && defined)
			{
				List<string> symbols = new List<string>();
				symbols.AddRange(defines);
				symbols.Remove(define);
				PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, symbols.ToArray());
				Debug.Log($"[MSANB] Removed: {define}");
			}
		}
	}
}
#endif