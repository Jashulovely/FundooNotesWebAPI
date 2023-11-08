using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBusiness
    {
        public CollaboratorEntity AddCollaborator(int userid, int noteid, string collabEmail);
        public List<CollaboratorEntity> CollabsList(int userid, int noteid);
        public bool RemoveCollaborator(int userid, int noteid, string collabEmail);
    }
}
