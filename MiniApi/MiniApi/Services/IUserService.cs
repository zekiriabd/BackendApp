using MiniApi.Models;

namespace MiniApi.Services
{
    public interface IUserService
    {
        Task<IQueryable<User>> GetUsersAsync();
        Task<UserDetail> GetUserByIdAsync(int id);

        Task DeleteUserAsync(int id);

        Task AddUserAsync(UserDetail user);
        Task AddUsersAsync(string CategoryName, List<UserDetail> users);

        Task UpdateUserAsync(UserDetail user);

        Task<IEnumerable<User>> GetUsersWithRolesAsync();

        Task<IEnumerable<Category>> GetCategoriesWithUsersWithRolesAsync();
    }
}
