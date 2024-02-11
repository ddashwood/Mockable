using FakeItEasy;
using Mockable.Core;
using Mockable.Core.Exceptions;

namespace Mockable.FakeItEasy;

public class FakeItEasyCreator : IMockCreator
{
    public object GetMockOf(Type type, out object mockConfigurator)
    {
        var aType = typeof(A);
        var fakeMethod = aType.GetMethod("Fake", new Type[0])
            ?? throw new MockableException("Error getting Fake() method");
        var fakeGenericMethod = fakeMethod.MakeGenericMethod(type);

        var fake = fakeGenericMethod.Invoke(null, null)!;
        mockConfigurator = fake;
        return fake;
    }
}
