using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalTeste.Models;

namespace PortalTeste.Controllers
{
    public class TAB_UsuarioController : Controller
    {
        private PortalEntities2 db = new PortalEntities2();

        // GET: TAB_Usuario

        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View(db.TAB_Usuario.ToList());
        }

        // GET: TAB_Usuario/Details/5

        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAB_Usuario tAB_Usuario = db.TAB_Usuario.Find(id);
            if (tAB_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(tAB_Usuario);
        }

        // GET: TAB_Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TAB_Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Senha,Perfil,Turma")] TAB_Usuario tAB_Usuario)
        {
           
            if (ModelState.IsValid)
            {
                var vLogin = db.TAB_Usuario.Where(p => p.Nome.Equals(tAB_Usuario.Nome)).Count();
                if (vLogin > 0)
                {
                    ModelState.AddModelError("", "Este usuario já existe.");
                    return View(tAB_Usuario);
                }

                db.TAB_Usuario.Add(tAB_Usuario);
                db.SaveChanges();
                return RedirectToAction("Login","Account");
            }
            ModelState.AddModelError("", "Preencha todos os Campos.");
            return View(tAB_Usuario);
        }

        // GET: TAB_Usuario/Edit/5

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAB_Usuario tAB_Usuario = db.TAB_Usuario.Find(id);
            if (tAB_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(tAB_Usuario);
        }

        // POST: TAB_Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Senha,Perfil,Turma")] TAB_Usuario tAB_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAB_Usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tAB_Usuario);
        }

        // GET: TAB_Usuario/Delete/5

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAB_Usuario tAB_Usuario = db.TAB_Usuario.Find(id);
            if (tAB_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(tAB_Usuario);
        }

        // POST: TAB_Usuario/Delete/5

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAB_Usuario tAB_Usuario = db.TAB_Usuario.Find(id);
            db.TAB_Usuario.Remove(tAB_Usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
