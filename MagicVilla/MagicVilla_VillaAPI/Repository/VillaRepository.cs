using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {

        private readonly ApplicationDbContext _db;


        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        //https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/expression-trees/
        public async Task<Villa> GetOne(String s, bool tracked = true)
        {
            string exp = s;
            Console.WriteLine(exp);
            //var villa = await _db.Villas.FirstOrDefaultAsync(exp);

            await Task.Delay(1);
            return null;
        }



        public async Task<List<Villa>> GetAllSimple()
        {
            Console.Write("called simple");

            return await _db.Villas.ToListAsync();
        }


        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
