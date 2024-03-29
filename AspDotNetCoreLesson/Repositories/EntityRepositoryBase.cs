﻿using AspDotNetCoreLesson.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public class EntityRepositoryBase<T> : IEntityRepository<T> where T : class
	{
		private readonly DbContext Context;

		public EntityRepositoryBase(DbContext _context)
		{
			Context = _context;
		}

		public async Task<T> Add(T model)
		{
			var result = await Context.AddAsync<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> Get(uint id) => await Context.FindAsync<T>(id);

		public async Task<IEnumerable<T>> Get() => Context.Set<T>().AsEnumerable();

		public async Task<T> Update(T model)
		{
			var result = Context.Update<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<T> Delete(T model)
		{
			var result = Context.Remove<T>(model);
			await Context.SaveChangesAsync();
			return result.Entity;
		}
	}
}