using Autofac;
using UnityTransmitter;
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
            builder.RegisterInstance(new ViewProvider()).As<IViewProvider>().ExternallyOwned();
            builder.RegisterInstance(new TextureConverter()).As<ITextureConverter>();
            builder.RegisterType<StereoVidTransmitter>().As<IStereoVidTransmitter>();
            Container = builder.Build();
        }

        public static T Get<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
                return scope.Resolve<T>();
        }
    }
}
