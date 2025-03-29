namespace Cook_Eat
{
    /// <summary>
    /// Représente un nœud (ou sommet) dans un graphe.
    /// Un nœud possède un identifiant unique et une liste de successeurs (nœuds connectés).
    /// </summary>
    public class Node<T>
    {
        /// <summary>
        /// L'identifiant unique du nœud.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Le groupe auquel le noeud appartient.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// La description du noeud, ce qu'il représente.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// L'ensemble des successeurs du nœud.
        /// Un successeur est un nœud directement accessible depuis ce nœud via un lien.
        /// </summary>
        public HashSet<Node<T>> Successors { get; }



        /// <summary>
        /// Constructeur de la classe Node.
        /// </summary>
        /// <param name="id">L'identifiant unique du nœud.</param>
        /// <param name="group">Le groupe du noeud.</param>
        /// <param name="description">La descritpion du noeud.</param>
        public Node(int id, string group, string description)
        {
            Id = id;
            Group = group;
            Description = description;
            Successors = new HashSet<Node<T>>();
        }



        /// <summary>
        /// Ajoute un nœud successeur à la liste des successeurs du nœud actuel.
        /// </summary>
        /// <param name="node">Le nœud à ajouter comme successeur.</param>
        public void AddSuccessor(Node<T> node)
        {
            Successors.Add(node);
        }
    }
}