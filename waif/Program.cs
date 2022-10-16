using System.Text;
using System.Linq;

public class waif
{
    public static void Main(string[] args)
    {
        var g = new Graph();
        // g.AddToAdj(2, 3);
        // g.AddToAdj(3, 5);
        // g.AddToAdj(2, 4);
        // Console.WriteLine(g);
        var items = ReadLine();
        var n = items.ElementAt(0); // Number of children
        var m = items.ElementAt(1); // Number of toys
        var p = items.ElementAt(2); // Number of toy categories

        for (int i = 0; i < n; i++)
        {
            var lineParts = ReadLine();
            var childId = lineParts.ElementAt(0);
            var toys = lineParts.Skip(1);
            g.AddToAdj(childId, toys);
        }

        for (int i = 0; i < p; i++)
        {
            var lineParts = ReadLine();

        }

    }

    static List<int> ReadLine()
    {
        return Console.ReadLine().Split(" ").Select(x => int.Parse(x)).ToList();
    }

    class Graph
    {
        Dictionary<int, List<int>> adj = new();
        new Dictionary<int, int> toyLimits = new();

        public void AddToAdj(int id, int item)
        {
            if (adj.TryGetValue(id, out var lst))
            {
                lst.Add(item);
            }
            else
            {
                var newLst = new List<int>() { item };
                adj.Add(id, newLst);
            }

        }

        public void AddToAdj(int id, IEnumerable<int> items)
        {
            if (adj.TryGetValue(id, out var lst))
            {
                lst.AddRange(items);
            }
            else
            {
                adj.Add(id, items.ToList());
            }

        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var (key, value) in adj)
            {
                sb.Append(key + ": { ");
                foreach (var entry in value)
                {
                    sb.Append(entry + " ");
                }
                sb.Append("}\n");
            }

            return sb.ToString();
        }
    }
}
