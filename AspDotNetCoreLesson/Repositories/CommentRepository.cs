using AspDotNetCoreLesson.Database;
using AspDotNetCoreLesson.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Repositories
{
	public class CommentRepository: IEntityRepository<Comment>
	{
		private readonly ApplicationDbContext _dbContext;

		public CommentRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Comment> Create(Comment model)
		{
			var result = await _dbContext.Comments.AddAsync(model);
			_dbContext.SaveChanges();
			return result.Entity;
		}

		public async Task<Comment> Get(uint id)
		{
			return await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<Comment>> GetAll()
		{
			return _dbContext.Comments.AsEnumerable();
		}

		public async Task<Comment> Update(Comment model)
		{
			var result = _dbContext.Comments.Update(model);
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}

		public async Task<Comment> Delete(uint id)
		{
			var result = _dbContext.Comments.Remove(await Get(id));
			await _dbContext.SaveChangesAsync();
			return result.Entity;
		}
	}
}