﻿using Mockable.Core;
using Mockable.Core.Exceptions;
using NSubstitute;

namespace Mockable.NSubstitute;

internal class NSubstituteCreator : IMockCreator
{
    public object GetMockOf(Type type, out object mockConfigurator)
    {
        var substituteType = typeof(Substitute);
        var forMethod = substituteType.GetMethod("For", 1, [typeof(object[])])
            ?? throw new MockableException("Error getting Substitute.For<> method");

        var forGenericMethod = forMethod.MakeGenericMethod(type);

        var mock = forGenericMethod.Invoke(null, [ Array.Empty<object?>() ])
            ?? throw new MockableException($"Failed to get mock for {type.FullName}");

        mockConfigurator = mock;
        return mock;
    }
}
