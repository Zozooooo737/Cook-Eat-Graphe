internal class Graph
{
    public List<Node> nodes { get; }
    public List<Link> links { get; }
    public bool isDirected { get; }
    public bool isWeighted { get; }



    public Graph(bool isDirected, bool isWeighted)
    {
        nodes = new List<Node>();
        links = new List<Link>();

        this.isDirected = isDirected;
        this.isWeighted = isWeighted;
    }



    public void AddNode(int id)
    {
        if (!nodes.Any(n => n.Id == id))
        {
            nodes.Add(new Node(id));
        }
    }


    public void AddLink(int sourceId, int targetId, double weight = 1.0)
    {
        Node source = nodes.FirstOrDefault(n => n.Id == sourceId) ?? new Node(sourceId);
        if (!nodes.Contains(source)) nodes.Add(source);

        Node target = nodes.FirstOrDefault(n => n.Id == targetId) ?? new Node(targetId);
        if (!nodes.Contains(target)) nodes.Add(target);

        if ((!isDirected && !links.Any(l => l.StartNode == target && l.EndNode == source) || (isDirected && !links.Any(l => l.StartNode == source && l.EndNode == target))))
        {
            links.Add(new Link(source, target, weight));

            source.AddSuccessor(target);
            if (!isDirected)
            {
                target.AddSuccessor(source);
            }
        }
    }


    public void FromAdjacencyList(Dictionary<int, List<(int, int)>> adjacencyList)
    {
        foreach ((int sourceId, List<(int, int)> targets) in adjacencyList)
        {
            AddNode(sourceId);
            foreach ((int targetId, int weight) in targets)
            {
                AddLink(sourceId, targetId, weight);
            }
        }
    }


    public void FromAdjacencyMatrix(int[,] adjacencyMatrix)
    {
        for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
            {
                if (adjacencyMatrix[i, j] > 0)
                {
                    AddLink(i + 1, j + 1, adjacencyMatrix[i, j]);
                }
            }
        }
    }


    public List<Node> BreadthFirstSearch(int startId)
    {
        HashSet<Node> visited = new HashSet<Node>();
        Queue<Node> queue = new Queue<Node>();
        Node startNode = nodes.First(n => n.Id == startId);
        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            foreach (Node successor in currentNode.Successors)
            {
                if (!visited.Contains(successor))
                {
                    visited.Add(successor);
                    queue.Enqueue(successor);
                }
            }
        }
        return visited.ToList();
    }


    public List<Node> DepthFirstSearch(int startId)
    {
        HashSet<Node> visited = new HashSet<Node>();
        DFS(nodes.First(n => n.Id == startId), visited);

        return visited.ToList();
    }

    private void DFS(Node node, HashSet<Node> visited)
    {
        if (!visited.Contains(node))
        {
            visited.Add(node);
            foreach (Node successor in node.Successors)
            {
                DFS(successor, visited);
            }
        }
    }


    public int GetOrder()
    {
        return nodes.Count;
    }


    public int GetSize()
    {
        return links.Count;
    }


    public bool IsConnected()
    {
        if (nodes.Count == 0) return true;

        HashSet<Node> visited = new HashSet<Node>();
        DFS(nodes[0], visited);

        return visited.Count == nodes.Count;
    }


    public bool ContainsCycle()
    {
        HashSet<Node> visited = new HashSet<Node>();
        HashSet<Node> recursionStack = new HashSet<Node>();

        foreach (Node node in nodes)
        {
            if (ContainsCycleTool(node, visited, recursionStack))
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainsCycleTool(Node node, HashSet<Node> visited, HashSet<Node> recursionStack)
    {
        if (recursionStack.Contains(node))
        {
            return true;
        }

        if (visited.Contains(node))
        {
            return false;
        }

        visited.Add(node);
        recursionStack.Add(node);

        foreach (Node successor in node.Successors)
        {
            if (ContainsCycleTool(successor, visited, recursionStack))
            {
                return true;
            }
        }

        recursionStack.Remove(node);
        return false;
    }


    public override string ToString()
    {
        string description = "Descritption générale du graphe :\n";

        description += "\nOrienté : " + isDirected;
        description += "\nPondéré : " + isWeighted;
        description += "\nNombre de nœuds (Ordre) : " + GetOrder();
        description += "\nNombre de liens (Taille) : " + GetSize();
        description += "\nLe graphe est-il connexe ? " + IsConnected();
        description += "\nLe graphe contient-il un cycle ? " + ContainsCycle();

        description += "\n\nParcours en profondeur (DFS) à partir du nœud 1 : " +
                       string.Join(", ", DepthFirstSearch(1).Select(node => node.Id));

        description += "\nParcours en largeur (BFS) à partir du nœud 1 : " +
                       string.Join(", ", BreadthFirstSearch(1).Select(node => node.Id));

        description += "\n\nNodes:";
        foreach (Node node in nodes)
        {
            description += "\n  - Node " + node.Id;
        }

        description += "\n\nLinks:";
        foreach (var link in links)
        {
            if (isWeighted)
            {
                description += $"\n  - {link.StartNode.Id} {(isDirected ? "->" : "<->")} {link.EndNode.Id} (Weight: {link.Weight})";
            }
            else
            {
                description += $"\n  - {link.StartNode.Id} {(isDirected ? "->" : "<->")} {link.EndNode.Id}";
            }
        }
        return description;
    }
}