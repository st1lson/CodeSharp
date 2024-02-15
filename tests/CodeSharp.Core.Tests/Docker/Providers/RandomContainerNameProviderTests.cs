using CodeSharp.Core.Docker.Providers;
using Xunit;

namespace CodeSharp.Core.Tests.Docker.Providers;

public class RandomContainerNameProviderTests
{
    [Fact]
    public void GetName_ReturnsNonEmptyString()
    {
        // Arrange
        var provider = new RandomContainerNameProvider();

        // Act
        var name = provider.GetName();

        // Assert
        Assert.False(string.IsNullOrEmpty(name), "The container name should not be null or empty.");
    }

    [Fact]
    public void GetName_ReturnsConsistentNameOnSubsequentCalls()
    {
        // Arrange
        var provider = new RandomContainerNameProvider();

        // Act
        var firstNameCall = provider.GetName();
        var secondNameCall = provider.GetName();

        // Assert
        Assert.Equal(firstNameCall, secondNameCall);
    }
}
