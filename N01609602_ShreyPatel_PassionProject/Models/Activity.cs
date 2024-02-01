using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models
{
    public class Activity
    {
        [Key]
        // The primary key of activity
        public int ActivityId { get; set; }

        // The name of the activity
        public string ActivityName { get; set; }

        // A short description to explan the activity
        public string ActivityDescription { get; set; }

        // The priority of the Activity (Easy, Medium, Hard).
        public string ActivityPriority { get; set; }

        // Current Status of the Activity (Todo, In-progress, Done).
        public string ActivityStatus { get; set; }

        // Due Date of the activity
        public DateTime ActivityDueDate { get; set; }

        // Estimates to how long it will take to complete the activity
        // Estimates are in HOURS
        public string ActivityEstimates { get; set; }

        // An activity belongs to one project
        // A project can have many activities
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        // An Activity belongs to one collaborator
        // A Collaborator can have many activities
        [ForeignKey("Collaborator")]
        public int CollaboratorId { get; set; }
        public virtual Collaborator Collaborator { get; set; }
    }

    public class ActivityDto
    {
        // The primary key of activity
        public int ActivityId { get; set; }

        // The name of the activity
        public string ActivityName { get; set; }

        // A short description to explan the activity
        public string ActivityDescription { get; set; }

        // The priority of the Activity (Easy, Medium, Hard).
        public string ActivityPriority { get; set; }

        // Current Status of the Activity (Todo, In-progress, Done).
        public string ActivityStatus { get; set; }

        // Due Date of the activity
        public DateTime ActivityDueDate { get; set; }

        // Estimates to how long it will take to complete the activity
        // Estimates are in HOURS
        public string ActivityEstimates { get; set; }

        // First name of the collaborator
        public string CollaboratorFirstName { get; set; }

        // Last name of the collaborator
        public string CollaboratorLastName { get; set; }

        // Name of the project in which the task is
        public string ProjectName { get; set; }

        // Description of the project
        public string ProjectDescription { get; set; }

        // Due date of the project
        public DateTime DueDate { get; set; }
    }
}