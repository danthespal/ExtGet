namespace ExtGet
{
    public class RecursiveFileProcessor
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("\nExtGet v0.2 BETA\n");
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

        public static void ProcessFiles(string targetDirectory)
        {
            Dictionary<string, int> extensions = new();

            // count all extensions in directory and sub directory
            void CalcFilesCount(string targetDirectory)
            {
                IEnumerable<IGrouping<string, string>>? files = Directory.GetFiles(targetDirectory).GroupBy(p => Path.GetExtension(p));

                foreach (IGrouping<string, string>? ex in files)
                {
                    if (extensions.ContainsKey(ex.Key))
                    {
                        extensions[ex.Key] += ex.Count();
                    }
                    else
                    {
                        extensions[ex.Key] = ex.Count();
                    }
                }

                foreach (string? p2 in Directory.GetDirectories(targetDirectory))
                {
                    CalcFilesCount(p2);
                }
            }

            CalcFilesCount(targetDirectory);

            // count all the files from directory and sub directory
            int fCount = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories).Length;

            // print out the results
            Console.WriteLine("\nExtGet v0.2 BETA\n");
            foreach (KeyValuePair<string, int> file in extensions)
            {
                double percentage = (double)Math.Round((double)(file.Value * 100) / fCount, 2);

                if (file.Key == "")
                {
                    Console.WriteLine("----------" + ".unknown" + " - " + percentage + "%");
                    Console.WriteLine(".unknown : " + file.Value + " files");
                    Console.WriteLine("------------------------------\n");
                }
                else
                {
                    Console.WriteLine("----------" + file.Key + " - " + percentage + "%");
                    Console.WriteLine(file.Key + " : " + file.Value + " files");
                    Console.WriteLine("------------------------------\n");
                }
            }
            Console.WriteLine("Total Files: " + fCount);
        }
    }
}