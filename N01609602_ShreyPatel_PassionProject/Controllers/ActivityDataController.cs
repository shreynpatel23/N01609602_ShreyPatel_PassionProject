using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using N01609602_ShreyPatel_PassionProject.Models;

namespace N01609602_ShreyPatel_PassionProject.Controllers
{
    public class ActivityDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ActivityData/GetAllActivities
        [HttpGet]
        [Route("api/ActivityData/GetAllActivities")]
        public IEnumerable<ActivityDto> GetAllActivities()
        {
            // capture the list of result in activity list;
            List<Activity> Activities = db.Activities.ToList();
            // create a new activity dtos list to store the response
            List<ActivityDto> ActivityDtos = new List<ActivityDto>();

            // loop through the activites array and push it to activities dto
            Activities.ForEach(activity => ActivityDtos.Add(new ActivityDto()
            {
                ActivityId = activity.ActivityId,
                ActivityName = activity.ActivityName,
                ActivityDescription = activity.ActivityDescription,
                ActivityDueDate = activity.ActivityDueDate,
                ActivityEstimates = activity.ActivityEstimates, 
                ActivityPriority = activity.ActivityPriority,   
                ActivityStatus = activity.ActivityStatus,
                CollaboratorId = activity.Collaborator.CollaboratorId,
                CollaboratorFirstName = activity.Collaborator.CollaboratorFirstName,
                CollaboratorLastName = activity.Collaborator.CollaboratorLastName,
                ProjectId = activity.Project.ProjectId,
                ProjectName = activity.Project.ProjectName,
                ProjectDescription = activity.Project.ProjectDescription,
                DueDate = activity.Project.DueDate,
            }));

            // return the activity dto
            return ActivityDtos;
        }

        // Get all activites for a project with id
        [HttpGet]
        [Route("api/ActivityData/GetActivitiesForProject/{id}")]
        public IEnumerable<ActivityDto> GetActivitiesForProject(int id) {
            // capture the list of result in activity list;
            List<Activity> Activities = db.Activities.Where(a => a.ProjectId == id).ToList();
            // create a new activity dtos list to store the response
            List<ActivityDto> ActivityDtos = new List<ActivityDto>();

            // loop through the activites array and push it to activities dto
            Activities.ForEach(activity => ActivityDtos.Add(new ActivityDto()
            {
                ActivityId = activity.ActivityId,
                ActivityName = activity.ActivityName,
                ActivityDescription = activity.ActivityDescription,
                ActivityDueDate = activity.ActivityDueDate,
                ActivityEstimates = activity.ActivityEstimates,
                ActivityPriority = activity.ActivityPriority,
                ActivityStatus = activity.ActivityStatus,
                CollaboratorId = activity.Collaborator.CollaboratorId,
                CollaboratorFirstName = activity.Collaborator.CollaboratorFirstName,
                CollaboratorLastName = activity.Collaborator.CollaboratorLastName,
                ProjectId = activity.Project.ProjectId,
                ProjectName = activity.Project.ProjectName,
                ProjectDescription = activity.Project.ProjectDescription,
                DueDate = activity.Project.DueDate,
            }));

            // return the activity dto
            return ActivityDtos;
        }

        // GET: api/ActivityData/GetActivityDetails/5
        [ResponseType(typeof(Activity))]
        [HttpGet]
        [Route("api/ActivityData/GetActivityDetails/{id}")]
        public IHttpActionResult GetActivityDetails(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/ActivityData/UpdateActivity/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/ActivityData/UpdateActivity/{id}")]
        public IHttpActionResult UpdateActivity(int id, Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.ActivityId)
            {
                return BadRequest();
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ActivityData/AddActivity
        [ResponseType(typeof(Activity))]
        [HttpPost]
        [Route("api/ActivityData/AddActivity")]
        public IHttpActionResult AddActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Activities.Add(activity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = activity.ActivityId }, activity);
        }

        // DELETE: api/ActivityData/DeleteActivity/5
        [ResponseType(typeof(Activity))]
        [HttpPost]
        [Route("api/ActivityData/DeleteActivity/{id}")]
        public IHttpActionResult DeleteActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Count(e => e.ActivityId == id) > 0;
        }
    }
}