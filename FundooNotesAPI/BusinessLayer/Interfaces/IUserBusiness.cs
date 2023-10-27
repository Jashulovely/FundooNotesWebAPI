using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        public UserEntity UserRegistration(RegisterModel model);
        public string UserLogin(LoginModel login);
        public bool IsRegisteredAlready(string email);

    }
}
