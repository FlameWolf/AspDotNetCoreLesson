using AspDotNetCoreLesson.Database;
using AspDotNetCoreLesson.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public class PostRepository: IEntityRepository<Post>
	{
		private readonly ApplicationDbContext _dbContext;

		public PostRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Post> Create(Post model)
		{
			var result = await _dbContext.Posts.AddAsync(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Post> Get(uint id)
		{
			return await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Post>> GetAll()
		{
			return _dbContext.Posts.AsEnumerable();
		}

		public async Task<Post> Update(Post model)
		{
			var result = _dbContext.Posts.Update(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Post> Delete(uint id)
		{
			var result = _dbContext.Posts.Remove(await Get(id));
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}
	}
}