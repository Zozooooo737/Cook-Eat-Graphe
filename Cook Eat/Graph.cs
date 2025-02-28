namespace Cook_Eat
{
    /// <summary>
    /// Représente un graphe composé de nœuds (nodes) et de liens (links).
    /// Le graphe peut être orienté ou non orienté, et pondéré ou non pondéré.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Liste des nœuds du graphe.
        /// </summary>
        public List<Node> Nodes { get; }

        /// <summary>
        /// Liste des liens du graphe.
        /// </summary>
        public List<Link> Links { get; }

        /// <summary>
        /// Indique si le graphe est orienté.
        /// </summary>
        public bool isDirected { get; }

        /// <summary>
        /// Indique si le graphe est pondéré.
        /// </summary>
        public bool isWeighted { get; }



        /// <summary>
        /// Constructeur de la classe Graph.
        /// </summary>
        /// <param name="isDirected">True si le graphe est orienté, False sinon.</param>
        /// <param name="isWeighted">True si le graphe est pondéré, False sinon.</param>
        public Graph(bool isDirected, bool isWeighted)
        {
            Nodes = new List<Node>();
            Links = new List<Link>();

            this.isDirected = isDirected;
            this.isWeighted = isWeighted;
        }



        /// <summary>
        /// Ajoute un nœud au graphe s'il n'existe pas déjà.
        /// </summary>
        /// <param name="id">L'identifiant du nœud à ajouter.</param>
        public void AddNode(int id)
        {
            if (!Nodes.Any(n => n.Id == id))
            {
                Nodes.Add(new Node(id));
            }
        }


        /// <summary>
        /// Ajoute un lien entre deux nœuds dans le graphe.
        /// Si les nœuds n'existent pas, ils sont créés.
        /// Si le graphe est non orienté, un lien bidirectionnel est ajouté.
        /// </summary>
        /// <param name="sourceId">L'identifiant du nœud source.</param>
        /// <param name="targetId">L'identifiant du nœud cible.</param>
        /// <param name="weight">Le poids du lien (par défaut 1.0).</param>
        public void AddLink(int sourceId, int targetId, double weight = 1.0)
        {
            // Recherche ou création du nœud source
            Node source = Nodes.FirstOrDefault(n => n.Id == sourceId) ?? new Node(sourceId);
            if (!Nodes.Contains(source)) Nodes.Add(source);

            // Recherche ou création du nœud cible
            Node target = Nodes.FirstOrDefault(n => n.Id == targetId) ?? new Node(targetId);
            if (!Nodes.Contains(target)) Nodes.Add(target);

            // Vérifie si le lien existe déjà pour éviter les doublons
            if ((!isDirected && !Links.Any(l => l.StartNode == target && l.EndNode == source) || (isDirected && !Links.Any(l => l.StartNode == source && l.EndNode == target))))
            {
                // Ajoute le lien entre les nœuds
                Links.Add(new Link(source, target, weight));

                // Ajoute le successeur au nœud source
                source.AddSuccessor(target);
                if (!isDirected)
                {
                    // Si le graphe est non orienté, ajoute également le successeur au nœud cible
                    target.AddSuccessor(source);
                }
            }
        }


        /// <summary>
        /// Construit le graphe à partir d'une liste d'adjacence.
        /// La liste d'adjacence est un dictionnaire où chaque clé représente un nœud source,
        /// et la valeur est une liste de tuples représentant les nœuds cibles et les poids des liens.
        /// </summary>
        /// <param name="adjacencyList">La liste d'adjacence sous forme de dictionnaire.</param>
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


        /// <summary>
        /// Construit le graphe à partir d'une matrice d'adjacence.
        /// La matrice d'adjacence est une matrice carrée où chaque élément représente un poids de lien.
        /// Un poids de 0 signifie qu'il n'y a pas de lien entre les nœuds.
        /// </summary>
        /// <param name="adjacencyMatrix">La matrice d'adjacence.</param>
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


        /// <summary>
        /// Effectue un parcours en largeur (BFS) à partir d'un nœud donné.
        /// Retourne la liste des nœuds visités dans l'ordre du parcours.
        /// </summary>
        /// <param name="startId">L'identifiant du nœud de départ.</param>
        /// <returns>La liste des nœuds visités.</returns>
        public List<Node> BreadthFirstSearch(int startId)
        {
            HashSet<Node> visited = new HashSet<Node>();
            Queue<Node> queue = new Queue<Node>();
            Node startNode = Nodes.First(n => n.Id == startId);
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


        /// <summary>
        /// Effectue un parcours en profondeur (DFS) à partir d'un nœud donné.
        /// Retourne la liste des nœuds visités dans l'ordre du parcours.
        /// </summary>
        /// <param name="startId">L'identifiant du nœud de départ.</param>
        /// <returns>La liste des nœuds visités.</returns>
        public List<Node> DepthFirstSearch(int startId)
        {
            HashSet<Node> visited = new HashSet<Node>();
            DFS(Nodes.First(n => n.Id == startId), visited);

            return visited.ToList();
        }


        /// <summary>
        /// Méthode récursive pour le parcours en profondeur (DFS).
        /// </summary>
        /// <param name="node">Le nœud actuel.</param>
        /// <param name="visited">L'ensemble des nœuds déjà visités.</param>
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


        /// <summary>
        /// Retourne l'ordre du graphe, c'est-à-dire le nombre de nœuds.
        /// </summary>
        /// <returns>Le nombre de nœuds dans le graphe.</returns>
        public int GetOrder()
        {
            return Nodes.Count;
        }


        /// <summary>
        /// Retourne la taille du graphe, c'est-à-dire le nombre de liens.
        /// </summary>
        /// <returns>Le nombre de liens dans le graphe.</returns>
        public int GetSize()
        {
            return Links.Count;
        }


        /// <summary>
        /// Vérifie si le graphe est connexe.
        /// Un graphe est connexe si tous les nœuds sont connectés directement ou indirectement.
        /// </summary>
        /// <returns>True si le graphe est connexe, False sinon.</returns>
        public bool IsConnected()
        {
            if (Nodes.Count == 0) return true;

            HashSet<Node> visited = new HashSet<Node>();
            DFS(Nodes[0], visited);

            return visited.Count == Nodes.Count;
        }


        /// <summary>
        /// Vérifie si le graphe contient un cycle.
        /// Un cycle est un chemin qui commence et se termine au même nœud.
        /// </summary>
        /// <returns>True si le graphe contient un cycle, False sinon.</returns>
        public bool ContainsCycle()
        {
            HashSet<Node> visited = new HashSet<Node>();
            HashSet<Node> recursionStack = new HashSet<Node>();

            foreach (Node node in Nodes)
            {
                if (ContainsCycleTool(node, visited, recursionStack))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Méthode récursive pour détecter un cycle dans le graphe.
        /// Utilise une pile de récursion pour suivre les nœuds visités dans le chemin actuel.
        /// </summary>
        /// <param name="node">Le nœud actuel.</param>
        /// <param name="visited">L'ensemble des nœuds déjà visités.</param>
        /// <param name="recursionStack">L'ensemble des nœuds dans le chemin actuel de la récursion.</param>
        /// <returns>True si un cycle est détecté, False sinon.</returns>
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


        /// <summary>
        /// Retourne une description textuelle du graphe.
        /// Inclut des informations sur l'orientation, la pondération, l'ordre, la taille,
        /// la connexité, la présence de cycles, ainsi que les parcours DFS et BFS.
        /// </summary>
        /// <returns>Une chaîne de caractères décrivant le graphe.</returns>
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
            foreach (Node node in Nodes)
            {
                description += "\n  - Node " + node.Id;
            }

            description += "\n\nLinks:";
            foreach (var link in Links)
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
}