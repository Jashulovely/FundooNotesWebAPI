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
    }
}
