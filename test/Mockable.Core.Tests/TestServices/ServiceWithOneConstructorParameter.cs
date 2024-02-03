using Mockable.Core.Tests.TestDependencies;

namespace Mockable.Core.Tests.TestServices;

public class ServiceWithOneConstructorParameter
{
    public IDependency1 Dependency1 { get; }

    public ServiceWithOneConstructorParameter(IDependency1 dependency1)
    {
        Dependency1 = dependency1;
    }
}

public class ServiceWithOneConstructorParameterConfiguratorsWithSuffix
{
    public object Dependency1Configurator { get; set; } = null!;
}

public class ServiceWithOneConstructorParameterConfiguratorsWithoutSuffix
{
    public object Dependency1 { get; set; } = null!;
}