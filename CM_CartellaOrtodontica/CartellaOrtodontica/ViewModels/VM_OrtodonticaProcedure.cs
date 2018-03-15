using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_OrtodonticaProcedure
    {
        public int CodProcedura { get; set; }
        public string DescrizioneRaggruppamento { get; set; }
        public string DescrizioneProcedure { get; set; }
        public bool InUso { get; set; }
        public bool InizioNuovaTerapia { get; set; }
        public bool Diagnostico { get; set; }
        public bool Controllo { get; set; }
    }
}
