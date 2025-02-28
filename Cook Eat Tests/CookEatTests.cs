using System.Xml.Linq;

namespace Cook_Eat
{
    public class CookEatTests
    {
        [Fact]
        public void Graph_Constructor_ShouldInitializeProperties()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);

            Assert.NotNull(g.Nodes);
            Assert.NotNull(g.Links);
            Assert.False(g.isDirected);
            Assert.False(g.isWeighted);
        }


        [Fact]
        public void Graph_AddNode_ShouldAddNode()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);

            g.AddNode(1);

            Assert.Equal(1, g.Nodes.Count);
            Assert.Equal(1, g.Nodes[0].Id);
        }


        [Fact]
        public void Graph_AddLink_ShouldAddLink()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);
            g.AddNode(1);
            g.AddNode(2);

            g.AddLink(1, 2);

            Assert.Equal(1, g.Links.Count);
            Assert.Equal(1, g.Links[0].StartNode.Id);
            Assert.Equal(2, g.Links[0].EndNode.Id);
        }


        [Fact]
        public void Graph_FromAdjacencyList_ShouldBuildGraph()
        {
            Graph g = new Graph(isDirected: false, isWeighted: true);
            var adjacencyList = new Dictionary<int, List<(int, int)>>
            {
                { 1, new List<(int, int)> { (2, 5), (3, 10) } },
                { 2, new List<(int, int)> { (3, 2) } }
            };

            g.FromAdjacencyList(adjacencyList);

            Assert.Equal(3, g.Nodes.Count);
            Assert.Equal(3, g.Links.Count);
        }


        [Fact]
        public void Graph_FromAdjacencyMatrix_ShouldBuildGraph()
        {
            Graph g = new Graph(isDirected: false, isWeighted: true);
            int[,] adjacencyMatrix = new int[,]
            {
                { 0, 5, 10 },
                { 5, 0, 2 },
                { 10, 2, 0 }
            };

            g.FromAdjacencyMatrix(adjacencyMatrix);

            Assert.Equal(3, g.Nodes.Count);
            Assert.Equal(3, g.Links.Count);
        }


        [Fact]
        public void Graph_IsConnected_ShouldReturnTrueForConnectedGraph()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);
            g.AddNode(1);
            g.AddNode(2);
            g.AddLink(1, 2);

            bool isConnected = g.IsConnected();

            Assert.True(isConnected);
        }


        [Fact]
        public void Graph_ContainsCycle_ShouldReturnTrueForCyclicGraph()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);
            g.AddNode(1);
            g.AddNode(2);
            g.AddNode(3);
            g.AddLink(1, 2);
            g.AddLink(2, 3);
            g.AddLink(3, 1);

            bool containsCycle = g.ContainsCycle();

            Assert.True(containsCycle);
        }


        [Fact]
        public void Node_Constructor_ShouldInitializeProperties()
        {
            Node node = new Node(1);

            Assert.Equal(1, node.Id);
            Assert.NotNull(node.Successors);
            Assert.Equal(0, node.Successors.Count);
        }


        [Fact]
        public void Node_AddSuccessor_ShouldAddSuccessor()
        {
            Node node1 = new Node(1);
            Node node2 = new Node(2);

            node1.AddSuccessor(node2);

            Assert.Equal(1, node1.Successors.Count);
            Assert.True(node1.Successors.Contains(node2));
        }


        [Fact]
        public void Link_Constructor_ShouldInitializeProperties()
        {
            Node node1 = new Node(1);
            Node node2 = new Node(2);

            Link link = new Link(node1, node2, 5.0);

            Assert.Equal(node1, link.StartNode);
            Assert.Equal(node2, link.EndNode);
            Assert.Equal(5.0, link.Weight);
        }


        [Fact]
        public void GraphVisualizer_Constructor_ShouldInitializeProperties()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);

            GraphVisualizer visualizer = new GraphVisualizer(g);

            Assert.NotNull(visualizer);
        }


        [Fact]
        public void GraphVisualizer_Visualize_ShouldCreateImageFile()
        {
            Graph g = new Graph(isDirected: false, isWeighted: false);
            g.AddNode(1);
            g.AddNode(2);
            g.AddLink(1, 2);
            GraphVisualizer visualizer = new GraphVisualizer(g);
            string filePath = "test_graph.png";

            visualizer.Visualize(filePath);

            Assert.True(File.Exists(filePath));
        }
    }
}
