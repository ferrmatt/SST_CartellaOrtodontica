using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_NoteCliniche
    {
        public int NTrattamento { get; set; }
        public int NumPianoDiCura { get; set; }
        public string Raggruppamento { get; set; }
        public string Procedura { get; set; }
        public string SottoProcedura { get; set; }
        public int? Dente_1 { get; set; }
        public int? Dente_2 { get; set; }
        public string OsservazioniGenerali { get; set; }
        public string DaEseguire { get; set; }
        public string Eseguito { get; set; }
        public DateTime? DataEsecuzione { get; set; }
        public string DataAppuntamento { get; set; }
        public string Prescrizione { get; set; }
        public string Laboratorio { get; set; }
        public List<VM_OrtodonticaRefertazione> DatiClinici { get; set; }
    }
}
