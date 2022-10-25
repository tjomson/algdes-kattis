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
