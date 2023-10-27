using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Linq;

namespace RepositoryLayer.Services
{
    public class UserRepo:IUserRepo
    {
        private readonly FundoDBContext fundoocontext;

        public UserRepo(FundoDBContext fundoocontext)
        {
            this.fundoocontext = fundoocontext;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            UserEntity entity = new UserEntity();   
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;   
            entity.Email = model.Email; 
            entity.Password = model.Password;
            fundoocontext.Users.Add(entity);
            var result = fundoocontext.SaveChanges();
            if (result > 0)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        public string UserLogin(LoginModel login)
        {
            UserEntity checkEmailPassword = fundoocontext.Users.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);
            if (checkEmailPassword != null)
            {
                return "Login Successfull";
            }
            else
            {
                return "Login failed";
            }
        }

        
    }
}
