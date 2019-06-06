using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {

        // cria VAR que representa a BD
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes

        //a anotação seguinte obriga a pelo menos 1 dos roles
        [Authorize(Roles = "RecursosHumanos,Agente")] //só os agentes e as pessoas dos recursos humanos acedem a esta listagem

        //obrigatoriamente tem de ter os 2 roles
        //[Authorize(Roles ="RecursosHumanos")]
        //[Authorize(Roles ="Agente")]

        public ActionResult Index()
        {

            // procura a totalidade dos Agentes na BD

            // Instrução feita em LINQ
            // SELECT * FROM Agentes ORDER BY nome
            var listaAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();

            if (!User.IsInRole("RecursosHumanos"))
            {
                //o utilizador NÃO pertence as 'recursos humanos'

                //preciso de saber o ID do agente que está autenticado
                //como?

                //SELECT FROM Agentes WHERE UserNameId=(UserName da pessoa autenticada)
                int idAgente = db.Agentes   
                                    .Where(a=>a.UserNameId==User.Identity.Name)
                                    .FirstOrDefault()
                                    .ID
                                    ;
                return RedirectToAction("Details", new { id = idAgente });
            }
            return View(listaAgentes);
        }




        // GET: Agentes/Details/5
        /// <summary>
        /// Mostra os dados de um Agente 
        /// </summary>
        /// <param name="id">identifica o Agente</param>
        /// <returns>devolve a View com os dados</returns>
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                // vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                // este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }

            // SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            // o Agente foi encontrado?
            if (agente == null)
            {
                // o Agente não foi encontrado, porque o utilizador está 'à pesca'
                // return HttpNotFound();
                return RedirectToAction("Index");
            }

            ///o Agente foi encontrado.
            ///posso monstrar os seus dados a quem os solicitou?
            ///sim, se
            /// -o utilizador pertence ao role 'recursos humanos' ou
            /// -o utilizador pertence ao role 'gestão multas' ou
            /// -é o utilizador autenticado
            if (User.IsInRole("RecursosHumanos") ||
               User.IsInRole("GestorMultas") ||
               agente.UserNameId == User.Identity.Name)
            {
                // enviar para a View os dados do Agente que foi procurado e encontrado
                return View(agente);
            }
            else
            {
                //não pode ver os dados
                return RedirectToAction("index");
            }
        }




        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// criação de um novo agente
        /// </summary>
        /// <param name="agente">recolhe os dados do Nome e da Esquadra do Agente</param>
        /// <param name="fotografia">representa a fotografia que identifica o Agente</param>
        /// <returns>devolve uma View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RecursosHumanos,Agente")]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")]Agentes agente,
                                   HttpPostedFileBase fotografia)
        {
            /// precisamos de processar a fotografia
            /// 1º será q foi fornecido um ficheiro?
            /// 2º será do tipo correto?
            /// 3º se for do tipo correto, guarda-se
            ///    se não for, atribui-se um 'avatar genérico' ao utilizador

            // var auxiliar
            string caminho = "";
            bool haFoto = false;

            // há ficheiro?
            if (fotografia == null)
            {
                // não há ficheiro,
                // atribui-se-lhe o avatar
                agente.Fotografia = "noUser.png";
            }
            else
            {
                // há ficheiro
                // será correto?
                if (fotografia.ContentType == "image/jpeg" ||
                   fotografia.ContentType == "image/png")
                {
                    // estamos perante uma foto correta
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                    Guid g;
                    g = Guid.NewGuid();
                    // nome do ficheiro
                    string nome = g.ToString() + extensao;
                    // onde guardar o ficheiro
                    caminho = Path.Combine(Server.MapPath("~/imagens"), nome);
                    // atribuir ao agente o nome do ficheiro
                    agente.Fotografia = nome;
                    // assinalar q há foto
                    haFoto = true;
                }
            }


            if (ModelState.IsValid)
            {
                // valida se os dados fornecidos estão de acordo 
                // com as regras definidas na especificação do Modelo
                try
                {
                    // adiciona o novo Agente ao Modelo
                    db.Agentes.Add(agente);
                    // consolida os dados na BD
                    db.SaveChanges();
                    // vou guardar o ficheiro no disco rígido
                    if (haFoto) fotografia.SaveAs(caminho);
                    // redireciona o utilizador para a página do INDEX
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a escrita " +
                                             "dos dados do novo Agente");
                }
            }

            return View(agente);
        }




        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                // vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                // este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }

            // SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            // o Agente foi encontrado?
            if (agente == null)
            {
                // o Agente não foi encontrado, porque o utilizador está 'à pesca'
                // return HttpNotFound();
                return RedirectToAction("Index");
            }

            return View(agente);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }




        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                // vamos alterar esta resposta por defeito
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //
                // este erro ocorre porque o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }

            // SELECT * FROM Agentes WHERE Id=id
            Agentes agente = db.Agentes.Find(id);

            // o Agente foi encontrado?
            if (agente == null)
            {
                // o Agente não foi encontrado, porque o utilizador está 'à pesca'
                // return HttpNotFound();
                return RedirectToAction("Index");
            }

            // o Agente foi encontrado
            // vou salvaguardar os dados para posterior validação
            // - guardar o ID do Agente num cookie cifrado
            // - guardar o ID numa variável de sessão 
            //       (se se usar o ASP .NET Core, esta ferramenta já não existe...)
            // - outras alternativas válidas...

            Session["Agente"] = agente.ID;

            // mostra na View os dados do Agente
            return View(agente);
        }




        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {

            if (id == null)
            {
                // há um 'xico experto' a tentar dar-me a volta ao código
                return RedirectToAction("Index");
            }

            // o ID não é null
            // será o ID o que eu espero?
            // vamos validar se o ID está correto
            if (id != (int)Session["Agente"])
            {
                // há aqui outro 'xico experto'...
                return RedirectToAction("Index");
            }



            // procura o Agente a remover
            Agentes agente = db.Agentes.Find(id);

            if (agente == null)
            {
                // não foi encontrado o Agente
                return RedirectToAction("Index");
            }


            try
            {
                // dá ordem de remoção do Agente
                db.Agentes.Remove(agente);

                // consolida a remoção
                db.SaveChanges();
            }
            catch (Exception)
            {
                // devem aqui ser escritas todas as instruções que são consideradas necessárias

                // informar q houve um erro
                ModelState.AddModelError("", "Não é possível remover o Agente " + agente.Nome + ". " +
                "Provavelmente, tem multas associadas a ele...");

                // redirecionar para a página onde o erro foi gerado
                return View(agente);
            }


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
