using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Api.Infrastructure.AutofacHandler
{
    public class ApplicationModule : Autofac.Module
    {
        public ApplicationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Microsoft.AspNetCore.Http.HttpContextAccessor>().As<Microsoft.AspNetCore.Http.IHttpContextAccessor>().InstancePerLifetimeScope();
            ResolveRepositories(builder);
        }

        private void ResolveRepositories(ContainerBuilder builder)
        {
            System.Type[] repositories = System.Reflection.Assembly.Load(typeof(Mahamma.Identity.Infrastructure.Repositories.UserRepository).Assembly.GetName()).GetTypes().ToArray();
            System.Type[] iRepositories = System.Reflection.Assembly.Load(typeof(Domain.User.Repository.IUserRepository).Assembly.GetName()).GetTypes().Where(r => r.IsInterface).ToArray();
            Resolve(builder, repositories, iRepositories);
        }


        private void Resolve(ContainerBuilder builder, System.Type[] repos, System.Type[] irepos)
        {
            foreach (var repoInterface in irepos)
            {
                System.Type classType = repos.FirstOrDefault(r => repoInterface.IsAssignableFrom(r));
                if (classType != null)
                {
                    builder.RegisterType(classType).As(repoInterface).InstancePerLifetimeScope();
                }
            }
        }
    }
}
