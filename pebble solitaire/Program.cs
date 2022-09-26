using System;
using System.Linq;
using System.Collections.Generic;

public class pebblesolitaire
{

    static Dictionary<string, int> cache = new Dictionary<string, int>();

    public static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            var pebbles = Console.ReadLine();
            var res = MinPebbles(pebbles);
            Console.WriteLine(res);
        }
    }

    public static int MinPebbles(string pebbles)
    {
        if (cache.TryGetValue(pebbles, out int val)) return val;
        var validMoves = ValidMoves(pebbles);
        if (validMoves.Length == 0) return PebbleCount(pebbles);

        var bestResult = int.MaxValue;

        foreach (var move in validMoves)
        {
            var moveApplied = ApplyMove(pebbles, move);
            var pebbleCount = cache.TryGetValue(moveApplied, out int found) ? found : MinPebbles(moveApplied);
            cache.TryAdd(moveApplied, pebbleCount);
            if (pebbleCount < bestResult) bestResult = pebbleCount;
        }
        return bestResult;
    }

    public static Move[] ValidMoves(string pebbles)
    {
        var moves = new List<Move>();
        for (int i = 0; i < pebbles.Length - 2; i++)
        {
            if (pebbles[i + 1] == 'o')
            {
                if ((pebbles[i] == 'o') && (pebbles[i + 2] == '-')) moves.Add(new Move(i, i + 2));
                else if (pebbles[i] == '-' && pebbles[i + 2] == 'o') moves.Add(new Move(i + 2, i));
            }
        }
        return moves.ToArray();
    }

    public static string ApplyMove(string pebbles, Move move)
    {
        var arr = pebbles.ToCharArray(); // faster than .ToArray() for some reason
        arr[move.from] = '-';
        arr[move.to] = 'o';
        // arr[Math.Abs(move.from - move.to)] = '-';
        if (move.from > move.to) arr[move.from - 1] = '-';
        else arr[move.from + 1] = '-';
        return new string(arr); // Much faster than string.Join()
    }

    public static int PebbleCount(string pebbles)
    {
        return pebbles.Where(x => x == 'o').Count();
    }

}

public class Move
{
    public int from;
    public int to;
    public Move(int from, int to)
    {
        this.from = from;
        this.to = to;
    }
}
