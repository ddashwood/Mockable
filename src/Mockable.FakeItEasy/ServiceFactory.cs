using Mockable.Core;

namespace Mockable.FakeItEasy;

public class ServiceFactory : ServiceFactoryBase
{
    public ServiceFactory()
        :base(new FakeItEasyCreator())
    {
    }
}
