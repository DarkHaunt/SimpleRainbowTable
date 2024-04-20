namespace RainbowTableGenerator
{
    /// <summary>
    /// Using SHA256 algorithm. You can use this source to encrypt readable words into hash
    /// https://emn178.github.io/online-tools/sha256.html
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var table = new TableHandler();
            table.GenerateRandomTable();
            
            Utils.WriteColor("///--- Table Generated ---/// \n", ConsoleColor.Magenta);

            foreach (var el in table.GetRawTable())
                Utils.WriteColor($"| - {el} - |", ConsoleColor.DarkYellow);
            
            Utils.WriteColor("Enter hash to crack - ", ConsoleColor.White);
            
            var hashToCrack = Console.ReadLine();
            
            if (table.TryToGetPassword(hashToCrack, out var password))
                Utils.WriteColor($"Cracked hash: {hashToCrack}, Password: {password}", ConsoleColor.Green);
            else
                Utils.WriteColor("Hash not found in rainbow table.", ConsoleColor.DarkRed);
        }
    }
}