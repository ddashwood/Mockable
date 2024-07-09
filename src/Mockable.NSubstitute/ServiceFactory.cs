using Mockable.Core;

namespace Mockable.NSubstitute;

/// <summary>
/// A Service Factory for creating services, using NSubstitute
/// for mocking dependencies
/// </summary>
public class ServiceFactory : ServiceFactoryBase
{
    /// <inheritdoc />
    public ServiceFactory()
        :base(new NSubstituteCreator())
    {
    }
}
