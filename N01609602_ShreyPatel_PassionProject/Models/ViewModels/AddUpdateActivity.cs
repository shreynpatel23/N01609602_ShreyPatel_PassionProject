using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models.ViewModels
{
    public class AddUpdateActivity
    {
        // selected activity details
        public ActivityDto SelectedActivity { get; set; }
        // list of all collaborators in the system
        public IEnumerable<Collaborator> Collaborators { get; set; }
        // list of all projects in the system
        public IEnumerable<Project> Projects { get; set; }
    }
}