using Mockable.Core.Tests.TestDependencies;

namespace Mockable.Core.Tests.TestServices;

internal class ServiceWithAmbiguousConstructorChoice
{
    public ServiceWithAmbiguousConstructorChoice(IDependency1 dependency1)
    {
        
    }

    public ServiceWithAmbiguousConstructorChoice(IDependency1 dependency1, IDependency2 dependency)
    {
        
    }

    public ServiceWithAmbiguousConstructorChoice(IDependency1 firstDependency1, IDependency1 secondDependency1)
    {
        
    }
}
