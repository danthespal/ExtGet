namespace ExtGet
{
    public class RecursiveFileProcessor
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("\nExtGet v0.3 BETA\n");
                Console.WriteLine("you should use the program with a path argument.\n");
                Console.WriteLine("eg: ExtGet.exe <path>\n");
            }
            else
            {
                foreach (string path in args)
                {
                    if (File.Exists(path) || Directory.Exists(path))
                    {
                        // this path is a file or directory
                        ProcessFiles(path);
                    }
                    else
                    {
                        Console.WriteLine($"{path} is not a valid file or directory.");
                    }
                }
            }
        }

        public class FileStatisticInfo
        {
            public int Count { get; set; }
            public long TotalSize { get; set; }
        }

        public static void ProcessFiles(string targetDirectory)
        {
            Dictionary<string, FileStatisticInfo> extensions = new();

            // count all extensions in directory and sub directory
            void CalcFilesCount(string targetDirectory)
            {
                IEnumerable<IGrouping<string, string>>? files = Directory.GetFiles(targetDirectory).GroupBy(p => Path.GetExtension(p));

                foreach (IGrouping<string, string>? ex in files)
                {
                    if (extensions.ContainsKey(ex.Key))
                    {
                        extensions[ex.Key].Count += ex.Count();
                        extensions[ex.Key].TotalSize += ex.Sum(filePath => new FileInfo(filePath).Length);
                    }
                    else
                    {
                        extensions[ex.Key] = new FileStatisticInfo
                        {
                            Count = ex.Count(),
                            TotalSize = ex.Sum(filePath => new FileInfo(filePath).Length)
                        };
                    }
                }

                foreach (string? p2 in Directory.GetDirectories(targetDirectory))
                {
                    CalcFilesCount(p2);
                }
            }

            CalcFilesCount(targetDirectory);

            // transfer bytes to more human readable
            static string BytesToString(long byteCount)
            {
                string[] suf = { " B", " KB", " MB", " GB", " TB", " PB", " EB" };

                if (byteCount == 0)
                {
                    return "0" + suf[0];
                }

                long bytes = Math.Abs(byteCount);
                int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                double num = Math.Round(bytes / Math.Pow(1024, place), 1);

                return (Math.Sign(byteCount) * num).ToString() + suf[place];
            }


            // count all the files from directory and sub directory
            int fCount = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories).Length;

            // print out the results
            Console.WriteLine("\nExtGet v0.3 BETA\n");
            foreach (KeyValuePair<string, FileStatisticInfo> items in extensions)
            {
                double percentage = (double)Math.Round((double)(items.Value.Count * 100) / fCount, 2);

                if (items.Key == "")
                {
                    Console.WriteLine($"----------.unknown - {percentage}%");
                    Console.WriteLine($".unknown : {items.Value.Count} files, {BytesToString(items.Value.TotalSize)}");
                    Console.WriteLine("------------------------------\n");
                }
                else
                {
                    Console.WriteLine($"----------{items.Key} - {percentage}%");
                    Console.WriteLine($"{items.Key} : {items.Value.Count} files, {BytesToString(items.Value.TotalSize)}");
                    Console.WriteLine("------------------------------\n");
                }
            }
            Console.WriteLine("Total Files: " + fCount);
        }
    }
}