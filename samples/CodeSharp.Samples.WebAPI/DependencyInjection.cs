using CodeSharp.Core.Models;

namespace CodeSharp.Samples.WebAPI;

public static class DependencyInjection
{
    public static IServiceProvider InitDbData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MyContext>();


        var testSampleExists = context.Tests.Any(t => t.Id == Guid.Parse("02465560-66C5-480B-8F80-A8F0AC692AC9"));
        if (!testSampleExists)
        {
            var testSample = new Test
            {
                Id = Guid.Parse("02465560-66C5-480B-8F80-A8F0AC692AC9"),
                Name = "Breadth-first traversal (BFT)",
                Description = "In this task, you'll design and implement a function to perform a breadth-first traversal (BFT) of a binary tree. This traversal technique involves visiting all the nodes at the current depth level before moving on to the nodes at the next depth level. Your implementation should efficiently navigate through the tree, ensuring each node is visited exactly once, and print the value of each node in the order it's visited. This task will test your understanding of tree data structures, queues, and algorithmic problem-solving.",
                Tests = "using CodeSharp.Playground;\n\nnamespace CodeSharp.Tests;\n\npublic class BinaryTreeTests\n{\n    [Theory]\n    [InlineData(new int[] { 5 }, new int[] { 5 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]\n    [InlineData(new int[] { 1 }, new int[] { 1 })]\n    [InlineData(new int[] { 1, 2 }, new int[] { 1, 2 })]\n    [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]\n    [InlineData(new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]\n    public void BreadthFirstTraversal_ShouldReturnCorrectTraversalOrder(int[] inputValues, int[] expectedTraversal)\n    {\n        // Arrange\n        var root = CreateTreeFromArray(inputValues);\n        var binaryTree = new BinaryTree<int>(root);\n\n        // Act\n        var result = binaryTree.BreadthFirstTraversal();\n\n        // Assert\n        Assert.Equal(expectedTraversal, result);\n    }\n\n    [Fact]\n    public void BreadthFirstTraversal_EmptyTree_ShouldReturnEmptyList()\n    {\n        // Arrange\n        var binaryTree = new BinaryTree<int>(null);\n\n        // Act\n        var result = binaryTree.BreadthFirstTraversal();\n\n        // Assert\n        Assert.Empty(result);\n    }\n\n    private static TreeNode<int>? CreateTreeFromArray(int[] values)\n    {\n        if (values.Length == 0)\n        {\n            return default;\n        }\n\n        var root = new TreeNode<int>(values[0]);\n        var queue = new Queue<TreeNode<int>>();\n        queue.Enqueue(root);\n        var i = 1;\n\n        while (i < values.Length)\n        {\n            var current = queue.Dequeue();\n\n            var leftValue = (i < values.Length) ? values[i++] : (int?)null;\n            if (leftValue.HasValue)\n            {\n                current.Left = new TreeNode<int>(leftValue.Value);\n                queue.Enqueue(current.Left);\n            }\n\n            var rightValue = (i < values.Length) ? values[i++] : (int?)null;\n            if (rightValue.HasValue)\n            {\n                current.Right = new TreeNode<int>(rightValue.Value);\n                queue.Enqueue(current.Right);\n            }\n        }\n\n        return root;\n    }\n}"
            };
            context.Tests.Add(testSample);
            context.SaveChanges();
        }

        return services;
    }
}
