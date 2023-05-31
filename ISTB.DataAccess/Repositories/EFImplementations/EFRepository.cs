using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected readonly DataContext _context;

        public EFRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _context.Set<TEntity>().AddAsync(entity);
            var actualEntity = result.Entity;
            await _context.SaveChangesAsync();
            return actualEntity;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task RemoveById(int id)
        {
            _context.Remove(id);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var actualEntity = _context.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return actualEntity;
        }
    }
}
