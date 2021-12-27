public class RecursiveFileProcessor
{
    public static void Main(string[] args)
    {
        string path = args[0];
        var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).GroupBy(p => Path.GetExtension(p));

        Console.WriteLine("\nExtGet v0.1 BETA\n");

        foreach (var file in files)
        {
            Console.WriteLine(file.Key + " : " + file.Count());
        }
    }
}