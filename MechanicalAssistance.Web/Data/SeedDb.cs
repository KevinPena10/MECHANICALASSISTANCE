using MechanicalAssistance.Common.Enums;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;

        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckProductBrandsAsync();
            await CheckUserAsync("1010", "Kevin", "Stiven", "kevin.stiven.pena@gmail.com", "3506342747", "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2020", "Mauricio", "Hernandez", "kmy151617@gmail.com", "3506342747", "Calle Luna Calle Sol", UserType.Mechanic);
            await CheckUserAsync("2020", "Sara", "Zapata", "sara1515@yopmail.com", "3226578909", "Carrera 77 #80-93", UserType.User);
        }

        private async Task<UserEntity> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    UserType = userType,
                    LoginType = LoginType.MechanicalAssistance
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Mechanic.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckProductBrandsAsync()
        {
            if (!_context.ProductBrands.Any())
            {
                AddProductBrands("Toyota");
                AddProductBrands("Mercedes");
                AddProductBrands("BMW");
                AddProductBrands("Honda");
                AddProductBrands("Ford");
                AddProductBrands("Nissan");
                AddProductBrands("Tesla");
                AddProductBrands("Audi");
                await _context.SaveChangesAsync();
            }
        }

        private void AddProductBrands(string name)
        {
            _context.ProductBrands.Add(new ProductBrandEntity { BrandName = name });
        }


    }
}
