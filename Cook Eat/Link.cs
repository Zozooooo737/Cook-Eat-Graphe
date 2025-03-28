namespace Cook_Eat
{
    /// <summary>
    /// Représente un lien (ou arête) entre deux nœuds dans un graphe.
    /// Un lien peut être pondéré (avec un poids) ou non pondéré.
    /// </summary>
    public class Link<T>
    {
        /// <summary>
        /// Le nœud de départ du lien.
        /// </summary>
        public Node<T> StartNode { get; }

        /// <summary>
        /// Le nœud d'arrivée du lien.
        /// </summary>
        public Node<T> EndNode { get; }

        /// <summary>
        /// Le poids du lien. Par défaut, il est égal à 1.0 pour un graphe non pondéré.
        /// </summary>
        public double Weight { get; }



        /// <summary>
        /// Constructeur de la classe Link.
        /// </summary>
        /// <param name="startNode">Le nœud de départ du lien.</param>
        /// <param name="endNode">Le nœud d'arrivée du lien.</param>
        /// <param name="weight">Le poids du lien (par défaut 1.0).</param>
        public Link(Node<T> startNode, Node<T> endNode, double weight)
        {
            StartNode = startNode;
            EndNode = endNode;
            Weight = weight;
        }
    }
}