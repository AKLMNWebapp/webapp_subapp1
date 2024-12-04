using Microsoft.EntityFrameworkCore;
using mvc.DAL.Models;

namespace mvc.DAL.Repositories;
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
            _logger.LogError("[AllergyRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return new List<Allergy>();
        }
    }

    public async Task<Allergy?> GetById(int id)
    {
        try
        {
            return await _db.Allergies.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] Allergies FindAsync() failed when GetById() for AllergyID {AllergyID:0000}, error message: {e}", 
            id ,e.Message);
            return null;
        }
    }

    public async Task <bool>Create(Allergy allergy)
    {
        try
        {
            var existingAllergy = await _db.Allergies
            .FirstOrDefaultAsync(a => a.Name == allergy.Name);

            if (existingAllergy == null)
            {
                _db.Allergies.Add(allergy);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                _logger.LogInformation("[AllergyRepository] allergy creation failed for allergy {@allergy}, already exists.", allergy);
                return false;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] allergy creation failed for categoy {@allergy}, error message: {e}", allergy, e.Message);
            return false;
        }
        
    }

    public async Task<bool> Update(Allergy allergy)
    {
        try
        {
            _db.Allergies.Update(allergy); 
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] allergy FindAsync(id) failed when updating the AllergyID {AllergyID:0000}, error message: {e}",
            allergy, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var allergy = await _db.Allergies.FindAsync(id);
            if (allergy == null)
            {
                _logger.LogError("[AllergyRepository] product not found for the AllergyID {AllergyID:0000}", id);
                return false;
            }
            _db.Allergies.Remove(allergy);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AllergyRepository] product deletion failed for the AllergyID {AllergyID:0000}, error message: {e}", id, e.Message );
            return false;
        }
    }
}
