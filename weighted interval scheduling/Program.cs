using System.Collections.Generic;
using System;
using System.Linq;

public class weightedintervalscheduling
{
    static int?[] cache;
    static Interval[] intervals;
    static int n;
    public static void Main(string[] args)
    {
        ReadData();
        Array.Sort(intervals, (a, b) => a.end - b.end);
        var result = Solve(n - 1);
        Console.WriteLine(result);
    }

    public static void ReadData()
    {
        n = int.Parse(Console.ReadLine());

        intervals = new Interval[n];
        cache = new int?[n];
        for (int i = 0; i < n; i++)
        {
            var ints = Console.ReadLine().Split(' ');
            intervals[i] = new Interval(int.Parse(ints[0]), int.Parse(ints[1]), int.Parse(ints[2]));
        }
    }

    public static int Solve(int i)
    {
        if (i < 0) return 0;
        if (cache[i] != null) return (int)cache[i];

        var curr = intervals[i];
        var skip = binSearchRec(intervals, 0, n - 1, curr.start);
        int include = curr.weight + Solve(skip);
        var exclude = Solve(i - 1);
        var bestVal = Math.Max(include, exclude);
        cache[i] = bestVal;
        return bestVal;
    }

    static int binSearchRec(Interval[] section, int low, int high, int maxEndValue)
    {
        if (low > high) return -1;

        var mid = (low + high) / 2;
        var midEnd = section[mid].end;

        if (midEnd <= maxEndValue)
        {
            return Math.Max(mid, binSearchRec(section, mid + 1, high, maxEndValue));
        }
        else
        {
            return Math.Max(-1, binSearchRec(section, low, mid - 1, maxEndValue));
        }
    }

    static int binSearchIte(Interval[] section, int low, int high, int maxEndValue)
    {
        int bestRes = -1;

        while (low <= high)
        {
            int mid = low + (high - low + 1) / 2;
            int midEnd = section[mid].end;

            if (midEnd <= maxEndValue)
            {
                bestRes = mid;
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }
        return bestRes;
    }

}

public class Interval
{
    public int start;
    public int end;
    public int weight;

    public Interval(int start, int end, int weight)
    {
        this.start = start;
        this.end = end;
        this.weight = weight;
    }
}
