using System.Text;
using Microsoft.Extensions.AI;

namespace BMADness.Tools;

public class FileTools
{
    public static string ReadFile(string path) => File.ReadAllText(path);

    public static string ListDirectory(string path)
    {
        //TODO: there's probably a better way but I'm being lazy and just getting a poc up
        if(!Directory.Exists(path))
            return "no directory";
        
        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var builder = new StringBuilder();
        foreach (var dir in directories)
            builder.Append(dir).Append('\n');
        foreach (var file in  files)
            builder.Append(file).Append('\n');
        return builder.ToString();
    }
    
    public static bool FileExists(string path) => File.Exists(path);

    public static async Task<bool> WriteFile(string path, string content)
    {
        try
        {
            await File.WriteAllTextAsync(path, content, cancellationToken: CancellationToken.None);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static IList<AITool>? GetAllTools()
    {
        return
        [
            AIFunctionFactory.Create(FileTools.ReadFile),
            AIFunctionFactory.Create(FileTools.ListDirectory),
            AIFunctionFactory.Create(FileTools.FileExists),
            AIFunctionFactory.Create(FileTools.WriteFile)
        ];
    }
}