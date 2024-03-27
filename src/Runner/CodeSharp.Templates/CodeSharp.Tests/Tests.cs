using CodeSharp.Playground;

namespace CodeSharp.Tests;

public class BinaryTreeTests
{
    [Theory]
    [InlineData(new int[] { 5 }, new int[] { 5 })]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [InlineData(new int[] { 1 }, new int[] { 1 })]
    [InlineData(new int[] { 1, 2 }, new int[] { 1, 2 })]
    [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
    [InlineData(new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
    public void BreadthFirstTraversal_ShouldReturnCorrectTraversalOrder(int[] inputValues, int[] expectedTraversal)
    {
        // Arrange
        var root = CreateTreeFromArray(inputValues);
        var binaryTree = new BinaryTree<int>(root);

        // Act
        var result = binaryTree.BreadthFirstTraversal();

        // Assert
        Assert.Equal(expectedTraversal, result);
    }

    [Fact]
    public void BreadthFirstTraversal_EmptyTree_ShouldReturnEmptyList()
    {
        // Arrange
        var binaryTree = new BinaryTree<int>(null);

        // Act
        var result = binaryTree.BreadthFirstTraversal();

        // Assert
        Assert.Empty(result);
    }

    private static TreeNode<int>? CreateTreeFromArray(int[] values)
    {
        if (values.Length == 0)
        {
            return default;
        }

        var root = new TreeNode<int>(values[0]);
        var queue = new Queue<TreeNode<int>>();
        queue.Enqueue(root);
        var i = 1;

        while (i < values.Length)
        {
            var current = queue.Dequeue();

            var leftValue = (i < values.Length) ? values[i++] : (int?)null;
            if (leftValue.HasValue)
            {
                current.Left = new TreeNode<int>(leftValue.Value);
                queue.Enqueue(current.Left);
            }

            var rightValue = (i < values.Length) ? values[i++] : (int?)null;
            if (rightValue.HasValue)
            {
                current.Right = new TreeNode<int>(rightValue.Value);
                queue.Enqueue(current.Right);
            }
        }

        return root;
    }
}