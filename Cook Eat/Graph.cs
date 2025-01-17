namespace Cook_Eat
{
    internal class Graph
    {
        private List<Node> nodes;
        private List<Link> links;



        public Graph()
        {
            nodes = new List<Node>();
            links = new List<Link>();
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
            links.Add(new Link(new Node(source), new Node(target)));
            this.AddNode(source);
            this.AddNode(target);
            // Vérifier que le lien n'existe pas déjà
        }


        public void FromAdjacencyList((int, int[])[] adjacencyList)
        {
            for(int i = 0;i < adjacencyList.Length; i++)
            {
                for (int j = 0; j < adjacencyList[i].Item2.Length; j++)
                {
                    AddLink(adjacencyList[i].Item1, adjacencyList[i].Item2[j]);
                }
            }
        }


        public void FromAdjacencyMatrix(bool[,] adjacencyMatrix)
        {
            for(int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for(int j=0;j < adjacencyMatrix.GetLength(1); j++)
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
