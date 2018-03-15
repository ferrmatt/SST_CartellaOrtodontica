using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    public class VM_ReportConsensoOrtodontico
    {
        public string NomeStudio { get; set; }
        public string DataDocumento { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string LuogoNascita { get; set; }
        public string DataNascita { get; set; }
        
        public string IOTN { get; set; }
        public string DataEsecuzione { get; set; }
        public string NomeProcedura { get; set; }
        public string ElencoSottoProcedura { get; set; }
        public string Diagnosi { get; set; }
        public string DurataTerapia { get; set; }
        public string PropostaTrattamento { get; set; }
        public string DurataMesi { get; set; }
        
    }
}
