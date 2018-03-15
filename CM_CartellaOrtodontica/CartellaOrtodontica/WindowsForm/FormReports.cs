using Microsoft.Reporting.WinForms;
using CartellaOrtodontica.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CartellaOrtodontica.WindowsForm
{
    public partial class FormReports : Form
    {
        private List<VM_ReportConsensoOrtodontico> consensoOrtodontico;
        private List<VM_OrtodonticaRefertazione> datiClinici;

        public FormReports(VM_ReportConsensoOrtodontico consensoOrtodontico, List<VM_OrtodonticaRefertazione> datiClinici)
        {
            this.consensoOrtodontico = new List<VM_ReportConsensoOrtodontico>();
            this.consensoOrtodontico.Add(consensoOrtodontico);
            this.datiClinici = datiClinici;
            InitializeComponent();
        }
        
        private void FormReport_Load(object sender, EventArgs e)
        {
            List<ReportDataSource> reportDataSource = new List<ReportDataSource>();

            reportDataSource.Add(new ReportDataSource("ConsensoOrtodontico", consensoOrtodontico.AsEnumerable()));
            reportDataSource.Add(new ReportDataSource("DatiClinici", datiClinici.AsEnumerable()));
            
            //carichiamo tutti i DataSource
            if (reportDataSource != null)
                foreach (ReportDataSource rds in reportDataSource)
                    reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.RefreshReport();
        }
    }
}
