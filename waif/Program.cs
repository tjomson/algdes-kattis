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
        var g = Waif();
        // var g = Thore();
        // var path = BFS(g, 0, g.GetMaxId());
        // path.ForEach(Console.WriteLine);
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        // Console.WriteLine(MaxFlow(g, g.GetMaxId()));
        MaxFlow(g, g.GetMaxId());
        var marked = FindMarked(g, 0, g.GetMaxId());
        // foreach (var x in marked) Console.WriteLine(x);
        // Console.WriteLine(g);
        Console.WriteLine(GetMinCutCapacity(originalGraph, marked));
        // Console.WriteLine(GetMinCutCapacity(originalGraph, marked) / 2); // Divide by two because there is added capacity in both directions
        // Console.WriteLine(g);


    }

    static void RunPaintball()
    {
        var g = Paintball();
        var originalGraph = new CoolGraph(new Dictionary<int, List<Edge>>(g.adj));
        MaxFlow(g, g.GetMaxId());
        var marked = FindMarked(g, 0, g.GetMaxId());
        var cap = GetMinCutCapacity(g, marked);
        var cut = GetMinCut(g, marked);
        // cut.ForEach(Console.WriteLine);
        // Console.WriteLine(g);
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
            whoShootsWho.ToList().OrderBy(x => x.Key).ToList().ForEach(x => Console.WriteLine(x.Value));
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
            // g.AddEdge(line[1], line[0], cap, true);
        }
        return g;
    }

    static int GetMinCutCapacity(CoolGraph originalGraph, HashSet<int> marked)
    {
        var count = 0;
        foreach (var vertex in originalGraph.adj)
        {
            foreach (var edge in vertex.Value)
            {
                if (marked.Contains(edge.to) && !marked.Contains(edge.from) && !edge.isOriginal) count += edge.capacity;
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
                allToys.Add(toyId + 10000);
                g.AddEdge(i, toyId + 10000, 1);
            }
        }

        for (int i = n + 1; i <= p + n; i++)
        {
            var lineParts = ReadLine();
            var categoryLimit = lineParts.Last();
            var toyIds = lineParts.Skip(1);
            for (int j = 0; j < toyIds.Count() - 1; j++)
            {
                allToys.Remove(toyIds.ElementAt(j) + 10000);
                g.AddEdge(toyIds.ElementAt(j), i, 1);
            }
            g.AddEdge(i, terminalNode, categoryLimit);
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

    public class CoolGraph
    {
        public Dictionary<int, List<Edge>> adj = new Dictionary<int, List<Edge>>();
        public CoolGraph() { }

        public CoolGraph(Dictionary<int, List<Edge>> adj)
        {
            this.adj = adj;
        }

        public int GetVertexCount()
        {
            return adj.Count();
        }

        public int GetMaxId()
        {
            return adj.Max(x => x.Key);
        }

        public int GetVertexOutCapacity(int vertex)
        {
            var v = adj[vertex];
            var count = 0;
            foreach (var edge in v)
            {
                count += edge.capacity;
            }
            return count;
        }

        public void AddEdge(Edge edge)
        {
            if (adj.TryGetValue(edge.from, out var val))
            {
                var exists = val.Find(x => x.to == edge.to);
                if (exists == null) val.Add(edge);
                else
                {
                    exists.capacity = edge.capacity; // If same edge is added, overwrite capacity
                    exists.isOriginal = edge.isOriginal;
                }
            }
            else
            {
                adj.Add(edge.from, new List<Edge>() { edge });
            }

            if (!adj.ContainsKey(edge.to))
            {
                adj.Add(edge.to, new List<Edge>() { new Edge(edge.to, edge.from, 0) });
            }
            else
            {
                adj[edge.to].Add(new Edge(edge.to, edge.from, 0)); // Add zero capacity opposite edge
            }
        }

        public void AddEdge(int from, int to, int weight, bool isOriginal = false)
        {
            AddEdge(new Edge(from, to, weight, isOriginal));
        }

        public bool TryGetEdge(int from, int to, out Edge edge)
        {
            if (adj.TryGetValue(from, out var val))
            {
                var found = val.Find(x => x.to == to);
                if (found != null)
                {
                    edge = found;
                    return true;
                }
            }
            edge = new Edge(-1, -1, -1);
            return false;
        }

        public List<Edge> GetVertexAdj(int id)
        {
            return adj[id];
        }

        public void Augment(List<Edge> path)
        {
            var bottleneck = path.Min(x => x.capacity);
            foreach (var edge in path)
            {
                this.IncreaseFlow(edge.from, edge.to, bottleneck);
            }
        }

        public void IncreaseFlow(int from, int to, int amount)
        {
            var e = adj[from].Where(x => x.to == to).FirstOrDefault();
            IncreaseFlow(e, amount);
        }

        public void IncreaseFlow(Edge edge, int amount)
        {
            edge.capacity -= amount;

            if (TryGetReverseEdge(edge, out var reverse))
            {
                reverse.capacity += amount;
            }
            else
            {
                AddEdge(new Edge(edge.to, edge.from, amount));
            }
        }

        public void DecreaseFlow(Edge edge, int amount)
        {
            edge.capacity += amount;

            if (TryGetReverseEdge(edge, out var reverse))
            {
                if (amount >= reverse.capacity) adj[edge.to].Remove(reverse);
                else reverse.capacity -= amount;
            }
        }

        public bool TryGetReverseEdge(Edge edge, out Edge other)
        {
            if (adj.TryGetValue(edge.to, out var val))
            {
                var found = val.Find(x => x.to == edge.from);
                if (found != null)
                {
                    other = found;
                    return true;
                }
            }
            other = new Edge(-1, -1, -1);
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var keyvalue in adj)
            {
                foreach (var entry in keyvalue.Value)
                {
                    sb.Append($"{entry}\n");
                }
            }

            return sb.ToString();
        }
    }

    public class Edge
    {
        public int from;
        public int to;
        public int capacity;
        public bool isOriginal;
        public Edge(int from, int to, int capacity, bool isOriginal = false)
        {
            this.from = from;
            this.to = to;
            this.capacity = capacity;
            this.isOriginal = isOriginal;
        }

        public override string ToString()
        {
            return $"{this.from}=>{this.to} ({this.capacity})";
        }
    }
}
