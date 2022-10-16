using System;
using System.Linq;

public class canonicalcoinsystems
{
    static bool?[] cache;
    public static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine());
        cache = new bool?[n];
        cache[0] = true;
        var coins = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
    }

    public static bool IsCanonical(int coinIndex)
    {
        if (cache[coinIndex] != null) return (bool)cache[coinIndex];
        
    }
}