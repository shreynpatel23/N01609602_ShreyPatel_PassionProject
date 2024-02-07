using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models.ViewModels
{
    public class CollaboratorActivity
    {
        // Collaborator from the collaborator modal
        public Collaborator SelectedCollaborator { get; set; }

        // list of activities for a particular collaborator
        public IEnumerable<ActivityDto> activities { get; set; }
    }
}