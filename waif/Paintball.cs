using System.Linq;
using System;
using System.Collections.Generic;

public static class Paintball
{
    static int n;
    public static void RunPaintball()
    {
        var g = GetPaintballGraph();
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        Flow.MaxFlow(g, g.GetMaxId());
        var marked = Flow.FindMarked(g, 0, g.GetMaxId());
        var cap = Flow.GetMinCutCapacity(g, marked);
        var whoShootsWho = new Dictionary<int, int>();
        if (cap != n) Console.WriteLine("Impossible");
        else
        {
            for (int i = 1; i <= n; i++)
            {
                var vertex = g.GetVertexAdj(10000 + i);
                var e = vertex.Find(x => x.capacity > 0 && x.to < 10000);
                whoShootsWho.Add(e.to, i);
            }
            whoShootsWho.OrderBy(x => x.Key).ToList().ForEach(x => Console.WriteLine(x.Value));
        }
    }
    public static CoolGraph GetPaintballGraph()
    {
        var g = new CoolGraph();
        var items = Flow.ReadLine();
        n = items[0]; // num players
        var m = items[1];
        var offset = 10000;

        var start = 0;
        var terminal = int.MaxValue;

        for (int i = 0; i < m; i++)
        {
            var line = Flow.ReadLine();
            g.AddEdge(line[0], line[1] + offset, 1);
            g.AddEdge(line[1], line[0] + offset, 1);
        }

        for (int i = 1; i <= n; i++) // Connect players to source
        {
            g.AddEdge(start, i, 1);
        }

        for (int i = 1; i <= n; i++) // Connect players to terminal
        {
            g.AddEdge(i + offset, terminal, 1);
        }

        return g;
    }
}
