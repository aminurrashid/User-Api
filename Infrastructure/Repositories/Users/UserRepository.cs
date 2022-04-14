using Domain.Enums;
using Domain.Users;
using Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly CustomMemoryCache _memoryCache;
        private const string cacheKey = "users";

        public UserRepository(CustomMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool DeleteByEmail(string email)
        {
            bool success = false;
            List<User> users;

            if (_memoryCache.Cache.TryGetValue(cacheKey, out users))
            {
                if (users != null)
                {
                    User existingUser = users.FirstOrDefault(x => x.Email == email);
                    if (existingUser != null)
                    {
                        users.Remove(existingUser);
                        _memoryCache.Cache.Set(cacheKey, users);
                        success = true;
                    }
                }
            }

            return success;
        }

        public List<User> GetUsers(string email, string phone, string sortingField, SortOrder sortOrder)
        {
            List<User> users;

            if (_memoryCache.Cache.TryGetValue(cacheKey, out users))
            {
                if (users != null)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        users = users.Where(x => x.Email.Contains(email)).ToList();
                    }
                    if (users != null && !string.IsNullOrEmpty(phone))
                    {
                        users = users.Where(x => x.Phone.Contains(phone)).ToList();
                    }
                    if (users != null && !string.IsNullOrEmpty(sortingField))
                    {
                        System.Reflection.PropertyInfo prop = typeof(User).GetProperty(sortingField);
                        if (prop != null)
                        {
                            if (sortOrder == SortOrder.Ascending)
                                users = users.OrderBy(x => prop.GetValue(x, null)).ToList();
                            else
                                users = users.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                        }
                        
                    }
                }
            }

            return users;
        }

        public User Save(User user)
        {
            bool success = false;
            List<User> users;

            if (_memoryCache.Cache.TryGetValue(cacheKey, out users))
            {
                if (users != null)
                {
                    User existingUser = users.FirstOrDefault(x => x.Email == user.Email);
                    if (existingUser == null)
                    {
                        users.Add(user);
                        _memoryCache.Cache.Set(cacheKey, users);
                        success = true;
                    }
                }
            }
            else
            {
                users = new List<User> { user };
                _memoryCache.Cache.Set(cacheKey, users);
                success = true;
            }

            return success ? user : null;
        }

        public User Update(User user)
        {
            bool success = false;
            List<User> users;

            if (_memoryCache.Cache.TryGetValue(cacheKey, out users))
            {
                if (users != null)
                {
                    User existingUser = users.FirstOrDefault(x => x.Email == user.Email);
                    if (existingUser != null)
                    {
                        users.Remove(existingUser);
                        users.Add(user);
                        _memoryCache.Cache.Set(cacheKey, users);
                        success = true;
                    }
                }
            }

            return success ? user : null;
        }
    }
}
