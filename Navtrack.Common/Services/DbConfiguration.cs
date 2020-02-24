using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using static System.String;

namespace Navtrack.Common.Services
{
    [Service(typeof(IDbConfiguration), ServiceLifetime.Singleton)]
    public class DbConfiguration : IDbConfiguration
    {
        private readonly IServiceProvider serviceProvider;

        public DbConfiguration(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> Get<T>(Expression<Func<T, string>> expression)
        {
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            IRepository repository = serviceScope.ServiceProvider.GetService<IRepository>();

            string key = GetKey(expression);

            Configuration configuration =
                await repository.GetEntities<Configuration>().FirstOrDefaultAsync(x => x.Key == key);

            if (configuration == null)
            {
                await Save(expression, Empty);
                
                return Empty;
            }

            return configuration.Value;
        }

        public async Task Save<T>(Expression<Func<T, string>> expression, string value)
        {
            string key = GetKey(expression);
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            IRepository repository = serviceScope.ServiceProvider.GetService<IRepository>();
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            Configuration configuration =
                await unitOfWork.GetEntities<Configuration>().FirstOrDefaultAsync(x => x.Key == key);

            if (configuration != null && configuration.Value != value)
            {
                configuration.Value = value;

                unitOfWork.Update(configuration);

                await unitOfWork.SaveChanges();
            }
            else if (configuration == null)
            {
                unitOfWork.Add(new Configuration
                {
                    Key = key,
                    Value = value
                });

                await unitOfWork.SaveChanges();
            }
        }
        
        private static string GetKey<TObject>(Expression<Func<TObject, string>> property)
        {
            MemberExpression body = property.Body as MemberExpression;
            body ??= ((UnaryExpression)property.Body).Operand as MemberExpression;

            if (body != null)
            {
                return $"{typeof(TObject).Name}.{body.Member.Name}";
            }

            throw new ArgumentException("Could not generate key.");
        }
    }
}