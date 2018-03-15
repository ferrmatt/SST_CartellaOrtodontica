using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_SottoProcedureRefertazione
    {
        public int CodSottoProcedura { get; set; }
        public string DescrizioneProcedura { get; set; }
        public string DescrizioneSottoProcedura { get; set; }
        public int? Durata { get; set; }
        public int? NumeroAppuntamento { get; set; }
        public bool InUso { get; set; }
        public string Raggruppamento { get; set; }
    }
}
