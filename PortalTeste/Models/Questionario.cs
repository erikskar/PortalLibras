using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalTeste.Models
{
    public partial class Questionario
    {
        public List<char> Alfabeto { get; set; }
        public int correto { get; set; }
        public int pergunta { get; set; }
        public List<String> UrlImgNormal { get; set; }
    }
}
