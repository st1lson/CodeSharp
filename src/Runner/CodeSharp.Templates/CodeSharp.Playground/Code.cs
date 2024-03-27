namespace CodeSharp.Playground;

public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T>? Left { get; set; }
    public TreeNode<T>? Right { get; set; }

    public TreeNode(T x)
    {
        Value = x;
    }
}

public class BinaryTree<T>
{
    private readonly TreeNode<T>? _root;

    public BinaryTree(TreeNode<T>? rootNode)
    {
        _root = rootNode;
    }

    public IList<T> BreadthFirstTraversal()
    {
        var traversalResult = new List<T>();

        if (_root is null)
        {
            return traversalResult;
        }

        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            traversalResult.Add(currentNode.Value);

            if (currentNode.Left != null)
            {
                queue.Enqueue(currentNode.Left);
            }

            if (currentNode.Right != null)
            {
                queue.Enqueue(currentNode.Right);
            }
        }

        return traversalResult;
    }
}