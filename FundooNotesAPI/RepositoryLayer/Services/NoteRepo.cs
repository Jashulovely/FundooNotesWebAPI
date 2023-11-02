using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundoDBContext fundoocontext;

        public NoteRepo(FundoDBContext fundoocontext)
        {
            this.fundoocontext = fundoocontext;
        }
        public NoteEntity AddNote(NoteModel model, int UserId)
        {
            NoteEntity entity = new NoteEntity();
            entity.Title = model.Title;
            entity.Note = model.Note;
            entity.Reminder = model.Reminder;
            entity.Color = model.Color;
            entity.Image = model.Image;
            entity.IsArchieve = model.IsArchieve;
            entity.IsPin = model.IsPin;
            entity.IsTrash = model.IsTrash;
            entity.CreatedAt = model.CreatedAt;
            entity.UpdatedAt = model.UpdatedAt;
            entity.UserId = UserId;

            fundoocontext.Notes.Add(entity);
            var result = fundoocontext.SaveChanges();
            if (result > 0)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        public List<NoteEntity> NotessList()
        {
            try
            {
                List<NoteEntity> notes = (List<NoteEntity>)fundoocontext.Notes.ToList();
                return notes;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
