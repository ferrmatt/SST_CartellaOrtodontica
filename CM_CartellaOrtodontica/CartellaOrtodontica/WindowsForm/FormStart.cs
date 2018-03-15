using Janus.Windows.GridEX;
using CartellaOrtodontica.Models;
using CartellaOrtodontica.ViewModels;
using CartellaOrtodontica.WindowsForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using Janus.Windows.TimeLine;

namespace CartellaOrtodontica
{
    public partial class FormStart : Form
    {
        private CM_INVOICEEntities1 ctx = null;
        private int codPaziente, durataTerapia;
        private string cognomePaziente, nomePaziente;
        private List<VM_OrtodonticaRaggruppamento> CartellaOrtodonticaRaggruppamentiList;
        private List<VM_Procedure> ProcedureList;
        private List<VM_OrtodonticaProcedure> ProcedureOrtodonticaList;
        private List<VM_SottoProcedureRefertazione> SottoProcedureRefertazioneList;
        public FormStart(string[] args)
        {
            InitializeComponent();
            DisableTab();
            CartellaOrtodonticaRaggruppamentiList = new List<VM_OrtodonticaRaggruppamento>();
            ProcedureList = new List<VM_Procedure>();
            ProcedureOrtodonticaList = new List<VM_OrtodonticaProcedure>();
            SottoProcedureRefertazioneList = new List<VM_SottoProcedureRefertazione>();
            codPaziente = -1;
            durataTerapia = -1;
            if (args != null && args.Count() > 0)
            {
                try
                {
                    codPaziente = Convert.ToInt32(args[0]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Impossibile leggere il parametro Codice Paziente. Controllare il valore del parametro e riprovare.", "Errore generico:", MessageBoxButtons.OK);
                }
            }
        }

        private void DisableTab()
        {
            this.uiTabPage_Raggruppamenti.Enabled = false;
            this.uiTabPage_Procedure.Enabled = false;
            this.uiTabPage_SottoProcedure.Enabled = false;
            this.uiTabPage_DatiClinici.Enabled = false;
        }

        #region Load
        public void LoadRaggruppamenti()
        {
            try
            {
                var list = (from r in ctx.T_LISTINO_RAGGRUPPAMENTI.Where(r => r.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI == null && r.InUso == true)
                            select new VM_Raggruppamenti
                            {
                                CodRaggruppamento = r.CodRaggruppamento,
                                Descrizione = r.Descrizione
                            }).ToList();
                gridEX_ListaRaggruppamenti.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void LoadOrtodonticaRaggruppamenti()
        {
            try
            {
                CartellaOrtodonticaRaggruppamentiList = (from x in ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI
                                                         select new VM_OrtodonticaRaggruppamento
                                                         {
                                                             CodRagg = x.CodRaggruppamento,
                                                             Descrizione = x.T_LISTINO_RAGGRUPPAMENTI.Descrizione,
                                                             InUso = x.In_Uso
                                                         }).ToList();

                gridEX_OrtodonticaRaggruppamenti.DataSource = CartellaOrtodonticaRaggruppamentiList;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void LoadProcedure()
        {
            try
            {
                List<int> idRaggruppamento = ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI.Where(r =>r.In_Uso).Select(r => r.CodRaggruppamento).ToList();
                ProcedureList = (from x in ctx.T_LISTINO_PROCEDURE.Where(l => l.T_CARTELLA_ORTODONTICA_PROCEDURE == null && (l.In_uso??true) && idRaggruppamento.Contains(l.CodRaggruppamento.Value))
                                 select new VM_Procedure
                                 {
                                     CodProcedura = x.CodProcedura,
                                     DescrizioneRaggruppamento = ctx.T_LISTINO_RAGGRUPPAMENTI.FirstOrDefault(l => l.CodRaggruppamento == x.CodRaggruppamento).Descrizione,
                                     DescrizioneProcedure = x.Descrizione,
                                     Durata = x.Durata,
                                     Prezzo = x.Prezzo
                                 }).ToList();
                gridEX_Procedure.DataSource = ProcedureList;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void LoadProcedureOrtodontiche()
        {
            try
            {
                List<int> idProcedure = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Select(r => r.CodProcedura).ToList();
                ProcedureOrtodonticaList = (from x in ctx.T_LISTINO_PROCEDURE.Where(l => idProcedure.Contains(l.CodProcedura))
                                            select new VM_OrtodonticaProcedure
                                            {
                                                CodProcedura = x.CodProcedura,
                                                DescrizioneRaggruppamento = ctx.T_LISTINO_RAGGRUPPAMENTI.FirstOrDefault(l => l.CodRaggruppamento == x.CodRaggruppamento).Descrizione,
                                                DescrizioneProcedure = x.Descrizione,
                                                InUso = x.T_CARTELLA_ORTODONTICA_PROCEDURE.In_Uso,
                                                InizioNuovaTerapia = x.T_CARTELLA_ORTODONTICA_PROCEDURE.InizioNuovaTerapia,
                                                Diagnostico = x.T_CARTELLA_ORTODONTICA_PROCEDURE.Diagnostico,
                                                Controllo = x.T_CARTELLA_ORTODONTICA_PROCEDURE.Controllo
                                            }).ToList();
                gridEX_ProcedureOrtodontiche.DataSource = ProcedureOrtodonticaList;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void LoadSottoProcedure()
        {
            try
            {
                List<int> idProcedure = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Select(r => r.CodProcedura).ToList();
                var raggruppamenti = (from x in ctx.T_LISTINO_RAGGRUPPAMENTI
                                      select new ViewModels.KeyValue
                                      {
                                          Key = x.CodRaggruppamento,
                                          Value = x.Descrizione
                                      });
                List<int> idSottoProcedure = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Select(s => s.CodSottoprocedura).ToList();
                var list = (from x in ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE.Where(l => !idSottoProcedure.Contains(l.CodSottoprocedura) && idProcedure.Contains(l.CodProcedura)  )
                            join sott in ctx.T_LISTINO_SOTTOPROCEDURE.Where(sott =>(sott.In_uso??true))  on x.CodSottoprocedura equals sott.CodSottoprocedura
                            select new VM_SottoProcedure
                            {
                                CodSottoProcedura = x.CodSottoprocedura,
                                Raggruppamento = raggruppamenti.FirstOrDefault(r => r.Key == ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == x.CodProcedura).CodRaggruppamento).Value,
                                DescrizioneProcedura = ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == x.CodProcedura).Descrizione,
                                DescrizioneSottoProcedura = ctx.T_LISTINO_SOTTOPROCEDURE.FirstOrDefault(s => s.CodSottoprocedura == x.CodSottoprocedura).Descrizione,
                                Durata = x.Durata,
                                NumeroAppuntamento = x.NumeroAppuntamento
                            }).ToList();
                gridEX_SottoProcedure.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void LoadSottoProcedureRefertazione()
        {
            try
            {
                var raggruppamenti = (from x in ctx.T_LISTINO_RAGGRUPPAMENTI
                                      select new ViewModels.KeyValue
                                      {
                                          Key = x.CodRaggruppamento,
                                          Value = x.Descrizione
                                      });
                List<int> idSottoProcedure = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Select(r => r.CodSottoprocedura).ToList();

                SottoProcedureRefertazioneList = new List<VM_SottoProcedureRefertazione>();
                VM_SottoProcedureRefertazione row = new VM_SottoProcedureRefertazione();
                int procedura = 0;
                foreach (var x in ctx.T_LISTINO_SOTTOPROCEDURE.Where(p => idSottoProcedure.Contains(p.CodSottoprocedura)))
                {
                    row = new VM_SottoProcedureRefertazione();
                    row.CodSottoProcedura = x.CodSottoprocedura;
                    procedura = ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE.FirstOrDefault(s => s.CodSottoprocedura == x.CodSottoprocedura).CodProcedura;
                    row.Raggruppamento = raggruppamenti.FirstOrDefault(r => r.Key == ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == procedura).CodRaggruppamento).Value;
                    row.DescrizioneProcedura = ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == procedura).Descrizione;
                    row.DescrizioneSottoProcedura = x.Descrizione;
                    row.InUso = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.FirstOrDefault(o => o.CodSottoprocedura == x.CodSottoprocedura).In_Uso;
                    SottoProcedureRefertazioneList.Add(row);
                }
                gridEX_SottoProcedureRefertazione.DataSource = SottoProcedureRefertazioneList.OrderBy(o => o.Raggruppamento).ThenBy(o => o.DescrizioneProcedura).ThenBy(o => o.DescrizioneSottoProcedura).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void LoadTemplateDoc()
        {
          
            var setup = ctx.T_SETUP.SingleOrDefault(s => s.Nome == "TEMPLATE_CONSENSO_ORTODONTICO_PATH");
            if (setup != null)
            {
                editBox_template_nome.Text = setup.Nome;
                editBox_template_descrizione.Text = setup.Descrizione;
                editBox_template_valore.Text = setup.Valore;
            }
            
        }
        #endregion

        private void DisableButton()
        {
            this.button_new.Enabled = false;
            this.button_delete.Enabled = false;
            this.button_abort.Enabled = false;
            this.button_save.Enabled = false;
        }

        private void btn_OrtodonticaRaggruppamenti_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            DisableTab();
            this.uiTabPage_Raggruppamenti.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_Raggruppamenti;
            DisableButton();

            LoadRaggruppamenti();
            LoadOrtodonticaRaggruppamenti();

            Cursor.Current = Cursors.Default;
        }

        private void aggiungiRaggruppamenti_Click(object sender, EventArgs e)
        {
            try
            {
                // prendo le rows selezionate
                var rows = gridEX_ListaRaggruppamenti.GetCheckedRows();

                VM_OrtodonticaRaggruppamento newItem = new VM_OrtodonticaRaggruppamento();
                foreach (var row in rows)
                {
                    newItem = new VM_OrtodonticaRaggruppamento();
                    newItem.CodRagg = Convert.ToInt32(row.Cells[1].Value);
                    newItem.Descrizione = row.Cells[2].Value.ToString();
                    newItem.InUso = true;
                    CartellaOrtodonticaRaggruppamentiList.Add(newItem);
                }
                //ctx.SaveChanges();
                gridEX_OrtodonticaRaggruppamenti.DataSource = CartellaOrtodonticaRaggruppamentiList;
                gridEX_OrtodonticaRaggruppamenti.Refetch();

                // eliminiamo dalla grid Lista Raggruppamenti i record appena aggiunti
                foreach (var elem in rows)
                {
                    elem.Delete();
                }
                this.button_save.Enabled = true;
                this.button_abort.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_eliminaRaggruppamenti_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("Sei sicuro di voler eliminare i raggruppamenti selezionati? Una volta confermata l'operazione non sarà possibile tornare indietro.",
                                     "Elimina",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    List<string> errors = new List<string>();
                    // elimina OrtodonticaRaggruppamenti
                    var rows = gridEX_OrtodonticaRaggruppamenti.GetCheckedRows();
                    foreach (var row in rows)
                    {
                        int id = Convert.ToInt32(row.Cells[1].Value);
                        var ris = ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI.Single(o => o.CodRaggruppamento == id);
                        // non ci devono essere trattamenti associati

                            // non devono esserci procedure ortodontiche associate
                            // 1. controllo se ci sono procedure legate al raggruppamento
                        int procedure_count = (from PROCEDURE in ctx.T_LISTINO_PROCEDURE.Where(s => s.CodRaggruppamento == id)
                                                join PROCEDURE_ORTODONTICHE in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE on PROCEDURE.CodProcedura equals PROCEDURE_ORTODONTICHE.CodProcedura
                                                select PROCEDURE_ORTODONTICHE).Count();
                        if (procedure_count == 0)
                        {
                            ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI.Remove(ris);
                        }
                        else
                        {
                            errors.Add(ris.T_LISTINO_RAGGRUPPAMENTI.Descrizione);
                        }

                    }


                    if (errors.Count > 0)
                    {
                        string msg_errors = string.Join(",", errors);
                        throw new Exception("Non è possibile eliminare i seguenti raggruppamenti perché ci sono trattamenti o procedure associate: " + msg_errors);
                    }
                    ctx.SaveChanges();
                    LoadRaggruppamenti();
                    LoadOrtodonticaRaggruppamenti();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_Procedure_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            DisableTab();
            this.uiTabPage_Procedure.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_Procedure;
            DisableButton();

            LoadProcedure();
            LoadProcedureOrtodontiche();
            Cursor.Current = Cursors.Default;

        }

        private void btn_SottoProcedure_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DisableTab();
            this.uiTabPage_SottoProcedure.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_SottoProcedure;
            DisableButton();

            LoadSottoProcedure();
            LoadSottoProcedureRefertazione();
            Cursor.Current = Cursors.Default;
        }

        private void FormStart_Load(object sender, EventArgs e)
        {
            ctx = new CM_INVOICEEntities1();
            FormLoading frm = new FormLoading();
            frm.Show();

            frm.label_loading.Text = "1/4 caricamento raggruppamenti...";
            frm.Refresh();

            LoadRaggruppamenti();
            LoadOrtodonticaRaggruppamenti();
            frm.uiProgressBar_loading.Value = 25;

            frm.label_loading.Text = "2/4 caricamento procedure...";
            frm.Refresh();
            LoadProcedure();
            LoadProcedureOrtodontiche();
            frm.uiProgressBar_loading.Value = 50;

            frm.label_loading.Text = "3/4 caricamento sottoprocedure...";
            frm.Refresh();
            LoadSottoProcedure();
            LoadSottoProcedureRefertazione();
            frm.uiProgressBar_loading.Value = 75;

            frm.label_loading.Text = "4/4 caricamento dati clinici...";
            frm.Refresh();
            LoadDatiClinici();
            LoadTemplateDoc();
            frm.uiProgressBar_loading.Value = 100;
            frm.Hide();



            if (this.codPaziente != -1)
            {
                var paziente = ctx.T_ANAGRAFICA_PAZIENTI.SingleOrDefault(p => p.CodPaziente == this.codPaziente);
                if (paziente != null)
                {
                    this.codPaziente = paziente.CodPaziente;
                    ViewCartellaOrtodontica(codPaziente, paziente.Cognome, paziente.Nome);
                }
            }
        }

        private void btn_AggiungiProcedura_Click(object sender, EventArgs e)
        {
            try
            {
                // prendo le rows selezionate
                var rows = gridEX_Procedure.GetCheckedRows();
                VM_OrtodonticaProcedure newItem = new VM_OrtodonticaProcedure();
                foreach (var row in rows)
                {
                    newItem = new VM_OrtodonticaProcedure();

                    newItem.CodProcedura = Convert.ToInt32(row.Cells[1].Value);
                    newItem.DescrizioneRaggruppamento = row.Cells[2].Value.ToString();
                    newItem.DescrizioneProcedure = row.Cells[3].Value.ToString();
                    newItem.Diagnostico = false;
                    newItem.InizioNuovaTerapia = false;
                    newItem.InUso = true;
                    ProcedureOrtodonticaList.Add(newItem);
                }
                gridEX_ProcedureOrtodontiche.DataSource = ProcedureOrtodonticaList;
                gridEX_ProcedureOrtodontiche.Refetch();

                foreach (var elem in rows)
                {
                    elem.Delete();
                }
                this.button_save.Enabled = true;
                this.button_abort.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_EliminaProcedureOrtodontiche_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("Sei sicuro di voler eliminare le procedure selezionate? Una volta confermata l'operazione non sarà possibile tornare indietro.",
                                    "Elimina",
                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    List<string> errors = new List<string>();
                    // elimina OrtodonticaProcedure
                    var rows = gridEX_ProcedureOrtodontiche.GetCheckedRows();
                    foreach (var row in rows)
                    {
                        int id = Convert.ToInt32(row.Cells[1].Value);
                        var ris = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Single(o => o.CodProcedura == id);
                        // controllo se ci sono trattamenti associati
                   //     if (ctx.T_TRATTAMENTI.Count(t => t.CodProcedura == id) == 0)
                   //    {
                            // non devono esserci sottoprocedure ortodontiche associate
                            // 1. controllo se ci sono sottoprocedure legate alle procedure di refertazione
                            int sottoprocedure_count = (from PROCEDURE_SOTTOPROCEDURE in ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE.Where(s => s.CodProcedura == id)
                                                        join SOTTOPROCEDURE in ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE on PROCEDURE_SOTTOPROCEDURE.CodSottoprocedura equals SOTTOPROCEDURE.CodSottoprocedura
                                                        select SOTTOPROCEDURE).Count();
                            if (sottoprocedure_count == 0)
                            {
                                ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Remove(ris);
                            }
                            else
                            {
                                errors.Add(ris.T_LISTINO_PROCEDURE.Descrizione);
                            }
                 /*     }
                        else
                        {
                            errors.Add(ris.T_LISTINO_PROCEDURE.Descrizione);
                        } */
                    }

                    if (errors.Count > 0)
                    {
                        string msg_errors = string.Join(",", errors);
                        throw new Exception("Non è possibile eliminare le seguenti procedure perché ci sono sottoprocedure associate: " + msg_errors);
                    }
                    ctx.SaveChanges();
                    LoadProcedure();
                    LoadProcedureOrtodontiche();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_AggiungiSottoProcedure_Click(object sender, EventArgs e)
        {
            try
            {
                // prendo le rows selezionate
                var rows = gridEX_SottoProcedure.GetCheckedRows();
                VM_SottoProcedureRefertazione newItem;
                foreach (var row in rows)
                {
                    newItem = new VM_SottoProcedureRefertazione();
                    newItem.CodSottoProcedura = Convert.ToInt32(row.Cells[1].Value);
                    newItem.Raggruppamento = row.Cells[2].Value.ToString();
                    newItem.DescrizioneProcedura = row.Cells[3].Value.ToString();
                    newItem.DescrizioneSottoProcedura = row.Cells[4].Value.ToString();
                    newItem.InUso = true;
                    SottoProcedureRefertazioneList.Add(newItem);
                }
                gridEX_SottoProcedureRefertazione.DataSource = SottoProcedureRefertazioneList;
                gridEX_SottoProcedureRefertazione.Refetch();

                foreach (var elem in rows)
                {
                    elem.Delete();
                }
                this.button_save.Enabled = true;
                this.button_abort.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_DatiClinici_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            DisableTab();
            this.uiTabPage_DatiClinici.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_DatiClinici;
            DisableButton();

            LoadDatiClinici();
            Cursor.Current = Cursors.Default;
        }

        public void LoadDatiClinici()
        {
            //this.t_CARTELLA_ORTODONTICA_DATI_CLINICITableAdapter.Fill(this.cM_INVOICEDataSet.T_CARTELLA_ORTODONTICA_DATI_CLINICI);
            var list = (from d in ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI
                        select new VM_DatiClinici
                        {
                            CodDato = d.CodDato,
                            Descrizione = d.Descrizione,
                            InUso = d.In_Uso
                        }).ToList();
            gridEX_DatiClinici.DataSource = list;
        }

        private void gridEX_DatiClinici_AddingRecord(object sender, CancelEventArgs e)
        {
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;

            Janus.Windows.GridEX.GridEXRow newRow = this.gridEX_DatiClinici.GetRow();
            var value = newRow.Cells[3];
            if (value.Text == string.Empty)
                value.Value = false;
        }

        private void button_save_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int id = 0;
                //controllare la tab corrente
                switch (this.uiTab.SelectedTab.Name)
                {
                    case "uiTabPage_Raggruppamenti":
                        gridEX_OrtodonticaRaggruppamenti.UpdateData();
                        var listRagg = gridEX_OrtodonticaRaggruppamenti.GetDataRows();
                        foreach (var row in listRagg)
                        {
                            id = Convert.ToInt32(row.Cells[1].Value);
                            T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI ragg = ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI.SingleOrDefault(r => r.CodRaggruppamento == id);
                            if (ragg != null)
                            {
                                ragg.In_Uso = Convert.ToBoolean(row.Cells[3].Value);
                            }
                            else
                            {
                                ragg = new T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI();
                                ragg.CodRaggruppamento = Convert.ToInt32(row.Cells[1].Value);
                                ragg.In_Uso = Convert.ToBoolean(row.Cells[3].Value);
                                ctx.T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI.Add(ragg);
                            }
                            ctx.SaveChanges();
                        }
                        break;
                    case "uiTabPage_Procedure":
                        gridEX_ProcedureOrtodontiche.UpdateData();
                        var listProcedure = gridEX_ProcedureOrtodontiche.GetDataRows();
                        foreach (var row in listProcedure)
                        {
                            id = Convert.ToInt32(row.Cells[1].Value);
                            T_CARTELLA_ORTODONTICA_PROCEDURE prod = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.SingleOrDefault(p => p.CodProcedura == id);
                            if (prod != null)
                            {
                                prod.In_Uso = Convert.ToBoolean(row.Cells[4].Value);
                                prod.InizioNuovaTerapia = Convert.ToBoolean(row.Cells[5].Value);
                                prod.Diagnostico = Convert.ToBoolean(row.Cells[6].Value);
                                prod.Controllo = Convert.ToBoolean(row.Cells[7].Value);
                            }
                            else
                            {
                                prod = new T_CARTELLA_ORTODONTICA_PROCEDURE();
                                prod.CodProcedura = Convert.ToInt32(row.Cells[1].Value);
                                prod.In_Uso = Convert.ToBoolean(row.Cells[4].Value);
                                prod.InizioNuovaTerapia = Convert.ToBoolean(row.Cells[5].Value);
                                prod.Diagnostico = Convert.ToBoolean(row.Cells[6].Value);
                                prod.Controllo = Convert.ToBoolean(row.Cells[7].Value);
                                ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Add(prod);
                            }
                            ctx.SaveChanges();
                        }
                        break;
                    case "uiTabPage_SottoProcedure":
                        gridEX_SottoProcedureRefertazione.UpdateData();
                        var listSottoProcedure = gridEX_SottoProcedureRefertazione.GetDataRows();
                        foreach (var row in listSottoProcedure)
                        {
                            id = Convert.ToInt32(row.Cells[1].Value);
                            T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE sottoProd = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.SingleOrDefault(p => p.CodSottoprocedura == id);
                            if (sottoProd != null)
                            {
                                sottoProd.In_Uso = Convert.ToBoolean(row.Cells[5].Value);
                            }
                            else
                            {
                                sottoProd = new T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE();
                                sottoProd.CodSottoprocedura = Convert.ToInt32(row.Cells[1].Value);
                                sottoProd.In_Uso = Convert.ToBoolean(row.Cells[5].Value);
                                ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Add(sottoProd);
                            }
                            ctx.SaveChanges();
                        }
                        break;
                    case "uiTabPage_DatiClinici":
                        var listDatiClinici = gridEX_DatiClinici.GetDataRows();
                        bool newRow = false;
                        foreach (var row in listDatiClinici)
                        {
                            string idString = row.Cells[1].Value.ToString();
                            T_CARTELLA_ORTODONTICA_DATI_CLINICI datiClinici = ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.SingleOrDefault(p => p.CodDato == idString);
                            if (datiClinici == null)
                            {
                                datiClinici = new T_CARTELLA_ORTODONTICA_DATI_CLINICI();
                                datiClinici.CodDato = idString;
                                newRow = true;
                            }
                            datiClinici.Descrizione = row.Cells[2].Value.ToString();
                            datiClinici.In_Uso = Convert.ToBoolean(row.Cells[3].Value);
                            if (newRow)
                            {
                                ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Add(datiClinici);
                            }
                            ctx.SaveChanges();
                        }
                        break;
                    case "uiTabPage_TemplateDoc":
                        /*   string TEMPLATE_CONSENSO_ORTODONTICO_PATH = ConfigurationManager.AppSettings["TEMPLATE_CONSENSO_ORTODONTICO_PATH"];
                           T_SETUP setup = new T_SETUP();
                           if (TEMPLATE_CONSENSO_ORTODONTICO_PATH != string.Empty)
                           {

                               setup = ctx.T_SETUP.SingleOrDefault(s => s.Nome == TEMPLATE_CONSENSO_ORTODONTICO_PATH);
                               setup.Nome = editBox_template_nome.Text;
                               setup.Descrizione = editBox_template_descrizione.Text;
                               setup.Valore = editBox_template_valore.Text;
                           }
                           else
                           {
                               setup.Nome = editBox_template_nome.Text;
                               setup.Descrizione = editBox_template_descrizione.Text;
                               setup.Valore = editBox_template_valore.Text;
                               ctx.T_SETUP.Add(setup);
                           } */
                        T_SETUP setup = ctx.T_SETUP.FirstOrDefault(s => s.Nome == "TEMPLATE_CONSENSO_ORTODONTICO_PATH");
                      
                        if(setup==null)
                        {
                            MessageBox.Show("Attenzione parametro di setup TEMPLATE_CONSENSO_ORTODONTICO_PATH mancante");
                            return;
                        }
                        setup.Valore = editBox_template_valore.Text;
                        ctx.SaveChanges();
                        MessageBox.Show("La nuova configurazione è stata salvata con successo.");
                        break; 
                }
                //salvare i record
                //ctx.SaveChanges();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void gridEX_OrtodonticaRaggruppamenti_EditModeChanged(object sender, EventArgs e)
        {
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;
        }

        private void gridEX_ProcedureOrtodontiche_EditModeChanged(object sender, EventArgs e)
        {
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;
        }

        private void gridEX_SottoProcedureRefertazione_EditModeChanged(object sender, EventArgs e)
        {
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;
        }

        private void gridEX_DatiClinici_EditModeChanged(object sender, EventArgs e)
        {
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;
        }

        private void uiButton_search_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string nome = editBox_nome.Text;
                string cognome = editBox_cognome.Text;

                var pazienti = (from x in ctx.T_ANAGRAFICA_PAZIENTI.Where(a => (String.IsNullOrEmpty(nome) || a.Nome.Contains(nome))
                                    && (String.IsNullOrEmpty(cognome) || a.Cognome.Contains(cognome))).ToList()
                                select new ViewModels.VM_RicercaPaziente
                                {
                                    CodPaziente = x.CodPaziente,
                                    Nome = x.Nome,
                                    Cognome = x.Cognome,
                                    Indirizzo = x.Indirizzo,
                                    Cap = x.CAP,
                                    Citta = x.Citta,
                                    Provincia = x.Provincia,
                                    DataNascita = x.Natoil,
                                    Eta = (DateTime.Today.Year - x.Natoil.Value.Year) + (new DateTime(DateTime.Today.Year, x.Natoil.Value.Month, x.Natoil.Value.Day) <= DateTime.Today ? 1 : 0),
                                    Sesso = x.Sesso
                                }).ToList();
                gridEX_RicercaPaziente.DataSource = pazienti;
            }
            catch(Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
            Cursor.Current = Cursors.Default;
        }

        public void ViewCartellaOrtodontica(int codPaziente, string cognomePaziente, string nomePaziente)
        {
            Cursor.Current = Cursors.WaitCursor;
            label_CartellaOrtodontica_Paziente.Text = "PAZIENTE: " + cognomePaziente + " " + nomePaziente;
            btn_PazienteCartellaOrtodontica.Enabled = true;
            btn_PazienteDatiClinici.Enabled = true;
            DisableTab();

            #region Anamnesi
            // Anamnesi - allergie

            var allergieAnamnesi = string.Join(", ", ctx.ANM_AllergieFarmacologiche.Where(a => a.CodPaziente == codPaziente).Select(f => f.Farmaco).OrderBy(o => o));
            txt_Anamnesi_Allergie.Text = "ALLERGIE: " + allergieAnamnesi;

            // anamnesi - farmaci
            var farmaci = string.Join(", ", ctx.ANM_FarmaciAssunti.Where(a => a.CodPaziente == codPaziente).Select(f => f.Farmaco).OrderBy(o => o));
            label_Anamnesi_Farmaci.Text = "FARMACI: " + farmaci;

            // anamnesi - patologie
            var patologie = string.Join(", ", ctx.ANM_Patologie.Where(a => a.CodPaziente == codPaziente).Select(f => f.DescrizionePatologia).OrderBy(o => o));
            label_Anamnesi_Patologie.Text = "PATOLOGIE: " + patologie;
            #endregion

            #region Piano di Trattamento Accettato
            // Diagnosi - OsservazioniGenerali
            var SottoprocedureDiagnostico = (from PROCEDURE in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(x => x.In_Uso && x.InizioNuovaTerapia)
                                             join PROCEDURESOTTOPROCEDURE in ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE on PROCEDURE.CodProcedura equals PROCEDURESOTTOPROCEDURE.CodProcedura
                                             join SOTTOPROCEDURE in ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Where(x => x.In_Uso) on PROCEDURESOTTOPROCEDURE.CodSottoprocedura equals SOTTOPROCEDURE.CodSottoprocedura
                                             select SOTTOPROCEDURE.CodSottoprocedura).ToList();
                                    
            var IdTrattamenti = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente && !t.Cancellato &&  SottoprocedureDiagnostico.Contains(t.CodSottoprocedura.Value)).OrderByDescending(o => o.DataEsecuzione).Select(o => o.NTrattamento).ToList();
            var Diagnosi = string.Join(", ", ctx.T_TRATTAMENTI_NOTE.Where(t => t.Daeseguire != null
                && t.Daeseguire != string.Empty
                && t.NTrattamento.HasValue
                && IdTrattamenti.Contains(t.NTrattamento.Value)).Select(o => o.Daeseguire));
            Diagnosi=Diagnosi+","+string.Join(", ", ctx.T_TRATTAMENTI_NOTE.Where(t => t.Eseguito != null
                 && t.Eseguito != string.Empty
                 && t.NTrattamento.HasValue
                 && IdTrattamenti.Contains(t.NTrattamento.Value)).Select(o => o.Eseguito));
            Diagnosi = Diagnosi+"," + string.Join(", ", ctx.T_TRATTAMENTI_NOTE.Where(t => t.OsservazioniGenerali != null
               && t.OsservazioniGenerali != string.Empty
               && t.NTrattamento.HasValue
               && IdTrattamenti.Contains(t.NTrattamento.Value)).Select(o => o.OsservazioniGenerali));
            if (Diagnosi != string.Empty)
                txt_PianiTrattamento.Text = "OSSERVAZIONI GENERALI: " + Diagnosi;

            var OsservazioniReferti = string.Join(", ", ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Where(r => IdTrattamenti.Contains(r.NTrattamento)).Select(r => r.Osservazioni));
            if (OsservazioniReferti != string.Empty)
                txt_Diagnosi_Refertazioni.Text  = "REFERTI: " + OsservazioniReferti;
            #endregion

            #region Diagnosi
            // Diagnosi - OsservazioniGenerali
            var SottoprocedureControllo = (from PROCEDURE in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(x => x.In_Uso && x.Diagnostico)
                                             join PROCEDURESOTTOPROCEDURE in ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE on PROCEDURE.CodProcedura equals PROCEDURESOTTOPROCEDURE.CodProcedura
                                             join SOTTOPROCEDURE in ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Where(x => x.In_Uso) on PROCEDURESOTTOPROCEDURE.CodSottoprocedura equals SOTTOPROCEDURE.CodSottoprocedura
                                             select SOTTOPROCEDURE.CodSottoprocedura).ToList();

            IdTrattamenti = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente && !t.Cancellato && t.Concordato && SottoprocedureControllo.Contains(t.CodSottoprocedura.Value)).OrderByDescending(o => o.DataEsecuzione).Select(o => o.NTrattamento).ToList();
            var Eseguito = string.Join(", ", ctx.T_TRATTAMENTI_NOTE.Where(t => t.Eseguito != null
                && t.Daeseguire != string.Empty
                && t.NTrattamento.HasValue
                && IdTrattamenti.Contains(t.NTrattamento.Value)).Select(o => o.Eseguito));
            txt_Diagnosi_Osservazioni.Text = "OSSERVAZIONI DIAGNOSI:"+Eseguito;
            #endregion

            #region TimeLineTrattamenti
            var ProcedureInizioTerapiaList = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(p => p.InizioNuovaTerapia == true).Select(p => p.CodProcedura).ToList();
            var SottoprocedureControlloList = (from Procedure in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(p => p.Controllo == true && p.In_Uso)
                                          join ProcedureSottoprocedure in ctx.T_LISTINO_PROCEDURE_SOTTOPROCEDURE on Procedure.CodProcedura equals ProcedureSottoprocedure.CodProcedura
                                          join Sottoprocedure in ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Where(s => s.In_Uso) on ProcedureSottoprocedure.CodSottoprocedura equals Sottoprocedure.CodSottoprocedura
                                          select Sottoprocedure.CodSottoprocedura).ToList();
            if (ProcedureInizioTerapiaList.Count() > 0) 
            { 
                //La data di inizio terapia è la più recente data di esecuzione tra  i trattamenti associati a procedure identificate come procedure di inizio terapia.
                var trattamentiAux = (from TRATTAMENTI in ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente
                     && t.CodProcedura != null
                     && t.DataEsecuzione != null
                     && t.CodSottoprocedura != null
                     && !t.Cancellato
                     && t.Concordato)
                    join PROCEDURE in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(x => x.In_Uso && x.InizioNuovaTerapia)
                    on TRATTAMENTI.CodProcedura equals PROCEDURE.CodProcedura
                    select TRATTAMENTI).ToList();
                DateTime ? DataInizioTerapia = null;
                if (trattamentiAux.Count() > 0)
                    DataInizioTerapia = trattamentiAux.Max(m => m.DataEsecuzione.Value);
                // La durata in mesi della terapia è pari al numero di trattamenti associati a procedure identificate come procedure di controllo che 
                // devono ancora essere eseguiti ( data esecuzione = null ) 
                // o che hanno data esecuzione posteriore alla data di esecuzione del più recente trattamento associato ad una procedura di inizio terapia.
                var trattamentiControllo = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente
                       && t.CodSottoprocedura != null
                       && !t.Cancellato
                       && t.Concordato
                       && (t.DataEsecuzione == null || t.DataEsecuzione > DataInizioTerapia) && SottoprocedureControlloList.Contains(t.CodSottoprocedura.Value)
                        ).ToList();
                int durataTerapia = trattamentiControllo.Count();
                if (durataTerapia > 0)
                {
                    DataInizioTerapia = trattamentiControllo.Where(x =>x.DataEsecuzione.HasValue).Min(x => x.DataEsecuzione.Value);
                }
                // Il mese attuale di terapia è pari al numero di trattamenti associati a procedure identificate come procedure di controllo 
                // che hanno data esecuzione posteriore alla data esecuzione del più recente trattamento associato ad una procedura di inizio terapia.
                int meseAttuale = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente
                         && t.CodSottoprocedura != null
                         && !t.Cancellato
                         && t.Concordato
                         &&  t.DataEsecuzione >= DataInizioTerapia && SottoprocedureControlloList.Contains(t.CodSottoprocedura.Value)
                            ).Count();

                Janus.Windows.TimeLine.TimeLineItem item = null;
                //int i = 1;
                timeLine_Trattamenti.MaxDate = (DataInizioTerapia.HasValue ? DataInizioTerapia.Value.AddMonths(-2) : DateTime.Today);
                timeLine_Trattamenti.MinDate = (DataInizioTerapia.HasValue ? DataInizioTerapia.Value.AddMonths(-2) : DateTime.Today);
                timeLine_Trattamenti.Items.Clear();
                for(int i = 1; i <= durataTerapia; i++)
                {
                    item = new Janus.Windows.TimeLine.TimeLineItem();
                    item.StartTime = DataInizioTerapia.Value.AddMonths(i-1);
                    if (i == meseAttuale)
                    {
                        item.FormatStyle.BackColor = Color.PaleTurquoise;
                    }
                    item.Text = i + "/" + durataTerapia;
                    item.Image = Image.FromFile("Img/businessman-30.png");
                    timeLine_Trattamenti.Items.Add(item);
                }
                timeLine_Trattamenti.MaxDate = item.StartTime.AddMonths(3);
            }
            #endregion

            #region NoteCliniche
            var ProcedureControlloList = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(x => x.Controllo && x.In_Uso).Select(x =>x.CodProcedura).ToList();
            var noteCliniche = (from TRATTAMENTI in ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente
                    && t.CodProcedura != null && !t.Cancellato && t.Concordato && t.CodSottoprocedura!=null
                    && ProcedureControlloList.Contains(t.CodProcedura??0)
                   )
                                select new VM_NoteCliniche
                                {
                                    NTrattamento = TRATTAMENTI.NTrattamento,
                                    NumPianoDiCura = TRATTAMENTI.NumPianoDiCura,
                                    Raggruppamento = ctx.T_LISTINO_RAGGRUPPAMENTI.FirstOrDefault(r => r.CodRaggruppamento == TRATTAMENTI.CodRaggruppamento).Descrizione,
                                    Procedura = ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == TRATTAMENTI.CodProcedura).Descrizione,
                                    SottoProcedura = ctx.T_LISTINO_SOTTOPROCEDURE.FirstOrDefault(s => s.CodSottoprocedura == TRATTAMENTI.CodSottoprocedura).Descrizione,
                                    OsservazioniGenerali = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).OsservazioniGenerali,
                                    DaEseguire = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).Daeseguire,
                                    Eseguito = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).Eseguito,
                                    DataEsecuzione = TRATTAMENTI.DataEsecuzione,
                                    DataAppuntamento = ctx.T_AGENDA_APPUNTAMENTI.FirstOrDefault(a => a.NTrattamento == TRATTAMENTI.NTrattamento) == null ? string.Empty : ctx.T_AGENDA_APPUNTAMENTI.FirstOrDefault(a => a.NTrattamento == TRATTAMENTI.NTrattamento).DataAppuntamento.ToString(),
                                    Prescrizione = ctx.T_PRESC_DETTAGLIO.FirstOrDefault(p => p.Num_Trat==TRATTAMENTI.NTrattamento)==null ? string.Empty : ctx.T_PRESC_DETTAGLIO.FirstOrDefault(p => p.Num_Trat==TRATTAMENTI.NTrattamento).Numero,
                                    Laboratorio = ctx.T_PRESC_DETTAGLIO.FirstOrDefault(p => p.Num_Trat == TRATTAMENTI.NTrattamento) == null ? string.Empty : ( from PRESC_DETTAGLIO in ctx.T_PRESC_DETTAGLIO.Where(p => p.Num_Trat == TRATTAMENTI.NTrattamento)
                                                                                                                                                               join PRESCRIZIONE in ctx.T_PRESCRIZIONE on PRESC_DETTAGLIO.Numero equals PRESCRIZIONE.Numero
                                                                                                                                                               join FORNITORI in ctx.T_ANAGRAFICA_FORNITORI on PRESCRIZIONE.CodiceLaboratorio equals FORNITORI.CodFornitore
                                                                                                                                                               select FORNITORI.RagioneSociale).FirstOrDefault()
                                }).ToList();
            gridEX_NoteCliniche.DataSource = noteCliniche;
            #endregion

            #region DatiClinici
            LoadPazienteDatiClinici();
            btn_PazienteDatiClinici.Enabled = true;
            btn_CartellaOrtodontica_nuovo.Enabled = true;
            btn_CartellaOrtodontica_modifica.Enabled = true;
            bt_GeneraConsensoInformato.Enabled = true;
            #endregion

            this.uiTab.SelectedTab = this.uiTabPage_CartellaOrtodontica;
            Cursor.Current = Cursors.Default;
        }

        private void vaiACartellaOrtodonticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                nomePaziente = gridEX_RicercaPaziente.CurrentRow.Cells[2].Value.ToString();
                cognomePaziente = gridEX_RicercaPaziente.CurrentRow.Cells[1].Value.ToString();
                this.codPaziente = Convert.ToInt32(gridEX_RicercaPaziente.CurrentRow.Cells[0].Value);
                ViewCartellaOrtodontica(codPaziente, cognomePaziente, nomePaziente);
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_PazienteRicerca_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            btn_PazienteCartellaOrtodontica.Enabled = false;
            btn_PazienteDatiClinici.Enabled = false;
            this.uiTab.SelectedTab = this.uiTabPage_RicercaPaziente;
        }

        private void btn_EliminaSottoProcedure_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("Sei sicuro di voler eliminare le sottoprocedure selezionate? Una volta confermata l'operazione non sarà possibile tornare indietro.",
                                    "Elimina",
                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    List<string> errors = new List<string>();
                    // elimina OrtodonticaSottoprocedure
                    var rows = gridEX_SottoProcedureRefertazione.GetCheckedRows();
                    foreach (var row in rows)
                    {
                        int id = Convert.ToInt32(row.Cells[1].Value);
                        var ris = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Single(o => o.CodSottoprocedura == id);
                        // controllo se ci sono dati clinici associati
                        var datiClinici = (from REFERTAZIONE in ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE
                                           join TRATTAMENTI in ctx.T_TRATTAMENTI.Where(t => t.CodSottoprocedura == id)
                                           on REFERTAZIONE.NTrattamento equals TRATTAMENTI.NTrattamento
                                           select TRATTAMENTI.NTrattamento);
                       if (datiClinici==null || datiClinici.Count()==0)
                       {
                            ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Remove(ris);
                       }
                       else
                       {
                            errors.Add(ris.T_LISTINO_SOTTOPROCEDURE.Descrizione);
                       } 
                    }

                    if (errors.Count > 0)
                    {
                        string msg_errors = string.Join(",", errors);
                        throw new Exception("Non è possibile eliminare le seguenti sottoprocedure perché ci sono trattamenti associati: " + msg_errors);
                    }
                    ctx.SaveChanges();
                    LoadSottoProcedure();
                    LoadSottoProcedureRefertazione();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void button_abort_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //controllare la tab corrente
                switch (this.uiTab.SelectedTab.Name)
                {
                    case "uiTabPage_Raggruppamenti":
                        LoadRaggruppamenti();
                        LoadOrtodonticaRaggruppamenti();
                        break;
                    case "uiTabPage_Procedure":
                        LoadProcedure();
                        LoadProcedureOrtodontiche();
                        break;
                    case "uiTabPage_SottoProcedure":
                        LoadSottoProcedure();
                        LoadSottoProcedureRefertazione();
                        break;
                    case "uiTabPage_DatiClinici":
                        LoadDatiClinici();
                        break;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void gridEX_NoteCliniche_Click(object sender, EventArgs e)
        {
            this.gridEX_NoteCliniche.ContextMenuStrip = contextMenuStrip_Trattamenti;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadPazienteDatiClinici();
            btn_PazienteDatiClinici.Enabled = true;
            btn_CartellaOrtodontica_nuovo.Enabled = true;
            btn_CartellaOrtodontica_modifica.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_Paziente_DatiClinici;
        }

        public void LoadPazienteDatiClinici()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int NTrattamento = Convert.ToInt32(gridEX_NoteCliniche.CurrentRow.Cells[0].Value);

                var trattamentiId = (from TRATTAMENTI in ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente)
                                     join Procedure in ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(p => p.Diagnostico == true && p.In_Uso) on TRATTAMENTI.CodProcedura equals Procedure.CodProcedura
                                     join Sottoprocedure in ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Where(s => s.In_Uso) on TRATTAMENTI.CodSottoprocedura equals Sottoprocedure.CodSottoprocedura
                                    select TRATTAMENTI.NTrattamento).ToList().Distinct();
                                    
                var TrattamentiDiagnostici = (from TRATTAMENTI in ctx.T_TRATTAMENTI.Where(t => trattamentiId.Contains(t.NTrattamento))
                                                  select new VM_NoteCliniche
                                                  {
                                                      NTrattamento = TRATTAMENTI.NTrattamento,
                                                      NumPianoDiCura = TRATTAMENTI.NumPianoDiCura,
                                                      Raggruppamento = ctx.T_LISTINO_RAGGRUPPAMENTI.FirstOrDefault(r => r.CodRaggruppamento == TRATTAMENTI.CodRaggruppamento).Descrizione,
                                                      Procedura = ctx.T_LISTINO_PROCEDURE.FirstOrDefault(p => p.CodProcedura == TRATTAMENTI.CodProcedura).Descrizione,
                                                      SottoProcedura = ctx.T_LISTINO_SOTTOPROCEDURE.FirstOrDefault(s => s.CodSottoprocedura == TRATTAMENTI.CodSottoprocedura).Descrizione,
                                                      Dente_1 = TRATTAMENTI.Dente_1,
                                                      Dente_2 = TRATTAMENTI.Dente_2,
                                                      OsservazioniGenerali = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).OsservazioniGenerali,
                                                      DaEseguire = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).Daeseguire,
                                                      Eseguito = ctx.T_TRATTAMENTI_NOTE.FirstOrDefault(o => o.NTrattamento == TRATTAMENTI.NTrattamento).Eseguito,
                                                      DataEsecuzione = TRATTAMENTI.DataEsecuzione,
                                                      DatiClinici = TRATTAMENTI.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Select(s => new VM_OrtodonticaRefertazione(){
                                                          CodDato = s.CodDato,
                                                          DescrizioneDato = s.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Descrizione,
                                                          NTrattamento = s.NTrattamento,
                                                          Osservazioni = s.Osservazioni,
                                                          Valore = s.Valore
                                                      }).ToList()
                                                  }).ToList();
                this.gridEX_trattamentiDiagnostici.DataSource = TrattamentiDiagnostici;
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void gridEX_Paziente_DatiClinici_Click(object sender, EventArgs e)
        {
            btn_CartellaOrtodontica_modifica.Enabled = true;
        }

        private void btn_CartellaOrtodontica_modifica_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //controllare la tab corrente
                switch (this.uiTab.SelectedTab.Name)
                {
                    case "uiTabPage_Raggruppamenti":
                        LoadOrtodonticaRaggruppamenti();
                        break;
                    case "uiTabPage_Procedure":
                        LoadProcedureOrtodontiche();
                        break;
                    case "uiTabPage_SottoProcedure":
                        LoadSottoProcedureRefertazione();
                        break;
                    case "uiTabPage_DatiClinici":
                        LoadDatiClinici();
                        break;
                    case "uiTabPage_Paziente_DatiClinici":
                        editTrattamentoRefertazione();
                        break;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }
        public void editTrattamentoRefertazione()
        {
            ////////// TODO: dacompletare edit e new
            if (gridEX_trattamentiDiagnostici.CurrentTable.Caption == "VM_OrtodonticaRefertazione")
            {
                int NTrattamento = Convert.ToInt32(gridEX_trattamentiDiagnostici.CurrentRow.Cells[0].Value);
                string codDato = gridEX_trattamentiDiagnostici.CurrentRow.Cells[1].Value.ToString();
                
                //string codDato = gridEX_Paziente_DatiClinici.CurrentRow.Cells[1].Value.ToString();
                //int NTrattamento = Convert.ToInt32(gridEX_Paziente_DatiClinici.CurrentRow.Cells[2].Value);
                var listDatiClinici = from x in ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI
                                      select new
                                      {
                                          x.CodDato,
                                          x.Descrizione
                                      };

                FormTrattamentoRefertazione frmTrattamento = new FormTrattamentoRefertazione(codDato, true);
                frmTrattamento.editBox_NumeroTrattamento.Text = NTrattamento.ToString();
                frmTrattamento.editBox_NumeroTrattamento.ReadOnly = true;
                frmTrattamento.Show();
                frmTrattamento.uiComboBox_DescrizioneDato.DataSource = listDatiClinici.ToList();
                frmTrattamento.uiComboBox_DescrizioneDato.ValueMember = "CodDato";
                frmTrattamento.uiComboBox_DescrizioneDato.DisplayMember = "Descrizione";
                frmTrattamento.uiComboBox_DescrizioneDato.SelectedValue = codDato;
                frmTrattamento.editBox_Valore.Text = gridEX_trattamentiDiagnostici.CurrentRow.Cells[3].Value.ToString();
                frmTrattamento.editBox_Osservazioni.Text = gridEX_trattamentiDiagnostici.CurrentRow.Cells[4].Value.ToString();
                GridEXRow parentRow = gridEX_trattamentiDiagnostici.CurrentRow.Parent;
                frmTrattamento.editBox_Procedura.Text = parentRow.Cells["Procedura"].Value.ToString();
                frmTrattamento.editBox_Sottoprocedura.Text = parentRow.Cells["SottoProcedura"].Value.ToString();

                frmTrattamento.FormClosing += HandleFrmTrattamento_Closing;
            }
        }

        private void btn_CartellaOrtodontica_nuovo_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //controllare la tab corrente
                switch (this.uiTab.SelectedTab.Name)
                {
                    case "uiTabPage_Paziente_DatiClinici":
                        newTrattamentoRefertazione();
                        break;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        public void newTrattamentoRefertazione()
        {
            var listDatiClinici = from x in ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI
                                  select new
                                  {
                                      x.CodDato,
                                      x.Descrizione
                                  };

            FormTrattamentoRefertazione frmTrattamento = new FormTrattamentoRefertazione(string.Empty, false);

            frmTrattamento.uiComboBox_DescrizioneDato.DataSource = listDatiClinici.ToList();
            frmTrattamento.uiComboBox_DescrizioneDato.ValueMember = "CodDato";
            frmTrattamento.uiComboBox_DescrizioneDato.DisplayMember = "Descrizione";
            //int NTrattamento = Convert.ToInt32(gridEX_Paziente_DatiClinici.CurrentRow.Cells[2].Value);
            int NTrattamento = Convert.ToInt32(gridEX_trattamentiDiagnostici.CurrentRow.Cells[0].Value);
            frmTrattamento.editBox_NumeroTrattamento.Text = NTrattamento.ToString();
            frmTrattamento.editBox_NumeroTrattamento.ReadOnly = true;
            frmTrattamento.editBox_Procedura.Text = gridEX_trattamentiDiagnostici.CurrentRow.Cells["Procedura"].Value.ToString();
            frmTrattamento.editBox_Sottoprocedura.Text = gridEX_trattamentiDiagnostici.CurrentRow.Cells["SottoProcedura"].Value.ToString();
            frmTrattamento.Show();
            frmTrattamento.FormClosing += HandleFrmTrattamento_Closing;
        }

        private void HandleFrmTrattamento_Closing(object sender, FormClosingEventArgs e)
        {
            LoadPazienteDatiClinici();
        }

        private void btn_PazienteCartellaOrtodontica_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            this.uiTab.SelectedTab = this.uiTabPage_CartellaOrtodontica;
        }

        private void btn_PazienteDatiClinici_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            this.uiTab.SelectedTab = this.uiTabPage_Paziente_DatiClinici;
        }

        private void gridEX_RicercaPaziente_MouseClick(object sender, MouseEventArgs e)
        {
            this.gridEX_RicercaPaziente.ContextMenuStrip = contextMenuStrip_menu;
        }



        private void GeneraConsensoOrtodontico()
        {
            try
            {
                VM_ReportConsensoOrtodontico consensoOrtodontico = new VM_ReportConsensoOrtodontico();
                consensoOrtodontico.DataDocumento = DateTime.Today.ToShortDateString();

                var paziente = ctx.T_ANAGRAFICA_PAZIENTI.FirstOrDefault(p => p.CodPaziente == codPaziente);

                consensoOrtodontico.NomeStudio = paziente.T_STUDI.NomeStruttura;
                consensoOrtodontico.Cognome = paziente.Cognome;
                consensoOrtodontico.Nome = paziente.Nome;
                var IOTN = ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.FirstOrDefault(d => d.Descrizione == "IOTN");
                if (IOTN == null)
                    consensoOrtodontico.IOTN = "0";
                else
                {
                    consensoOrtodontico.IOTN = ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.SingleOrDefault(d => d.CodDato == IOTN.CodDato).Valore;
                }

                consensoOrtodontico.LuogoNascita = paziente.CittaNasc;
                consensoOrtodontico.DataNascita = paziente.Natoil.HasValue ? paziente.Natoil.Value.ToShortDateString() : string.Empty;
                // Data di esecuzione della procedura di Diagnostico con data esecuzione più recente
                var procedureID = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(p => p.Diagnostico == true).Select(p => p.CodProcedura).ToList();

                var trattamentiDiagnostici = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente && t.DataEsecuzione.HasValue && t.CodProcedura.HasValue && procedureID.Contains(t.CodProcedura.Value));


                if (trattamentiDiagnostici.Count() > 0)
                {
                    var trattamentoDiagnostica = (from t in trattamentiDiagnostici
                                                  group t by t.CodProcedura into g
                                                  select new
                                                  {
                                                      CodProcedura = g.Key,
                                                      DataEsecuzione = (from t2 in g select t2.DataEsecuzione).Max()
                                                  }).OrderByDescending(o => o.DataEsecuzione);
                    consensoOrtodontico.DataEsecuzione = trattamentoDiagnostica.FirstOrDefault().DataEsecuzione.Value.ToShortDateString();
                    int codProceduraDiagnostica = trattamentoDiagnostica.FirstOrDefault().CodProcedura.Value;
                    consensoOrtodontico.NomeProcedura = ctx.T_LISTINO_PROCEDURE.Single(l => l.CodProcedura == codProceduraDiagnostica).Descrizione;
                    // sottoprocedure della procedura diagnostica
                    var sottoProcedureDiagnosticaID = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Select(p => p.CodSottoprocedura).ToList();
                    var sottoProcedureID = trattamentiDiagnostici.Where(t => t.CodSottoprocedura.HasValue && sottoProcedureDiagnosticaID.Contains(t.CodSottoprocedura.Value)).Select(s => s.CodSottoprocedura).ToList();
                    var sottoprocedureList = ctx.T_LISTINO_SOTTOPROCEDURE.Where(s => sottoProcedureID.Contains(s.CodSottoprocedura)).Select(s => s.Descrizione).ToList();
                    consensoOrtodontico.ElencoSottoProcedura = string.Join(", ", sottoprocedureList);
                    consensoOrtodontico.Diagnosi = "\r\n" + txt_Diagnosi_Osservazioni.Text + "\r\n" + txt_Diagnosi_Refertazioni.Text;
                    var trattamentiID = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente).Select(p => p.NTrattamento).ToList();
                    List<VM_OrtodonticaRefertazione> datiClinici = (from d in ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Where(r => trattamentiID.Contains(r.NTrattamento))
                                                                    select new VM_OrtodonticaRefertazione
                                                                    {
                                                                        DescrizioneDato = d.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Descrizione,
                                                                        Valore = d.Valore,
                                                                        Osservazioni = d.Osservazioni
                                                                    }).ToList();

                    consensoOrtodontico.DurataMesi = durataTerapia + " Mesi";
                    consensoOrtodontico.PropostaTrattamento = txt_PianiTrattamento.Text;

                    //  string TEMPLATE_CONSENSO_ORTODONTICO_PATH = ConfigurationManager.AppSettings["TEMPLATE_CONSENSO_ORTODONTICO_PATH"];
                    string pathTMP = ctx.T_SETUP.SingleOrDefault(s => s.Nome == "TEMPLATE_CONSENSO_ORTODONTICO_PATH") == null ? string.Empty : ctx.T_SETUP.SingleOrDefault(s => s.Nome == "TEMPLATE_CONSENSO_ORTODONTICO_PATH").Valore;
                    if (pathTMP == string.Empty)
                    {
                        MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è configurata la cartella di input per il template del file word.", "Errore generico:", MessageBoxButtons.OK);
                        return;
                    }
                    string path = pathTMP.Substring(0, pathTMP.LastIndexOf("\\"));
                    DirectoryInfo dirFolder = new DirectoryInfo(path);
                    if (!dirFolder.Exists)
                    {
                        MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è stato trovato il file template word.", "Errore generico:", MessageBoxButtons.OK);
                        return;
                    }
                    DirectoryInfo dirInput = new DirectoryInfo(pathTMP);
                    saveFileDoc.Filter = "doc (*.doc)|*.doc";
                    saveFileDoc.FileName = cognomePaziente + nomePaziente + DateTime.Today.ToString("ddMMyyyy");
                    if (saveFileDoc.ShowDialog() == DialogResult.OK)
                    {
                        CreateWordDocument(dirInput.FullName, saveFileDoc.FileName, consensoOrtodontico, datiClinici);
                    }
                }
                else
                {
                    MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è stato selezionato un trattamento valido.", "Errore generico:", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
}

        private void generaConsensoInformativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
              /*  int NTrattamento = Convert.ToInt32(gridEX_NoteCliniche.CurrentRow.Cells[0].Value);
                var trattamento = ctx.T_TRATTAMENTI.FirstOrDefault(t => t.NTrattamento == NTrattamento);
                if (trattamento == null)
                {
                    MessageBox.Show("Non è possibile generare il consento ortodontico per il trattamento selezionato.", "Errore generico:", MessageBoxButtons.OK);
                    return;
                } */

                VM_ReportConsensoOrtodontico consensoOrtodontico = new VM_ReportConsensoOrtodontico();
                consensoOrtodontico.DataDocumento = DateTime.Today.ToShortDateString();

                var paziente = ctx.T_ANAGRAFICA_PAZIENTI.FirstOrDefault(p => p.CodPaziente == codPaziente);

                consensoOrtodontico.NomeStudio = paziente.T_STUDI.NomeStruttura;
                consensoOrtodontico.Cognome = paziente.Cognome;
                consensoOrtodontico.Nome = paziente.Nome;
                var IOTN = ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.FirstOrDefault(d => d.Descrizione == "IOTN");
                if (IOTN == null)
                    consensoOrtodontico.IOTN = "0";
                else
                {
                    consensoOrtodontico.IOTN = ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.SingleOrDefault(d => d.CodDato == IOTN.CodDato).Valore;
                }

                consensoOrtodontico.LuogoNascita = paziente.CittaNasc;
                consensoOrtodontico.DataNascita = paziente.Natoil.HasValue ? paziente.Natoil.Value.ToShortDateString() : string.Empty;
                // Data di esecuzione della procedura di Diagnostico con data esecuzione più recente
                var procedureID = ctx.T_CARTELLA_ORTODONTICA_PROCEDURE.Where(p => p.Diagnostico == true).Select(p => p.CodProcedura).ToList();

                var trattamentiDiagnostici = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente && t.DataEsecuzione.HasValue && t.CodProcedura.HasValue && procedureID.Contains(t.CodProcedura.Value));


                if (trattamentiDiagnostici.Count() > 0)
                {
                    var trattamentoDiagnostica = (from t in trattamentiDiagnostici
                                                  group t by t.CodProcedura into g
                                                  select new
                                                  {
                                                      CodProcedura = g.Key,
                                                      DataEsecuzione = (from t2 in g select t2.DataEsecuzione).Max()
                                                  }).OrderByDescending(o => o.DataEsecuzione);
                    consensoOrtodontico.DataEsecuzione = trattamentoDiagnostica.FirstOrDefault().DataEsecuzione.Value.ToShortDateString();
                    int codProceduraDiagnostica = trattamentoDiagnostica.FirstOrDefault().CodProcedura.Value;
                    consensoOrtodontico.NomeProcedura = ctx.T_LISTINO_PROCEDURE.Single(l => l.CodProcedura == codProceduraDiagnostica).Descrizione;
                    // sottoprocedure della procedura diagnostica
                    var sottoProcedureDiagnosticaID = ctx.T_CARTELLA_ORTODONTICA_SOTTOPROCEDURE.Select(p => p.CodSottoprocedura).ToList();
                    var sottoProcedureID = trattamentiDiagnostici.Where(t => t.CodSottoprocedura.HasValue && sottoProcedureDiagnosticaID.Contains(t.CodSottoprocedura.Value)).Select(s => s.CodSottoprocedura).ToList();
                    var sottoprocedureList = ctx.T_LISTINO_SOTTOPROCEDURE.Where(s => sottoProcedureID.Contains(s.CodSottoprocedura)).Select(s => s.Descrizione).ToList();
                    consensoOrtodontico.ElencoSottoProcedura = string.Join(", ", sottoprocedureList);
                    consensoOrtodontico.Diagnosi = "\r\n" + txt_Diagnosi_Osservazioni.Text + "\r\n" + txt_Diagnosi_Refertazioni.Text;
                    var trattamentiID = ctx.T_TRATTAMENTI.Where(t => t.CodPaziente == codPaziente).Select(p => p.NTrattamento).ToList();
                    List<VM_OrtodonticaRefertazione> datiClinici = (from d in ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Where(r => trattamentiID.Contains(r.NTrattamento))
                                                                   select new VM_OrtodonticaRefertazione
                                                                   {
                                                                       DescrizioneDato = d.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Descrizione,
                                                                       Valore = d.Valore,
                                                                       Osservazioni = d.Osservazioni
                                                                   }).ToList();

                    consensoOrtodontico.DurataMesi = durataTerapia + " Mesi";
                    consensoOrtodontico.PropostaTrattamento = txt_PianiTrattamento.Text;
                    
                  //  string TEMPLATE_CONSENSO_ORTODONTICO_PATH = ConfigurationManager.AppSettings["TEMPLATE_CONSENSO_ORTODONTICO_PATH"];
                    string pathTMP = ctx.T_SETUP.SingleOrDefault(s => s.Nome =="TEMPLATE_CONSENSO_ORTODONTICO_PATH") == null ? string.Empty : ctx.T_SETUP.SingleOrDefault(s => s.Nome == "TEMPLATE_CONSENSO_ORTODONTICO_PATH").Valore;
                    if (pathTMP == string.Empty)
                    {
                        MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è configurata la cartella di input per il template del file word.", "Errore generico:", MessageBoxButtons.OK);
                        return;
                    }
                    string path = pathTMP.Substring(0, pathTMP.LastIndexOf("\\"));
                    DirectoryInfo dirFolder = new DirectoryInfo(path);
                    if (!dirFolder.Exists)
                    {
                        MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è stato trovato il file template word.", "Errore generico:", MessageBoxButtons.OK);
                        return;
                    }
                    DirectoryInfo dirInput = new DirectoryInfo(pathTMP);
                    saveFileDoc.Filter = "doc (*.doc)|*.doc";
                    saveFileDoc.FileName = cognomePaziente + nomePaziente + DateTime.Today.ToString("ddMMyyyy");
                    if (saveFileDoc.ShowDialog() == DialogResult.OK)
                    {
                        CreateWordDocument(dirInput.FullName, saveFileDoc.FileName, consensoOrtodontico, datiClinici);
                    }
                }
                else
                {
                    MessageBox.Show("Impossibile generare il Consenso Ortodontico perché non è stato selezionato un trattamento valido.", "Errore generico:", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object findText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref findText,
                        ref matchCase, ref matchWholeWord,
                        ref matchWildCards, ref matchSoundLike,
                        ref nmatchAllForms, ref forward,
                        ref wrap, ref format, ref replaceWithText,
                        ref replace, ref matchKashida,
                        ref matchDiactitics, ref matchAlefHamza,
                        ref matchControl);
        }

        //Methode Create the document :
        private void CreateWordDocument(object filename, object savaAs, VM_ReportConsensoOrtodontico consensoOrtodontico, List<VM_OrtodonticaRefertazione> datiClinici)
        {
            List<int> processesbeforegen = getRunningProcesses();
            object missing = Missing.Value;
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document aDoc = null;
                if (File.Exists((string)filename))
                {
                    DateTime today = DateTime.Now;
                    object readOnly = false; //default
                    object isVisible = false;
                    wordApp.Visible = false;

                    aDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing);

                    aDoc.Activate();

                    //Find and replace:
                    this.FindAndReplace(wordApp, "<StudioDiAppartenenza>", consensoOrtodontico.NomeStudio);
                    this.FindAndReplace(wordApp, "<DataDocumento>", consensoOrtodontico.DataDocumento);
                    this.FindAndReplace(wordApp, "<Cognome>", consensoOrtodontico.Cognome);
                    this.FindAndReplace(wordApp, "<Nome>", consensoOrtodontico.Nome);
                    this.FindAndReplace(wordApp, "<LuogoNascita>", consensoOrtodontico.LuogoNascita);
                    this.FindAndReplace(wordApp, "<DataNascita>", consensoOrtodontico.DataNascita);
                    this.FindAndReplace(wordApp, "<DataEsecuzione>", consensoOrtodontico.DataEsecuzione);
                    this.FindAndReplace(wordApp, "<IOTN>", consensoOrtodontico.IOTN);
                    this.FindAndReplace(wordApp, "<NomeProcedura>", consensoOrtodontico.NomeProcedura);
                    this.FindAndReplace(wordApp, "<ElencoSottoprocedure>", consensoOrtodontico.ElencoSottoProcedura);
                    this.FindAndReplace(wordApp, "<Diagnosi>", consensoOrtodontico.Diagnosi);
                    // TODO: da create una table in word
                    this.FindAndReplace(wordApp, "<ElencoDatiClinici>", datiClinici.ToString());
                    this.FindAndReplace(wordApp, "<DurataMesi>", consensoOrtodontico.DurataMesi);
                    this.FindAndReplace(wordApp, "<PropostaTrattamento>", consensoOrtodontico.PropostaTrattamento);


                    Table tbl = aDoc.Tables[1];
                    tbl.Rows[1].Cells[1].Range.Text = "Dato clinico";
                    tbl.Rows[1].Cells[1].Range.Font.Bold = 1;
                    tbl.Rows[1].Cells[2].Range.Text = "Valore";
                    tbl.Rows[1].Cells[2].Range.Font.Bold = 1;
                    tbl.Rows[1].Cells[3].Range.Text = "Osservazioni";
                    tbl.Rows[1].Cells[3].Range.Font.Bold = 1;

                    // Insert document properties into cells. 
                    int i = 2;
                    foreach (var dato in datiClinici)
                    {
                        tbl.Rows.Add(ref missing);
                        tbl.Rows[i].Cells[1].Range.Text = dato.DescrizioneDato;
                        tbl.Rows[i].Cells[1].Range.Font.Bold = 0;
                        tbl.Rows[i].Cells[2].Range.Text = dato.Valore;
                        tbl.Rows[i].Cells[2].Range.Font.Bold = 0;
                        tbl.Rows[i].Cells[3].Range.Text = dato.Osservazioni;
                        tbl.Rows[i].Cells[3].Range.Font.Bold = 0;
                        i++;
                    }

                    //Save as: filename
                    aDoc.SaveAs2(ref savaAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);

                    MessageBox.Show("File generato con successo.");
                }
                else
                {
                    MessageBox.Show("file dose not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
            //Close Document:
            //aDoc.Close(ref missing, ref missing, ref missing);

            List<int> processesaftergen = getRunningProcesses();
            killProcesses(processesbeforegen, processesaftergen);

        }
        
        private void btn_template_sfoglia_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                editBox_template_valore.Text = openFileDialog1.FileName;
            }
        }
        
        private void gridEX_trattamentiDiagnostici_Click(object sender, EventArgs e)
        {
            // parent VM_NoteCliniche
            // child VM_Refertazione
            GridEX curGrid = (GridEX)sender;
            GridEXTable curTable = curGrid.CurrentTable;
            string nameTable = curTable.Caption;
            if(nameTable == "VM_NoteCliniche")
            {
                //inserisci dato clinico
                contextMenuStrip_daticlinici_modifica.Items[0].Visible = true;
                //nascondiamo modifica
                contextMenuStrip_daticlinici_modifica.Items[1].Visible = false;
            }
            else if(nameTable == "VM_OrtodonticaRefertazione")
            {
                //modifica dato clinico
                contextMenuStrip_daticlinici_modifica.Items[1].Visible = true;
                // nascondiamo inserisci
                contextMenuStrip_daticlinici_modifica.Items[0].Visible = false;
            }
            gridEX_trattamentiDiagnostici.ContextMenuStrip = contextMenuStrip_daticlinici_modifica;
        }

        private void nuovoDatoClinicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newTrattamentoRefertazione();
        }

        private void modificaDatoclinicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editTrattamentoRefertazione();
        }

        private void btn_EliminaDatiClinici_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("Sei sicuro di voler eliminare i dati clinici selezionati? Una volta confermata l'operazione non sarà possibile tornare indietro.",
                                    "Elimina",
                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    List<string> errors = new List<string>();
                    // elimina Dati Clinici
                    var rows = gridEX_DatiClinici.GetCheckedRows();
                    foreach (var row in rows)
                    {
                        string id = row.Cells[1].Value.ToString();
                        var ris = ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Single(o => o.CodDato == id);
                        // non ci devono essere trattamenti associati
                        if (ctx.T_CARTELLA_ORTODONTICA_REFERTAZIONE.Count(t => t.CodDato == id) == 0)
                            ctx.T_CARTELLA_ORTODONTICA_DATI_CLINICI.Remove(ris);
                        else
                        {
                            errors.Add(ris.Descrizione);
                        }
                    }
                    ctx.SaveChanges();
                    LoadDatiClinici();
                    if (errors.Count > 0)
                    {
                        string msg_errors = string.Join(",", errors);
                        throw new Exception("Non è possibile eliminare i seguenti dati clinici perché ci sono trattamenti associati: " + msg_errors);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((ex.InnerException == null ? ex.Message : ex.InnerException.Message), "Errore generico:", MessageBoxButtons.OK);
            }
        }

        private void btn_templateWord_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DisableTab();
            this.uiTabPage_TemplateDoc.Enabled = true;
            this.uiTab.SelectedTab = this.uiTabPage_TemplateDoc;
            DisableButton();
            this.button_save.Enabled = true;
            this.button_abort.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void bt_GeneraConsensoInformato_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            GeneraConsensoOrtodontico();
        }

        public List<int> getRunningProcesses()
        {
            List<int> ProcessIDs = new List<int>();
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (Process.GetCurrentProcess().Id == clsProcess.Id)
                    continue;
                if (clsProcess.ProcessName.Contains("WINWORD"))
                {
                    ProcessIDs.Add(clsProcess.Id);
                }
            }
            return ProcessIDs;
        }

        private void killProcesses(List<int> processesbeforegen, List<int> processesaftergen)
        {
            foreach (int pidafter in processesaftergen)
            {
                bool processfound = false;
                foreach (int pidbefore in processesbeforegen)
                {
                    if (pidafter == pidbefore)
                    {
                        processfound = true;
                    }
                }

                if (processfound == false)
                {
                    Process clsProcess = Process.GetProcessById(pidafter);
                    clsProcess.Kill();
                }
            }
        }
    }
}
