using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using CodeSharp.Samples.WebAPI.Models;
using CodeSharp.Samples.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CodeSharp.Samples.WebAPI.Controllers;

[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    private readonly ITestService<Test, TestLog, Guid> _testingService;

    public TestingController(ITestService<Test, TestLog, Guid> testingService)
    {
        _testingService = testingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTest()
    {
        var test = await _testingService.GetTestAsync(Guid.Parse("02465560-66C5-480B-8F80-A8F0AC692AC9"));
        if (test is null)
        {
            return BadRequest();
        }

        return Ok(new TestSample { Description = test.Description, Id = test.Id, InitialUserCode = "namespace CodeSharp.Playground;\n\npublic class TreeNode<T>\n{\n    public T Value { get; set; }\n    public TreeNode<T>? Left { get; set; }\n    public TreeNode<T>? Right { get; set; }\n\n    public TreeNode(T x)\n    {\n        Value = x;\n    }\n}\n\npublic class BinaryTree<T>\n{\n    private readonly TreeNode<T>? _root;\n\n    public BinaryTree(TreeNode<T>? rootNode)\n    {\n        _root = rootNode;\n    }\n\n    public IList<T> BreadthFirstTraversal()\n    {\n        var traversalResult = new List<T>();\n\n        if (_root is null)\n        {\n            return traversalResult;\n        }\n\n        var queue = new Queue<TreeNode<T>>();\n        queue.Enqueue(_root);\n\n        while (queue.Count > 0)\n        {\n            var currentNode = queue.Dequeue();\n            traversalResult.Add(currentNode.Value);\n\n            if (currentNode.Left != null)\n            {\n                queue.Enqueue(currentNode.Left);\n            }\n\n            if (currentNode.Right != null)\n            {\n                queue.Enqueue(currentNode.Right);\n            }\n        }\n\n        return traversalResult;\n    }\n}" });
    }

    [HttpPost]
    public async Task<IActionResult> Test([FromBody] TestingRequest request)
    {
        var test = await _testingService.GetTestAsync(Guid.Parse("02465560-66C5-480B-8F80-A8F0AC692AC9"));
        if (test is null || test.Id != request.TestId)
        {
            return BadRequest();
        }

        var testingResult = await _testingService.ExecuteTestByIdAsync(request.TestId, request.Code);

        return Ok(testingResult);
    }
}
