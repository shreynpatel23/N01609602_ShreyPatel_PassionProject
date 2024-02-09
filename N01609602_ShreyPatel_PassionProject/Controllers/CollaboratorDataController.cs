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
    public class CollaboratorDataController : ApiController
    {
        // initialize the db context once and for all
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of all the collaborators present in the database
        /// </summary>
        /// <example>GET api/CollaboratorData/GetAllCollaborators</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/CollaboratorData/GetAllCollaborators"
        /// </example>
        /// <returns>
        /// A list of all the collaboratorspresent in DB.
        /// </returns>
        // GET: api/CollaboratorData/GetAllCollaborators
        [HttpGet]
        [Route("api/CollaboratorData/GetAllCollaborators")]
        public IQueryable<Collaborator> GetAllCollaborators()
        {
            return db.Collaborators;
        }


        /// <summary>
        /// Returns details of a particular Collaborator
        /// </summary>
        /// <param name="id">the id of collaborator to fetch the details of it</param>
        /// <example>GET api/CollaboratorData/GetCollaboratorsDetails/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/CollaboratorData/GetCollaboratorsDetails/1"
        /// GET: curl "http://localhost:50860/api/CollaboratorData/GetCollaboratorsDetails/2"
        /// GET: curl "http://localhost:50860/api/CollaboratorData/GetCollaboratorsDetails/3"
        /// </example>
        /// <returns>
        /// A single collaborator object
        /// </returns>
        // GET: api/CollaboratorData/GetCollaboratorsDetails/5
        [ResponseType(typeof(Collaborator))]
        [HttpGet]
        [Route("api/CollaboratorData/GetCollaboratorsDetails/{id}")]
        public IHttpActionResult GetCollaborator(int id)
        {
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return NotFound();
            }

            return Ok(collaborator);
        }

        /// <summary>
        /// Update a collaborator with a given id and the updated details
        /// </summary>
        /// <param name="id">The id for which we need to change the data</param>
        /// <param name="collaborator">the updated collaborator data</param>
        /// <example>
        /// POST api/CollaboratorData/UpdateCollaborator/{id}
        /// Request body
        /// {
        ///	"CollaboratorFirstName":"Jhon",
        ///	"CollaboratorLastName": "Doe",
        ///	"CollaboratorEmail":"jhon@gmail.com",
        ///	"CollaboratorRole":"Web Developer"
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/CollaboratorData/UpdateCollaborator/2" -d @collaborator.json -H "Content-type: application/json"
        /// </example>
        // POST: api/CollaboratorData/UpdateCollaborator/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/CollaboratorData/UpdateCollaborator/{id}")]
        public IHttpActionResult UpdateCollaborator(int id, Collaborator collaborator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != collaborator.CollaboratorId)
            {
                return BadRequest();
            }

            db.Entry(collaborator).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollaboratorExists(id))
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
        /// Add a new collaborator in the database
        /// </summary>
        /// <param name="collaborator">the collaborator data which we need to add in the database</param>
        /// <example>
        /// POST api/CollaboratorData/AddCollaborator
        /// Request body
        /// {
        ///	"CollaboratorFirstName":"Jhon",
        ///	"CollaboratorLastName": "Doe",
        ///	"CollaboratorEmail":"jhon@gmail.com",
        ///	"CollaboratorRole":"Web Developer"
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/CollaboratorData/AddCollaborator" -d @collaborator.json -H "Content-type: application/json"
        /// </example>
        // POST: api/CollaboratorData/AddCollaborator
        [ResponseType(typeof(Collaborator))]
        [HttpPost]
        [Route("api/CollaboratorData/AddCollaborator")]
        public IHttpActionResult AddCollaborator(Collaborator collaborator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Collaborators.Add(collaborator);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes a particular collaborator from the database
        /// </summary>
        /// <param name="id">The collaborator id which we want to remove.</param>
        /// <example>POST api/CollaboratorData/DeleteCollaborators/3</example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/CollaboratorData/DeleteCollaborators/3" -d "" -H "Content-type: application/json"
        /// </example>
        // POST: api/CollaboratorData/DeleteCollaborators/5
        [ResponseType(typeof(Collaborator))]
        [HttpPost]
        [Route("api/CollaboratorData/DeleteCollaborators/{id}")]
        public IHttpActionResult DeleteCollaborator(int id)
        {
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return NotFound();
            }

            db.Collaborators.Remove(collaborator);
            db.SaveChanges();

            return Ok(collaborator);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CollaboratorExists(int id)
        {
            return db.Collaborators.Count(e => e.CollaboratorId == id) > 0;
        }
    }
}