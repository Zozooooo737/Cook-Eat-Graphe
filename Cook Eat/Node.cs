using System.Xml.Linq;

namespace Cook_Eat
{
    internal class Node
    {
        private int id;

        private List<int> successors;


        public Node(int id)
        {
            this.id = id;
            successors = [];
        }



        public int Id
        {
            get { return id; }
        }


        public List<int> Successors
        {
            get { return successors; }
        }


        public void NewSuccessor(int id)
        {
            for (int i = 0; i < successors.Count; i++)
            {
                if (successors[i] == id)
                {
                    return;
                }
            }
            successors.Add(id);
        }
    }
}
