using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

public class weightedintervalscheduling
{
    // static Dictionary<int, int> cache = new Dictionary<int, int>();
    static int?[] cache;
    static Interval[] intervals;
    static int n;
    public static void Main(string[] args)
    {
        n = int.Parse(Console.ReadLine());

        var stop = new Stopwatch();
        stop.Start();

        intervals = new Interval[n];
        cache = new int?[n];
        for (int i = 0; i < n; i++)
        {
            var ints = Console.ReadLine().Split(' ');
            intervals[i] = new Interval(int.Parse(ints[0]), int.Parse(ints[1]), int.Parse(ints[2]));
        }

        Array.Sort(intervals, (a, b) => a.end - b.end);

        stop.Stop();
        // Console.WriteLine("Parse ms: " + stop.ElapsedMilliseconds);
        stop.Restart();
        var result = Solve(0);
        stop.Stop();
        // Console.WriteLine("calc ms: " + stop.ElapsedMilliseconds);
        Console.WriteLine(result);
    }

    public static int Solve(int i, int acc)
    {
        if (i >= n) return 0;
        if (cache[i] != null) return (int)cache[i];

        var curr = intervals[i];

        var skip = getSkipIndex(i);
        // var include = (skip != i && skip < n) ? curr.weight + Solve(skip) : 0;
        var include = curr.weight + Solve(skip);
        // cache.TryAdd(skip, include);

        var exclude = Solve(i + 1);
        // cache.TryAdd(i + 1, exclude);

        var bestVal = Math.Max(include, exclude);
        // cache.TryAdd(i, bestVal);
        cache[i] = bestVal;
        return bestVal;
    }

    public static int getSkipIndex(int currIndex)
    {
        var curr = intervals[currIndex];
        currIndex++;
        while (currIndex < n)
        {
            if (!curr.overlapsForward(intervals[currIndex]))
            {
                return currIndex;
            }
            currIndex++;
        }
        return currIndex;
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

    public bool overlaps(Interval that)
    {
        // return (that.start < this.end && that.start > this.start) ||
        // (that.start < this.end && that.end < this.start);

        return !(that.end <= this.start || that.start >= this.end);
    }

    public bool overlapsForward(Interval that)
    {
        return that.start < this.end;
    }
}
