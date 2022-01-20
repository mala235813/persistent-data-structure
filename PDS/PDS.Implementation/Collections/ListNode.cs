namespace PDS.Implementation.Collections
{
    internal class ListNode<T>
    {
        public ListNode(int version, T value, ListFatNode<T>? leftNode = null, ListFatNode<T>? rightNode = null)
        {
            Version = version;
            Value = value;
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public int Version { get; }

        public T Value { get; }

        public ListFatNode<T>? LeftNode { get; set; }
        
        public ListFatNode<T>? RightNode { get; set; }
    }
}