using CodeSharp.Core.Services;
using CodeSharp.Samples.WebAPI.Models;
using CodeSharp.Samples.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CodeSharp.Samples.WebAPI.Controllers;

[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    private readonly ITestingService _testingService;

    private static readonly Test _test = new()
    {
        Id = Guid.NewGuid(),
        Description = "In this task, you'll design and implement a function to perform a breadth-first traversal (BFT) of a binary tree. This traversal technique involves visiting all the nodes at the current depth level before moving on to the nodes at the next depth level. Your implementation should efficiently navigate through the tree, ensuring each node is visited exactly once, and print the value of each node in the order it's visited. This task will test your understanding of tree data structures, queues, and algorithmic problem-solving.",
        InitialUserCode = "namespace CodeSharp.Playground;\n\npublic class TreeNode<T>\n{\n    public T Value { get; set; }\n    public TreeNode<T>? Left { get; set; }\n    public TreeNode<T>? Right { get; set; }\n\n    public TreeNode(T x)\n    {\n        Value = x;\n    }\n}\n\npublic class BinaryTree<T>\n{\n    private readonly TreeNode<T>? _root;\n\n    public BinaryTree(TreeNode<T>? rootNode)\n    {\n        _root = rootNode;\n    }\n\n    public IList<T> BreadthFirstTraversal()\n    {\n        var traversalResult = new List<T>();\n\n        if (_root is null)\n        {\n            return traversalResult;\n        }\n\n        var queue = new Queue<TreeNode<T>>();\n        queue.Enqueue(_root);\n\n        while (queue.Count > 0)\n        {\n            var currentNode = queue.Dequeue();\n            traversalResult.Add(currentNode.Value);\n\n            if (currentNode.Left != null)\n            {\n                queue.Enqueue(currentNode.Left);\n            }\n\n            if (currentNode.Right != null)\n            {\n                queue.Enqueue(currentNode.Right);\n            }\n        }\n\n        return traversalResult;\n    }\n}",
        TestsCode = "using CodeSharp.Playground;\n\nnamespace CodeSharp.Tests;\n\npublic class BinaryTreeTests\n{\n    [Theory]\n    [InlineData(new int[] { 5 }, new int[] { 5 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]\n    [InlineData(new int[] { 1 }, new int[] { 1 })]\n    [InlineData(new int[] { 1, 2 }, new int[] { 1, 2 })]\n    [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]\n    [InlineData(new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]\n    [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]\n    public void BreadthFirstTraversal_ShouldReturnCorrectTraversalOrder(int[] inputValues, int[] expectedTraversal)\n    {\n        // Arrange\n        var root = CreateTreeFromArray(inputValues);\n        var binaryTree = new BinaryTree<int>(root);\n\n        // Act\n        var result = binaryTree.BreadthFirstTraversal();\n\n        // Assert\n        Assert.Equal(expectedTraversal, result);\n    }\n\n    [Fact]\n    public void BreadthFirstTraversal_EmptyTree_ShouldReturnEmptyList()\n    {\n        // Arrange\n        var binaryTree = new BinaryTree<int>(null);\n\n        // Act\n        var result = binaryTree.BreadthFirstTraversal();\n\n        // Assert\n        Assert.Empty(result);\n    }\n\n    private static TreeNode<int>? CreateTreeFromArray(int[] values)\n    {\n        if (values.Length == 0)\n        {\n            return default;\n        }\n\n        var root = new TreeNode<int>(values[0]);\n        var queue = new Queue<TreeNode<int>>();\n        queue.Enqueue(root);\n        var i = 1;\n\n        while (i < values.Length)\n        {\n            var current = queue.Dequeue();\n\n            var leftValue = (i < values.Length) ? values[i++] : (int?)null;\n            if (leftValue.HasValue)\n            {\n                current.Left = new TreeNode<int>(leftValue.Value);\n                queue.Enqueue(current.Left);\n            }\n\n            var rightValue = (i < values.Length) ? values[i++] : (int?)null;\n            if (rightValue.HasValue)\n            {\n                current.Right = new TreeNode<int>(rightValue.Value);\n                queue.Enqueue(current.Right);\n            }\n        }\n\n        return root;\n    }\n}"
    };

    public TestingController(ITestingService testingService)
    {
        _testingService = testingService;
    }

    [HttpGet]
    public IActionResult GetTest()
    {
        return Ok(_test);
    }

    [HttpPost]
    public async Task<IActionResult> Test([FromBody] TestingRequest request)
    {
        if (_test.Id != request.TestId)
        {
            return BadRequest();
        }

        var testingResult = await _testingService.TestAsync(request.Code, _test.TestsCode);

        return Ok(testingResult);
    }
}
