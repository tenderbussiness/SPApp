using Bogus;
using Domain.Constants;
using Domain.Data;
using Domain.Data.Entities;
using Infrastraction.Events;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Transactions;

namespace Infrastraction.Services
{
    public class UserService
    {
        private readonly MyDataContext _dataContext;
        private CancellationToken cancellationToken;

        public event UserInsertItemDelegate InsertUserEvent;
        public UserService()
        {
            _dataContext = new MyDataContext();
            _dataContext.Database.Migrate();
        }

        public UserService(CancellationToken token) : this()
        {

            cancellationToken = token;
        }

        public void InsertRandomUser(int count)
        {
            var faker = new Faker<UserEntity>("uk")
                .RuleFor(u => u.LastName, f => f.Person.LastName) // Random lastname
                .RuleFor(u => u.FistName, f => f.Person.FirstName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone);

            var users = faker.Generate(count); // Generate 20 dummy users
            int i = 0;
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var user in users)
                    {
                        _dataContext.Users.Add(user);
                        _dataContext.SaveChanges();
                        InsertUserEvent?.Invoke(++i);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw new Exception("Cansel operation");
                        }
                    }
                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Rollback the transaction if an exception occurs
                    InsertRandomUser(0); //операція була скасована
                }
            }
        }

        public Task InsertRnadomUserAsync(int count)
        {
            return Task.Run(() => InsertRandomUser(count));
        }
        public List<UserEntity> GetUsers()
        {
            using var conn = new SQLiteConnection(AppDatabase.ConnectionString);
            string query = "SELECT Id, LastName, FistName, Email, PhoneNumber, Image " +
                "FROM tblUsers";
            return conn.Query<UserEntity>(query).ToList();
        }

        public void InsertDapperRandomUser(int count)
        {
            var faker = new Faker<UserEntity>("uk")
                .RuleFor(u => u.LastName, f => f.Person.LastName) // Random lastname
                .RuleFor(u => u.FistName, f => f.Person.FirstName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone);

            var users = faker.Generate(count); // Generate 20 dummy users
            int i = 0;
            try
            {
                using var conn = new SQLiteConnection(AppDatabase.ConnectionString);
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var user in users)
                    {

                        string query = "INSERT INTO tblUsers (LastName, FistName, Email, PhoneNumber) " +
                        "VALUES (@LastName, @FistName, @Email, @PhoneNumber) ";
                        conn.Execute(query, user);
                        InsertUserEvent(++i);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw new Exception("Cansel operation");
                        }
                    }
                    transaction.Commit();

                }
            }
            catch (Exception)
            {
                //transaction.Rollback(); // Rollback the transaction if an exception occurs
                InsertRandomUser(0); //операція була скасована
            }
        }

        public Task InsertDapperRandomUserAsync(int count)
        {
            return Task.Run(() => InsertDapperRandomUser(count));
        }
    }
}
