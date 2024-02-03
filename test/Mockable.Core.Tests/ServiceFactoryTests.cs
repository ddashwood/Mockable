using Mockable.Core.Tests.TestDependencies;
using Mockable.Core.Tests.TestServices;

namespace Mockable.Core.Tests;

public class ServiceFactoryTests
{
    [Fact]
    public void NoConstructorParameetersTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();

        // Act
        var result = factory.Create<ServiceWithNoConstructorParameters>();

        // Assert
        Assert.IsType<ServiceWithNoConstructorParameters>(result);
    }

    [Fact]
    public void ConstructorParameetersTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();
        Mock<IDependency1> mock;
        object configurator = mock = new();
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency1), out configurator)).Returns(mock.Object);

        // Act
        var result = factory.Create<ServiceWithOneConstructorParameter>();

        // Assert
        Assert.IsType<ServiceWithOneConstructorParameter>(result);
        Assert.Same(mock.Object, result.Dependency1);
    }

    [Fact]
    public void ConfiguratorWithSuffixTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();
        Mock<IDependency1> mock;
        object configurator = mock = new();
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency1), out configurator)).Returns(mock.Object);

        // Act
        var result = factory.Create<ServiceWithOneConstructorParameter, ServiceWithOneConstructorParameterConfiguratorsWithSuffix>(out var configurators);

        // Assert
        Assert.IsType<ServiceWithOneConstructorParameter>(result);
        Assert.Same(mock.Object, result.Dependency1);
        Assert.Same(configurator, configurators.Dependency1Configurator);
    }

    [Fact]
    public void ConfiguratorWithoutSuffixTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();
        Mock<IDependency1> mock;
        object configurator = mock = new();
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency1), out configurator)).Returns(mock.Object);

        // Act
        var result = factory.Create<ServiceWithOneConstructorParameter, ServiceWithOneConstructorParameterConfiguratorsWithoutSuffix>(out var configurators);

        // Assert
        Assert.IsType<ServiceWithOneConstructorParameter>(result);
        Assert.Same(mock.Object, result.Dependency1);
        Assert.Same(configurator, configurators.Dependency1);
    }
}