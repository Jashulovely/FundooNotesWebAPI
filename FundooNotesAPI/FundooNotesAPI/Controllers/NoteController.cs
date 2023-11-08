using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness noteBusiness;
        private readonly ILogger<NoteController> log;
        private readonly IDistributedCache distributedCache;
        public NoteController(INoteBusiness noteBusiness, ILogger<NoteController> log, IDistributedCache distributedCache)
        {
            this.noteBusiness = noteBusiness;
            this.log = log;
            this.distributedCache = distributedCache;
        }

        //[Authorize]
        [HttpPost]
        [Route("AddNote")]
        //localhost:6789/api/Notes/AddNote
        public IActionResult AddNote(NoteModel model)
        {
            log.LogInformation("NOTE ADDING STARTED.....");
            int UserId =Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            //int userId = (int)HttpContext.Session.GetInt32("UserId");
            var result = noteBusiness.AddNote(model, UserId);
            if(result != null)
            {
                log.LogInformation("NOTE ADDED SUCCESSFULLY.....");
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Note added Successfully.", Data = result});
            }
            else
            {
                log.LogError("NOTE ADDING FAILED....");
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message = "Note adding Failed."});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("AllNotes")]
        public async Task<IActionResult> AllNotes()
        {
            try
            {
                var CacheKey = "NotesList";
                List<NoteEntity> NoteList;
                byte[] RedisNoteList = await distributedCache.GetAsync(CacheKey);
                if (RedisNoteList != null)
                {
                    log.LogDebug("Getting the list from Redis Cache");
                    var SerializedNoteList = Encoding.UTF8.GetString(RedisNoteList);
                    NoteList = JsonConvert.DeserializeObject<List<NoteEntity>>(SerializedNoteList);
                }
                else
                {
                    log.LogDebug("Setting the list to cache which is requested for the first time");
                    NoteList = (List<NoteEntity>)noteBusiness.NotessList();
                    var SerializedNoteList = JsonConvert.SerializeObject(NoteList);
                    var redisNoteList = Encoding.UTF8.GetBytes(SerializedNoteList);
                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    await distributedCache.SetAsync(CacheKey, redisNoteList, options);
                }
                log.LogInformation("Got the notes list successfully from Redis");
                return Ok(NoteList);
            }
            catch (Exception ex)
            {
                log.LogCritical(ex, "Exception thrown...");
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetNote")]
        public IActionResult GetNote(int noteid)
        {
            log.LogInformation("GETTING NOTE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            NoteEntity note = noteBusiness.GetNote(userid, noteid);
            if (note != null)
            {
                log.LogInformation("GOT THE NOTE .....");
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Got the Note", Data = note });
            }
            else
            {
                log.LogError("GETTING THE NOTE FAILED....");
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message = "Note deos not exists." });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetDateNote")]
        public IActionResult DateNote(DateTime date)
        {
            log.LogInformation("GETTING NOTE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            List<NoteEntity> note = noteBusiness.DateNote(userid, date);
            if (note != null)
            {
                log.LogInformation("GOT THE NOTE .....");
                return Ok(new ResponseModel<List<NoteEntity>> { Status = true, Message = "Got the Note", Data = note });
            }
            else
            {
                log.LogError("GETTING THE NOTE FAILED....");
                return BadRequest(new ResponseModel<List<NoteEntity>> { Status = false, Message = "Note deos not exists." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("UpdateNote")]

        public IActionResult UpdateNote(int noteid, NoteModel model)
        {
            log.LogInformation("UPDATING NOTE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.UpdateNote(noteid, userid, model);

            if (result != false)
            {
                log.LogInformation("UPDATING NOTE SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note Updated successfully." });
            }
            else
            {
                log.LogError("UPDATING NOTE FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note Updation failed." });
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("DeleteteNote")]

        public IActionResult DeleteteNote(int noteid)
        {
            log.LogInformation("DELETING NOTE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.DeleteNote(noteid, userid);

            if (result != false)
            {
                log.LogInformation("DELETING NOTE SUCCESSFULL.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note Deleted successfully." });
            }
            else
            {
                log.LogError("DELETING NOTE FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note Deletion failed." });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("IsPinOrNot")]
        public IActionResult IsPinOrNot(int noteid)
        {
            log.LogInformation("IS PIN OR NOT PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.IsPinOrNot(noteid, userid);

            if (result != null)
            {
                log.LogInformation("NOTE IS PINNED/UNPINNED SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note pinned successfully." });
            }
            else
            {
                log.LogError("NOTE PINNING FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note pinning failed." });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("IsArchieveOrNot")]
        public IActionResult IsArchieveOrNot(int noteid)
        {
            log.LogInformation("IS ARCHIEVE OR NOT PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.IsArchieveOrNot(noteid, userid);

            if (result != null)
            {
                log.LogInformation("NOTE IS ARCHIEVED/UNARCHIEVED SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note archieved successfully." });
            }
            else
            {
                log.LogError("NOTE ARCHIEVING FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note archieving failed." });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("IsTrashOrNot")]
        public IActionResult IsTrashOrNot(int noteid)
        {
            log.LogInformation("IS TRASH OR NOT PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.IsTrashOrNot(noteid, userid);

            if (result != null)
            {
                log.LogInformation("NOTE IS TRASHED/UNTRASHED SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note trashed successfully." });
            }
            else
            {
                log.LogError("NOTE TRASHING FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note trashing failed." });
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("DeleteForever")]
        public IActionResult DeleteForever(int noteid)
        {
            log.LogInformation("DELETE FOREVER PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.DeleteForever(noteid, userid);

            if (result != null)
            {
                log.LogInformation("DELETED FOREVER SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note deleted permanantly." });
            }
            else
            {
                log.LogError("DELETING FOREVER FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note deleting permanantly failed." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("Restore")]
        public IActionResult Restore(int noteid)
        {
            log.LogInformation("RESTORE PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.Restore(noteid, userid);

            if (result != null)
            {
                log.LogInformation("RESTORED SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "Note restored successfully." });
            }
            else
            {
                log.LogError("RESTORING FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Note restoring failed." });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("AddColour")]
        public IActionResult AddColour(int noteid, string colour)
        {
            log.LogInformation("ADD COLOUR PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.Colour(noteid, userid, colour);

            if (result != null)
            {
                log.LogInformation("ADDED COLOUR SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "added colour successfully." });
            }
            else
            {
                log.LogError("ADDING COLOUR FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "adding colour failed." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("AddReminder")]
        public IActionResult AddReminder(int noteid, DateTime reminder)
        {
            log.LogInformation("ADD REMINDER PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.Reminder(noteid, userid, reminder);

            if (result != null)
            {
                log.LogInformation("ADDED REMINDER SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "added reminder successfully." });
            }
            else
            {
                log.LogError("ADDING REMINDER FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "adding reminder failed." });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("UploadImage")]
        public IActionResult UploadImage(int noteid, IFormFile img)
        {
            log.LogInformation("UPLOADING IMAGE PROCEDURE STARTED.....");
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = noteBusiness.UploadImage(noteid, userid, img);

            if (result != null)
            {
                log.LogInformation("UPLOADED IMAGE SUCCESSFULLY.....");
                return Ok(new ResponseModel<string> { Status = true, Message = "uploaded image successfully." });
            }
            else
            {
                log.LogError("UPLOADING IMAGE FAILED.....");
                return BadRequest(new ResponseModel<string> { Status = false, Message = "uploading image failed." });
            }
        }

    }
}
