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
    
    public partial class T_ANAGRAFICA_PAZIENTI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_ANAGRAFICA_PAZIENTI()
        {
            this.T_ANAGRAFICA_PAZIENTI1 = new HashSet<T_ANAGRAFICA_PAZIENTI>();
            this.T_AGENDA_APPUNTAMENTI = new HashSet<T_AGENDA_APPUNTAMENTI>();
        }
    
        public int CodPaziente { get; set; }
        public Nullable<int> CodTitolo { get; set; }
        public string Titolo { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Sesso { get; set; }
        public string Professione { get; set; }
        public string Nota { get; set; }
        public string Salute { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string CodNazione { get; set; }
        public string TelCasa { get; set; }
        public string TelUff { get; set; }
        public string TelCell { get; set; }
        public string EMail { get; set; }
        public Nullable<System.DateTime> Natoil { get; set; }
        public string CAPNasc { get; set; }
        public string CittaNasc { get; set; }
        public string ProvinciaNasc { get; set; }
        public string CodNazioneNasc { get; set; }
        public string CodFisc { get; set; }
        public string PartitaIVA { get; set; }
        public string Swift { get; set; }
        public string IBAN { get; set; }
        public Nullable<int> NListino { get; set; }
        public Nullable<decimal> Affidabilita { get; set; }
        public Nullable<decimal> CodInviato { get; set; }
        public string InviatoDa { get; set; }
        public Nullable<decimal> CodPazfatt { get; set; }
        public byte[] Foto { get; set; }
        public string Orari { get; set; }
        public Nullable<double> Resto { get; set; }
        public Nullable<int> PosizioneTicket { get; set; }
        public string MedCurante { get; set; }
        public string Dentista { get; set; }
        public Nullable<int> Studio { get; set; }
        public string Osservazioni { get; set; }
        public string MisBande { get; set; }
        public string Terapia { get; set; }
        public string ERP_NUM { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserName1 { get; set; }
        public string Password1 { get; set; }
        public string Permessi { get; set; }
        public Nullable<bool> Infetto { get; set; }
        public Nullable<bool> Allergico { get; set; }
        public string NoTesseraSanitaria { get; set; }
        public Nullable<int> CodiceConvenzione { get; set; }
        public bool Convenzione { get; set; }
        public string NomeCartella { get; set; }
        public string InseritoDa { get; set; }
        public string UltimaModifica { get; set; }
        public Nullable<System.DateTime> DataIns { get; set; }
        public Nullable<System.DateTime> DataMod { get; set; }
        public byte[] LastEdit { get; set; }
        public Nullable<decimal> LastEditBy { get; set; }
        public Nullable<int> CodImportazione { get; set; }
        public string FatturareANome { get; set; }
        public string FatturareACognome { get; set; }
        public string FatturareAIndirizzo { get; set; }
        public string FatturareACAP { get; set; }
        public string FatturareACitta { get; set; }
        public string FatturareAProvincia { get; set; }
        public string FatturareACodNazione { get; set; }
        public string FatturareAPartitaIVA { get; set; }
        public Nullable<int> CodMedicoRiferimento { get; set; }
        public byte OrigineCliente { get; set; }
        public Nullable<int> OrigineCodPaziente { get; set; }
        public Nullable<int> OrigineCodMedico { get; set; }
        public string OrigineMedicoEsterno { get; set; }
        public string Note { get; set; }
        public string CodiceEsenzione { get; set; }
        public bool IscrittoAllaNewsletter { get; set; }
        public Nullable<int> CodCapoFamiglia { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_ANAGRAFICA_PAZIENTI> T_ANAGRAFICA_PAZIENTI1 { get; set; }
        public virtual T_ANAGRAFICA_PAZIENTI T_ANAGRAFICA_PAZIENTI2 { get; set; }
        public virtual T_ANAGRAFICA_PAZIENTI T_ANAGRAFICA_PAZIENTI11 { get; set; }
        public virtual T_ANAGRAFICA_PAZIENTI T_ANAGRAFICA_PAZIENTI3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_AGENDA_APPUNTAMENTI> T_AGENDA_APPUNTAMENTI { get; set; }
        public virtual T_STUDI T_STUDI { get; set; }
    }
}
