using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepo:IUserRepo
    {
        private readonly FundoDBContext fundoocontext;
        private readonly IConfiguration configuration;

        public UserRepo(FundoDBContext fundoocontext, IConfiguration configuration)
        {
            this.fundoocontext = fundoocontext;
            this.configuration = configuration;
        }

        public UserEntity UserRegistration(RegisterModel model)
        {
            UserEntity entity = new UserEntity();   
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;   
            entity.Email = model.Email; 
            entity.Password = EncryptPassword(model.Password);
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
        

        public List<UserEntity> UsersList()
        {
            try
            {
                List<UserEntity> users = (List<UserEntity>)fundoocontext.Users.ToList();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserEntity UserInfo(int userid)
        {
            try
            {
                UserEntity user = (UserEntity)fundoocontext.Users.FirstOrDefault(u => u.UserId == userid);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool UpdateUser(int userid, UserUpdateModel model)
        {
            try
            {
                var result = fundoocontext.Users.FirstOrDefault(e => e.UserId == userid);
                if (result != null)
                {
                    if (model.FirstName != null)
                    {
                        result.FirstName = model.FirstName;
                    }
                    if (model.LastName != null)
                    {
                        result.LastName = model.LastName;
                    }
                    if (model.Email != null)
                    {
                        result.Email = model.Email;
                    }
                    if (model.Password != null)
                    {
                        result.Password = EncryptPassword(model.Password);
                    }

                    fundoocontext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool IsRegisteredAlready(string email)
        {
            var checkEmail = fundoocontext.Users.Where(x => x.Email == email).Count();
            return checkEmail > 0;
        }

        public bool IsEmailExists(string email)
        {
            var checkEmail = fundoocontext.Users.Where(x => x.Email == email).Count();
            return checkEmail > 0;
        }
        public string UserLogin(LoginModel login)
        {
            var encodePwd = EncryptPassword(login.Password);
            UserEntity checkEmail = fundoocontext.Users.Where(x => x.Email == login.Email).FirstOrDefault();
            UserEntity checkPwd = fundoocontext.Users.FirstOrDefault(y => y.Password == encodePwd);
            if (checkEmail != null)
            {
                if (checkPwd != null)
                {
                    var token = GenerateToken(checkEmail.Email, checkEmail.UserId);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public UserEntity LogginSession(LoginModel login)
        {
            var encodePwd = EncryptPassword(login.Password);
            UserEntity result = fundoocontext.Users.Where(x => x.Email == login.Email && x.Password == encodePwd).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static string EncryptPassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        private string GenerateToken(string Email, int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", Email),
                new Claim("UserId", UserId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string ForgetPassword(string EmailId)
        {
            try
            {
                var result = fundoocontext.Users.FirstOrDefault(x => x.Email == EmailId);
                if(result != null)
                {
                    var token = this.GenerateToken(result.Email, result.UserId);
                    MSMQModel mSMQModel = new MSMQModel();
                    mSMQModel.SendMessage(token, result.Email, result.FirstName);
                    return token.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ResetnewPassword(string Email, ResetPwdModel reset)
        {
            var pwd = EncryptPassword(reset.Password);
            var confpwd = EncryptPassword(reset.ConfirmPassword);
            try
            {
                if (pwd.Equals(confpwd))
                {
                    var user = fundoocontext.Users.Where(x => x.Email == Email).FirstOrDefault();
                    user.Password = confpwd;
                    fundoocontext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex) 
            { 
                throw ex; 
            }
        }

        public UserTicketModel CreateTicketForPassword(string emailId, string token)
        {
            try
            {
                var userDetails = fundoocontext.Users.FirstOrDefault(u => u.Email == emailId);
                if(userDetails != null)
                {
                    UserTicketModel ticketResponse = new UserTicketModel
                    {
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName,
                        EmailId = emailId,
                        Token = token,
                        IssuedAt = DateTime.Now
                    };
                    return ticketResponse;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
