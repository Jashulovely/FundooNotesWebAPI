using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }
        [HttpPost]
        [Route("Register")]
        //localhost:6789/api/User/Register
        public IActionResult Registration(RegisterModel model)
        {
            var isExists = userBusiness.IsRegisteredAlready(model.Email);
            if (isExists)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Email exists already..." });
            }
            else
            {
                var result = userBusiness.UserRegistration(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register Successful", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Registration Failed." });
                }
            }
        }


        [HttpGet]
        [Route("UsersList")]
        public IActionResult UsersList()
        {
            List<UserEntity> allUsers = userBusiness.UsersList();
            if(allUsers != null)
            {
                return Ok(new ResponseModel<List<UserEntity>> { Status = true, Message = "All Users", Data = allUsers });
            }
            else
            {
                return BadRequest(new ResponseModel<List<UserEntity>> { Status = false, Message = "Users not exists."});
            }
        }


        [HttpPost]
        [Route("CheckEmail")]
        //localhost:6789/api/User/CheckEmail
        public IActionResult CheckEmailExists(CheckEmailModel model)
        {
            var emailExists = userBusiness.IsEmailExists(model.Email);
            if (emailExists)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Email exists already..." });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Email not exists." });
            }
        }

        [HttpPost]
        [Route("login")]
        //localhost:6789/api/User/login
        public IActionResult Login(LoginModel login)
        {
            var result = userBusiness.UserLogin(login);
            if (result != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "login successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "login failed" });
            }

        }

        [HttpPost]
        [Route("ForgotPassword")]
        //localhost:6789/api/User/ForgotPassword
        public IActionResult ForgotPassword(string EmailId)
        {
            var result = userBusiness.ForgetPassword(EmailId);
            if(result != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "mail sent successfull.", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "mail sending failed." });
            }
        }
    }
}
