﻿
using Mockable.Core.Exceptions;
using System.Reflection;

namespace Mockable.Core;

public abstract class ServiceFactoryBase
{
    private readonly IMockCreator _mockCreator;
    public ServiceFactoryBase(IMockCreator mockCreator)
    {
        _mockCreator = mockCreator;
    }

    public T Create<T, TConfigurators>(out TConfigurators configurators, params NamedParameter[] namedParameters) where TConfigurators : new()
    {
        configurators = new(); 
        return Create<T>(configurators, namedParameters);
    }

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

        if (constructors.Length > 1)
        {
            throw new MockableException($"Multiple constructors found for class {type.FullName}");
        }

        return constructors[0];
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
