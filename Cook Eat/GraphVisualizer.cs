using System.Drawing;
using System.Drawing.Imaging;

namespace Cook_Eat
{
    /// <summary>
    /// Classe s'occupant de la visualisation d'un graphe sous forme d'image.
    /// Permet de dessiner les nœuds et les liens du graphe, avec des options pour le positionnement des nœuds et la coloration.
    /// </summary>
    public class GraphVisualizer<T> 
    {
        /// <summary>
        /// Le graphe à visualiser.
        /// </summary>
        private Graph<T> graph;

        /// <summary>
        /// Bitmap utilisé pour dessiner l'image du graphe.
        /// </summary>
        private Bitmap bitmap;

        /// <summary>
        /// Objet Graphics utilisé pour dessiner sur le bitmap.
        /// </summary>
        private Graphics graphics;

        /// <summary>
        /// Largeur de l'image.
        /// </summary>
        private int width;

        /// <summary>
        /// Hauteur de l'image.
        /// </summary>
        private int height;

        /// <summary>
        /// Dictionnaire stockant les positions des nœuds sur l'image.
        /// </summary>
        private Dictionary<Node<T>, PointF> nodePositions;

        /// <summary>
        /// Dictionnaire stockant les couleurs attribuées à chaque nœud.
        /// </summary>
        private Dictionary<Node<T>, Brush> nodeColors;

        /// <summary>
        /// Indique si les nœuds doivent être positionnés de manière régulière (en cercle) ou aléatoire.
        /// </summary>
        private bool useRegularPositioning;



        /// <summary>
        /// Constructeur de la classe GraphVisualizer.
        /// </summary>
        /// <param name="graph">Le graphe à visualiser.</param>
        /// <param name="useRegularPositioning">True pour positionner les nœuds en cercle, False pour un positionnement aléatoire.</param>
        /// <param name="width">Largeur de l'image.</param>
        /// <param name="height">Hauteur de l'image.</param>
        public GraphVisualizer(Graph<T> graph, bool useRegularPositioning = false, int width = 1600, int height = 1200)
        {
            this.graph = graph;
            this.width = width;
            this.height = height;
            this.useRegularPositioning = useRegularPositioning;

            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);

            nodePositions = new Dictionary<Node<T>, PointF>();
            nodeColors = new Dictionary<Node<T>, Brush>();

            AssignRandomColors(); // Attribue des couleurs aléatoires aux nœuds
        }



        /// <summary>
        /// Visualise le graphe en générant une image et en l'enregistrant à l'emplacement spécifié.
        /// </summary>
        /// <param name="outputPath">Le chemin où enregistrer l'image.</param>
        public void Visualize(string outputPath)
        {
            CalculateNodePositions(); // Calcule les positions des nœuds
            DrawGraph(); // Dessine le graphe
            SaveImage(outputPath); // Enregistre l'image
            OpenImage(outputPath); // Ouvre l'image
        }


        /// <summary>
        /// Calcule les positions des nœuds sur l'image.
        /// Utilise un positionnement régulier (en cercle) ou aléatoire selon la configuration.
        /// </summary>
        private void CalculateNodePositions()
        {
            if (useRegularPositioning)
            {
                CalculateRegularPositions(); // Positionnement en cercle
            }
            else
            {
                CalculateRandomPositions(); // Positionnement aléatoire
            }
        }


        /// <summary>
        /// Calcule des positions aléatoires pour les nœuds, en évitant les chevauchements.
        /// </summary>
        private void CalculateRandomPositions()
        {
            Random rand = new Random();
            int margin = 50; // Marge pour éviter les bords
            int minDistance = 100; // Distance minimale entre deux nœuds
            int maxAttempts = 50; // Nombre maximal de tentatives pour trouver une position valide

            foreach (var node in graph.Nodes)
            {
                PointF newPos;
                bool validPosition;
                int attempts = 0;

                do
                {
                    newPos = new PointF(rand.Next(margin, width - margin), rand.Next(margin, height - margin));
                    validPosition = true;

                    // Vérifie que la nouvelle position est suffisamment éloignée des autres nœuds
                    foreach (var existingPos in nodePositions.Values)
                    {
                        if (Distance(newPos, existingPos) < minDistance)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                    attempts++;
                } while (!validPosition && attempts < maxAttempts);

                nodePositions[node] = newPos; // Enregistre la position du nœud
            }
        }


        /// <summary>
        /// Calcule des positions régulières pour les nœuds, en les plaçant en cercle.
        /// </summary>
        private void CalculateRegularPositions()
        {
            double angleIncrement = 2 * Math.PI / graph.Nodes.Count; // Angle entre chaque nœud
            double radius = Math.Min(width, height) / 2.3; // Rayon du cercle
            int centerX = width / 2, centerY = height / 2; // Centre de l'image
            int index = 0;

            foreach (var node in graph.Nodes)
            {
                double angle = index * angleIncrement;
                nodePositions[node] = new PointF(
                    (float)(centerX + radius * Math.Cos(angle)), // Coordonnée X
                    (float)(centerY + radius * Math.Sin(angle))  // Coordonnée Y
                );
                index++;
            }
        }


        /// <summary>
        /// Attribue des couleurs aléatoires à chaque nœud.
        /// </summary>
        private void AssignRandomColors()
        {
            Random rand = new Random();
            foreach (var node in graph.Nodes)
            {
                nodeColors[node] = new SolidBrush(Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255)));
            }
        }


        /// <summary>
        /// Dessine le graphe sur l'image.
        /// </summary>
        private void DrawGraph()
        {
            graphics.Clear(Color.White); // Efface l'image avec un fond blanc

            // Dessine les liens
            foreach (var link in graph.Links)
            {
                DrawLink(link);
            }

            // Dessine les nœuds
            foreach (var node in graph.Nodes)
            {
                DrawNode(node);
            }
        }


        /// <summary>
        /// Dessine un nœud sur l'image.
        /// </summary>
        /// <param name="node">Le nœud à dessiner.</param>
        private void DrawNode(Node<T> node)
        {
            var position = nodePositions[node];
            int totalNodes = graph.Nodes.Count;
            float nodeSize = Math.Max(20, 80 - totalNodes); // Taille du nœud en fonction du nombre total de nœuds

            // Dessine le cercle représentant le nœud
            graphics.FillEllipse(nodeColors[node], position.X - nodeSize / 2, position.Y - nodeSize / 2, nodeSize, nodeSize);

            // Ajoute l'identifiant du nœud au centre
            string nodeId = node.Id.ToString();
            Font font = new Font("Arial", nodeSize / 3);
            SizeF textSize = graphics.MeasureString(nodeId, font);
            graphics.DrawString(nodeId, font, Brushes.Black, position.X - textSize.Width / 2, position.Y - textSize.Height / 2);
        }


        /// <summary>
        /// Dessine un lien entre deux nœuds.
        /// </summary>
        /// <param name="link">Le lien à dessiner.</param>
        private void DrawLink(Link<T> link)
        {
            var start = nodePositions[link.StartNode];
            var end = nodePositions[link.EndNode];

            if (link.StartNode.Id != link.EndNode.Id)
            {
                // Dessine une ligne pour les liens normaux
                graphics.DrawLine(Pens.Black, start, end);
            }
            else
            {
                // Dessine une boucle pour les liens réflexifs
                graphics.DrawEllipse(Pens.Black, start.X - 20, start.Y - 20, 80, 80);
            }

            // Ajoute le poids du lien si le graphe est pondéré
            if (graph.isWeighted)
            {
                var midPoint = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
                graphics.DrawString(link.Weight.ToString(), new Font("Arial", 10), Brushes.Red, midPoint);
            }

            // Ajoute une flèche si le graphe est orienté
            if (graph.isDirected)
            {
                DrawArrowhead(start, end);
            }
        }


        /// <summary>
        /// Dessine une flèche (carré) à l'extrémité d'un lien orienté.
        /// </summary>
        /// <param name="start">Point de départ du lien.</param>
        /// <param name="end">Point d'arrivée du lien.</param>
        private void DrawArrowhead(PointF start, PointF end)
        {
            float size = 6; // Taille de la flèche
            float offset = 28; // Distance entre la flèche et le nœud

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            float squareX = end.X - dx / length * offset - size / 2;
            float squareY = end.Y - dy / length * offset - size / 2;

            // Dessine un carré pour représenter la flèche
            graphics.FillRectangle(Brushes.Black, squareX, squareY, size, size);
        }


        /// <summary>
        /// Calcule la distance entre deux points.
        /// </summary>
        /// <param name="a">Premier point.</param>
        /// <param name="b">Deuxième point.</param>
        /// <returns>La distance entre les deux points.</returns>
        private float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }


        /// <summary>
        /// Enregistre l'image du graphe à l'emplacement spécifié.
        /// </summary>
        /// <param name="filePath">Le chemin où enregistrer l'image.</param>
        private void SaveImage(string filePath)
        {
            bitmap.Save(filePath, ImageFormat.Png);
        }


        /// <summary>
        /// Ouvre l'image du graphe avec l'application par défaut.
        /// </summary>
        /// <param name="filePath">Le chemin de l'image à ouvrir.</param>
        private void OpenImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }
    }
}