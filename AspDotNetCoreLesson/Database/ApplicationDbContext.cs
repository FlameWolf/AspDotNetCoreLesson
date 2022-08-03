using System.Diagnostics.CodeAnalysis;
using AspDotNetCoreLesson.Models;
using Microsoft.EntityFrameworkCore;

namespace AspDotNetCoreLesson.Database
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { set; get; }
		public DbSet<Post> Posts { set; get; }
		public DbSet<Comment> Comments { set; get; }

		public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData
			(
				new User
				{
					Id = 101,
					Handle = "TestUSer1",
					Email = "test.user1@server.net"
				},
				new User
				{
					Id = 102,
					Handle = "TestUSer2",
					Email = "test.user2@server.net"
				}
			);
			modelBuilder.Entity<Post>().HasData
			(
				new Post
				{
					Id = 201,
					UserId = 101,
					Content = "Test post 1"
				},
				new Post
				{
					Id = 202,
					UserId = 102,
					Content = "Test post 2"
				}
			);
			modelBuilder.Entity<Comment>().HasData
			(
				new Comment
				{
					Id = 301,
					PostId = 201,
					UserId = 101,
					Content = "Test comment 1"
				},
				new Comment
				{
					Id = 302,
					PostId = 202,
					UserId = 102,
					Content = "Test comment 2"
				}
			);
		}
	}
}