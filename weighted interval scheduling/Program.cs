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
        ReadData();

        // n = 5;
        // intervals = new Interval[] { new(1, 2, 2), new(5, 7, 1), new(2, 4, 2), new(14, 20, 1), new(10, 12, 4) };

        // n = 3;
        // intervals = new Interval[] { new(1, 2, 1), new(1, 2, 1), new(1, 2, 1) };

        // n = 8;
        // intervals = new Interval[] { new(0, 6, 3), new(1, 4, 2), new(3, 5, 1), new(3, 8, 6), new(4, 7, 7), new(5, 9, 1), new(6, 10, 4), new(8, 11, 3) };
        // cache = new int?[n];

        Array.Sort(intervals, (a, b) => a.end - b.end);

        // var res = binSearch(intervals, 8, 0, n - 1);

        // var res = binSearchInterval(intervals, 5, 0);
        // Console.WriteLine(res);

        // stop.Stop();
        // // Console.WriteLine("Parse ms: " + stop.ElapsedMilliseconds);
        // stop.Restart();
        var result = Solve(n - 1);
        // stop.Stop();
        // // Console.WriteLine("calc ms: " + stop.ElapsedMilliseconds);
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
        var skip = greatestlesser(intervals, 0, n - 1, curr.start);
        int include = i == skip ? curr.weight : curr.weight + Solve(skip);
        var exclude = Solve(i - 1);
        var bestVal = Math.Max(include, exclude);
        cache[i] = bestVal;
        return bestVal;
    }

    public static int getSkipIndex(int currIndex)
    {
        var curr = intervals[currIndex];
        currIndex--;
        while (currIndex >= 0)
        {
            if (curr.start >= intervals[currIndex].end)
            {
                return currIndex;
            }
            currIndex--;
        }
        return currIndex;
    }


    static int binSearch(Interval[] section, int maxEndTime, int low, int high)
    {
        if (high == 0)
        {
            if (section[low].end > maxEndTime) return -1;
            return low;
        };
        if (low >= high) return high;
        var mid = (low + high) / 2;
        if (section[mid].end >= maxEndTime)
        {
            return binSearch(section, maxEndTime, low, mid);
        }
        return binSearch(section, maxEndTime, mid + 1, high);
    }

    static int greatestlesser(Interval[] section, int low, int high, int key)
    {
        int ans = -1;

        while (low <= high)
        {
            int mid = low + (high - low + 1) / 2;
            int midVal = section[mid].end;

            if (midVal < key)
            {
                ans = mid;
                low = mid + 1;
            }
            else if (midVal > key)
            {
                high = mid - 1;
            }
            else if (midVal == key)
            {
                ans = mid;
                low = mid + 1;
            }
        }

        return ans;
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
