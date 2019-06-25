using PortalTeste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalTeste.Controllers
{
    public class EstatisticasController : Controller
    {
        // GET: Estatisticas
        public ActionResult Index()
        {
            return View();
        }
        // GET: Estatisticas
        public ActionResult Desempenho()
        {
           
            return View();
        }
       
        public JsonResult Data()
        {
            List<Tab_Questionario> t = new List<Tab_Questionario>();
            using (PortalEntities2 db = new PortalEntities2())
            {

                int id = (int)Session["Id"];
                t = db.Tab_Questionario.Where(x => x.UsuarioId == id).ToList();
            }
            List<object> obj = new List<object>();

            var count = 1;
            obj.Add(new
            {
                Questionario = "0",
                Acertos = 0,
                Erros = 0
            });
            foreach (var item in t)
            {
                obj.Add(new
                {
                    Questionario = count.ToString(),
                    Acertos = item.Acertos,
                    Erros = 10 - item.Acertos
                });
                count++;
            }


            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LetrasMaisErradas()
        {
            List<LetrasMaisErradas> erradas = new List<LetrasMaisErradas>();
            LetrasMaisErradas l = new Models.LetrasMaisErradas();
            using (PortalEntities2 db = new PortalEntities2())
            {
                int id = (int)Session["Id"];
                var x = (from ql in db.Tab_QuestionarioLetras
                         join q in db.Tab_Questionario on ql.Questionario equals q.Id
                         where q.UsuarioId == id
                         && ql.Correta != ql.Resposta
                         select new {
                         correta = ql.Correta}).Distinct().ToList();

                
                

                foreach (var item in x)
                {
                    LetrasMaisErradas letra = new LetrasMaisErradas();
                    letra.Alfabeto = item.correta;
                    letra.UrlImgNormal = "~/IMG/Normal/" + item.correta.ToLower() + ".png";
                    letra.quantidade = (from ql in db.Tab_QuestionarioLetras
                                        join q in db.Tab_Questionario on ql.Questionario equals q.Id
                                        where q.UsuarioId == id
                                        && ql.Correta != ql.Resposta
                                        && ql.Correta== item.correta
                                        select new
                                        {
                                            ql
                                        }).Count();

                    erradas.Add(letra); 
                }
                erradas = erradas.OrderByDescending(y => y.quantidade).Take(5).ToList();
            }
                return View(erradas);
        }

    }
}