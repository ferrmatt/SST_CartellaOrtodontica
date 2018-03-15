using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_Procedure
    {
        public int CodProcedura { get; set; }
        public string DescrizioneRaggruppamento { get; set; }
        public string DescrizioneProcedure { get; set; }
        public int? Durata { get; set; }
        public double? Prezzo { get; set; }
    }
}
