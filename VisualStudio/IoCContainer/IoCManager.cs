﻿using Autofac;
using ConfigService.Client;
using ViewProvision;
using ViewProvision.Contract;

namespace IoCContainer
{
    public static class IoCManager
    {
        public static IContainer Container { get; private set; }

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            //Register modules here            
            //builder.RegisterInstance(new ViewProvider()).As<IViewProvider>().ExternallyOwned();
            builder.RegisterInstance(new ViewProviderClient()).As<IViewProvider>().ExternallyOwned();


            Container = builder.Build();
        }

        public static T Get<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}
