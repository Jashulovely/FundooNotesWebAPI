using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepo
    {
        public LabelEntity AddLabel(int UserId, int NoteId, string LabelName);
        public List<LabelEntity> LabelsList();
        public List<LabelEntity> GetLabel(int userid, string lableName);
        public bool ChangeLabels(int userid, int noteid, string labelName);
        public bool EditLabels(int userid, string labelName, string newLabelName);
        public bool DeleteLabel(int userid, int noteid, string labelName);
        public bool DeleteLabels(int userid, string labelName);
    }
}
