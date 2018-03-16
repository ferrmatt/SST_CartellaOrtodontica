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
    
    public partial class T_AGENDA_APPUNTAMENTI
    {
        public int CodAppuntamento { get; set; }
        public int CodPaziente { get; set; }
        public int CodMedico { get; set; }
        public Nullable<int> CodEquipe { get; set; }
        public int CodPoltrona { get; set; }
        public Nullable<int> NTrattamento { get; set; }
        public System.DateTime DataAppuntamento { get; set; }
        public System.DateTime DalleOre { get; set; }
        public System.DateTime AlleOre { get; set; }
        public string Note { get; set; }
        public Nullable<int> Stato { get; set; }
        public Nullable<int> TipoApp { get; set; }
        public System.DateTime DataIns { get; set; }
        public string InseritoDa { get; set; }
        public string UltimaModifica { get; set; }
        public Nullable<bool> SMS { get; set; }
        public Nullable<int> Spostato { get; set; }
        public Nullable<decimal> Completato { get; set; }
        public Nullable<int> CodImpegno { get; set; }
        public Nullable<System.DateTime> DataUltimaModifica { get; set; }
        public byte[] LastEdit { get; set; }
        public Nullable<decimal> LastEditBy { get; set; }
        public string CodAppuntamento_Master { get; set; }
        public Nullable<int> CodImportazione { get; set; }
        public string ERP_NUM_CODAPPUNTAMENTO { get; set; }
        public string CodPrenotazione { get; set; }
        public Nullable<int> CodModuloPagamento { get; set; }
    
        public virtual T_ANAGRAFICA_PAZIENTI T_ANAGRAFICA_PAZIENTI { get; set; }
    }
}