using Antlr.Runtime.Misc;
using N01609602_ShreyPatel_PassionProject.Models;
using N01609602_ShreyPatel_PassionProject.Models.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace N01609602_ShreyPatel_PassionProject.Controllers
{
    public class CollaboratorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CollaboratorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET  Error
        public ActionResult Error()
        {

            return View();
        }

        // GET: Collaborator/List
        public ActionResult List()
        {

            string url = "CollaboratorData/GetAllCollaborators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            return View(collaborators);
        }

        // GET Collaborator/Details/2
        public ActionResult Details(int id)
        {
            // as we need data from two modals (collaborator and tasks) we are going to use ViewModal
            CollaboratorActivity collaboratorActivity = new CollaboratorActivity();

            // get the selected collaborator with the given id
            //curl https://localhost:44346/api/CollaboratorData/GetCollaboratorsDetails/{id}

            string url = "CollaboratorData/GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Collaborator selectedCollaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            // assign the selected collaborator in the view modal
            collaboratorActivity.SelectedCollaborator = selectedCollaborator;

            // get the list of activities for a particular collaborator
            url = "ActivityData/GetActivitiesForCollaborator/" + id;

            // capture the response
            response = client.GetAsync(url).Result;

            // convert the response into the project modal
            IEnumerable<ActivityDto> activities = response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;

            // assign the activities to the viewmodal
            collaboratorActivity.activities = activities;

            return View(collaboratorActivity);
        }

        // GET Collaborator/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Collaborator/Create
        [HttpPost]
        public ActionResult Create(Collaborator collaborator)
        {
            // curl -H "Content-Type:application/json" -d @collaborator.json https://localhost:44346/api/CollaboratorData/AddCollaborator
            string url = "CollaboratorData/AddCollaborator";


            string jsonpayload = jss.Serialize(collaborator);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine("response is" + response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET Collaborator/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "CollaboratorData/GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Collaborator collaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            return View(collaborator);
        }

        // POST Collaborator/Update/2
        [HttpPost]
        public ActionResult Update(int id, Collaborator collaborator)
        {
            try
            {
                // curl -H "Content-Type:application/json" -d @collaborator.json https://localhost:44346/api/CollaboratorData/UpdateCollaborator/2
                string url = "CollaboratorData/UpdateCollaborator/" + id;

                string jsonpayload = jss.Serialize(collaborator);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                
                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Collaborator/ConfirmDelete/2
        public ActionResult ConfirmDelete(int id)
        {
            string url = "CollaboratorData/GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Collaborator collaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            return View(collaborator);
        }

        // POST: Collaborator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                // curl -d "" https://localhost:44346/api/CollaboratorData/DeleteCollaborators/2
                string url = "CollaboratorData/DeleteCollaborators/" + id;

                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}