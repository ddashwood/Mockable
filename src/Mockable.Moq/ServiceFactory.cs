using Mockable.Core;

namespace Mockable.Moq;

/// <summary>
/// A Service Factory for creating services, using Moq
/// for mocking dependencies
/// </summary>
public class ServiceFactory : ServiceFactoryBase
{
    /// <inheritdoc />
    public ServiceFactory()
        :base(new MoqCreator())
    {
    }
}
