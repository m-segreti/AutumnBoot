#nullable enable
using System;

namespace Server.Discovery;

/// <summary>
/// Marks a concrete implementation for discovery, optionally declaring its service interface.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ServiceAttribute : Attribute
{
    /// <summary>
    /// Explicit interface this implementation is intended to satisfy.
    /// </summary>
    public Type? ServiceInterface { get; }
    
    /// <summary>
    /// Desired Dependency Injection lifetime; defaults to <see cref="ServiceType.Scoped"/>.
    /// Can be overridden via a named attribute argument.
    /// </summary>
    public ServiceType Lifetime { get; set; } = ServiceType.Scoped;
    
    /// <summary>
    /// Initializes the attribute.
    /// </summary>
    /// <param name="serviceInterface">
    /// Optional interface type this class implements. If omitted, the scanner will try to infer one.
    /// </param>
    public ServiceAttribute(Type? serviceInterface = null)
    {
        ServiceInterface = serviceInterface;
    }
}