namespace Mockable.Core;

public interface  IMockCreator
{
    object GetMockOf(Type type, out object mockConfigurator);
}
