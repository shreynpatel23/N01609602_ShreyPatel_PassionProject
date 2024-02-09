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

namespace N01609602_ShreyPatel_PassionProject.Controllers
{
    public class ProjectController : Controller
    {
        // declare HTTP CLient here
        private static readonly HttpClient client;

        // declare JS serializer to serialize json data
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // configure the http client and base url
        static ProjectController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // this method will list the projects on the screen
        // GET: Project/List
        public ActionResult List()
        {
            // url to communicate with projects database use the following api
            //curl https://localhost:44346/api/ProjectData/GetAllProjects

            // assign the url to a variable
            string url = "ProjectData/GetAllProjects";
            // get data from the httpclient and convert it to response.
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create empty project list and read the data from the response
            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;
            
            // pass the list of projects to the view
            return View(projects);
        }

        // this method will show the project details
        // this method will also utilize the activity controller to fetch activities for 
        // a particular project id.
        // GET Project/Details/2
        public ActionResult Details(int id)
        {
            // as we need data from two modals (project and tasks) we are going to use ViewModal
            ProjectActivity projectActivity = new ProjectActivity();

            // get the project details by calling the getProjectDetails api
            //curl https://localhost:44346/api/GetProjectDetails/{id}

            string url = "ProjectData/GetProjectDetails/" + id;

            // capture the response
            HttpResponseMessage response = client.GetAsync(url).Result;

            // convert the response into the project modal
            Project project = response.Content.ReadAsAsync<Project>().Result;

            // assign the selected project to the viewmodal
            projectActivity.selectedProject = project;

            // get the list of activities for a particular project by calling the api
            // curl http://localhost:44346/api/ActivityData/GetActivitiesForProject/{id}

            url = "ActivityData/GetActivitiesForProject/" + id;

            // capture the response
            response = client.GetAsync(url).Result;

            // convert the response into the project modal
            IEnumerable<ActivityDto> activities = response.Content.ReadAsAsync<IEnumerable<ActivityDto>>().Result;

            // assign the activities to the viewmodal
            projectActivity.activities = activities;

            return View(projectActivity);
        }

        // This method is used to render the add project screen
        // GET Project/Add
        public ActionResult Add()
        {
            return View();
        }

        // use this function to call the add project api to store the data in the database
        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            // to communicate with the database call the addPRoject method with below curl request
            // curl -H "Content-Type:application/json" -d @project.json https://localhost:44346/api/ProjectData/AddProject

            // assign to a variable
            string url = "ProjectData/AddProject";

            // serialize the json payload
            string jsonpayload = jss.Serialize(project);

            // add content in the request header
            HttpContent content = new StringContent(jsonpayload);

            // update content type
            content.Headers.ContentType.MediaType = "application/json";

            // get data from database thorugh response
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            

            // check status code
            // if ok then navigate to list
            // else navigate to error
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // use this function to render the edit project form
        // GET Project/Edit/5
        public ActionResult Edit(int id)
        {
            // to autofill the form we need to call the getProject details api
            
            // get the project details by calling the getProjectDetails api
            //curl https://localhost:44346/api/GetProjectDetails/{id}
            string url = "ProjectData/GetProjectDetails/" + id;

            // get the data from database
            HttpResponseMessage response = client.GetAsync(url).Result;

            // create a project and read the data as async
            Project project = response.Content.ReadAsAsync<Project>().Result;

            // convert the date recieved in response to DateTime.
            ViewData["DueDate"] = Convert.ToDateTime(project.DueDate).ToString("yyyy-MM-dd");

            // pass the project to the view
            return View(project);
        }


        // use this function to update the project and store it in database
        // POST Project/Update/2
        [HttpPost]
        public ActionResult Update(int id, Project project)
        {
            try
            {
                // you can call the updateProject api with the following curl command.
                // curl -H "Content-Type:application/json" -d @project.json https://localhost:44346/api/ProjectData/UpdateProject/2

                // assign the api endpoint to a string
                string url = "ProjectData/UpdateProject/" + id;

                // serialize the json payload
                string jsonpayload = jss.Serialize(project);

                // assign content to the request headers
                HttpContent content = new StringContent(jsonpayload);

                // update the content type to application/json
                content.Headers.ContentType.MediaType = "application/json";

                // store the data and get the response 
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // on success redirect to details page.
                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }


        // use this function to render the confirm delete page for a project with id
        // GET: Project/ConfirmDelete/5
        public ActionResult ConfirmDelete(int id)
        {
            // fetch the project details to show it on the confirm page
            // you can also fetch it by running the curl command
            // curl -d "" https://localhost:44346/api/ProjectData/GetProjectDetails/2
            string url = "ProjectData/GetProjectDetails/" + id;

            // get the details in the response
            HttpResponseMessage response = client.GetAsync(url).Result;

            // read as async and store in the a project object
            Project project = response.Content.ReadAsAsync<Project>().Result;

            // pass the object to the view
            return View(project);
        }


        // use this function to delete a record from the database
        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // you can also call the api by the following command
                // curl -d "" https://localhost:44346/api/ProjectData/DeleteProject/2
                string url = "ProjectData/DeleteProject/" + id;

                // as this is a post request we will assign empty string as content
                HttpContent content = new StringContent("");

                // change the content type to application json
                content.Headers.ContentType.MediaType = "application/json";

                // get the response
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // check if the status code is success
                // if yes then redirect to the list pageS
                // else redirect to the error page.
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