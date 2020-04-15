using System;
using System.Collections.Generic;
using System.IO;

namespace Watermark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set src ande dest Dest Root Folder
            string srcRootFolder, destRootFolder;
            if (args.Length >= 2)
            {
                srcRootFolder = args[0];
                destRootFolder = args[1];
            }
            else
            {
                srcRootFolder = @"C:\Users\suzhong\Google Drive\CACA\咔咔抗疫";
                destRootFolder = @"C:\Users\suzhong\Google Drive\CACA\FightCovid19\WaltermarkedPhotos";
            }

            Console.WriteLine("Searching image files ...");
            var files = GetFilesFrom(srcRootFolder, new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" }, true);

            // Copy directory
            Console.WriteLine("Copying directory structure ...");
            CopyDirectoryStructure(srcRootFolder, destRootFolder);
            Console.WriteLine("Copied directory structure.");

            foreach (var srcFile in files)
            {
                var releventFilePath = srcFile.Substring(srcRootFolder.Length);
                var destFile = destRootFolder + releventFilePath;

                FileInfo destFileInfo = new FileInfo(destFile);
                var destFolder = destFileInfo.DirectoryName;

                var watermarkFileName = destFile.Replace(destFileInfo.Extension, $".waltermark{destFileInfo.Extension}");
                if (!File.Exists(watermarkFileName))
                {
                    string text = "Chinese American Civic Association";
                    WaterMark walterMark = new WaterMark(srcFile, text, destFolder);
                    walterMark.ShortText = "咔咔华人社区";

                    Console.WriteLine($"Adding watermark to file {releventFilePath} ...");
                    var outImage = walterMark.AddTextWalterMark();
                    Console.WriteLine($"Added watermark to file {releventFilePath} . ");
                }
            }

            Console.WriteLine("Done.");
        }

        private static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        private static void CopyDirectoryStructure(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            //// Copy each file into the new directory.
            //foreach (FileInfo fi in source.GetFiles())
            //{
            //    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            //    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            //}

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
