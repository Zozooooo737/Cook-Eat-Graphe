namespace Cook_Eat
{
    internal class Graph
    {
        private List<Node> nodes;
        private List<Link> links;

        private bool isDirected;



        public Graph(bool isDirected)
        {
            nodes = new List<Node>();
            links = new List<Link>();

            this.isDirected = isDirected;
        }



        public void AddNode(int id)
        {
            for (int i = 0; i < nodes.Count; i++) 
            {
                if (nodes[i].Id == id)
                {
                    return;
                }
            }
            nodes.Add(new Node(id));

            //bool exists = nodes.Any(n => n.Id == id); // a ecrire sur google doc
        }


        public void AddLink(int source, int target)
        {
            if((!isDirected && !links.Any(l => l.Node1 == target && l.Node2 == source)) || isDirected)
            {
                links.Add(new Link(source, target));
                this.AddNode(source);
                this.AddNode(target);
            }
        }


        public void FromAdjacencyList((int, int[])[] adjacencyList)
        {
            for(int i = 0; i < adjacencyList.Length; i++)
            {
                for(int j = 0; j < adjacencyList[i].Item2.Length; j++)
                {
                    AddLink(adjacencyList[i].Item1, adjacencyList[i].Item2[j]);
                }
            }
        }


        public void FromAdjacencyMatrix(bool[,] adjacencyMatrix)
        {
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for(int j=0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] == true)
                    {
                        AddLink(i+1,j+1);
                    }
                }
            }
        }
    }
}
