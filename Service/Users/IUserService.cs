using System.Collections.Generic;
using Domain.Users;
using Domain.Enums;

namespace Service.Users
{
    public interface IUserService
    {
        User Register(User user);
        User Update(User user);
        bool DeleteByEmail(string email);
        List<User> GetUsers(string email = "", string phone = "", string sortingField = "", SortOrder sortOrder = SortOrder.Ascending);
    }
}
