using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;
        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            return userRepo.UserRegistration(model);
        }
        public bool IsRegisteredAlready(string email)
        {
            return userRepo.IsRegisteredAlready(email);
        }

        public bool IsEmailExists(string email)
        {
            return userRepo.IsEmailExists(email);
        }

        public string UserLogin(LoginModel login)
        {
            return userRepo.UserLogin(login);
        }

        public List<UserEntity> UsersList()
        {
            return userRepo.UsersList();
        }

        public string ForgetPassword(string EmailId)
        {
            return userRepo.ForgetPassword(EmailId);
        }

        public bool ResetnewPassword(string Email, ResetPwdModel reset)
        {
            return userRepo.ResetnewPassword(Email, reset);
        }
    }
}
