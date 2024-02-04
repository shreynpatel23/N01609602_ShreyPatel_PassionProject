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
    public class CollaboratorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CollaboratorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/CollaboratorData/");
        }

        // GET  Error
        public ActionResult Error()
        {

            return View();
        }

        // GET: Collaborator/List
        public ActionResult List()
        {

            string url = "GetAllCollaborators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Collaborator> collaborators = response.Content.ReadAsAsync<IEnumerable<Collaborator>>().Result;

            return View(collaborators);
        }

        // GET Collaborator/Details/2
        public ActionResult Details(int id)
        {
            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44346/api/GetCollaboratorsDetails/{id}

            string url = "GetCollaboratorsDetails/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Collaborator collaborator = response.Content.ReadAsAsync<Collaborator>().Result;

            return View(collaborator);
        }
    }
}