using System.Security.Cryptography;
using System.Text;
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
        var rainbowTable = new Dictionary<string, string>();

        for (int i = 0; i < TableLength; i++)
        {
            string firstWord = GenerateRandomString();
            string hash = Utils.ComputeSHA256ASCIIHash(firstWord);
            
            string word;

            for (int j = 0; j < ChainLength; j++)
            {
                word = ReduceHash(hash, j);
                hash = Utils.ComputeSHA256ASCIIHash(word);

                Utils.WriteColor($"Chain hidden password - {word}", ConsoleColor.Yellow);
            }

            _table.Add(firstWord);
        }
    }

    public bool TryToGetPassword(string hashToCrack, out string password)
    {
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
        string hash = Utils.ComputeSHA256ASCIIHash(chainWord);
        string word;
        
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

    private string ReduceHash(string hash, int index) =>
        hash.Substring(index % (hash.Length - WordLength + 1), WordLength);

    private string GenerateRandomString()
    {
        var chars = Enumerable.Repeat(CharSet, WordLength)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray();

        return new string(chars);
    }
}