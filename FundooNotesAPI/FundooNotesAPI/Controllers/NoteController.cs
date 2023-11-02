using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness noteBusiness;
        public NoteController(INoteBusiness noteBusiness)
        {
            this.noteBusiness = noteBusiness;
        }

        [Authorize]
        [HttpPost]
        [Route("AddNote")]
        //localhost:6789/api/Notes/AddNote
        public IActionResult AddNote(NoteModel model)
        {
            int UserId =Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.AddNote(model, UserId);
            if(result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Note added Successfully.", Data = result});
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message = "Note adding Failed."});
            }
        }

        [HttpGet]
        [Route("AllNotes")]
        public IActionResult AllNotes()
        {
            List<NoteEntity> allNotes = noteBusiness.NotessList();
            if (allNotes != null)
            {
                return Ok(new ResponseModel<List<NoteEntity>> { Status = true, Message = "All Notes", Data = allNotes });
            }
            else
            {
                return BadRequest(new ResponseModel<List<NoteEntity>> { Status = false, Message = "Notes not exists." });
            }
        }
    }
}
