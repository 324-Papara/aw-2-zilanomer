using Microsoft.EntityFrameworkCore;
using Para.Base.Entity;
using Para.Data.Context;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Para.Data.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ParaSqlDbContext dbContext;
    private readonly DbSet<TEntity> dbSet;

    public GenericRepository(ParaSqlDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Set<TEntity>(); // DbSet'i baþlat
    }

    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetById(long Id)
    {
        return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task Insert(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        await dbSet.AddAsync(entity);
        await Save(); // Ekledikten sonra deðiþiklikleri kaydet
    }

    public async Task Update(TEntity entity)
    {
        dbSet.Update(entity);
        await Save(); // Güncelledikten sonra deðiþiklikleri kaydet
    }

    public async Task Delete(TEntity entity)
    {
        dbSet.Remove(entity);
        await Save(); // Sildikten sonra deðiþiklikleri kaydet
    }

    public async Task Delete(long Id)
    {
        var entity = await GetById(Id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await Save(); // Sildikten sonra deðiþiklikleri kaydet
        }
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    // Dinamik Include iþlemi
    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }

    // Dinamik Where iþlemi
    public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return await dbSet.Where(predicate).ToListAsync();
    }
}