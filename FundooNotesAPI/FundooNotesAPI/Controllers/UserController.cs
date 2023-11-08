using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly ILogger<UserController> log;
        public UserController(IUserBusiness userBusiness, ILogger<UserController> log)
        {
            this.userBusiness = userBusiness;
            this.log = log;
        }
        [HttpPost]
        [Route("Register")]
        //localhost:6789/api/User/Register
        public IActionResult Registration(RegisterModel model)
        {
            log.LogInformation("REGISTRATION STARTED.....");
            var isExists = userBusiness.IsRegisteredAlready(model.Email);
            if (isExists)
            {
                log.LogInformation("EMAIL EXISTS ALREADY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Email exists already..." });
            }
            else
            {
                var result = userBusiness.UserRegistration(model);
                if (result != null)
                {
                    log.LogInformation("REGISTRATION WAS SUCCESSFULL.....");
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register Successful", Data = result });
                }
                else
                {
                    log.LogError("REGISTRATION FAILED.....");
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Registration Failed." });
                }
            }
        }


        [HttpGet]
        [Route("UsersList")]
        public IActionResult UsersList()
        {
            log.LogInformation("GETTING USERS LIST STARTED.....");
            List<UserEntity> allUsers = userBusiness.UsersList();
            if(allUsers != null)
            {
                log.LogInformation("GOT USERS LIST SUCCESSFULLY.....");
                return Ok(new ResponseModel<List<UserEntity>> { Status = true, Message = "All Users", Data = allUsers });
            }
            else
            {
                log.LogError("GETTING USERS LIST FAILED.....");
                return BadRequest(new ResponseModel<List<UserEntity>> { Status = false, Message = "Users not exists."});
            }
        }

        [HttpGet]
        [Route("UserInfo")]
        public IActionResult UserInfo(int userid)
        {
            log.LogInformation("GETTING USER INFO STARTED.....");
            UserEntity user = userBusiness.UserInfo(userid);
            if (user != null)
            {
                log.LogInformation("GOT USER INFO SUCCESSFULLY.....");
                return Ok(new ResponseModel<UserEntity> { Status = true, Message = "user info", Data = user });
            }
            else
            {
                log.LogError("GETTING USER INFO FAILED.....");
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "User not exists." });
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(int userid, UserUpdateModel model)
        {
            log.LogInformation("UPDATING USER STARTED.....");
            var result = userBusiness.UpdateUser(userid, model);

            if (result != null)
            {
                log.LogInformation("UPDATING USER SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "user Updated successfully." });
            }
            else
            {
                log.LogError("UPDATING USER FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "user Updation failed." });
            }
        }

        [HttpPost]
        [Route("CheckEmail")]
        //localhost:6789/api/User/CheckEmail
        public IActionResult CheckEmailExists(CheckEmailModel model)
        {
            log.LogInformation("CHECKING EMAIL STARTED.....");
            var emailExists = userBusiness.IsEmailExists(model.Email);
            if (emailExists)
            {
                log.LogInformation("EMAIL EXISTS ALREADY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Email exists already..." });
            }
            else
            {
                log.LogError("EMAIL NOT EXISTS....");
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Email not exists." });
            }
        }

        [HttpPost]
        [Route("login")]
        //localhost:6789/api/User/login
        public IActionResult Login(LoginModel login)
        {
            log.LogInformation("LOGIN STARTED.....");
            var result = userBusiness.UserLogin(login);
            if (result != null)
            {
                log.LogInformation("LOGIN SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "login successfull", Data = result });
            }
            else
            {
                log.LogError("LOGIN FAILED....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "login failed" });
            }

        }


        [HttpPost]
        [Route("LoginSession")]
        //localhost:6789/api/User/login
        public IActionResult LogginSession(LoginModel login)
        {
            log.LogInformation("LOGIN IN STARTED.....");
            var result = userBusiness.LogginSession(login);
            if (result != null)
            {
                log.LogInformation("LOGIN IN SUCCESSFULL.....");
                HttpContext.Session.SetInt32("UserId", result.UserId);
                return Ok(new ResponseModel<UserEntity> { Status = true, Message = "LOGIN successfull", Data = result });
            }
            else
            {
                log.LogError("LOGIN IN FAILED....");
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "LOGIN in failed" });
            }

        }

        [HttpPost]
        [Route("ForgotPassword")]
        //localhost:6789/api/User/ForgotPassword
        public IActionResult ForgotPassword(string EmailId)
        {
            log.LogInformation("FORGOT PASSWORD STARTED......");
            var result = userBusiness.ForgetPassword(EmailId);
            if(result != null)
            {
                log.LogInformation("MAIL SENT SUCCESSFULLY......");
                return Ok(new ResponseModel<string> { Status = true, Message = "mail sent successfull.", Data = result });
            }
            else
            {
                log.LogError("MAIL SENDING FAILED........");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "mail sending failed." });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("ResetNewPassword")]
        //localhost:6789/api/User/ResetNewPassword
        public IActionResult ResetnewPassword(ResetPwdModel reset)
        {
            log.LogInformation("RESET NEW PASSWORD STARTED......");
            string email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var result = userBusiness.ResetnewPassword(email, reset);
            if (result != null)
            {
                log.LogInformation("RESET NEW PASSWORD SUCCESSFULL......");
                return Ok(new ResponseModel<string> { Status = true, Message = "resetted password successfully." });
            }
            else
            {
                log.LogError("RESET NEW PASSWORD FAILED......");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "password resetting failed." });
            }
        }


    }
}
