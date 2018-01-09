using Autofac;
using ConfigService.Client;
using ConfigService.Contract;

namespace IoCContainer
{
    public static class IoCManager
    {
        public static IContainer Container { get; private set; }

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            //Register modules here            
            builder.RegisterInstance(new ViewProviderClient()).As<IViewProviderService>().ExternallyOwned();

            Container = builder.Build();
        }

        public static T Get<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}
