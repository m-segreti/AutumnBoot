namespace Server.Discovery
{
    /// <summary>
    /// Specifies the dependency-injection lifetime to use when registering discovered services.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Singleton"/> — one instance for the entire application; disposed when the host shuts down.</para>
    /// <para><see cref="Scoped"/> — one instance per scope (in ASP.NET Core, per HTTP request); disposed at the end of the scope.</para>
    /// <para><see cref="Transient"/> — a new instance every time the service is requested; disposed when the resolving scope ends.</para>
    /// <para>Rule of thumb: default to <see cref="Scoped"/> for services that use per-request state or a DbContext; 
    /// use <see cref="Singleton"/> for stateless, thread-safe utilities; use <see cref="Transient"/> for lightweight helpers.</para>
    /// </remarks>
    /// <seealso cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>
    public enum ServiceType
    {
        /// <summary>
        /// A single instance is created and reused for the entire application lifetime.
        /// Suitable for stateless, thread-safe services; disposed on host shutdown.
        /// </summary>
        Singleton,

        /// <summary>
        /// One instance per DI scope. In web apps, the default scope is an HTTP request.
        /// Recommended for services that use per-request state or a DbContext; disposed when the scope ends.
        /// </summary>
        Scoped,

        /// <summary>
        /// A new instance is created each time the service is requested.
        /// Best for lightweight, stateless helpers; disposed when the resolving scope ends.
        /// </summary>
        Transient
    }
}