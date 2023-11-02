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
        public bool IsEmailExists(string email);
        public List<UserEntity> UsersList();
        public string ForgetPassword(string EmailId);
        public bool ResetnewPassword(string Email, ResetPwdModel reset);

    }
}
