using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
namespace AdBrixRmAOS
{
    public class PostProcess
    {
        private static string lib;
        private static bool hasPlayServiceLib = false;
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {

            //string sourceFolder = Path.Combine(Application.dataPath, "Plugins/Android");
          
            string[] dirs = Directory.GetDirectories(Application.dataPath);

            foreach (string dir in dirs){
            
                SearchPlayService(Application.dataPath, dir);
            }

        }
        public static void SearchPlayService(string path, string dir)
        {
            string sourceFolder = Path.Combine(Application.dataPath, dir);
            string[] dirs = Directory.GetDirectories(sourceFolder);
            if (dirs != null)
            {
                foreach (string subDir in dirs)
                {
                    SearchPlayService(sourceFolder, subDir);
                }
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);
                if (Path.GetExtension(filename) != ".meta")
                {
                    if (filename.StartsWith("play-services-base"))
                    {
                        if (hasPlayServiceLib) Debug.LogError("AdbrixRm::Duplicated libraries!! : " + lib + ", " + filename);
                        else
                        {
                            lib = filename;
                            hasPlayServiceLib = true;
                        }

                    }
                }

            }



        }
    }
}