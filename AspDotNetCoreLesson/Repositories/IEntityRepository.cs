using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public interface IEntityRepository<T>
	{
		public Task<T> AddAsync(T model);
		public Task<T> GetAsync(uint id);
		public Task<IEnumerable<T>> GetAsync();
		public Task<T> UpdateAsync(T model);
		public Task<T> DeleteAsync(T model);
	}
}