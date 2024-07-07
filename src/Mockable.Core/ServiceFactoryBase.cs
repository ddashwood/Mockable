using Mockable.Core.Exceptions;
using System.Reflection;

namespace Mockable.Core;

/// <summary>
/// A class from which Service Factories must inherit. Each Service Factory generates services
/// using a specific mocking framework. This class contains shared functionality which is 
/// not dependent on the specific mocking framework, which will generate those services.
/// </summary>
public abstract class ServiceFactoryBase
{
    private readonly IMockCreator _mockCreator;

    /// <summary>
    /// Creates a Service Factory.
    /// </summary>
    /// <param name="mockCreator">The Mock Creator to be used by this Service Factory.</param>
    public ServiceFactoryBase(IMockCreator mockCreator)
    {
        _mockCreator = mockCreator;
    }

    /// <summary>
    /// Creates a Service.
    /// </summary>
    /// <typeparam name="T">The data type of the Service to create.</typeparam>
    /// <typeparam name="TConfigurators">
    /// The data type of an object which is able to hold configurators for some or all of the Service's constructor parameters.
    /// </typeparam>
    /// <param name="configurators">An object which holds configurators for some or all of the Service's constructor parameters.</param>
    /// <param name="namedParameters">
    /// Any constructor parameters for the Service whose values are being supplied by the test, and not by Mockable.
    /// </param>
    /// <returns>The newly-created Service.</returns>
    public T Create<T, TConfigurators>(out TConfigurators configurators, params NamedParameter[] namedParameters) where TConfigurators : new()
    {
        configurators = new(); 
        return Create<T>(configurators, namedParameters);
    }

    /// <summary>
    /// Creates a Service.
    /// </summary>
    /// <typeparam name="T">The data type of the Service to create.</typeparam>
    /// <param name="namedParameters">
    /// Any constructor parameters for the Service whose values are being supplied by the test, and not by Mockable.
    /// </param>
    /// <returns>The newly-created Service.</returns>
    public T Create<T>(params NamedParameter[] namedParameters)
    {
        return Create<T>(null, namedParameters);
    }

    private T Create<T>(object? configurators, NamedParameter[] namedParameters)
    {
        var constructor = GetBestPublicConstructor<T>();
        var obj = Construct<T>(constructor, configurators, namedParameters);
        return obj;
    }

    private ConstructorInfo GetBestPublicConstructor<T>()
    {
        var type = typeof(T);
        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            throw new MockableException($"No constructor found for class {type.FullName}");
        }

        if (constructors.Length == 1)
        {
            return constructors[0];
        }

        var sortedConstructors = constructors.OrderByDescending(c => c.GetParameters().Length).ToList();
        if (sortedConstructors[0].GetParameters().Length == sortedConstructors[1].GetParameters().Length)
        {
            throw new MockableException($"Ambiguous choice of constructor for class {type.FullName} - multiple constructors all with {sortedConstructors[0].GetParameters().Length} parameters");
        }
        return sortedConstructors[0];
    }

    private T Construct<T>(ConstructorInfo constructor, object? configurators, NamedParameter[] namedParameters)
    {
        var constructorParams = constructor.GetParameters();
        var constructorArgs = new object?[constructorParams.Length];

        for (int i = 0; i < constructorParams.Length; i++)
        {
            var namedParameter = namedParameters.FirstOrDefault(p => p.Name == constructorParams[i].Name);
            if (namedParameter != null)
            {
                constructorArgs[i] = namedParameter.Value;
            }
            else
            {
                constructorArgs[i] = CreateArgument(constructorParams[i].ParameterType, configurators, constructorParams[i].Name);
            }
        }

        return (T)constructor.Invoke(constructorArgs);
    }

    private object CreateArgument(Type parameterType, object? configurators, string? parameterName)
    {
        object configurator;
        var result = _mockCreator.GetMockOf(parameterType, out configurator);
        if (configurators != null && parameterName != null)
        {
            StoreConfigurator(configurator, configurators, parameterName);
        }
        return result;
    }

    private void StoreConfigurator(object configurator, object configurators, string parameterName)
    {
        var propertyNameBase = parameterName.ToPascalCase();
        var configuratorsType = configurators.GetType();
        var property = GetPropertyFromNameOptions(configuratorsType, [propertyNameBase + "Configurator", propertyNameBase]);
        if (property != null)
        {
            property.SetValue(configurators, configurator);
        }
    }

    private PropertyInfo? GetPropertyFromNameOptions(Type type, string[] names)
    {
        foreach (var name in names)
        {
            var property = type.GetProperty(name);
            if (property != null)
            {
                return property;
            }
        }
        return null;
    }
}
