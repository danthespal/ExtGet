namespace ExtGet
{
    public class RecursiveFileProcessor
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("\nExtGet v0.1 BETA\n");
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

            // print out the results
            Console.WriteLine("\nExtGet v0.1 BETA\n");
            foreach (KeyValuePair<string, int> file in extensions)
            {
                if (file.Key == "")
                {
                    Console.WriteLine("---------------" + ".unknown");
                    Console.WriteLine(".unknown : " + file.Value + "\n");
                }
                else
                {
                    Console.WriteLine("---------------" + file.Key);
                    Console.WriteLine(file.Key + " : " + file.Value + " files" + "\n");
                }
            }

            // count all files from input directory and all sub directory
            // print the result
            int fCount = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories).Length;
            Console.WriteLine("Total Files: " + fCount);
        }
    }
}