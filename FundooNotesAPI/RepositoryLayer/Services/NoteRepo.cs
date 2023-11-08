using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundoDBContext fundoocontext;
        private readonly IConfiguration configuration;

        public NoteRepo(FundoDBContext fundoocontext, IConfiguration configuration)
        {
            this.fundoocontext = fundoocontext;
            this.configuration = configuration;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity GetNote(int userid, int noteid)
        {
            NoteEntity result = fundoocontext.Notes.FirstOrDefault(e => e.NoteId == noteid && e.UserId == userid);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public List<NoteEntity> DateNote(int userid, DateTime date)
        {
            List<NoteEntity> result = (List<NoteEntity>)fundoocontext.Notes.Where(e => e.CreatedAt == date).ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateNote(int noteid, int userid, NoteModel model)
        {
            try
            {
                var result = fundoocontext.Notes.FirstOrDefault(e => e.NoteId == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (model.Title != null)
                    {
                        result.Title = model.Title;
                    }
                    if (model.Note != null)
                    {
                        result.Note = model.Note;
                    }
                    result.UpdatedAt = DateTime.Now;
                    fundoocontext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteNote(int noteid, int userid)
        {
            try
            {
                var result = fundoocontext.Notes.FirstOrDefault(e => e.NoteId == noteid && e.UserId == userid);
                if (result != null)
                {
                    fundoocontext.Notes.Remove(result);
                    fundoocontext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsPinOrNot(int noteid, int userid)
        {
            try
            {
                NoteEntity result = fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if(result != null)
                {
                    if (result.IsPin == true)
                    {
                        result.IsPin = false;
                        this.fundoocontext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsPin = true;
                        this.fundoocontext.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public bool IsArchieveOrNot(int noteid, int userid)
        {
            try
            {
                NoteEntity result = fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if (result != null)
                {
                    if (result.IsArchieve == true)
                    {
                        result.IsArchieve = false;
                        this.fundoocontext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsArchieve = true;
                        this.fundoocontext.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTrashOrNot(int noteid, int userid)
        {
            try
            {
                NoteEntity result = fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if (result != null)
                {
                    if (result.IsTrash == true)
                    {
                        result.IsTrash = false;
                        this.fundoocontext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsTrash = true;
                        this.fundoocontext.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool DeleteForever(int noteid, int userid)
        {
            try
            {
                NoteEntity result = this.fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if(result.IsTrash == true) 
                {
                    this.fundoocontext.Remove(result);
                    this.fundoocontext.SaveChanges();
                    return false;
                }
                result.IsTrash = true;
                this.fundoocontext.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            { 
                throw ex; 
            }
        }


        public bool Restore(int noteid, int userid)
        {
            try
            {
                NoteEntity result = this.fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if (result.IsTrash == true)
                {
                    result.IsTrash = false;
                    this.fundoocontext.SaveChanges();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity Colour(int noteid,int userid, string colour)
        {
            try
            {
                NoteEntity note = this.fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if (note.Color != null)
                {
                    note.Color = colour;
                    this.fundoocontext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public NoteEntity Reminder(int noteid, int userid,DateTime reminder)
        {
            try
            {
                NoteEntity note = this.fundoocontext.Notes.FirstOrDefault(x => x.NoteId == noteid && x.UserId == userid);
                if (note.Reminder != null)
                {
                    note.Reminder = reminder;
                    this.fundoocontext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string UploadImage(int noteId, int userid, IFormFile img)
        {

            var result = fundoocontext.Notes.FirstOrDefault(i => i.NoteId == noteId && i.UserId == userid);
            if (result != null)
            {
                Account acc = new Account(
                    this.configuration["CloudinarySettings:CloudName"],
                    this.configuration["CloudinarySettings:ApiKey"],
                    this.configuration["CloudinarySettings:ApiSecret"]);

                Cloudinary cloudinary = new Cloudinary(acc);

                var ulP = new ImageUploadParams()
                {
                    File = new FileDescription(img.FileName, img.OpenReadStream()),

                };
                var uploadResult = cloudinary.Upload(ulP);
                string imagepath = uploadResult.Url.ToString();
                result.Image = imagepath;
                fundoocontext.SaveChanges();
                return "Image Uploaded Successfully";
            }
            else
            {
                return null;
            }
        }


    }
}
