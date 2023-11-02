using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelLayer.Models
{
    public class NoteModel
    {
        public string Title { get; set; }

        public string Note { get; set; }

        public DateTime? Reminder { get; set; }

        public string Color { get; set; }

        public string Image { get; set; }

        public bool IsArchieve { get; set; }

        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
