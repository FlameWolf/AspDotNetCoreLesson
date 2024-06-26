using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspDotNetCoreLesson.Repositories
{
	public class EntityRepositoryBase<T>(DbContext Context) : IEntityRepository<T> where T : class
	{
		public async Task<T> AddAsync(T model)
		{
			var result = await Context.AddAsync<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> GetAsync(uint id) => await Context.FindAsync<T>(id);

		public async Task<IEnumerable<T>> GetAsync() => await Task.FromResult(Context.Set<T>().AsEnumerable());

		public async Task<T> UpdateAsync(T model)
		{
			var result = Context.Update<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> DeleteAsync(T model)
		{
			var result = Context.Remove<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}
	}
}