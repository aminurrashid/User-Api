using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;

namespace Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool DeleteByEmail(string email)
        {
            return _userRepository.DeleteByEmail(email);
        }

        public List<User> GetUsers(string email, string phone, string sortingField, SortOrder sortOrder)
        {
            return _userRepository.GetUsers(email, phone, sortingField, sortOrder);
        }

        public User Register(User user)
        {
            return _userRepository.Save(user);
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }
    }
}
