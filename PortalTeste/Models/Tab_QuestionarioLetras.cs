//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortalTeste.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tab_QuestionarioLetras
    {
        public int Id { get; set; }
        public int Questionario { get; set; }
        public string Correta { get; set; }
        public string Resposta { get; set; }
    
        public virtual Tab_Questionario Tab_Questionario { get; set; }
    }
}
