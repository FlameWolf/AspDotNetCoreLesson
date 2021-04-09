using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public interface IEntityRepository<T>
	{
		public Task<T> Add(T model);

		public Task<T> Get(uint id);

		public Task<IEnumerable<T>> Get();

		public Task<T> Update(T model);

		public Task<T> Delete(T model);
	}
}