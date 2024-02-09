﻿using System;
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
        // initialize the db context once in for all
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of all the activities present in the database. Here as
        /// we have a foreign key we need to return ActivityDto.
        /// </summary>
        /// <example>GET api/ActivityData/GetAllActivities</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ActivityData/GetAllActivities"
        /// </example>
        /// <returns>
        /// A list of all the activities DTO (Data trasnferable object) present in DB.
        /// </returns>
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
            }));

            // return the activity dto
            return ActivityDtos;
        }

        /// <summary>
        /// Returns a list of all the activities for a particular project id
        /// </summary>
        /// <param name="id">The project id for which we want to fetch all the activities</param>
        /// <example>GET api/ActivityData/GetActivitiesForProject/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ActivityData/GetActivitiesForProject/2"
        /// </example>
        /// <returns>
        /// A list of all the activities present in DB for a particular project.
        /// </returns>
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
            }));

            // return the activity dto
            return ActivityDtos;
        }

        /// <summary>
        /// Returns a list of all the activities for a particular collaborator id
        /// </summary>
        /// <param name="id">The collaborator id for which we want to fetch all the activities</param>
        /// <example>GET api/ActivityData/GetActivitiesForCollaborator/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ActivityData/GetActivitiesForCollaborator/2"
        /// </example>
        /// <returns>
        /// A list of all the activities present in DB for a particular collaborator.
        /// </returns>
        // Get all activites for a collaborator with id
        [HttpGet]
        [Route("api/ActivityData/GetActivitiesForCollaborator/{id}")]
        public IEnumerable<ActivityDto> GetActivitiesForCollaborator(int id)
        {
            // capture the list of result in activity list;
            List<Activity> Activities = db.Activities.Where(a => a.CollaboratorId == id).ToList();
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
            }));

            // return the activity dto
            return ActivityDtos;
        }

        /// <summary>
        /// Returns details of a particular activity
        /// </summary>
        /// <param name="id">the id of activity to fetch the details of it</param>
        /// <example>GET api/ActivityData/GetActivityDetails/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ActivityData/GetActivityDetails/1"
        /// GET: curl "http://localhost:50860/api/ActivityData/GetActivityDetails/2"
        /// GET: curl "http://localhost:50860/api/ActivityData/GetActivityDetails/3"
        /// </example>
        /// <returns>
        /// A single activity DTO (data transferable object) with data
        /// </returns>
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

            ActivityDto activityDto = new ActivityDto()
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
            };

            return Ok(activityDto);
        }

        /// <summary>
        /// Update a particular collaborator with a given id and the updated activity details
        /// </summary>
        /// <param name="id">The id for which we need to change the data</param>
        /// <param name="activity">the updated activity data</param>
        /// <example>
        /// POST api/ActivityData/UpdateActivity/{id}
        /// Request body
        /// {
        /// "ActivityName" : "Test Activity Updated",
        /// "ActivityDescription" : "Test Activity Updated description",
        /// "ActivityDueDate" : "2024-05-21",
        ///	"ActivityStatus": "Todo",
        ///	"ActivityPriority" : "Easy",
        ///	"ActivityEstimates":"2",
        ///	"ProjectId": 2,
        ///	"collaboratorId": 1,
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ActivityData/UpdateActivity/2" -d @activity.json -H "Content-type: application/json"
        /// </example>
        // POST: api/ActivityData/UpdateActivity/5
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

        /// <summary>
        /// Update a particular collaborator with a given id and the updated activity details
        /// </summary>
        /// <param name="activity">the activity data which needs to be entered in the database</param>
        /// <example>
        /// POST api/ActivityData/AddActivity/{id}
        /// Request body
        /// {
        /// "ActivityName" : "New Test Activity",
        /// "ActivityDescription" : "New Test Activity description",
        /// "ActivityDueDate" : "2024-02-11",
        ///	"ActivityStatus": "Todo",
        ///	"ActivityPriority" : "Easy",
        ///	"ActivityEstimates":"3",
        ///	"ProjectId": 3,
        ///	"collaboratorId": 2,
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ActivityData/AddActivity" -d @activity.json -H "Content-type: application/json"
        /// </example>
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

            return Ok();
        }

        /// <summary>
        /// Deletes a particular activity from the database
        /// </summary>
        /// <param name="id">The id which we want to remove.</param>
        /// <example>POST api/ProjectData/DeleteActivity/3</example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ProjectData/DeleteActivity/16" -d "" -H "Content-type: application/json"
        /// </example>
        // POST: api/ActivityData/DeleteActivity/5
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