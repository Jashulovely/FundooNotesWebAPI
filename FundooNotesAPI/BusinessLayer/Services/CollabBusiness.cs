using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollabBusiness : ICollabBusiness
    {
        private readonly ICollabRepo repo;
        public CollabBusiness(ICollabRepo repo)
        {
            this.repo = repo;
        }
        public CollaboratorEntity AddCollaborator(int userid, int noteid, string collabEmail)
        {
            return repo.AddCollaborator(userid, noteid, collabEmail);
        }
        public List<CollaboratorEntity> CollabsList(int userid, int noteid)
        {
            return repo.CollabsList(userid, noteid);
        }

        public bool RemoveCollaborator(int userid, int noteid, string collabEmail)
        {
            return repo.RemoveCollaborator(userid, noteid, collabEmail);
        }
    }
}
