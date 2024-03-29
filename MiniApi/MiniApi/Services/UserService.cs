﻿using Dapper;
using MiniApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace MiniApi.Services
{
    public class UserService : IUserService
    {
        private readonly string? con;
        public UserService(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("TestDatabase");
        }

        public async Task AddUserAsync(UserDetail user)
        {
            using var db = new SqlConnection(con);
            //await db.ExecuteAsync("Insert Into [User] (FirstName,LastName,Age) VALUES (@FirstName,@LastName,@Age)", user);
            await db.ExecuteAsync("AddUser", new { user.FirstName, user.LastName, user.Age }, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteUserAsync(int UserId)
        {
            using var db = new SqlConnection(con);
            //await db.ExecuteAsync("Delete from [User] where UserId = @id", new { UserId }, commandType: CommandType.Text);
            await db.ExecuteAsync("DeleteUserById", new { UserId }, commandType: CommandType.StoredProcedure);

        }


        public async Task<IQueryable<User>> GetUsersAsync()
        {
            using var db = new SqlConnection(con);
            // return db.QueryAsync<User>("Select * From [User]");
            return (await db.QueryAsync<User>("GetAllUsers", commandType: CommandType.StoredProcedure)).AsQueryable();
        }

        public async Task<UserDetail> GetUserByIdAsync(int UserId)
        {
            using var db = new SqlConnection(con);
            //return db.QueryFirstAsync<User>("Select * From [User] Where UserId = @UserId" , new { UserId });
            return await db.QueryFirstAsync<UserDetail>("GetUserById", new { UserId }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateUserAsync(UserDetail user)
        {
            using var db = new SqlConnection(con);
            //await db.ExecuteAsync("Update [UserDetail] Set FirstName = @FirstName, LastName = @LastName, Age = @Age Where UserId = @UserId", user);
            await db.ExecuteAsync("UpdateUserById",
                new { user.UserId, user.FirstName, user.LastName, user.Age },
                commandType: CommandType.StoredProcedure);
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
        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            var users = new Dictionary<int, User>();
            using var db = new SqlConnection(con);
            var userRoles = await db.QueryAsync<User, Role, User>(@"SELECT  U.[UserId],U.[FirstName],U.[LastName],R.Id,R.[Name]  
                                               FROM [User] U 
                                               Left join [Role] R on (U.UserId = R.UserId)",
                                               (u, r) =>
                                               {
                                                   if (!users.TryGetValue(u.UserId, out User user))
                                                       users.Add(u.UserId, user = u);
                                                   user.Roles.Add(r);
                                                   return user;
                                               });
            return userRoles.Distinct();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithUsersWithRolesAsync()
        {
            using var db = new SqlConnection(con);
            string? jsonCategorys = await db.QueryFirstAsync<string>(@"SELECT *
                                                        FROM [Category] as Categories
                                                        left join [User] as Users on (Categories.CategoryId = Users.CategoryId)
                                                        left join [Role] as Roles  on (Roles.UserId = Users.UserId) 
                                                        FOR JSON AUTO");
            Category[] result = JsonSerializer.Deserialize<Category[]>(jsonCategorys);
            return result;
        }
    }
}
