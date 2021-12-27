public class RecursiveFileProcessor
{
    public static void Main(string[] args)
    {
        if(args.Length == 0)
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
        var files = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories).GroupBy(p => Path.GetExtension(p));

        Console.WriteLine("\nExtGet v0.1 BETA\n");

        foreach (var file in files)
        {
            Console.WriteLine(file.Key + " : " + file.Count());
        }
    }
}