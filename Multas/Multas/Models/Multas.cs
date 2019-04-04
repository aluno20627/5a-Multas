using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        public int ID { get; set; }

        public string Infracao { get; set; }

        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }

        public DateTime DataDaMulta { get; set; }

        //CRIAR CHAVES FORASTEIRAS

        //FK para o Agente
        [ForeignKey("Agente")]   //palavra reservada, tem de se por a biblioteca using....
        public int AgenteFK { get; set; }
        public Agentes Agente { get; set; }

        //FK para o Condutor
        [ForeignKey("Condutor")]   //palavra reservada, tem de se por a biblioteca using....
        public int CondutorFK { get; set; }
        public Condutores Condutor { get; set; }

        //FK para a Viatura
        [ForeignKey("Viatura")]   //palavra reservada, tem de se por a biblioteca using....
        public int ViaturaFK { get; set; }
        public Viaturas Viatura { get; set; }



    }
}