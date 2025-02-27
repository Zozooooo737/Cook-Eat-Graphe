using System.Drawing;
using System.Drawing.Imaging;

class GraphVisualizer
{
    private Graph graph;
    private Bitmap bitmap;
    private Graphics graphics;
    private int width;
    private int height;
    private Dictionary<Node, PointF> nodePositions;
    private Dictionary<Node, Brush> nodeColors;
    private bool useRegularPositioning;



    public GraphVisualizer(Graph graph, bool useRegularPositioning = false, int width = 1600, int height = 1200)
    {
        this.graph = graph;
        this.width = width;
        this.height = height;
        this.useRegularPositioning = useRegularPositioning;

        bitmap = new Bitmap(width, height);
        graphics = Graphics.FromImage(bitmap);

        nodePositions = new Dictionary<Node, PointF>();

        nodeColors = new Dictionary<Node, Brush>();
        AssignRandomColors();
    }



    public void Visualize(string outputPath)
    {
        CalculateNodePositions();
        DrawGraph();
        SaveImage(outputPath);
        OpenImage(outputPath);
    }


    private void CalculateNodePositions()
    {
        if (useRegularPositioning)
        {
            CalculateRegularPositions();
        }
        else
        {
            CalculateRandomPositions();
        }
    }


    private void CalculateRandomPositions()
    {
        Random rand = new Random();
        int margin = 50;
        int minDistance = 100;
        int maxAttempts = 50;

        foreach (var node in graph.nodes)
        {
            PointF newPos;
            bool validPosition;
            int attempts = 0;

            do
            {
                newPos = new PointF(rand.Next(margin, width - margin), rand.Next(margin, height - margin));
                validPosition = true;
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

            nodePositions[node] = newPos;
        }
    }


    private void CalculateRegularPositions()
    {
        double angleIncrement = 2 * Math.PI / graph.nodes.Count;
        double radius = Math.Min(width, height) / 2.3;
        int centerX = width / 2, centerY = height / 2;
        int index = 0;

        foreach (var node in graph.nodes)
        {
            double angle = index * angleIncrement;
            nodePositions[node] = new PointF(
                (float)(centerX + radius * Math.Cos(angle)),
                (float)(centerY + radius * Math.Sin(angle))
            );
            index++;
        }
    }


    private void AssignRandomColors()
    {
        Random rand = new Random();
        foreach (var node in graph.nodes)
        {
            nodeColors[node] = new SolidBrush(Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255)));
        }
    }


    private void DrawGraph()
    {
        graphics.Clear(Color.White);

        foreach (var link in graph.links)
        {
            DrawLink(link);
        }
            
        foreach (var node in graph.nodes)
        {
            DrawNode(node);
        }
    }

    private void DrawNode(Node node)
    {
        var position = nodePositions[node];
        int totalNodes = graph.nodes.Count;
        float nodeSize = Math.Max(20, 80 - totalNodes);

        graphics.FillEllipse(nodeColors[node], position.X - nodeSize / 2, position.Y - nodeSize / 2, nodeSize, nodeSize);

        string nodeId = node.Id.ToString();
        Font font = new Font("Arial", nodeSize / 3);
        SizeF textSize = graphics.MeasureString(nodeId, font);
        graphics.DrawString(nodeId, font, Brushes.Black, position.X - textSize.Width / 2, position.Y - textSize.Height / 2);
    }
    

    private void DrawLink(Link link)
    {
        var start = nodePositions[link.StartNode];
        var end = nodePositions[link.EndNode];

        if (link.StartNode.Id != link.EndNode.Id)
        {
            graphics.DrawLine(Pens.Black, start, end);
        }
        else
        {
            graphics.DrawEllipse(Pens.Black, start.X - 20, start.Y - 20, 80, 80);
        }

        if (graph.isWeighted)
        {
            var midPoint = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            graphics.DrawString(link.Weight.ToString(), new Font("Arial", 10), Brushes.Red, midPoint);
        }

        if (graph.isDirected)
        {
            DrawArrowhead(start, end);
        }
    }


    private void DrawArrowhead(PointF start, PointF end)
    {
        float size = 6; // Taille du carré
        float offset = 28; // Distance avant la fin

        // Calcul du vecteur directeur normalisé
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;
        float length = (float)Math.Sqrt(dx * dx + dy * dy);

        // Position approximative du carré
        float squareX = end.X - dx / length * offset - size / 2;
        float squareY = end.Y - dy / length * offset - size / 2;

        // Dessin du carré
        graphics.FillRectangle(Brushes.Black, squareX, squareY, size, size);
    }


    private float Distance(PointF a, PointF b)
    {
        return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }


    private void SaveImage(string filePath)
    {
        bitmap.Save(filePath, ImageFormat.Png);
    }


    private void OpenImage(string filePath)
    {
        if (File.Exists(filePath))
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}
