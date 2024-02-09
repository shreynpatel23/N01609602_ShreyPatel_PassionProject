using N01609602_ShreyPatel_PassionProject.Migrations;
using N01609602_ShreyPatel_PassionProject.Models;
using N01609602_ShreyPatel_PassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Activity = N01609602_ShreyPatel_PassionProject.Models.Activity;

namespace N01609602_ShreyPatel_PassionProject.Controllers
{
    public class ActivityController : Controller
    {
        // declare http client and js serializer
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // create a new client and configure the base url
        static ActivityController()
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

        // renders the activity list page
        // GET: Activity/List
        public ActionResult List()
        {
            // other way to fetch the data is 
            //curl https://localhost:44346/api/ActivityData/GetAllActivities

            // create a url to call api
            string url = "ActivityData/GetAllActivities";

            // get the response 
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create empty activityDTO list and read the data from the response
            IEnumerable<ActivityDto> activities = response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;


            // send the data in the view
            return View(activities);
        }


        // renders the activity details page
        // GET: Activity/Details/{id}
        public ActionResult Details(int id)
        {
            // other way to fetch the data is 
            //curl https://localhost:44346/api/ActivityData/GetActivityDetails/2

            // assign the url to a string
            string url = "ActivityData/GetActivityDetails/" + id;

            // use the string to get the response
            HttpResponseMessage response = client.GetAsync(url).Result;


            // create an empty DTO Object and read the data
            ActivityDto activity = response.Content.ReadAsAsync<ActivityDto>().Result;

            // pass the data to the view
            return View(activity);
        }

        // renders the add activity page
        // GET Activity/Add
        public ActionResult Add()
        {
            // for this page we need to list of projects and collaborators in dropdowns
            // so we created a viewModal for AddUpdateActivity.
            // this view modal will have
            // -> selectedActivity (in case of update)
            // -> listOfProject
            // -> listOfCollaborators

            // create a new viewmodel for {collaboratos, and projects}
            AddUpdateActivity AddUpdateActivity = new AddUpdateActivity();

            // fetch all the list of collaborators
            //curl https://localhost:44346/api/CollaboratorData/GetAllCollaborators
            string url = "CollaboratorData/GetAllCollaborators";

            // get the response
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create empty collaborator list and read the data
            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Collaborators = collaborators;

            // fetch all the list of projects in the system
            // curl https://localhost:44346/api/ProjectData/GetAllProjects

            url = "ProjectData/GetAllProjects";

            // get the response
            response = client.GetAsync(url).Result;

            // create an empty project list and read the data as async
            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Projects = projects;

            // pass the view modal to the view
            return View(AddUpdateActivity);
        }

        // use this function to create a new activity in the database
        // GET: Activity/Create
        [HttpPost]
        public ActionResult Create(Activity activity)
        {
            // call the AddActivity api from the data controller
            // curl -H "Content-Type:application/json" -d @activity.json https://localhost:44346/api/ActivityData/AddActivity

            // generate the url string
            string url = "ActivityData/AddActivity";

            // serialize the json payload
            string jsonpayload = jss.Serialize(activity);

            // create a new string content with the serialized json
            HttpContent content = new StringContent(jsonpayload);

            // update the content type to application/json
            content.Headers.ContentType.MediaType = "application/json";

            // get the response from the database
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            // check for status code
            // if success -> go to list page
            // else -> go to the error page
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // renders the edit form
        // GET: Activity/Edit/5
        public ActionResult Edit(int id)
        {
            // this function again uses the view modal which we created for add activity
            // the only difference is it will also have the selected activity to populate the form

            // create a new viewmodel for {collaboratos, and projects}
            AddUpdateActivity AddUpdateActivity = new AddUpdateActivity();

            // fetch the activity details from the given id
            //curl https://localhost:44346/api/ActivityData/GetActivityDetails/{id}
            string url = "ActivityData/GetActivityDetails/" + id;

            // get the result
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create an empty ActivityDTO and store the data in it
            ActivityDto SelectedActivity = response.Content.ReadAsAsync<ActivityDto>().Result;

            // convert the dueDate 
            ViewData["DueDate"] = Convert.ToDateTime(SelectedActivity.ActivityDueDate).ToString("yyyy-MM-dd");

            // assign the selectedActivity to the view modal
            AddUpdateActivity.SelectedActivity = SelectedActivity;

            // fetch all the list of collaborators
            //curl https://localhost:44346/api/CollaboratorData/GetAllCollaborators
            url = "CollaboratorData/GetAllCollaborators";

            // get the resposne
            response = client.GetAsync(url).Result;

            // create an empty collaborator list and read the response data
            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Collaborators = collaborators;

            // fetch all the list of projects in the system
            // curl https://localhost:44346/api/ProjectData/GetAllProjects

            url = "ProjectData/GetAllProjects";
            response = client.GetAsync(url).Result;

            // create an empty project list
            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Projects = projects;

            // send the view modal to the view
            return View(AddUpdateActivity);
        }

        // us this function to update the record in the database
        // POST: Activity/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Activity activity)
        {
            try
            {
                // call the update activity api using the following api
                // curl -H "Content-Type:application/json" -d @activity.json https://localhost:44346/api/ActivityData/UpdateActivity/2
                string url = "ActivityData/UpdateActivity/" + id;

                // serialize the json payload
                string jsonpayload = jss.Serialize(activity);

                // assign serialized payload to string
                HttpContent content = new StringContent(jsonpayload);

                // update the content type to application/json
                content.Headers.ContentType.MediaType = "application/json";

                // get the result and assign the response.
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // once done redirect to the details page
                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // render the confirm activity delete page
        // GET: Activity/ConfirmDelete/5
        public ActionResult ConfirmDelete(int id)
        {
            // call the get activity details api to fill the data on the confirm delete page
            string url = "ActivityData/GetActivityDetails/" + id;

            // get the result
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create an activity dto object and read the data
            ActivityDto activity = response.Content.ReadAsAsync<ActivityDto>().Result;

            // send the data to view
            return View(activity);
        }

        // delete an activity from the database
        // POST: Activity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Activity activity)
        {
            try
            {
                // call the delete activity api
                // curl -d "" https://localhost:44346/api/ActivityData/DeleteActivity/2
                string url = "ActivityData/DeleteActivity/" + id;

                // as this is a delete request the string content will be an empty string
                HttpContent content = new StringContent("");

                // change the content type to application json
                content.Headers.ContentType.MediaType = "application/json";

                // read the data 
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // check the status code
                // if sucess -> redirect to list
                // else -> redirect to error
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
