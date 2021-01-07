[assembly: WebActivator.PostApplicationStartMethod(typeof(POSWeb.POS.API.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace POSWeb.POS.API.App_Start
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Http;
    using POSWeb.POS.API.Helpers;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            string connectionString = Configuration.ConnectionString();;
            #region DAL

            container.Register<IDbConnection>(() => new SqlConnection(connectionString), Lifestyle.Scoped);

            #endregion

            #region Facade
            #endregion
        }
    }
}