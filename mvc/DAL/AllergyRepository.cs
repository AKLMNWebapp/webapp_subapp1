using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.DAL;

public class AllergyRepository : IRepository<Allergy>
{
    private readonly ProductDbContext _db;
    private readonly ILogger<AllergyRepository> _logger;

    public AllergyRepository(ProductDbContext db, ILogger<AllergyRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Allergy>> GetAll()
    {
        try
        {
            return await _db.Allergies.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] alergies ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<Allergy>();
        }
    }

    public async Task<Allergy?> GetById(int id)
    {
        try
        {
            return await _db.Allergies.FirstOrDefaultAsync(c => c.AllergyCode == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] FirstOrDefaultAsync() failed when GetById() for AllergyCode {AlergyCode:0000}, error message: {e}",
            id, e.Message);
            return null;
        }
    }

    public async Task<bool>Create(Allergy allergy)
    {
        return false; //placeholder
    }

    public async Task<bool> Update(Allergy allergy)
    {
        return false; //placeholder
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var allergy = await _db.Allergies.FindAsync(id);
            if (allergy == null)
            {
                _logger.LogError("[AllergRepository] alergy not found for the AllergyCode {AllergyCode:0000}", id);
                return false;
            }
            _db.Allergies.Remove(allergy);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] allergy deletion failed for AllergyCode {AllergyCode:0000}, error message: {e}", id, e.Message);
            return false;
        }
        
    }
}