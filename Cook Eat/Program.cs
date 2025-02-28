namespace Cook_Eat
{
    public class Program
    {
        static void Main(string[] args)
        {
            KarateExample();
        }



        static void KarateExample()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);
            g.FromAdjacencyList(ReadFileForAdjacencyList("../../../data/karate.txt"));

            Console.WriteLine("Exemple des adhérents du club associatif de ²Karaté :\n");

            Console.WriteLine(g.ToString());

            GraphVisualizer visualizer = new GraphVisualizer(g, false);
            visualizer.Visualize("karate.png");
        }



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
