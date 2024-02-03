using Mockable.Core.Tests.TestDependencies;

namespace Mockable.Core.Tests.TestServices;

internal class ServiceWithTwoConstructorParameters
{
    public IDependency1 Dependency1 { get; }
    public IDependency2 Dependency2 { get; }

    public ServiceWithTwoConstructorParameters(IDependency1 dependency1, IDependency2 dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }
}

public class ServiceWithTwoConstructorParameterConfiguratorsWithMissingParameter
{
    public object Dependency1Configurator { get; set; } = null!;
}
