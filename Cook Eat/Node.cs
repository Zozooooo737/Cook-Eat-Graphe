internal class Node
{
    public int Id { get; }
    public HashSet<Node> Successors { get; }




    public Node(int id)
    {
        Id = id;
        Successors = new HashSet<Node>();
    }



    public void AddSuccessor(Node node)
    {
        Successors.Add(node);
    }
}