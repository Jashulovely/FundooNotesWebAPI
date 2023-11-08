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
    public class LabelRepo : ILabelRepo
    {
        private readonly FundoDBContext fundoocontext;
        public LabelRepo(FundoDBContext fundoocontext)
        {
            this.fundoocontext = fundoocontext;
        }

        public LabelEntity AddLabel(int UserId, int NoteId, string LabelName)
        {
            LabelEntity entity = new LabelEntity();
            entity.UserId = UserId;
            entity.NoteId = NoteId;
            entity.LabelName = LabelName;

            fundoocontext.Labels.Add(entity);
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

        public List<LabelEntity> LabelsList()
        {
            try
            {
                List<LabelEntity> labels = (List<LabelEntity>)fundoocontext.Labels.ToList();
                return labels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<LabelEntity> GetLabel(int userid, string labelName)
        {
            List<LabelEntity> result = fundoocontext.Labels.Where(e => e.UserId == userid && e.LabelName == labelName).ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }


        public bool ChangeLabels(int userid, int noteid, string labelName)
        {
            try
            {
                var result = fundoocontext.Labels.FirstOrDefault(e => e.UserId == userid && e.NoteId == noteid);
                if (result != null)
                {
                    
                    if (labelName != null)
                    {
                        result.LabelName = labelName;
                    }

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


        public bool EditLabels(int userid, string labelName, string newLabelName)
        {
            try
            {
                List<LabelEntity> result = (List<LabelEntity>)fundoocontext.Labels.Where(e => e.UserId == userid && e.LabelName == labelName).ToList();
                if (result != null)
                {
                    foreach (var entity in result)
                    {
                        if (entity.LabelName != null)
                        {
                            entity.LabelName = newLabelName;
                        }
                    }

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


        public bool DeleteLabel(int userid, int noteid, string labelName)
        {
            try
            {
                var result = fundoocontext.Labels.FirstOrDefault(e => e.NoteId == noteid && e.UserId == userid && e.LabelName == labelName);
                if (result != null)
                {
                    fundoocontext.Labels.Remove(result);
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


        public bool DeleteLabels(int userid, string labelName)
        {
            try
            {
                List<LabelEntity> result = (List<LabelEntity>)fundoocontext.Labels.Where(e => e.UserId == userid && e.LabelName == labelName).ToList();
                if (result != null)
                {
                    foreach (var entity in result)
                    {
                        if (entity.LabelName == labelName)
                        {
                            fundoocontext.Labels.Remove(entity);
                        }
                    }

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

    }
}
