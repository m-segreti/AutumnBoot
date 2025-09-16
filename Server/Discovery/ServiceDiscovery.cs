#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OptionalLibrary;

namespace Server.Discovery;

/// <summary>
/// Reflection-based discovery utilities for locating concrete classes marked with
/// <see cref="ServiceAttribute"/> and producing <see cref="ServiceDetails"/> pairs
/// (interface, implementation) suitable for dependency-injection registration.
/// </summary>
/// <remarks>
/// Intended to be called at application startup. The scan is limited to the supplied
/// <see cref="System.Reflection.Assembly"/> and includes only non-abstract classes that
/// are annotated with <see cref="ServiceAttribute"/>.
/// </remarks>
/// <seealso cref="ServiceAttribute"/>
/// <seealso cref="ServiceDetails"/>
public static class ServiceDiscovery
{
    /// <summary>
    /// Scans <paramref name="assembly"/> for non-abstract classes annotated with
    /// <see cref="ServiceAttribute"/> and returns interface/implementation pairs.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>
    /// A read-only list of <see cref="ServiceDetails"/> items, where
    /// <see cref="ServiceDetails.Interface"/> is the service interface and
    /// <see cref="ServiceDetails.Implementation"/> is the implementing class.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="assembly"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an annotated class specifies a type in <see cref="ServiceAttribute"/> that
    /// is not an interface, does not implement the specified interface, or when interface
    /// inference (if your implementation supports it) is ambiguous.
    /// </exception>
    /// <example>
    /// Register all discovered services with the container:
    /// <code>
    /// using System.Reflection;
    /// using Microsoft.Extensions.DependencyInjection;
    ///
    /// Assembly asm = typeof(Program).Assembly;
    /// IReadOnlyList&lt;ServiceDetails&gt; services = ServiceDiscovery.FindServices(asm);
    /// foreach (ServiceDetails service in services)
    /// {
    ///     // choose lifetime as needed: AddSingleton/AddScoped/AddTransient
    ///     builder.Services.AddSingleton(service.Interface, service.Implementation);
    /// }
    /// </code>
    /// </example>
    public static IReadOnlyList<ServiceDetails?> FindServices(Assembly assembly)
    {
        List<ServiceDetails?> results = [];

        // Get all types in the current Assembly. This may be a lot....
        Type[] types = assembly.GetTypes();

        results.AddRange(
            from type in types 
            select Build(type) 
            into serviceDetails 
            where serviceDetails.IsPresent() 
            select serviceDetails.Value()
        );

        return results;
    }

    private static Optional<ServiceDetails> Build(Type type)
    {
        // If the type is a not a class or is abstract, skip it.
        if (!type.IsClass || type.IsAbstract || type.GetCustomAttribute<ServiceAttribute>(inherit: false) is null)
        {
            return Optional<ServiceDetails>.Empty;
        }

        ServiceAttribute service = type.GetCustomAttribute<ServiceAttribute>(inherit: false) ?? new ServiceAttribute();
        ServiceType lifetime = service.Lifetime; // Lifetime is always set, either defaulted to Scoped or explicitly set via an enum value.

        // If the developer explicitly typed an interface, trust them and use it
        if (service.ServiceInterface is not null)
        {
            return Optional<ServiceDetails>.Of(BuildByInterface(service, type, lifetime));
        }

        // The dev did not provide an explicit interface, get the interfaces from the class
        return Optional<ServiceDetails>.Of(BuildByInference(type, lifetime));
    }

    private static ServiceDetails BuildByInterface(ServiceAttribute service, Type type, ServiceType lifetime)
    {
        Type? interfaceName = service.ServiceInterface;
            
        // Given the interface verify what they specified is indeed an interface.
        if (interfaceName is not { IsInterface: true })
        {
            throw new InvalidOperationException($"[Service] on {type.FullName} specified {interfaceName?.FullName}, which is not an interface.");
        }

        // Given the interface verify that the class does in fact implement that interface.
        return !interfaceName.IsAssignableFrom(type) 
            ? throw new InvalidOperationException($"[Service] on {type.FullName} says it implements {interfaceName.FullName}, but it does not.") 
            : new ServiceDetails(interfaceName, type, lifetime);
    }

    private static ServiceDetails BuildByInference(Type type, ServiceType lifetime)
    {
        // The dev did not provide an explicit interface, get the interfaces from the class
        Type[] interfaces = type.GetInterfaces();
        IEnumerable<Type> candidates = interfaces.AsEnumerable();
        Type[] candidateArray = candidates.ToArray();

        // If there is exactly one candidate, use it
        if (candidateArray.Length == 1)
        {
            return new ServiceDetails(candidateArray[0], type, lifetime);
        }
        
        // To ambiguous, ask the developer to specify the interface in the attribute
        throw new InvalidOperationException($"Cannot infer a single interface for {type.FullName}. " +
                                            $"Found {candidateArray.Length}. Add [Service(typeof(TheInterface))].");
    }
}
