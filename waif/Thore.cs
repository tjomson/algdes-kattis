using System;
using System.Collections.Generic;

public static class Thore
{
    public static void RunThore()
    {
        var g = GetThoreGraph();
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        Flow.MaxFlow(g, g.GetMaxId());
        var marked = Flow.FindMarked(g, 0, g.GetMaxId());
        Console.WriteLine(Flow.GetMinCutCapacity(originalGraph, marked, true) / 2); // Divide by two because there is added capacity in both directions
    }

    static CoolGraph GetThoreGraph()
    {
        var n = int.Parse(Console.ReadLine());
        for (int i = 0; i < n; i++) Console.ReadLine();


        var m = int.Parse(Console.ReadLine());
        var g = new CoolGraph();

        for (int i = 0; i < m; i++)
        {
            var line = Flow.ReadLine();
            var cap = line[2] == -1 ? int.MaxValue : line[2];
            g.AddEdge(line[0], line[1], cap, true);
            g.AddEdge(line[1], line[0], cap, true);
        }
        return g;
    }

}
