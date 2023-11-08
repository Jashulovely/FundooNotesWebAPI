using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using NLog.Fluent;
using RepositoryLayer.Entity;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;
        private readonly ILogger<NoteController> log;
        public LabelController(ILabelBusiness labelBusiness, ILogger<NoteController> log)
        {
            this.labelBusiness = labelBusiness;
            this.log = log;
        }

        [Authorize]
        [HttpPost]
        [Route("AddLabel")]
        //localhost:6789/api/Notes/AddNote
        public IActionResult AddLabel(int NoteId,string labelName)
        {
            log.LogInformation("LABEL ADDING STARTED.....");
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = labelBusiness.AddLabel(UserId, NoteId, labelName);
            if (result != null)
            {
                log.LogInformation("LABEL ADDED SUCCESSFULLY.....");
                return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "label added Successfully.", Data = result });
            }
            else
            {
                log.LogError("LABEL ADDING FAILED....");
                return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "label adding Failed." });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("AllLabels")]
        public IActionResult AllLabels()
        {
            log.LogInformation("GETTING ALL LABELS STARTED.....");
            List<LabelEntity> allLabels = labelBusiness.LabelsList();
            if (allLabels != null)
            {
                log.LogInformation("GOT ALL LABELS .....");
                return Ok(new ResponseModel<List<LabelEntity>> { Status = true, Message = "All labels", Data = allLabels });
            }
            else
            {
                log.LogError("GETTING ALL LABELS FAILED....");
                return BadRequest(new ResponseModel<List<LabelEntity>> { Status = false, Message = "Labels not exists." });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("GetLabels")]
        public IActionResult GetLabel(string labelName)
        {
            log.LogInformation("GETTING LABEL STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            List<LabelEntity> label = labelBusiness.GetLabel(userid, labelName);
            if (label != null)
            {
                log.LogInformation("GOT THE LABEL .....");
                return Ok(new ResponseModel<List<LabelEntity>> { Status = true, Message = "Got the label", Data = label });
            }
            else
            {
                log.LogError("GETTING THE LABEL FAILED....");
                return BadRequest(new ResponseModel<List<LabelEntity>> { Status = false, Message = "label deos not exists." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("ChangeLabels")]

        public IActionResult ChangeLabels(int noteid, string labelName)
        {
            log.LogInformation("CHANGING LABELS STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = labelBusiness.ChangeLabels(userid, noteid, labelName);

            if (result != false)
            {
                log.LogInformation("CHANGING LABELS SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "label Changed successfully." });
            }
            else
            {
                log.LogError("CHANGING LABELS FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "label Updation failed." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("EditLabels")]

        public IActionResult EditLabels(string labelName, string newLabelName)
        {
            log.LogInformation("EDITING LABELS STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = labelBusiness.EditLabels(userid, labelName, newLabelName);

            if (result != false)
            {
                log.LogInformation("EDITING LABELS SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "label edited successfully." });
            }
            else
            {
                log.LogError("EDITING LABELS FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "label Updation failed." });
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("DeleteteLabel")]

        public IActionResult DeleteteLabel(int noteid, string labelName)
        {
            log.LogInformation("DELETING LABEL STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = labelBusiness.DeleteLabel(userid, noteid, labelName);

            if (result != false)
            {
                log.LogInformation("DELETING LABEL SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Label Deleted successfully." });
            }
            else
            {
                log.LogError("DELETING LABEL FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Label Deletion failed." });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteteLabels")]

        public IActionResult DeleteLabels(string labelName)
        {
            log.LogInformation("DELETING LABELS STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = labelBusiness.DeleteLabels(userid, labelName);

            if (result != false)
            {
                log.LogInformation("DELETING LABELS SUCCESSFULL.....");
                return Ok(new ResponseModel<string>{ Status = true, Message = "Labels Deleted successfully." });
            }
            else
            {
                log.LogError("DELETING LABELS FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Labels Deletion failed." });
            }
        }
    }
}
