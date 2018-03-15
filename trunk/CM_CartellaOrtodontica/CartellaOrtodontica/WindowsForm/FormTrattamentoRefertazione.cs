using CartellaOrtodontica.Models;
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
    public partial class FormTrattamentoRefertazione : Form
    {
        private CM_INVOICEEntities1 ctx = null;
        private string CodDatoPrev;
        private bool edit;
        public FormTrattamentoRefertazione(string prev, bool edit)
        {
            InitializeComponent();
            ctx = new CM_INVOICEEntities1();
            CodDatoPrev = prev;
            this.edit = edit;
        }

        private void FormTrattamentoRefertazione_Load(object sender, EventArgs e)
        {

        }

        private void btn_paziente_daticlinici_save_Click(object sender, EventArgs e)
        {
            try
            {
                int ntrattamento = Convert.ToInt32(editBox_NumeroTrattamento.Text);
                string codDato = uiComboBox_DescrizioneDato.SelectedItem.Value.ToString();
                
                string valore = editBox_Valore.Text;
                string osservazioni = editBox_Osservazioni.Text;
                Models.T_CARTELLA_ORTODONTICA_REFERTAZIONE re = null;
                Models.T_CARTELLA_ORTODONTICA_DATI_CLINICI dc = ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Single(d => d.CodDato == codDato);
                if (!edit)
                {
                    //nuovo record
                    var ris = ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.SingleOrDefault(r => r.NTrattamento == ntrattamento && r.CodDato == codDato);
                    if (ris != null)
                    {
                        MessageBox.Show("Esiste già un dato clinico per questo trattamento. Modificare il dato clinico esistente.", "Errore generico:", MessageBoxButtons.OK);
                        this.Close();
                        return;
                    }
                }
                if (edit)
                {
                    //escludo se stesso
                    if (codDato != CodDatoPrev)
                    {
                        var ris = ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.SingleOrDefault(r => r.NTrattamento == ntrattamento && r.CodDato == codDato);
                        if (ris != null)
                        {
                            MessageBox.Show("Esiste già un dato clinico per questo trattamento. Modificare il dato clinico esistente.", "Errore generico:", MessageBoxButtons.OK);
                            this.Close();
                            return;
                        }
                    }
                    re = ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.SingleOrDefault(r => r.NTrattamento == ntrattamento && r.CodDato == CodDatoPrev);
                    ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Remove(re);
                    ctx.SaveChanges();
                }
                
                re = new T_CARTELLA_ORTODONTICA_REFERTAZIONE();
                re.NTrattamento = ntrattamento;
                re.CodDato = codDato;
                
                re.Valore = valore;
                re.Osservazioni = osservazioni;
                //re.T_CARTELLA_ORTODONTICA_DATI_CLINICI = dc;
                ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Add(re);
                ctx.SaveChanges();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_paziente_daticlinici_annulla_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
