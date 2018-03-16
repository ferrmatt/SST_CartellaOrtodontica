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
    
    public partial class T_PRESC_DETTAGLIO
    {
        public int Codice { get; set; }
        public string Numero { get; set; }
        public Nullable<int> TipoProtesi { get; set; }
        public Nullable<double> PrezzoSingolo { get; set; }
        public Nullable<int> Quantita { get; set; }
        public Nullable<int> Num_Trat { get; set; }
        public Nullable<int> CodPrescrizione { get; set; }
        public string ColoreDenti { get; set; }
        public Nullable<int> CodMedico { get; set; }
        public Nullable<int> NumCarrello { get; set; }
        public Nullable<int> CodiceCapacitaProduttiva { get; set; }
        public Nullable<int> CodOdontotecnico { get; set; }
        public Nullable<System.DateTime> DataConsegna { get; set; }
        public string InseritoDa { get; set; }
        public string UltimaModifica { get; set; }
        public byte[] LastEdit { get; set; }
        public Nullable<int> LastEditBy { get; set; }
        public Nullable<int> CodImportazione { get; set; }
        public bool ControlloQualita { get; set; }
        public Nullable<int> Progressione { get; set; }
    
        public virtual T_PRESCRIZIONE T_PRESCRIZIONE { get; set; }
    }
}