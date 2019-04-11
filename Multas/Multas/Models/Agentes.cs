using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {

        public int ID { get; set; }

        [Required(ErrorMessage ="Escreva o Nome do Agente")]
        [RegularExpression("([A-ZÁÉÍÓÚÀÈÌÒÙa-záéíóúàèìòùäëïöüâêîôûãõçñ]+( |-|')?)+", 
            ErrorMessage ="Só pode escrever letras no nome. Deve começar por uma maiúscula")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="Indique a Esquadra onde o Agente trabalha")]
        //[RegularExpression("Tomar|Ourém|Torres Novas|Lisboa|Leiria")]
        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        //Identifica as multas passadas pelo Agente
        public ICollection<Multas> ListaMultas { get; set; }

    }
}