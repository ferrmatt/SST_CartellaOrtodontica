//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CartellaOrtodontica.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_CARTELLA_ORTODONTICA_PROCEDURE
    {
        public int CodProcedura { get; set; }
        public bool In_Uso { get; set; }
        public bool InizioNuovaTerapia { get; set; }
        public bool Diagnostico { get; set; }
        public bool Controllo { get; set; }
    
        public virtual T_LISTINO_PROCEDURE T_LISTINO_PROCEDURE { get; set; }
    }
}
