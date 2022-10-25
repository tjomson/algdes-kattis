using System.Text;
using System.Linq;
using System.Collections.Generic;

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
