using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo repo;
        public LabelBusiness(ILabelRepo repo)
        {
            this.repo = repo;
        }

        public LabelEntity AddLabel(int UserId, int NoteId, string LabelName)
        {
            return repo.AddLabel(UserId, NoteId, LabelName);
        }
        public List<LabelEntity> LabelsList()
        {
            return repo.LabelsList();
        }
        public List<LabelEntity> GetLabel(int userid, string labelName)
        {
            return repo.GetLabel(userid, labelName);
        }
        public bool ChangeLabels(int userid, int noteid, string labelName)
        {
            return repo.ChangeLabels(userid, noteid, labelName);
        }

        public bool EditLabels(int userid, string labelName, string newLabelName)
        {
            return repo.EditLabels(userid, labelName, newLabelName);
        }

        public bool DeleteLabel(int userid, int noteid, string labelName)
        {
            return repo.DeleteLabel(userid, noteid, labelName);
        }

        public bool DeleteLabels(int userid, string labelName)
        {
            return repo.DeleteLabels(userid, labelName);
        }
    }
}
