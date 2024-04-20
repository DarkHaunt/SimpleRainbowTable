using System.Security.Cryptography;
using System.Text;

namespace RainbowTableGenerator;

public static class Utils
{
    public static void WriteColor(object message, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static string ComputeSHA256ASCIIHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.ASCII.GetBytes(input));
        var builder = new StringBuilder();

        foreach (var b in bytes)
            builder.Append(b.ToString("x2"));

        return builder.ToString();
    }
}