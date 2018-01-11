using Autofac;
using ConfigService.Client;
using ConfigService.Contract;

namespace IoCContainer
{

    /// <summary>
    /// Inversion of Control Container
    /// </summary>
    public static class IoCManager
    {
        public static IContainer Container { get; private set; }


        /// <summary>
        /// Initialize the container. Call only once during the startup of application.
        /// </summary>
        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            //Register modules here            
            builder.RegisterInstance(new ViewProviderClient()).As<IViewProviderService>().ExternallyOwned();

            Container = builder.Build();
        }

        /// <summary>
        /// Get the registered module of type T
        /// </summary>
        /// <typeparam name="T">Type of the module to retrieve</typeparam>
        /// <returns>Module implementing T type</returns>
        public static T Get<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}
