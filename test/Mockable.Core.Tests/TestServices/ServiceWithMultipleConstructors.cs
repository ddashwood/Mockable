using Mockable.Core.Tests.TestDependencies;

namespace Mockable.Core.Tests.TestServices;

internal class ServiceWithMultipleConstructors
{
    public ServiceWithMultipleConstructors(IDependency1 dependency1, IDependency2 dependency2)
    {
        CorrectConstructorWasCalled = true;
    }

    public ServiceWithMultipleConstructors(IDependency1 dependency)
    {
        
    }

    public bool CorrectConstructorWasCalled { get; set; }
}
