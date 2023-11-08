using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollabRepo : ICollabRepo
    {
        private readonly FundoDBContext fundoocontext;
        public CollabRepo(FundoDBContext fundoocontext)
        {
            this.fundoocontext = fundoocontext;
        }

        public CollaboratorEntity AddCollaborator(int userid, int noteid, string collabEmail)
        {
            CollaboratorEntity co = new CollaboratorEntity();
            co.UserId = userid;
            co.NoteId = noteid;
            co.CollaboratorEmail = collabEmail;

            fundoocontext.Collaborators.Add(co);
            var result = fundoocontext.SaveChanges();
            if (result > 0)
            {

                return co;
            }
            else
            {
                return null;
            }
        }

        public List<CollaboratorEntity> CollabsList(int userid, int noteid)
        {
            try
            {
                List<CollaboratorEntity> collabs = (List<CollaboratorEntity>)fundoocontext.Collaborators.ToList();
                return collabs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveCollaborator(int userid,int noteid, string collabEmail)
        {
            var result = fundoocontext.Collaborators.FirstOrDefault(m => m.UserId == userid && m.NoteId == noteid && m.CollaboratorEmail == collabEmail);
            if (result != null)
            {
                fundoocontext.Collaborators.Remove(result);
                fundoocontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
