using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models
{
    public class Collaborator
    {
        [Key]
        // The primary key (Unique identifier) of collaborator
        public int CollaboratorId { get; set; }

        // First name of the collaborator
        public string CollaboratorFirstName { get; set; }

        // Last name of the collaborator
        public string CollaboratorLastName { get; set; }

        // Email of the collaborator
        public string CollaboratorEmail { get; set; }

        // Current role in the organisation.
        public string CollaboratorRole { get; set; }
    }
}