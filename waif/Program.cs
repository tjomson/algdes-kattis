using System.Text;
using System.Linq;

public class waif
{
    public static void Main(string[] args)
    {
        // var g = new CoolGraph();
        // // for (int i = 0; i < 5; i++)
        // // {
        // //     var l = ReadLine();
        // //     g.AddEdge(l[0], l[1], l[2]);
        // // }
        // g.AddEdge(0, 1, 10);
        // g.AddEdge(1, 2, 1);
        // g.AddEdge(1, 3, 1);
        // g.AddEdge(0, 2, 1);
        // g.AddEdge(2, 3, 10);
        var g = Waif();
        MaxFlow(g);
    }

    static void DfsTest()
    {
        var g = new CoolGraph();
        g.AddEdge(0, 1, 1);
        g.AddEdge(0, 2, 1);
        g.AddEdge(1, 3, 1);
        g.AddEdge(3, 4, 1);
        var p = DFS(g, 0, 4);
        p?.ForEach(Console.WriteLine);
    }

    static CoolGraph Thore()
    {
        var n = int.Parse(Console.ReadLine()!);
        for (int i = 0; i < n; i++) Console.ReadLine();


        var m = int.Parse(Console.ReadLine()!);
        var g = new CoolGraph();

        for (int i = 0; i < m; i++)
        {
            var line = ReadLine();
            var cap = line[2] == -1 ? int.MaxValue : line[2];
            g.AddEdge(line[0], line[1], cap);
            g.AddEdge(line[1], line[0], cap);
        }
        Console.WriteLine(g);
        return g;
    }

    static void MaxFlow(CoolGraph g)
    {
        var path = DFS(g, 0, g.GetMaxId());
        while (path is not null)
        {
            g.Augment(path);
            path = DFS(g, 0, g.GetMaxId());
        }
        var maxFlow = g.GetVertexOutCapacity(g.GetMaxId());
        Console.WriteLine(maxFlow);
    }

    static List<Edge>? DFS(CoolGraph graph, int start, int terminal)
    {
        var marked = new HashSet<int>();

        List<Edge>? Loop(int vertex, List<Edge> path)
        {
            marked.Add(vertex);
            var adj = graph.GetVertexAdj(vertex).Where(x => x.capacity > 0);
            if (vertex == terminal) return path;
            foreach (var edge in adj)
            {
                var newPath = new List<Edge>(path) { edge };
                if (!marked.Contains(edge.to))
                {
                    var found = Loop(edge.to, newPath);
                    if (found is not null) return found;
                }
            }

            return null;
        }

        return Loop(start, new List<Edge>()); ;
    }

    // static Edge GetBottleneck(List<Edge> path)
    // {
    //     Edge minEdge = new Edge(-1, -1, int.MaxValue);
    //     foreach (var edge in path)
    //     {
    //         if (edge.capacity < minEdge.capacity) minEdge = edge;
    //     }
    //     return minEdge;
    // }

    // static List<int> DFS(CoolGraph graph)
    // {
    //     var marked = new bool[graph.GetVertexCount()];
    //     // var count = 0;

    //     Loop(0, new List<int>() { 0 });

    //     List<int> Loop(int vertex, List<int> path)
    //     {
    //         marked[vertex] = true;
    //         // count++;
    //         var adj = graph.GetVertexAdj(vertex);
    //         if (adj.Count() == 0 || adj.TrueForAll(e => marked[e.to])) return path;
    //         foreach (var edge in adj)
    //         {
    //             var newPath = new List<int>(path) { edge.to };
    //             if (!marked[edge.to]) return Loop(edge.to, newPath);
    //         }

    //         throw new Exception("wtf");
    //     }

    //     return Loop(0, new List<int>() { 0 }); ;
    // }

    static CoolGraph Waif()
    {
        var g = new CoolGraph();
        var items = ReadLine();
        var n = items.ElementAt(0); // Number of children
        var m = items.ElementAt(1); // Number of toys
        var p = items.ElementAt(2); // Number of toy categories

        var startNode = 0;
        var terminalNode = int.MaxValue;

        for (int i = 0; i < n; i++)
        {
            var lineParts = ReadLine();
            var childId = lineParts.ElementAt(0);
            g.AddEdge(startNode, childId, 1);
            var toys = lineParts.Skip(1);
            foreach (var toyId in toys)
            {
                g.AddEdge(childId, toyId, 1);
            }
        }

        for (int i = 0; i < p; i++)
        {
            var lineParts = ReadLine();
            var categoryId = lineParts.ElementAt(0);
            var categoryLimit = lineParts.Last();
            var toyIds = lineParts.Skip(1);
            for (int j = 0; j < toyIds.Count() - 1; j++)
            {
                g.AddEdge(toyIds.ElementAt(j), categoryId, 1);
            }
            g.AddEdge(categoryId, terminalNode, categoryLimit);
        }
        return g;
    }

    static List<int> ReadLine()
    {
        return Console.ReadLine()!.Split(" ").Select(int.Parse).ToList();
    }

    public class CoolGraph
    {
        Dictionary<int, List<Edge>> adj = new();

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
                if (exists is null) val.Add(edge); // Don't allow dupe edges
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

        public void AddEdge(int from, int to, int weight)
        {
            AddEdge(new Edge(from, to, weight));
        }

        public bool TryGetEdge(int from, int to, out Edge edge)
        {
            if (adj.TryGetValue(from, out var val))
            {
                var found = val.Find(x => x.to == to);
                if (found is not null)
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
            IncreaseFlow(e!, amount);
        }

        public void IncreaseFlow(Edge edge, int amount)
        {
            // if (edge.capacity > amount) edge.capacity -= amount;
            // else
            // {
            //     var vertex = adj[edge.from];
            //     vertex.Remove(edge);
            // }
            edge.capacity -= amount;

            if (TryGetReverseEdge(edge, out var reverse))
            {
                reverse.capacity += amount;
            }
            else
            {
                AddEdge(new Edge(edge.to, edge.from, amount));
            }

            // if (adj.TryGetValue(edge.to, out var val))
            // {
            //     var opposite = val.Find(x => x.to == edge.from);
            //     if (opposite is not null)
            //     {
            //         opposite.capacity += amount;
            //     }
            // }
        }

        public void DecreaseFlow(Edge edge, int amount)
        {
            edge.capacity += amount;

            if (TryGetReverseEdge(edge, out var reverse))
            {
                if (amount >= reverse.capacity) adj[edge.to].Remove(reverse);
                else reverse.capacity -= amount;
            }
            // else
            // {
            //     AddEdge(new Edge(edge.to, edge.from, amount));
            // }

        }

        public bool TryGetReverseEdge(Edge edge, out Edge other)
        {
            if (adj.TryGetValue(edge.to, out var val))
            {
                var found = val.Find(x => x.to == edge.from);
                if (found is not null)
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

            foreach (var (key, value) in adj)
            {
                foreach (var entry in value)
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
        public Edge(int from, int to, int capacity)
        {
            this.from = from;
            this.to = to;
            this.capacity = capacity;
        }

        public override string ToString()
        {
            return $"{this.from}=>{this.to} ({this.capacity})";
        }
    }
}
