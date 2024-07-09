using NSubstitute;

namespace Mockable.NSubstitute.Tests;

public class NSubstituteTests
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
        public IDependency DependencyConfigurator { get; set; } = null!;
    }

    [Fact]
    public void NSubstituteCreatedTest()
    {
        // Arrange
        var factory = new ServiceFactory();

        // Act
        var result = factory.Create<Service, ServiceConfigurators>(out var configurators);

        // Assert
        Assert.NotNull(result.Dependency);
        Assert.Same(result.Dependency, configurators.DependencyConfigurator);
    }

    [Fact]
    public void NSubstituteSetupTest()
    {
        // Arrange
        var factory = new ServiceFactory();
        var obj = factory.Create<Service, ServiceConfigurators>(out var configurators);
        configurators.DependencyConfigurator.GetResult().Returns(3);

        // Act
        var result = obj.GetDoubleResult();

        // Assert
        Assert.Equal(6, result);
    }
}