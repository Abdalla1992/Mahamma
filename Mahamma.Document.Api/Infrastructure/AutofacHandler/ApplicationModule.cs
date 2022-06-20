using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Document.Api.Infrastructure.AutofacHandler
{
    public class ApplicationModule : Autofac.Module
    {
        public ApplicationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Microsoft.AspNetCore.Http.HttpContextAccessor>().As<Microsoft.AspNetCore.Http.IHttpContextAccessor>().InstancePerLifetimeScope();
            base.Load(builder);
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
