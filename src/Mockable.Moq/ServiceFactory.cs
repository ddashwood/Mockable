using Mockable.Core;

namespace Mockable.Moq;

public class ServiceFactory : ServiceFactoryBase
{
    public ServiceFactory()
        :base(new MoqCreator())
    {
    }
}
