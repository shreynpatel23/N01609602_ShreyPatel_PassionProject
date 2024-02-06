using N01609602_ShreyPatel_PassionProject.Models;
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
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProjectController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/ProjectData/");
        }

        // GET: Project/List
        public ActionResult List()
        {
            // url to communicate with projects database
            //curl https://localhost:44346/api/ProjectData/GetAllProjects


            string url = "GetAllProjects";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Project> projects = response.Content.ReadAsAsync<IEnumerable<Project>>().Result;
            

            return View(projects);
        }

        // GET Project/Details/2
        public ActionResult Details(int id)
        {
            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44346/api/GetProjectDetails/{id}

            string url = "GetProjectDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Project project= response.Content.ReadAsAsync<Project>().Result;

            return View(project);
        }

        // GET Project/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            // curl -H "Content-Type:application/json" -d @project.json https://localhost:44346/api/ProjectData/AddProject
            string url = "AddProject";


            string jsonpayload = jss.Serialize(project);


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

        // GET Project/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "GetProjectDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Project project = response.Content.ReadAsAsync<Project>().Result;

            ViewData["DueDate"] = Convert.ToDateTime(project.DueDate).ToString("yyyy-MM-dd");

            return View(project);
        }

        // POST Project/Update/2
        [HttpPost]
        public ActionResult Update(int id, Project project)
        {
            try
            {
                // curl -H "Content-Type:application/json" -d @project.json https://localhost:44346/api/ProjectData/UpdateProject/2
                string url = "UpdateProject/" + id;


                string jsonpayload = jss.Serialize(project);
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

        // GET: Project/ConfirmDelete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "GetProjectDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Project project = response.Content.ReadAsAsync<Project>().Result;
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // curl -d "" https://localhost:44346/api/ProjectData/DeleteProject/2
                string url = "DeleteProject/" + id;

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