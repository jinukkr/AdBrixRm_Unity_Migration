using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.iOS.Xcode.Extensions;

namespace AdBrixRm_iOS {
    public static class PostProcessor {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) {
            if(buildTarget == BuildTarget.iOS) {
                // So PBXProject.GetPBXProjectPath returns wrong path, we need to construct path by ourselves instead
                // var projPath = PBXProject.GetPBXProjectPath(buildPath);
                var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var proj = new PBXProject();
                proj.ReadFromFile(projPath);

                //var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                var targetGuid = proj.GetUnityMainTargetGuid();
                

                //// Configure build settings
                proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
				proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/AdBrixRm_iOS/AdBrixRm-Bridging-Header.h");
				proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "AdBrixRm-Swift.h");
                proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

                proj.WriteToFile(projPath);

				#if UNITY_EDITOR_OSX
				//EmbedFrameworks

				proj.ReadFromString(File.ReadAllText(projPath));
			
				const string defaultLocationInProj = "Plugins/iOS";
				const string coreFrameworkName = "AdBrixRM.framework";
				string framework = Path.Combine(defaultLocationInProj, coreFrameworkName);
				string fileGuid = proj.AddFile(framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
				PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid, fileGuid);
				proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
				proj.WriteToFile (projPath);
				//EmbedFrameworks end

				#endif
            }
		}
    }
}
