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
        // define the http client and json serializer
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // create a new client and configure the base url
        static CollaboratorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // renders the error page
        // GET  Error
        public ActionResult Error()
        {

            return View();
        }

        // renders the list of collaborator
        // GET: Collaborator/List
        public ActionResult List()
        {
            // call the getAllCollaborator api
            string url = "CollaboratorData/GetAllCollaborators";

            // store the data in a response
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create an empty collaborator list and assign the response
            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;
            // send the data to the view
            return View(collaborators);
        }

        // renders the collaboraotr list page
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

        // renders the add collaborator form
        // GET Collaborator/Add
        public ActionResult Add()
        {
            return View();
        }

        // use this function to store the collaborator details in the database
        // POST: Collaborator/Create
        [HttpPost]
        public ActionResult Create(Collaborator collaborator)
        {
            // call the add collaborator api
            // curl -H "Content-Type:application/json" -d @collaborator.json https://localhost:44346/api/CollaboratorData/AddCollaborator
            string url = "CollaboratorData/AddCollaborator";

            // serialize the json payload
            string jsonpayload = jss.Serialize(collaborator);

            // update the content type
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            // store the response
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            // check for status code
            // if success -> redirect to list
            // else -> redirect to error page
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // renders the edit collaborator form 
        // GET Collaborator/Edit/5
        public ActionResult Edit(int id)
        {
            // fetch the details to prefill the form 
            // for that call the getCollaboratorDetails api
            string url = "CollaboratorData/GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create an empty collaborator object and assign the result
            Collaborator collaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            // send the data into the view
            return View(collaborator);
        }

        // use this function to update the collaborator details in the database
        // POST Collaborator/Update/2
        [HttpPost]
        public ActionResult Update(int id, Collaborator collaborator)
        {
            try
            {
                // call the updateCollaborator api
                // curl -H "Content-Type:application/json" -d @collaborator.json https://localhost:44346/api/CollaboratorData/UpdateCollaborator/2
                string url = "CollaboratorData/UpdateCollaborator/" + id;

                // serialize the json payload
                string jsonpayload = jss.Serialize(collaborator);

                // update the conetne type
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                // read the data and store it in httpresponsemessage
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                
                // redirect to the details page
                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // renders the confirm delete page for a collaborator
        // GET: Collaborator/ConfirmDelete/2
        public ActionResult ConfirmDelete(int id)
        {
            // fetch the collaborator details to display data on the confirm page
            string url = "CollaboratorData/GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create an empty collaborator object and assign the response to it
            Collaborator collaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            // send the collaborator details to the view
            return View(collaborator);
        }


        // use this function to delete the collaborator details from the database
        // POST: Collaborator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // call the deleteCollaborator api 
                // curl -d "" https://localhost:44346/api/CollaboratorData/DeleteCollaborators/2
                string url = "CollaboratorData/DeleteCollaborators/" + id;

                // update the content type as this is a post request
                HttpContent content = new StringContent("");
                content.Headers.ContentType.MediaType = "application/json";

                // assign the response to httpresponsemessage
                HttpResponseMessage response = client.PostAsync(url, content).Result;


                // check for status code
                // if success -> redirect to list page
                // else redirect to the error page
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