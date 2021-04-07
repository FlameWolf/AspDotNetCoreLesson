using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public interface IEntityRepository<T>: IRepository
	{
		public Task<T> Create(T model);

		public Task<T> Get(uint id);

		public Task<IEnumerable<T>> GetAll();

		public Task<T> Update(T model);

		public Task<T> Delete(uint id);
	}
}