using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
       public UserEntity UserRegistration(RegisterModel model);
       public string UserLogin(LoginModel login);
        public UserEntity LogginSession(LoginModel login);
        public bool IsRegisteredAlready(string email);
        public bool IsEmailExists(string email);
        public List<UserEntity> UsersList();
        public string ForgetPassword(string EmailId);
        public bool ResetnewPassword(string Email, ResetPwdModel reset);
        public UserTicketModel CreateTicketForPassword(string emailId, string token);
        public UserEntity UserInfo(int userid);
        public bool UpdateUser(int userid, UserUpdateModel model);
    }
}
