using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class waif
{
    static int n;
    public static void Main(string[] args)
    {
        RunThore();
        // var g = Waif();
        // var g = Thore();
        // var path = BFS(g, 0, g.GetMaxId());
        // path.ForEach(Console.WriteLine);
        // var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        // Console.WriteLine(MaxFlow(g, g.GetMaxId()));
        // Console.WriteLine(MaxFlow(g, g.GetMaxId()));
        // Console.WriteLine(g);
        // MaxFlow(g, g.GetMaxId());
        // Console.WriteLine(MaxFlow(g, g.GetMaxId()));
        // var marked = FindMarked(g, 0, g.GetMaxId());
        // foreach (var x in marked) Console.WriteLine(x);
        // Console.WriteLine(g);
        // Console.WriteLine(GetMinCutCapacity(originalGraph, marked));
        // Console.WriteLine(GetMinCutCapacity(originalGraph, marked) / 2); // Divide by two because there is added capacity in both directions
        // Console.WriteLine(g);
    }

    static void RunWaif()
    {
        var g = Waif();
        Console.WriteLine(MaxFlow(g, g.GetMaxId()));
    }

    static void RunThore()
    {
        var g = Thore();
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        MaxFlow(g, g.GetMaxId());
        var marked = FindMarked(g, 0, g.GetMaxId());
        Console.WriteLine(GetMinCutCapacity(originalGraph, marked, true) / 2); // Divide by two because there is added capacity in both directions
    }

    static void RunPaintball()
    {
        var g = Paintball();
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        MaxFlow(g, g.GetMaxId());
        var marked = FindMarked(g, 0, g.GetMaxId());
        var cap = GetMinCutCapacity(g, marked);
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

    static CoolGraph Thore()
    {
        var n = int.Parse(Console.ReadLine());
        for (int i = 0; i < n; i++) Console.ReadLine();


        var m = int.Parse(Console.ReadLine());
        var g = new CoolGraph();

        for (int i = 0; i < m; i++)
        {
            var line = ReadLine();
            var cap = line[2] == -1 ? int.MaxValue : line[2];
            g.AddEdge(line[0], line[1], cap, true);
            g.AddEdge(line[1], line[0], cap, true);
        }
        return g;
    }

    static int GetMinCutCapacity(CoolGraph originalGraph, HashSet<int> marked, bool isBidirectional = false)
    {
        var count = 0;
        foreach (var vertex in originalGraph.adj)
        {
            foreach (var edge in vertex.Value)
            {
                if (marked.Contains(edge.to) && !marked.Contains(edge.from) && (!edge.isOriginal || isBidirectional)) count += edge.capacity;
            }
        }
        return count;
    }

    static List<Edge> GetMinCut(CoolGraph originalGraph, HashSet<int> marked)
    {
        var lst = new List<Edge>();
        foreach (var vertex in originalGraph.adj)
        {
            foreach (var edge in vertex.Value)
            {
                if (marked.Contains(edge.to) && !marked.Contains(edge.from) && !edge.isOriginal) lst.Add(edge);
            }
        }
        return lst;
    }

    static int MaxFlow(CoolGraph g, int terminal)
    {
        var path = BFS(g, 0, terminal);
        while (path != null)
        {
            g.Augment(path);
            path = BFS(g, 0, terminal);
        }
        var maxFlow = g.GetVertexOutCapacity(terminal);
        return maxFlow;
    }

    static List<Edge> Loop(int vertex, List<Edge> path, HashSet<int> marked, CoolGraph graph, int terminal)
    {
        var adj = graph.GetVertexAdj(vertex).Where(x => x.capacity > 0);
        if (vertex == terminal) return path;
        foreach (var edge in adj)
        {
            if (!marked.Contains(edge.to))
            {
                var newPath = new List<Edge>(path) { edge };
                var newMarked = new HashSet<int>(marked) { vertex };
                var found = Loop(edge.to, newPath, newMarked, graph, terminal);
                if (found != null) return found;
            }
        }

        return null;
    }

    static List<Edge> DFS(CoolGraph graph, int start, int terminal)
    {
        var marked = new HashSet<int>();

        return Loop(start, new List<Edge>(), marked, graph, terminal);
    }

    static List<Edge> ConstructPath(int start, int terminal, Dictionary<int, Edge> edgeTo)
    {
        var curr = terminal;
        Stack<Edge> x = new Stack<Edge>();
        var path = new List<Edge>();
        while (curr != start)
        {
            path.Add(edgeTo[curr]);
            curr = edgeTo[curr].from;
        }
        path.Reverse();
        return path;
    }

    static List<Edge> BFS(CoolGraph graph, int start, int terminal)
    {
        var marked = new HashSet<int>();
        Queue<int> queue = new Queue<int>();
        var edgeTo = new Dictionary<int, Edge>();
        marked.Add(start);
        queue.Enqueue(start);

        while (queue.Count() != 0)
        {
            int v = queue.Dequeue();
            foreach (var edge in graph.GetVertexAdj(v))
            {
                if (!marked.Contains(edge.to) && edge.capacity > 0)
                {
                    marked.Add(edge.to);
                    queue.Enqueue(edge.to);
                    edgeTo[edge.to] = edge;
                    if (edge.to == terminal) return ConstructPath(start, terminal, edgeTo);
                }
            }
        }
        return null;
    }

    static HashSet<int> FindMarked(CoolGraph graph, int start, int terminal)
    {
        var marked = new HashSet<int>();
        Queue<int> queue = new Queue<int>();
        marked.Add(start);
        queue.Enqueue(start);

        while (queue.Count() != 0)
        {
            int v = queue.Dequeue();
            foreach (var edge in graph.GetVertexAdj(v))
            {
                if (!marked.Contains(edge.to) && edge.capacity > 0)
                {
                    marked.Add(edge.to);
                    queue.Enqueue(edge.to);
                }
            }
        }
        return marked;
    }

    public static CoolGraph Paintball()
    {
        var g = new CoolGraph();
        var items = ReadLine();
        n = items[0]; // num players
        var m = items[1];
        var offset = 10000;

        var start = 0;
        var terminal = int.MaxValue;

        for (int i = 0; i < m; i++)
        {
            var line = ReadLine();
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

    public static CoolGraph Waif()
    {
        var g = new CoolGraph();
        var items = ReadLine();
        var n = items.ElementAt(0); // Number of children
        var m = items.ElementAt(1); // Number of toys
        var p = items.ElementAt(2); // Number of toy categories

        var startNode = 0;
        var terminalNode = int.MaxValue;

        var allToys = new HashSet<int>();
        for (int i = 1; i <= n; i++)
        {
            var lineParts = ReadLine();
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
            var lineParts = ReadLine();
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

    static List<int> ReadLine()
    {
        return Console.ReadLine().Split(" ").Select(int.Parse).ToList();
    }
}
