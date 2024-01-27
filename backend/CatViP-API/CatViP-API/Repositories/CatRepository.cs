using CatViP_API.Data;
using CatViP_API.DTOs.CatDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;

namespace CatViP_API.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly CatViPContext _context;

        public CatRepository(CatViPContext context)
        {
            this._context = context;
        }

        public bool CheckIfCatExist(long userId, long catId)
        {
            return _context.Cats.Any(x => x.Id == catId && x.Status && x.UserId == userId);
        }

        public async Task<bool> DeleteCat(long catId)
        {
            try
            {
                var cat = _context.Cats.FirstOrDefault(c => c.Id == catId);
                cat!.Status = false;
                _context.Update(cat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditCat(long catId, CatRequestDTO editCatRequestDTO)
        {
            try
            {
                var cat = _context.Cats.FirstOrDefault(x => x.Id == catId)!;
                cat.Name = editCatRequestDTO.Name;
                cat.Description = editCatRequestDTO.Description;
                cat.DateOfBirth = editCatRequestDTO.DateOfBirth;
                cat.Gender = editCatRequestDTO.Gender;
                cat.ProfileImage = editCatRequestDTO.ProfileImage;

                _context.Update(cat);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Cat GetCat(long catId)
        {
            return _context.Cats.FirstOrDefault(x => x.Id == catId)!;
        }

        public ICollection<Cat> GetCats(long userId)
        {
            return _context.Cats.Where(x => x.UserId == userId && x.Status).ToList();
        }

        public async Task<bool> StoreCat(long userId, CatRequestDTO createCatRequestDTO)
        {
            try
            {
                var cat = new Cat();
                cat.UserId = userId;
                cat.Name = createCatRequestDTO.Name;
                cat.Description = createCatRequestDTO.Description;
                cat.DateTimeCreated = DateTime.Now;
                cat.DateOfBirth = createCatRequestDTO.DateOfBirth;
                cat.Gender = createCatRequestDTO.Gender;
                cat.ProfileImage = createCatRequestDTO.ProfileImage;
                cat.Status = true;

                _context.Add(cat);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
