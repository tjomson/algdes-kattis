using System.Collections.Generic;
using System.Linq;
using System;

public class shopaholic
{
    public static void Main(string[] args)
    {
        var n = long.Parse(Console.ReadLine());
        var numbers = Console.ReadLine().Split(' ').Select(x => long.Parse(x)).ToList();
        numbers.Sort((x, y) => (int)(y - x));
        // numbers.ForEach(x => Console.WriteLine(x));

        long total = 0;
        int index = 2;
        int length = numbers.Count();
        while (index < length)
        {
            total += numbers[index];
            index += 3;
        }

        Console.WriteLine(total);
    }
}
