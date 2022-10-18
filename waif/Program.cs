using System.Text;
using System.Linq;

public class waif
{
    public static void Main(string[] args)
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

    }

    static List<int> ReadLine()
    {
        return Console.ReadLine().Split(" ").Select(int.Parse).ToList();
    }

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
                var exists = val.Find(x => x.to == edge.to);
                if (exists is null) val.Add(edge); // Don't allow dupe edges
            }
            else
            {
                adj.Add(edge.from, new List<Edge>() { edge });
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
