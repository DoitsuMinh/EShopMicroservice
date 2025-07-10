using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Ordering.Application.Configuration.Validation;
using Ordering.Application.Customers.GetCustomerDetails;
using Ordering.Application.Customers.RegisterCustomer;
using System.Reflection;

namespace Odering.Infrastructure.Processing;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterSource(new ScopedContravariantRegistrationSource(
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>)
            ));

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

        var mediatrOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestExceptionHandler<,,>),
            typeof(IRequestExceptionAction<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>),
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
        {
            builder
                .RegisterAssemblyTypes(Assemblies.Application, typeof(RegisterCustomerCommand).Assembly, ThisAssembly)
                .AsClosedTypesOf(mediatrOpenType)
                .FindConstructorsWith(new AllConstructorFinder())
                .AsImplementedInterfaces();
        }

        builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));


        builder.RegisterGeneric(typeof(CommandValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        var services = new ServiceCollection();

        builder.Populate(services);
    }

    private class ScopedContravariantRegistrationSource : IRegistrationSource
    {
        private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
        private readonly List<Type> _types = [];

        public ScopedContravariantRegistrationSource(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                throw new ArgumentException("Types cannot be null or empty.", nameof(types));
            }
            if (!types.All(x => x.IsGenericTypeDefinition))
            {
                throw new ArgumentException("All types must be generic type definitions.", nameof(types));
            }
            _types.AddRange(types);
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var components = _source.RegistrationsFor(service, registrationAccessor);

            foreach (var component in components)
            {
                var defs = component.Services
                    .OfType<TypedService>()
                    .Select(x => x.ServiceType.GetGenericTypeDefinition());

                if (defs.Any(_types.Contains))
                {
                    yield return component;
                }
            }
        }
    }
}
