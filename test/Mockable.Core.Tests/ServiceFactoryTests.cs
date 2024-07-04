using Mockable.Core.Tests.TestDependencies;
using Mockable.Core.Tests.TestServices;
using Moq;

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

    [Fact]
    public void ConfiguratorWithMissingParameterTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();
        Mock<IDependency1> mock1;
        Mock<IDependency2> mock2;
        object configurator1 = mock1 = new();
        object configurator2 = mock2 = new();
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency1), out configurator1)).Returns(mock1.Object);
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency2), out configurator2)).Returns(mock2.Object);

        // Act
        var result = factory.Create<ServiceWithTwoConstructorParameters, ServiceWithTwoConstructorParametersConfiguratorsWithMissingParameter>(out var configurators);

        // Assert
        Assert.IsType<ServiceWithTwoConstructorParameters>(result);
        Assert.Same(mock1.Object, result.Dependency1);
        Assert.Same(mock2.Object, result.Dependency2);
        Assert.Same(configurator1, configurators.Dependency1Configurator);

        // There is no Dependency2Configurator on the configurators object
        // Assert.Same(configurator2, configurators.Dependency2Configurator);
    }

    [Fact]
    public void ConfiguratorWithNamedParameterTest()
    {
        // Arrange
        var factory = new ServiceFactoryConcrete();
        Mock<IDependency1> mock1;
        IDependency2 mock2;
        object configurator1 = mock1 = new();
        mock2 = new Mock<IDependency2>().Object;
        factory.MockCreatorMock.Setup(m => m.GetMockOf(typeof(IDependency1), out configurator1)).Returns(mock1.Object);

        // Act
        var result = factory.Create<ServiceWithTwoConstructorParameters, ServiceWithTwoConstructorParametersConfiguratorsWithMissingParameter>
            (out var configurators, new NamedParameter { Name = "dependency2", Value = mock2 });

        // Assert
        Assert.IsType<ServiceWithTwoConstructorParameters>(result);
        Assert.Same(mock1.Object, result.Dependency1);
        Assert.Same(mock2, result.Dependency2);
        Assert.Same(configurator1, configurators.Dependency1Configurator);

        // There is no Dependency2Configurator on the configurators object
        // Assert.Same(configurator2, configurators.Dependency2Configurator);
    }
}