using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartellaOrtodontica.ViewModels
{
    class VM_RicercaPaziente
    {
        public int CodPaziente { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string Cap { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public DateTime? DataNascita { get; set; }
        public string Sesso { get; set; }
        public int Eta { get; set; }
    }
}
