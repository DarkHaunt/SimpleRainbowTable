using static RainbowTableGenerator.Indents;

namespace RainbowTableGenerator;

public class TableHandler
{
    private static readonly Random Random = new();

    private readonly List<string> _table = [];

    public IEnumerable<string> GetRawTable() =>
        _table;

    public void GenerateRandomTable()
    {
        for (int i = 0; i < TableLength; i++)
        {
            // Generate first word in row
            string firstWord = GenerateRandomString();
            string hash = Utils.ComputeSHA256ASCIIHash(firstWord);

            // Iterate throw full chain length, reduce hash & repeat
            for (int j = 0; j < ChainLength; j++)
            {
                // Current computed word
                var word = ReduceHash(hash, j);
                hash = Utils.ComputeSHA256ASCIIHash(word);

                // For test that table can find passwords in chain there is print of all "hidden"
                // Prints all chain passwords
                Utils.WriteColor($"Chain hidden password - {word}", ConsoleColor.Yellow);
            }

            _table.Add(firstWord);
        }
    }

    public bool TryToGetPassword(string hashToCrack, out string password)
    {
        // Iterate throw all table until get value or return with failure
        foreach (var chain in _table)
        {
            if (TryFindPasswordInChain(chain, hashToCrack, out password))
                return true;
        }

        password = "None";
        return false;
    }

    private bool TryFindPasswordInChain(string chainWord, string hashToFind, out string password)
    {
        // Recompute hash 
        string hash = Utils.ComputeSHA256ASCIIHash(chainWord);
        string word;
        
        // Iterate throw all chain, recomputing hashes and trying to compare with income hash
        for (int j = 0; j < ChainLength; j++)
        {
            password = ReduceHash(hash, j);
            hash = Utils.ComputeSHA256ASCIIHash(password);

            if (hash == hashToFind)
                return true;
        }
        
        password = "None";
        return false;
    }

    // It's simple algorithm for perform reduction 
    private string ReduceHash(string hash, int index) =>
        hash.Substring(index % (hash.Length - WordLength + 1), WordLength);

    // Generation random word
    private string GenerateRandomString()
    {
        var chars = Enumerable.Repeat(CharSet, WordLength)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray();

        return new string(chars);
    }
}