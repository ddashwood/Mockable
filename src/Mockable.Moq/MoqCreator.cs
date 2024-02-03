using Mockable.Core;
using Mockable.Core.Exceptions;
using Moq;

namespace Mockable.Moq;

internal class MoqCreator : IMockCreator
{
    public object GetMockOf(Type type, out object mockConfigurator)
    {
        var mockType = typeof(Mock<>).MakeGenericType(type);
        mockConfigurator = Activator.CreateInstance(mockType)
            ?? throw new MockableException($"Error creating Moq for {type.FullName}");

        var objectProperty = mockType.GetProperty("Object", type);
        if (objectProperty == null)
        {
            throw new MockableException($"Error getting Moq Object for {type.FullName}");
        }

        var obj = objectProperty.GetValue(mockConfigurator);
        if (obj == null)
        {
            throw new MockableException($"The Object returned for {type.FullName} was null");
        }
        return obj;
    }
}
