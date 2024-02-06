using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01609602_ShreyPatel_PassionProject.Models.ViewModels
{
    public class ProjectActivity
    {
        // project from the project modal
        public Project selectedProject { get; set; }

        // list of activities for a particular project
        public IEnumerable<ActivityDto> activities { get; set; }
    }
}