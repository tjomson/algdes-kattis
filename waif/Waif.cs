using System.Linq;
using System;
using System.Collections.Generic;

public static class Waif
{
    public static void RunWaif()
    {
        var g = GetWaifGraph();
        Console.WriteLine(Flow.MaxFlow(g, g.GetMaxId()));
    }

    public static CoolGraph GetWaifGraph()
    {
        var g = new CoolGraph();
        var items = Flow.ReadLine();
        var n = items.ElementAt(0); // Number of children
        var m = items.ElementAt(1); // Number of toys
        var p = items.ElementAt(2); // Number of toy categories

        var startNode = 0;
        var terminalNode = int.MaxValue;

        var allToys = new HashSet<int>();
        for (int i = 1; i <= n; i++)
        {
            var lineParts = Flow.ReadLine();
            g.AddEdge(startNode, i, 1);
            var toys = lineParts.Skip(1);
            foreach (var toyId in toys)
            {
                allToys.Add(toyId + 2000000);
                g.AddEdge(i, toyId + 2000000, 1);
            }
        }

        for (int i = 1; i <= p; i++)
        {
            var lineParts = Flow.ReadLine();
            var categoryLimit = lineParts.Last();
            var toyIds = lineParts.Skip(1);
            for (int j = 0; j < toyIds.Count() - 1; j++)
            {
                allToys.Remove(toyIds.ElementAt(j) + 2000000);
                g.AddEdge(toyIds.ElementAt(j) + 2000000, i * -1, 1);
            }
            g.AddEdge(i * -1, terminalNode, categoryLimit);
        }

        foreach (var toy in allToys) // Add toys not in category
        {
            g.AddEdge(toy, terminalNode, 1);
        }

        return g;
    }
}
