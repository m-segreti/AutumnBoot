using System;

namespace Server.Discovery;

public sealed record ServiceDetails(Type Interface, Type Implementation, ServiceType ServiceType){}