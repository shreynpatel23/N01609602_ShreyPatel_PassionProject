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
    public class CollaboratorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CollaboratorData/GetAllCollaborators
        [HttpGet]
        public IQueryable<Collaborator> GetAllCollaborators()
        {
            return db.Collaborators;
        }

        // GET: api/CollaboratorData/GetCollaboratorsDetails/5
        [ResponseType(typeof(Collaborator))]
        [HttpGet]
        public IHttpActionResult GetCollaborator(int id)
        {
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return NotFound();
            }

            return Ok(collaborator);
        }

        // PUT: api/CollaboratorData/UpdateCollaborators/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/CollaboratorData/AddCollaborators
        [ResponseType(typeof(Collaborator))]
        [HttpPost]
        public IHttpActionResult AddCollaborator(Collaborator collaborator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Collaborators.Add(collaborator);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = collaborator.CollaboratorId }, collaborator);
        }

        // DELETE: api/CollaboratorData/DeleteCollaborators/5
        [ResponseType(typeof(Collaborator))]
        [HttpPost]
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