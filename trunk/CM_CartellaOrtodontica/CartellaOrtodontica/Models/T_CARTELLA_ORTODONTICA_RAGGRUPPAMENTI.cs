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
    
    public partial class T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI
    {
        public int CodRaggruppamento { get; set; }
        public bool In_Uso { get; set; }
    
        public virtual T_LISTINO_RAGGRUPPAMENTI T_LISTINO_RAGGRUPPAMENTI { get; set; }
    }
}
