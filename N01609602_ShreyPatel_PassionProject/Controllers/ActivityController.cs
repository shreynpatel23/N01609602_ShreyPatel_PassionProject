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
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ActivityController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET  Error
        public ActionResult Error()
        {

            return View();
        }

        // GET: Activity/List
        public ActionResult List()
        {
            string url = "ActivityData/GetAllActivities";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ActivityDto> activities = response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;

            return View(activities);
        }

        // GET: Activity/Details/{id}
        public ActionResult Details(int id)
        {

            string url = "ActivityData/GetActivityDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ActivityDto activity = response.Content.ReadAsAsync<ActivityDto>().Result;

            return View(activity);
        }

        // GET Activity/Add
        public ActionResult Add()
        {
            // create a new viewmodel for {collaboratos, and projects}

            AddUpdateActivity AddUpdateActivity = new AddUpdateActivity();

            // fetch all the list of collaborators
            //curl https://localhost:44346/api/CollaboratorData/GetAllCollaborators
            string url = "CollaboratorData/GetAllCollaborators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Collaborators = collaborators;

            // fetch all the list of projects in the system
            // curl https://localhost:44346/api/ProjectData/GetAllProjects

            url = "ProjectData/GetAllProjects";
            response = client.GetAsync(url).Result;

            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Projects = projects;

            return View(AddUpdateActivity);
        }

        // GET: Activity/Create
        [HttpPost]
        public ActionResult Create(Activity activity)
        {
            string url = "ActivityData/AddActivity";


            string jsonpayload = jss.Serialize(activity);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Activity/Edit/5
        public ActionResult Edit(int id)
        {
            // create a new viewmodel for {collaboratos, and projects}
            AddUpdateActivity AddUpdateActivity = new AddUpdateActivity();

            // fetch the activity details from the given id
            //curl https://localhost:44346/api/ActivityData/GetActivityDetails/{id}
            string url = "ActivityData/GetActivityDetails/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ActivityDto SelectedActivity = response.Content.ReadAsAsync<ActivityDto>().Result;

            ViewData["DueDate"] = Convert.ToDateTime(SelectedActivity.ActivityDueDate).ToString("yyyy-MM-dd");

            AddUpdateActivity.SelectedActivity = SelectedActivity;

            Debug.WriteLine(AddUpdateActivity.SelectedActivity.ActivityName);

            // fetch all the list of collaborators
            //curl https://localhost:44346/api/CollaboratorData/GetAllCollaborators
            url = "CollaboratorData/GetAllCollaborators";
            response = client.GetAsync(url).Result;

            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Collaborators = collaborators;

            // fetch all the list of projects in the system
            // curl https://localhost:44346/api/ProjectData/GetAllProjects

            url = "ProjectData/GetAllProjects";
            response = client.GetAsync(url).Result;

            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;

            // assign them to the viewmodal
            AddUpdateActivity.Projects = projects;

            return View(AddUpdateActivity);
        }

        // POST: Activity/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Activity activity)
        {
            try
            {
                // curl -H "Content-Type:application/json" -d @project.json https://localhost:44346/api/ActivityData/UpdateActivity/2
                string url = "ActivityData/UpdateActivity/" + id;


                string jsonpayload = jss.Serialize(activity);
                Debug.WriteLine(jsonpayload);

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

        // GET: Activity/ConfirmDelete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "ActivityData/GetActivityDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ActivityDto activity = response.Content.ReadAsAsync<ActivityDto>().Result;

            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Activity activity)
        {
            try
            {
                // curl -d "" https://localhost:44346/api/ActivityData/DeleteActivity/2
                string url = "ActivityData/DeleteActivity/" + id;

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
