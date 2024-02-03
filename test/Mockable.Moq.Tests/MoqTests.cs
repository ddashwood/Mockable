using Moq;

namespace Mockable.Moq.Tests;

public class MoqTests
{
    public interface IDependency
    {
        int GetResult();
    }

    internal class Service
    {
        public IDependency Dependency { get; }

        public Service(IDependency dependency)
        {
            Dependency = dependency;
        }

        public int GetDoubleResult() => Dependency.GetResult() * 2;
    }

    internal class ServiceConfigurators
    {
        public Mock<IDependency> DependencyConfigurator { get; set; } = null!;
    }



    [Fact]
    public void MoqCreatedTest()
    {
        // Arrange
        var factory = new ServiceFactory();

        // Act
        var result = factory.Create<Service, ServiceConfigurators>(out var configurators);

        // Assert
        Assert.NotNull(result.Dependency);
        Assert.Same(result.Dependency, configurators.DependencyConfigurator.Object);
    }

    [Fact]
    public void MoqSetupTest()
    {
        // Arrange
        var factory = new ServiceFactory();
        var obj = factory.Create<Service, ServiceConfigurators>(out var configurators);
        configurators.DependencyConfigurator.Setup(d => d.GetResult()).Returns(3);

        // Act
        var result = obj.GetDoubleResult();

        // Assert
        Assert.Equal(6, result);
    }
}