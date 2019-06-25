using PortalTeste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PortalTeste.Controllers
{
    public class AccountController : Controller
    {
        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult Login(string returnURL)
        {
            /*Recebe a url que o usuário tentou acessar*/
            ViewBag.ReturnUrl = returnURL;
            Session.Clear();
            return View(new TAB_Usuario());
        }

        /// <param name = "login" ></ param >
        /// < param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TAB_Usuario login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (PortalEntities2 db = new PortalEntities2())
                {
                    var vLogin = db.TAB_Usuario.Where(p => p.Nome.Equals(login.Nome)).FirstOrDefault();
                    /*Verificar se a variavel vLogin está vazia. 
                    Isso pode ocorrer caso o usuário não existe. 
              Caso não exista ele vai cair na condição else.*/
                    if (vLogin != null)
                    {
                        /*Código abaixo verifica se a senha digitada no site é igual a 
                        senha que está sendo retornada 
                         do banco. Caso não cai direto no else*/
                        if (Equals(vLogin.Senha, login.Senha))
                        {
                            FormsAuthentication.SetAuthCookie(vLogin.Nome, false);
                            if (Url.IsLocalUrl(returnUrl)
                            && returnUrl.Length > 1
                            && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//")
                            && returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            /*código abaixo cria uma session para armazenar o nome do usuário*/
                            Session["Nome"] = vLogin.Nome;
                            Session["Id"] = vLogin.Id;
                            /*retorna para a tela inicial do Home*/
                            var questionario = db.Tab_Questionario.Where(x => x.UsuarioId == vLogin.Id).Count();
                            if (questionario>0)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Questionario");
                            }
                            
                        }
                        /*Else responsável da validação da senha*/
                        else
                        {
                            /*Escreve na tela a mensagem de erro informada*/
                            ModelState.AddModelError("", "Senha informada Inválida.");
                            /*Retorna a tela de login*/
                            return View(new TAB_Usuario());
                        }

                    }
                    /*Else responsável por verificar se o usuário existe*/
                    else
                    {
                        /*Escreve na tela a mensagem de erro informada*/
                        ModelState.AddModelError("", "Nome informado inválido.");
                        /*Retorna a tela de login*/
                        return View(new TAB_Usuario());
                    }
                }
            }
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login 
            com as mensagem dos campos*/
            return View(login);
        }

        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult Criar(string returnURL)
        {
            return RedirectToAction("Create","TAB_Usuario");
        }
    }
}