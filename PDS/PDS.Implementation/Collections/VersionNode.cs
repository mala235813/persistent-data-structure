namespace PDS.Implementation.Collections
{
    internal class VersionNode<T>
    {
        public VersionNode(int version, ListFatNode<T> front, ListFatNode<T> back, VersionNode<T>? parent = null)
        {
            Version = version;
            Front = front;
            Back = back;
            Parent = parent;
        }

        public int Version { get; }
        
        public VersionNode<T>? Parent { get; }

        public ListFatNode<T> Front { get; set; }
        
        public ListFatNode<T> Back { get; set; }
    }
}