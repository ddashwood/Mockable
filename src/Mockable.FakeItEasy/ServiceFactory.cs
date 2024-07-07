using Mockable.Core;

namespace Mockable.FakeItEasy;

/// <summary>
/// A Service Factory for creating services, using FakeItEasy
/// for mocking dependencies
/// </summary>
public class ServiceFactory : ServiceFactoryBase
{
    /// <inheritdoc />
    public ServiceFactory()
        :base(new FakeItEasyCreator())
    {
    }
}
