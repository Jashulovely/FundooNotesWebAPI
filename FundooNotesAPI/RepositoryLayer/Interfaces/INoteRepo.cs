using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRepo
    {
        public NoteEntity AddNote(NoteModel model, int UserId);
        public List<NoteEntity> NotessList();
    }
}
