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
    public class ProjectDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of all the projects present in the database
        /// </summary>
        /// <example>GET api/ProjectData/GetAllProjects</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ProjectData/GetAllProjects"
        /// GET: curl "http://localhost:50860/api/ProjectData/GetAllProjects"
        /// GET: curl "http://localhost:50860/api/ProjectData/GetAllProjects"
        /// </example>
        /// <returns>
        /// A list of all the projects present in DB.
        /// </returns>
        // GET: api/ProjectData/GetAllProjects
        [HttpGet]
        [Route("api/ProjectData/GetAllProjects")]
        public IQueryable<Project> GetAllProjects()
        {
            return db.Projects;
        }

        /// <summary>
        /// Returns details of a particular project
        /// </summary>
        /// <param name="id">the id of project to fetch the details of it</param>
        /// <example>GET api/ProjectData/GetProjectDetails/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/ProjectData/GetProjectDetails/1"
        /// GET: curl "http://localhost:50860/api/ProjectData/GetProjectDetails/2"
        /// GET: curl "http://localhost:50860/api/ProjectData/GetProjectDetails/3"
        /// </example>
        /// <returns>
        /// A single project object
        /// </returns>
        // GET: api/ProjectData/GetProjectDetails/5
        [ResponseType(typeof(Project))]
        [HttpGet]
        [Route("api/ProjectData/GetProjectDetails/{id}")]
        public IHttpActionResult GetProjectDetails(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        /// <summary>
        /// Update a project with a given id and the updated object
        /// </summary>
        /// <param name="id">The id for which we need to change the data</param>
        /// <param name="project">the updated project data</param>
        /// <example>
        /// POST api/ProjectData/UpdateProject/{id}
        /// Request body
        /// {
        ///	"ProjectName":"Test Project",
        ///	"ProjectDescription":"This is a updated project description",
        ///	"DueDate":"2024-03-21"
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ProjectData/UpdateProject/2" -d @project.json -H "Content-type: application/json"
        /// </example>
        // POST: api/ProjectData/UpdateProject/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/ProjectData/UpdateProject/{id}")]
        public IHttpActionResult UpdateProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        /// Add a new project in the database
        /// </summary>
        /// <param name="project">the new project data which we need to add in the database</param>
        /// <example>
        /// POST api/ProjectData/AddProject
        /// Request body
        /// {
        ///	"ProjectName":"Brand New Project",
        ///	"ProjectDescription":"This is a brand new project description",
        ///	"DueDate":"2024-02-03"
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ProjectData/AddProject" -d @project.json -H "Content-type: application/json"
        /// </example>
        // POST: api/ProjectData/AddProject
        [ResponseType(typeof(Project))]
        [HttpPost]
        [Route("api/ProjectData/AddProject")]
        public IHttpActionResult AddProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes a particular project from the database
        /// </summary>
        /// <param name="id">The project id which we want to remove.</param>
        /// <example>POST api/ProjectData/DeleteProject/3</example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/ProjectData/DeleteProject/16" -d "" -H "Content-type: application/json"
        /// </example>
        // POST: api/ProjectData/DeleteProject/5
        [ResponseType(typeof(Project))]
        [HttpPost]
        [Route("api/ProjectData/DeleteProject/{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }
}