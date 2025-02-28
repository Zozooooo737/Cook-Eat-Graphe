namespace Cook_Eat
{
    /// <summary>
    /// Représente un nœud (ou sommet) dans un graphe.
    /// Un nœud possède un identifiant unique et une liste de successeurs (nœuds connectés).
    /// </summary>
    public class Node
    {
        /// <summary>
        /// L'identifiant unique du nœud.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// L'ensemble des successeurs du nœud.
        /// Un successeur est un nœud directement accessible depuis ce nœud via un lien.
        /// </summary>
        public HashSet<Node> Successors { get; }



        /// <summary>
        /// Constructeur de la classe Node.
        /// </summary>
        /// <param name="id">L'identifiant unique du nœud.</param>
        public Node(int id)
        {
            Id = id;
            Successors = new HashSet<Node>();
        }



        /// <summary>
        /// Ajoute un nœud successeur à la liste des successeurs du nœud actuel.
        /// </summary>
        /// <param name="node">Le nœud à ajouter comme successeur.</param>
        public void AddSuccessor(Node node)
        {
            Successors.Add(node);
        }
    }
}