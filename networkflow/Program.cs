using System.Text;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // var g = new CoolGraph();
        // g.AddEdge(new Edge(1, 2, 5));
        // g.AddEdge(new Edge(1, 3, 10));
        // g.AddEdge(new Edge(4, 1, 15));
        // g.TryGetEdge(1, 2, out var e);
        // g.IncreaseFlow(e, 3);
        // g.TryGetEdge(2, 1, out var f);
        // g.DecreaseFlow(e, 2);
        // Console.WriteLine(g);
        unchecked
        {
            bruh((uint)-1);

        }
    }

    static void bruh(uint x)
    {
        Console.WriteLine(x);
    }

    public static void Search(CoolGraph g, int start, int finish)
    {

        HashSet<string> visitedCache = new();
        var edges = g.GetVertexAdj(start);
        Stack<Edge> path = new();

        foreach (var edge in edges)
        {

        }


        // Stack<Edge> Aux(int currentVertex, Stack<Edge> currPath)
        // {
        //     if (currentVertex == finish)
        //     {
        //         return currPath;
        //     }
        // }
    }

    // class DFS
    // {
    //     bool[] marked;
    //     int count;

    //     public DFS(CoolGraph g, int v)
    //     {
    //         marked = new bool[g.GetVertexCount()];
    //     }

    //     public void Search(CoolGraph g, int v)
    //     {
    //         count++;
    //         marked[v] = true;
    //         foreach (var w in g.GetVertexAdj(v))
    //         {
    //             if (!marked[w.to]) {
    //                 Search(g, w.to);
    //             }
    //         }
    //     }
    // }

    public class CoolGraph
    {
        Dictionary<int, List<Edge>> adj = new();

        public int GetVertexCount()
        {
            return adj.Count();
        }

        public void AddEdge(Edge edge)
        {
            if (adj.TryGetValue(edge.from, out var val))
            {
                val.Add(edge);
            }
            else
            {
                adj.Add(edge.from, new List<Edge>() { edge });
            }
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
            edge = null;
            return false;
        }

        public List<Edge> GetVertexAdj(int id)
        {
            return adj[id];
        }

        public void IncreaseFlow(Edge edge, int amount)
        {
            if (edge.capacity > amount) edge.capacity -= amount;
            else
            {
                var vertex = adj[edge.from];
                vertex.Remove(edge);
            }

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
            other = null;
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var (key, value) in adj)
            {
                foreach (var entry in value)
                {
                    sb.Append($"{entry.from}=>{entry.to} ({entry.capacity})\n");
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
    }
}