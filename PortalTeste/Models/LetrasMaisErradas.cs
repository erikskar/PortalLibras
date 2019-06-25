using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalTeste.Models
{
    public partial class LetrasMaisErradas
    {
        public string Alfabeto { get; set; }
        public int quantidade { get; set; }
        
        public String UrlImgNormal { get; set; }
    }
}
