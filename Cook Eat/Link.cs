internal class Link
{
    public Node StartNode { get; }
    public Node EndNode { get; }
    public double Weight { get; }



    public Link(Node startNode, Node endNode, double weight)
    {
        StartNode = startNode;
        EndNode = endNode;
        Weight = weight;
    }
}