using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NoteBusiness : INoteBusiness
    {
        private readonly INoteRepo noteRepo;
        public NoteBusiness(INoteRepo noteRepo)
        {
            this.noteRepo = noteRepo;
        }
        public NoteEntity AddNote(NoteModel model, int UserId)
        {
            return noteRepo.AddNote(model, UserId);
        }

        public List<NoteEntity> NotessList()
        {
            return noteRepo.NotessList();
        }

        public NoteEntity GetNote(int userid, int noteid)
        {
            return noteRepo.GetNote(userid, noteid);
        }

        public List<NoteEntity> DateNote(int userid, DateTime day)
        {
            return noteRepo.DateNote(userid, day);
        }

        public bool UpdateNote(int noteid, int userid, NoteModel model)
        {
            return noteRepo.UpdateNote(noteid, userid, model);
        }

        public bool DeleteNote(int noteid, int userid)
        {
            return noteRepo.DeleteNote(noteid, userid);
        }

        public bool IsPinOrNot(int noteid, int userid)
        {
            return noteRepo.IsPinOrNot(noteid, userid);
        }

        public bool IsArchieveOrNot(int noteid, int userid)
        {
            return noteRepo.IsArchieveOrNot(noteid, userid);
        }

        public bool IsTrashOrNot(int noteid, int userid)
        {
            return noteRepo.IsTrashOrNot(noteid, userid);
        }

        public bool DeleteForever(int noteid, int userid)
        {
            return noteRepo.DeleteForever(noteid, userid);
        }

        public bool Restore(int noteid, int userid)
        {
            return noteRepo.Restore(noteid, userid);
        }
        public NoteEntity Colour(int noteid, int userid, string colour)
        {
            return noteRepo.Colour(noteid, userid, colour);
        }

        public NoteEntity Reminder(int noteid, int userid, DateTime reminder)
        {
            return noteRepo.Reminder(noteid, userid, reminder);
        }

        public string UploadImage(int noteId, int userid, IFormFile img)
        {
            return noteRepo.UploadImage(noteId, userid, img);
        }
    }
}
