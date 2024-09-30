using CalorieCount.Core.Interfaces;
using CalorieCount.Infrastructur.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Infrastructure.Persistence
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private ApplicationbDbContext _context = null;
		private DbSet<TEntity> _entity;

        public Repository(ApplicationbDbContext context)
        {
			_context = context;
			_entity = _context.Set<TEntity>();
        }
        public async ValueTask<TEntity> AddAsync(TEntity entity)
		{
			await _entity.AddAsync(entity);
			return entity;
		}

		public async ValueTask<TEntity> Read(int EntityID) =>
			await _entity.FindAsync(EntityID);
		

		public async ValueTask<IEnumerable<TEntity>> ReadAll() =>
			await _entity.ToListAsync();

		public void Update(TEntity entity)
		{
			_entity.Update(entity);
		}


		public async ValueTask DeleteAsync(int EntityID)
		{
			var objectEntity = await _entity.FindAsync(EntityID);
			_entity.Remove(objectEntity);
		}

		public int SaveChanges() =>
			_context.SaveChanges();
		
	}
}
