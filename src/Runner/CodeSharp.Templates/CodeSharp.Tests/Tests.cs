using CodeSharp.Playground;

namespace CodeSharp.Tests;

public class MultiplyTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 5, 10)]
    [InlineData(33, 5, 165)]
    public void Test(int x, int y, int expectedResult)
    {
        var calculator = new Calculator();
        
        var result = calculator.Multiply(x, y);
        
        Assert.Equal(expectedResult, result);
    }
}