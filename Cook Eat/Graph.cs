using System.Xml.Linq;

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


        public void AddSuccessor(int predecessor, int successor)
        {
            foreach(Node element in nodes) { if(element.Id == predecessor) { element.NewSuccessor(successor); } }
        }


        public void AddLink(int source, int target)
        {
            if((!isDirected && !links.Any(l => l.Node1 == target && l.Node2 == source)) || isDirected)
            {
                links.Add(new Link(source, target));
                this.AddNode(source);
                this.AddNode(target);
                if(isDirected) { this.AddSuccessor(source, target); }
                else { this.AddSuccessor(source, target); this.AddSuccessor(target, source); }
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


        public int FindNode(int id)
        {
            int i = 0;
            bool found = false;
            while(!found && i<nodes.Count)
            {
                if (id == nodes[i].Id) { found = true; return i; }
                i++;
            }
            return -1;
        }


        public void BreadthFirstSearch(int id)
        {
            Queue<int> waitingList = [];
            waitingList.Enqueue(id);

            bool[] visited = new bool[nodes.Count];
            for (int i = 0; i < visited.Length; i++) { visited[i] = false; }
            visited[FindNode(id)]=true;

            int[] ids = new int[nodes.Count];
            ids[0] = id;

            int k = 1;
            while(waitingList.Count!=0)
            {
                int tmp = waitingList.Dequeue();
                foreach(int neighbor in nodes[FindNode(tmp)].Successors)
                {
                    if (!visited[FindNode(neighbor)])
                    {
                        waitingList.Enqueue(neighbor);
                        visited[FindNode(neighbor)] = true;
                        ids[k] = neighbor;
                        k++;
                    }
                    
                }
            }
        }


        public void DepthFirstSearch(int id)
        {
            bool[] visited = new bool[nodes.Count];
            for(int i=0; i<visited.Length; i++) { visited[i] = false; }
            DFS(id, visited);
        }


        public void DFS(int id, bool[] visited)
        {
            Console.WriteLine(id);
            int k = FindNode(id);
            if (!visited[k])
            {
                visited[k] = true;
                foreach (int element in nodes[k].Successors)
                {
                    DFS(id, visited);
                }
            }
        }
    }
}
