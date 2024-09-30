namespace CalorieCount.Core.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		ValueTask<TEntity> AddAsync(TEntity entity);
		ValueTask<TEntity> Read(int EntityID);
		ValueTask<IEnumerable<TEntity>> ReadAll();
		void Update(TEntity entity);
		ValueTask DeleteAsync(int EntityID);
		int SaveChanges();
	}
}
