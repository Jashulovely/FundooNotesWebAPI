using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INoteBusiness
    {
        public NoteEntity AddNote(NoteModel model, int UserId);
        public List<NoteEntity> NotessList();
        public NoteEntity GetNote(int userid, int noteid);
        public List<NoteEntity> DateNote(int userid, DateTime day);
        public bool UpdateNote(int noteid, int userid, NoteModel model);
        public bool DeleteNote(int noteid, int userid);
        public bool IsPinOrNot(int noteid, int userid);
        public bool IsArchieveOrNot(int noteid, int userid);
        public bool IsTrashOrNot(int noteid, int userid);
        public bool DeleteForever(int noteid, int userid);
        public bool Restore(int noteid, int userid);
        public NoteEntity Colour(int noteid, int userid, string colour);
        public NoteEntity Reminder(int noteid, int userid, DateTime reminder);
        public string UploadImage(int noteId, int userid, IFormFile img);
    }
}
