using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IBus bus;
        public TicketController(IUserBusiness userBusiness, IBus bus)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> CreateTicket(string emailId)
        {
            try
            {
                if (emailId != null)
                {
                    var token = userBusiness.ForgetPassword(emailId);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var ticketResponse = userBusiness.CreateTicketForPassword(emailId, token);
                        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                        var endPoint = await bus.GetSendEndpoint(uri);
                        await endPoint.Send(ticketResponse);
                        return Ok(new { Status = true, message = "Email sent Successfully...." });
                    }
                    else
                    {
                        return BadRequest(new { Status = false, message = "Email Id is not Registered...." });
                    }
                }
                else
                {
                    return  BadRequest(new { Status = false, message = "Something went wrong...." });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
