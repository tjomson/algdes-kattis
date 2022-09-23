using System.Collections.Generic;
using System.Linq;
using System;

public class intervalscheduling
{
    public static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine());

        var intervals = new List<Interval>();

        for (long i = 0; i < n; i++)
        {
            var numbers = Console.ReadLine().Split(new char[] { ' ' }).Select(x => int.Parse(x.Trim())).ToList();
            intervals.Add(new Interval(numbers[0], numbers[1]));
        }

        intervals.Sort((x, y) => (int)(x.end - y.end));

        // Console.WriteLine("----");
        // intervals.ForEach(x => Console.WriteLine(x.start + " " + x.end));
        // Console.WriteLine("----");

        var fitting = new List<Interval>();

        foreach (var interval in intervals)
        {
            if (fitting.Count() == 0 || fitting.Last().end <= interval.start)
            {
                fitting.Add(interval);
            }
        }

        Console.WriteLine(fitting.Count());
    }
}

class Interval
{
    public long start;
    public long end;
    public Interval(long start, long end)
    {
        this.start = start;
        this.end = end;
    }
}
