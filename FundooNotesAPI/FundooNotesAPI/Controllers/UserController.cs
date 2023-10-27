using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;

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
    }
}
