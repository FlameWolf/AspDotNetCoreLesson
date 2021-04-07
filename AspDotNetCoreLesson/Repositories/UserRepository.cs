using AspDotNetCoreLesson.Database;
using AspDotNetCoreLesson.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public class UserRepository : IEntityRepository<User>
	{
		private readonly ApplicationDbContext _dbContext;

		public UserRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<User> Create(User model)
		{
			var result = await _dbContext.Users.AddAsync(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<User> Get(uint id)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			return _dbContext.Users.AsEnumerable();
		}

		public async Task<User> Update(User model)
		{
			var result = _dbContext.Users.Update(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<User> Delete(uint id)
		{
			var result = _dbContext.Users.Remove(await Get(id));
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}
	}
}