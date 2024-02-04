using N01609602_ShreyPatel_PassionProject.Models;
using System;
using System.Collections.Generic;
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
    }
}