using BusinessLayer.Interfaces;
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


    }
}
