using NewPrjESDEDIBE.SubscribeTableDependencies;

namespace NewPrjESDEDIBE.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
            where T : ISubscribeTableDependency
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            service.SubscribeTableDependency(connectionString);
        }
        //public static void UseWorkOrderTableDependency(this IApplicationBuilder applicationBuilder)
        //{
        //    var serviceProvider = applicationBuilder.ApplicationServices;
        //    var service = serviceProvider.GetService<SubscribeWorkOrderTableDependency>();
        //    service.SubscribeTableDependency();
        //}
    }
}
