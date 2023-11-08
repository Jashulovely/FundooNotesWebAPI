using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System.Linq;
using System;
using System.Collections.Generic;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        private readonly ILogger<NoteController> log;
        public CollabController(ICollabBusiness collabBusiness, ILogger<NoteController> log)
        {
            this.collabBusiness = collabBusiness;
            this.log = log;
        }

        [Authorize]
        [HttpPost]
        [Route("AddCollaborator")]
        public IActionResult AddCollaborator(int noteid, string collabEmail)
        {
            log.LogInformation("COLLABORATOR ADDING STARTED.....");
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = collabBusiness.AddCollaborator(UserId, noteid, collabEmail);
            if (result != null)
            {
                log.LogInformation("COLLABORATOR ADDED SUCCESSFULLY.....");
                return Ok(new ResponseModel<CollaboratorEntity> { Status = true, Message = "collaborator added Successfully.", Data = result });
            }
            else
            {
                log.LogError("COLLABORATOR ADDING FAILED....");
                return BadRequest(new ResponseModel<CollaboratorEntity> { Status = false, Message = "collaborator adding Failed." });
            }

        }

        [Authorize]
        [HttpGet]
        [Route("CollaboratorsList")]
        public IActionResult CollabsList(int noteid)
        {
            log.LogInformation("COLLABORATORS GETTING STARTED.....");
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            List<CollaboratorEntity> result = collabBusiness.CollabsList(UserId, noteid);
            if (result != null)
            {
                log.LogInformation("COLLABORATORS GOT SUCCESSFULLY.....");
                return Ok(new ResponseModel<List<CollaboratorEntity>> { Status = true, Message = "collaborators got Successfully.", Data = result });
            }
            else
            {
                log.LogError("COLLABORATORS GETTING FAILED....");
                return BadRequest(new ResponseModel<List<CollaboratorEntity>> { Status = false, Message = "collaborators getting Failed." });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("RemoveCollaborator")]
        public IActionResult RemoveCollaborator(int noteid, string collabEmail)
        {
            log.LogInformation("COLLABORATOR REMOVING STARTED.....");
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = collabBusiness.RemoveCollaborator(UserId, noteid, collabEmail);
            if (result != false)
            {
                log.LogInformation("COLLABORATOR REMOVED SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "collaborator removed Successfully."});
            }
            else
            {
                log.LogError("COLLABORATOR REMOVING FAILED....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "collaborator removing Failed." });
            }
        }
    }
}
