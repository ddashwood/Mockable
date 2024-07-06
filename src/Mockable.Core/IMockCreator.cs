namespace Mockable.Core;

/// <summary>
/// Classes that implement this interface are able to create mocks.
/// </summary>
public interface  IMockCreator
{
    /// <summary>
    /// Gets a Mock object of the given base type.
    /// </summary>
    /// <param name="type">The base type of the mock object to be created.</param>
    /// <param name="mockConfigurator">An object which can be used to configure the mock.</param>
    /// <returns>The newly-created mock.</returns>
    object GetMockOf(Type type, out object mockConfigurator);
}
