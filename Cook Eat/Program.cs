namespace Cook_Eat
{
    public class Program
    {
        static void Main(string[] args)
        {
            KarateExample();
        }



        /// <summary>
        /// Exemple d'utilisation d'un graphe représentant les adhérents d'un club de Karaté.
        /// </summary>
        static void KarateExample()
        {
            Graph<int> g = new Graph<int>(isDirected: false, isWeighted: true); 
            g.FromAdjacencyList(ReadFileForAdjacencyList("../../../data/karate.txt"));

            Console.WriteLine("Exemple des adhérents du club associatif de Karaté :\n");

            Console.WriteLine(g.ToString());

            GraphVisualizer<int> visualizer = new GraphVisualizer<int>(g, false);
            visualizer.Visualize("karate.png");

            Console.WriteLine("Résultats de Bellman-Ford:");
            var bellmanFordResult = g.BellmanFord(g.Nodes[0]);  
            if (bellmanFordResult != null)
            {
                Console.WriteLine("Chemins depuis le noeud 1:");
                AfficherCheminsBellmanFord(bellmanFordResult, g, 0); 
            }
            else
            {
                Console.WriteLine("Un cycle négatif a été détecté.");
            }

            Console.WriteLine("\nRésultats de Dijkstra:");
            AfficherCheminsDijkstra(g.Nodes[0].Id, g); 
        }


        /// <summary>
        /// Affiche les plus courts chemins entre le noeud source et tous les autres noeuds en utilisant les résultats de l'algorithme Bellman-Ford.
        /// </summary>
        /// <param name="dist">Le tableau des distances et des prédécesseurs retourné par l'algorithme Bellman-Ford.</param>
        /// <param name="g">Le graphe dans lequel les chemins sont calculés.</param>
        /// <param name="sourceId">L'ID du noeud source à partir duquel les plus courts chemins sont calculés.</param>
        public static void AfficherCheminsBellmanFord((double, int)[] dist, Graph<int> g, int sourceId)
        {
            Console.WriteLine($"Plus courts chemins depuis le noeud {sourceId + 1} (Bellman-Ford) :\n");

            foreach (var node in g.Nodes)
            {
                if (node.Id == sourceId) continue;

                List<int> chemin = new List<int>();
                int? current = node.Id - 1;
                while (current != null && current != -1)
                {
                    chemin.Insert(0, current.Value + 1);
                    current = dist[current.Value].Item2;
                }

                if (chemin.Count > 1)
                {
                    Console.WriteLine($"Noeud cible: {node.Id}, Distance: {dist[node.Id - 1].Item1}, Chemin: {string.Join(" -> ", chemin)}");
                }
                else
                {
                    Console.WriteLine($"Noeud cible: {node.Id}, inaccessible depuis {sourceId + 1}");
                }
            }
        }


        /// <summary>
        /// Affiche les plus courts chemins entre le noeud de départ et tous les autres noeuds en utilisant l'algorithme de Dijkstra.
        /// </summary>
        /// <param name="startId">L'ID du noeud de départ à partir duquel les plus courts chemins sont calculés.</param>
        /// <param name="g">Le graphe dans lequel les chemins sont calculés.</param>
        public static void AfficherCheminsDijkstra(int startId, Graph<int> g)
        {
            (double, int)[] dist = g.Dijkstra(g.Nodes[startId]);

            Console.WriteLine($"Plus courts chemins depuis le noeud {startId + 1} (Dijkstra) :\n");

            foreach (var node in g.Nodes)
            {
                if (node.Id == startId) continue;

                List<int> chemin = new List<int>();
                int? current = node.Id - 1;
                while (current != null && current != -1)
                {
                    chemin.Insert(0, current.Value + 1); 
                    current = dist[current.Value].Item2; 
                }

                if (chemin.Count > 1)
                {
                    Console.WriteLine($"Noeud cible: {node.Id}, Distance: {dist[node.Id - 1].Item1}, Chemin: {string.Join(" -> ", chemin)}");
                }
                else
                {
                    Console.WriteLine($"Noeud cible: {node.Id}, inaccessible depuis {startId + 1}");
                }
            }
        }


        /// <summary>
        /// Lit un fichier texte pour le transformer en une liste d'adjacence.
        /// </summary>
        /// <param name="path">Chemin du fichier contenant les données.</param>
        /// <returns>Un dictionnaire représentant la liste d'adjacence.</returns>
        private static Dictionary<int, List<(int, int)>> ReadFileForAdjacencyList(string path)
        {
            Dictionary<int, List<(int, int)>> adjacencyList = new Dictionary<int, List<(int, int)>>();
            string[] lines = File.ReadAllLines(path);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(" ");
                int source = int.Parse(parts[0]);

                if (!adjacencyList.ContainsKey(source))
                {
                    adjacencyList[source] = new List<(int, int)>();
                }
                if (parts.Length > 1)
                {
                    int weight = 1;
                    if (parts.Length > 2)
                    {
                        weight = int.Parse(parts[2]);
                    }
                    int target = int.Parse(parts[1]);
                    adjacencyList[source].Add((target, weight));
                }
            }
            return adjacencyList;
        }


        /// <summary>
        /// Lit un fichier texte pour le transformer en une matrice d'adjacence.
        /// </summary>
        /// <param name="path">Chemin du fichier contenant les données.</param>
        /// <returns>Un dictionnaire représentant la liste d'adjacence.</returns>
        private static int[,] ReadFileForAdjacencyMatrix(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[] header = lines[0].Split(" ");

            int size = int.Parse(header[0]); // Nombre de nœuds
            int[,] adjacencyMatrix = new int[size, size];

            // Lecture des liens à partir de la deuxième ligne
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(" ");
                int target = int.Parse(parts[0]);
                int source = int.Parse(parts[1]);
                int weight = 1;

                if (parts.Length > 2)
                {
                    weight = int.Parse(parts[2]);
                }

                adjacencyMatrix[source - 1, target - 1] = weight;
            }
            return adjacencyMatrix;
        }
    }
}
