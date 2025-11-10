using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelaPrincipalAtualizado.Models
{
    public class Ferramenta
    {
        public int Id { get; set; } 
        public string Categoria { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string ImagemSource { get; set; } = string.Empty;
        public string PrecoDia { get; set; } = string.Empty;
        public string Disponibilidade { get; set; } = string.Empty;


        //Construtor adicional para facilitar a criação de objetos Ferramenta com Id e Categoria
        public Ferramenta(int id, string titulo, string categoria)
        {
            Id = id;
            Titulo = titulo;
            Categoria = categoria;
        }

    } 
}
