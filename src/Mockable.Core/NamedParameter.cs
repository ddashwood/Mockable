namespace Mockable.Core;

/// <summary>
/// Represents a constructor parameter with a value to be used as the argument for that
/// parameter. Typically this will be used when the test is supplying the argument, and
/// does not require Mockable to create the argument.
/// </summary>
public class NamedParameter
{
    /// <summary>
    /// The name of the parameter.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The value to be used as the argument for the parameter.
    /// </summary>
    public required object? Value { get; init; }
}
