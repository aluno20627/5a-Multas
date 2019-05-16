using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        public int ID { get; set; }

        [Display(Name = "Infração")]
        public string Infracao { get; set; }

        [Display(Name ="Local da Multa")]
        public string LocalDaMulta { get; set; }

        [Display(Name = "Valor da Multa")]
        public decimal ValorMulta { get; set; }

        [Display(Name = "Data da Multa")]
        public DateTime DataDaMulta { get; set; }

        //CRIAR CHAVES FORASTEIRAS

        //FK para o Agente
        [ForeignKey("Agente")]   //palavra reservada, tem de se por a biblioteca using....
        public int AgenteFK { get; set; }
        public virtual Agentes Agente { get; set; }

        //FK para o Condutor
        [ForeignKey("Condutor")]   //palavra reservada, tem de se por a biblioteca using....
        public int CondutorFK { get; set; }
        public virtual Condutores Condutor { get; set; }

        //FK para a Viatura
        [ForeignKey("Viatura")]   //palavra reservada, tem de se por a biblioteca using....
        public int ViaturaFK { get; set; }
        public virtual Viaturas Viatura { get; set; }



    }
}