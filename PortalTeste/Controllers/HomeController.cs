using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalTeste.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {


        // GET: Home
        [Authorize(Roles = "Usuario, Administrador")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Jogos");
        }
    }
}