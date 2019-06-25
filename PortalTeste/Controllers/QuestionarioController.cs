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
    public class QuestionarioController : Controller
    {
        // GET: Questionario

        [Authorize(Roles = "Usuario,Administrador")]
        public ActionResult Index(int? pergunta)
        {
            
            Questionario q = new Questionario();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = "";
            var valores = new HashSet<char>() { };
            q.pergunta = pergunta ?? 0;
            //se fez as dez perguntas redireciona
            if (q.pergunta==10)
            {
                return RedirectToAction("Index", "Home");
            }
            //gera o index da resposta correta aleatoriamente
            q.correto = random.Next(3);

            //gera um hashset com as letras
            for (int i = 0; i < 3; i++)
            {
                bool y = true;
                while (y)
                {
                    result = new string(
              Enumerable.Repeat(chars, 1)
                        .Select(s => s[random.Next(s.Length)])
                        .ToArray());
                    y = valores.Contains(result[0]);
                }
                valores.Add(result[0]);
            }
            //transforma o hashset em lista
            q.Alfabeto = valores.ToList();
            q.UrlImgNormal = new List<string>();
            //cria o caminho das imagens
            foreach (char c in q.Alfabeto)
            {
                q.UrlImgNormal.Add("~/IMG/Normal/" + c.ToString().ToLower() + ".png");
            }

            return View(q);
        }


        [Authorize(Roles = "Usuario")]
        public ActionResult resposta(int correto, char Alfabeto0, char Alfabeto1, char Alfabeto2, string resposta, int pergunta)
        {

            //instancia o model
            Questionario q = new Questionario();
            q.UrlImgNormal = new List<string>();
            q.Alfabeto = new List<char>();
            q.Alfabeto.Add(Alfabeto0);
            q.Alfabeto.Add(Alfabeto1);
            q.Alfabeto.Add(Alfabeto2);
            q.correto = correto;
            q.pergunta = pergunta;
            //monta o caminho das imagens indicando a certa ou errada
            foreach (char c in q.Alfabeto)
            {
                if (c.ToString().Equals(q.Alfabeto[q.correto].ToString()))
                {
                    q.UrlImgNormal.Add("~/IMG/Acerto/" + c.ToString().ToLower() + ".png");
                }
                else
                {
                    q.UrlImgNormal.Add("~/IMG/Errada/" + c.ToString().ToLower() + ".png");
                }

            }

            //salva os acertos no banco
            using (PortalEntities2 db = new PortalEntities2())
            {
                if (pergunta == 1)
                {
                    Tab_Questionario questionario = new Tab_Questionario();

                    questionario.UsuarioId = (int)Session["Id"];
                    if (q.Alfabeto[correto].ToString().Equals(resposta))
                    {
                        questionario.Acertos = 1;
                    }
                    else
                    {
                        questionario.Acertos = 0;
                    }

                    db.Tab_Questionario.Add(questionario);
                    db.SaveChanges();

                    Tab_QuestionarioLetras letras = new Tab_QuestionarioLetras();
                    letras.Correta = q.Alfabeto[correto].ToString();
                    letras.Resposta = resposta;
                    letras.Questionario = questionario.Id;
                    db.Tab_QuestionarioLetras.Add(letras);
                    db.SaveChanges();
                }
                else
                {
                    Tab_Questionario questionario = db.Tab_Questionario.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (q.Alfabeto[correto].ToString().Equals(resposta))
                    {
                        
                        questionario.Acertos++;
                        db.SaveChanges();
                    }

                    Tab_QuestionarioLetras letras = new Tab_QuestionarioLetras();
                    letras.Correta = q.Alfabeto[correto].ToString();
                    letras.Resposta = resposta;
                    letras.Questionario = questionario.Id;
                    db.Tab_QuestionarioLetras.Add(letras);
                    db.SaveChanges();

                }
            }

            return View(q);
        }
    }
}