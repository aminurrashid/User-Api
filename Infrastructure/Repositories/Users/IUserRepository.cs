using Domain.Enums;
using Domain.Users;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Users
{
    public interface IUserRepository
    {
        User Save(User user);
        User Update(User user);
        bool DeleteByEmail(string email);
        List<User> GetUsers(string email, string phone, string sortingField, SortOrder sortOrder);
    }
}
