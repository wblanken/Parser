using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parser
{
    class Program
    {
        static string inputFile;
        static string project;
        static List<string> fileList;

        static void Main(string[] args)
        {
            inputFile = args[0];
            fileList = new List<string>();

            readFile();
            createFileDir();
        }

        private static void readFile()
        {
            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            using (StreamReader list = new StreamReader(fs))
            {
                fs = null;

                project = list.ReadLine();
                processProject();

                while (!list.EndOfStream)
                {
                    string line = list.ReadLine().Trim();

                    if (line.StartsWith("File"))
                    {
                        fileList.Add(line.Substring(4));
                    }
                }
            }
        }

        private static void processProject()
        {
            string[] temp = project.Split('"');

            foreach (string s in temp)
            {
                if (s.Trim().StartsWith("C:\\") || s.Trim().StartsWith("E:\\"))
                {
                    temp = s.Trim().Split('\\');
                    if (temp[temp.Length - 1] != "")
                    {
                        project = temp[temp.Length - 1].Trim();
                    }
                    else
                    {
                        project = temp[temp.Length - 2].Trim();
                    }
                    project = project.Replace("\"", "");
                    break;
                }
            }
        }

        private static void createFileDir()
        {
            string src, dest;
            string root = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            root = root.Remove(0, 8);
            var rootDir = Path.GetDirectoryName(root);
            rootDir = Path.Combine(rootDir, "parserFiles");
            var projectDir = Path.Combine(rootDir, project);

            if (!Directory.Exists(projectDir))
            {
                Directory.CreateDirectory(projectDir);
            }

            // Copy the files
            foreach (string f in fileList)
            {
                src = f;
                dest = dirCat(src, projectDir);

                if (!Directory.Exists(Path.GetDirectoryName(dest)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                
                File.Copy(src, dest);
            }
        }

        private static string dirCat(string src, string projectDir)
        {
            string retval = projectDir;
            string[] srcPath = src.Split('\\');

            for (int i = 0; i < srcPath.Length; i++)
            {
                if (srcPath[i] == project)
                {
                    for (int j = i+1; j < srcPath.Length; j++)
                    {
                        retval = Path.Combine(retval, srcPath[j]);
                    }
                    break;
                }
            }

            return retval;
        }
    }
}
