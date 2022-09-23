using System.Collections.Generic;
using System;
using System.Linq;

public class weightedintervalscheduling
{
    static Dictionary<int, int> cache = new Dictionary<int, int>();
    static Interval[] intervals;
    public static void Main(string[] args)
    {
        // var i1 = new Interval(1, 4, 0);
        // var i2 = new Interval(0, 6, 0);
        // Console.WriteLine(i1.overlaps(i2));
        var n = int.Parse(Console.ReadLine());
        intervals = new Interval[n];
        for (int i = 0; i < n; i++)
        {
            var ints = Console.ReadLine().Split(" ").Select(x => int.Parse(x)).ToList();
            intervals[i] = new Interval(ints[0], ints[1], ints[2]);
        }

        intervals = intervals.OrderBy(x => x.end).ToArray();

        var result = Solve(0);
        Console.WriteLine(result);
    }

    public static int Solve(int i)
    {
        if (i == intervals.Count()) return 0;
        if (cache.TryGetValue(i, out int val))
        {
            // Console.WriteLine("using cache");
            return val;
        }
        var curr = intervals[i];

        var skip = getSkipIndex(i);
        var include = curr.weight + Solve(skip);
        cache.TryAdd(skip, include);

        var exclude = Solve(i + 1);
        cache.TryAdd(i + 1, exclude);

        var bestVal = Math.Max(include, exclude);
        cache.TryAdd(i, bestVal);
        return bestVal;
    }

    public static int getSkipIndex(int currIndex)
    {
        var curr = intervals[currIndex];
        currIndex++;
        var count = intervals.Count();
        while (currIndex < count)
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
