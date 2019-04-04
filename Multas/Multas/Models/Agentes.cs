using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {

        public int ID { get; set; }

        public string Nome { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        //Identifica as multas passadas pelo Agente
        public ICollection<Multas> ListaMultas { get; set; }

    }
}