[assembly: WebActivator.PostApplicationStartMethod(typeof(SilupostWeb.API.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace SilupostWeb.API.App_Start
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Http;
    using SilupostWeb.API.Helpers;
    using SilupostWeb.Data;
    using SilupostWeb.Data.Interface;
    using SilupostWeb.Facade;
    using SilupostWeb.Facade.Interface;
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
            string connectionString = Configuration.ConnectionString();

            GlobalVariables.goDefaultCrimeIncidentTypeProfilePicPath = GlobalVariables.GetApplicationConfig("DefaultCrimeIncidentTypeProfilePic");
            GlobalVariables.goDefaultEnforcementTypeProfilePicPath = GlobalVariables.GetApplicationConfig("DefaultEnforcementTypeProfilePic");
            GlobalVariables.goDefaultEnforcementUnitProfilePicPath = GlobalVariables.GetApplicationConfig("DefaultEnforcementUnitProfilePic");
            GlobalVariables.goDefaultEnforcementStationProfilePicPath = GlobalVariables.GetApplicationConfig("DefaultEnforcementStationProfilePic");
            #region DAL
            container.Register<IDbConnection>(() => new SqlConnection(connectionString), Lifestyle.Scoped);
            container.Register<ILookupTableRepositoryDAC, LookupTableDAC>(Lifestyle.Scoped);
            container.Register<ISystemUserRepository, SystemUserDAC>(Lifestyle.Scoped);
            container.Register<ILegalEntityRepository, LegalEntityDAC>(Lifestyle.Scoped);
            container.Register<ISystemWebAdminRoleRepositoryDAC, SystemWebAdminRoleDAC>(Lifestyle.Scoped);
            container.Register<ISystemWebAdminUserRolesRepositoryDAC, SystemWebAdminUserRolesDAC>(Lifestyle.Scoped);
            container.Register<ISystemWebAdminMenuRolesRepositoryDAC, SystemWebAdminMenurRolesDAC>(Lifestyle.Scoped);
            container.Register<ICrimeIncidentTypeRepositoryDAC, CrimeIncidentTypeDAC>(Lifestyle.Scoped);
            container.Register<ICrimeIncidentCategoryRepositoryDAC, CrimeIncidentCategoryDAC>(Lifestyle.Scoped);
            container.Register<IFileRepositoryDAC, FileDAC>(Lifestyle.Scoped);
            #endregion

            #region Facade
            container.Register<ILookupFacade, LookupFacade>(Lifestyle.Scoped);
            container.Register<ISystemUserFacade, SystemUserFacade>(Lifestyle.Scoped);
            container.Register<ISystemWebAdminRoleFacade, SystemWebAdminRoleFacade>(Lifestyle.Scoped);
            container.Register<ISystemWebAdminMenuRolesFacade, SystemWebAdminMenuRolesFacade>(Lifestyle.Scoped);
            container.Register<IUserAuthFacade, UserAuthFacade>(Lifestyle.Scoped);
            container.Register<ICrimeIncidentTypeFacade, CrimeIncidentTypeFacade>(Lifestyle.Scoped);
            container.Register<ICrimeIncidentCategoryFacade, CrimeIncidentCategoryFacade>(Lifestyle.Scoped);
            #endregion
        }
    }
}