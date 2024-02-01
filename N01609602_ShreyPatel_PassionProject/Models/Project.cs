using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models
{
    public class Project
    {
        [Key]
        // The primary Key of the Project
        public int ProjectId { get; set; }

        // Name of the Project
        public string ProjectName { get; set; }

        // Short description of the project
        public string ProjectDescription { get; set; }

        // Due date of the project
        public DateTime DueDate { get; set; }

    }
}