using System.Linq;
using System;
using System.Collections.Generic;

public static class Flow
{
    public static int GetMinCutCapacity(CoolGraph originalGraph, HashSet<int> marked, bool isBidirectional = false)
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

    public static List<Edge> GetMinCut(CoolGraph originalGraph, HashSet<int> marked)
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

    public static int MaxFlow(CoolGraph g, int terminal)
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

    public static List<Edge> Loop(int vertex, List<Edge> path, HashSet<int> marked, CoolGraph graph, int terminal)
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

    public static List<Edge> DFS(CoolGraph graph, int start, int terminal)
    {
        var marked = new HashSet<int>();

        return Loop(start, new List<Edge>(), marked, graph, terminal);
    }

    public static List<Edge> ConstructPath(int start, int terminal, Dictionary<int, Edge> edgeTo)
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

    public static List<Edge> BFS(CoolGraph graph, int start, int terminal)
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

    public static HashSet<int> FindMarked(CoolGraph graph, int start, int terminal)
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

    public static List<int> ReadLine()
    {
        return Console.ReadLine().Split(" ").Select(int.Parse).ToList();
    }
}