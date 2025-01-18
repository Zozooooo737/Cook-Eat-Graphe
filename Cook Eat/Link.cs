namespace Cook_Eat
{
    internal class Link
    {
        private int node1;
        private int node2;


        public Link(int node1, int node2) 
        {
            this.node1 = node1;
            this.node2 = node2;
        }



        public int Node1
        {
            get { return node1; }
        }

        public int Node2
        {
            get { return node2; }
        }
    }
}
