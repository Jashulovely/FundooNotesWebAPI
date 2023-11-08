using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }
        public string CollaboratorEmail { get; set; }

        [ForeignKey("Notes")]
        public int NoteId { get; set; }
        [JsonIgnore]
        public virtual NoteEntity Notes { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity Users { get; set; }
    }
}
