using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_Trattamenti
    {
        public int Id { get; set; }
        public DateTime? DataEsecuzione { get; set; }
        public DateTime DataInizioTerapia { get; set; }
        public int DurataInMesi { get; set; }
        public int MeseAttuale { get; set; }
        public int IdProcedura { get; set; }
        public int IdSottoProcedura { get; set; }
    }
}
