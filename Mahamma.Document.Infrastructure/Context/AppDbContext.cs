using Mahamma.Document.Domain._SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Document.Infrastructure.Context
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        #region DBSet


        #endregion

        #region CTRS
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        #endregion

        #region Model Creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.BuildEnums();
            //calling the method from base is a must for AspNetCore.Identity to be put the keys for its entities
            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Save Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken) > default(int);
            return result;
        }
        #endregion

    }
}
