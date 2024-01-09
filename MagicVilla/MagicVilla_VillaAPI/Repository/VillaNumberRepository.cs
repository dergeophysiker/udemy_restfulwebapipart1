using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {

        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedDate = DateTime.Now;

            try {

                _db.VillaNumbers.Update(entity);
                await _db.SaveChangesAsync();
            }

            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return entity;
              
            }


            return entity;
        }
    }
}
