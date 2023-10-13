using Dapper;
using MinimalApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace MinimalApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDetail>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task DeleteUserAsync(int id);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task AddUsersAsync(string CategoryName, List<UserDetail> users);
    }
    public class UserService : IUserService
    {
        private readonly string? con;
        public UserService(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("TestDatabase");
        }

        public async Task<bool> AddUserAsync(User user)
        {
            //using var db = new SqlConnection(con);
            //var userDto = new { user.FirstName, user.LastName, user.Age };
            //await db.ExecuteAsync("AddUser", userDto, commandType: CommandType.StoredProcedure);
            return false;
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            //using var db = new SqlConnection(con);
            //await db.ExecuteAsync("UpdateUserById", user, commandType: CommandType.StoredProcedure);
            return false;
        }

        public async Task DeleteUserAsync(int UserId)
        {
            using var db = new SqlConnection(con);
            await db.ExecuteAsync("DeleteUserById", new { UserId }, commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<UserDetail>> GetUsersAsync()
        {
            using var db = new SqlConnection(con);
            return await db.QueryAsync<UserDetail>("GetAllUsers", commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetUserByIdAsync(int UserId)
        {
            using var db = new SqlConnection(con);
            return await db.QueryFirstAsync<User>("GetUserById", new { UserId }, commandType: CommandType.StoredProcedure);
        }

     

        public async Task AddUsersAsync(string CategoryName, List<UserDetail> users)
        {
            using var db = new SqlConnection(con);
            if (db.State == ConnectionState.Closed) db.Open();

            var transaction = db.BeginTransaction();
            try
            {
                int newId = await db.QueryFirstAsync<int>("AddCategory", new { CategoryName }, transaction, commandType: CommandType.StoredProcedure);
                users = users.Select(c => { c.CategoryId = newId; return c; }).ToList();
                await db.ExecuteAsync("Insert Into [User] (FirstName,LastName,Age,CategoryId) VALUES (@FirstName,@LastName,@Age,@CategoryId)", users, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
       
    }
}
