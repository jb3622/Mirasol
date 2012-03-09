using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Disney.iDash.LocalData;
using Disney.iDash.Shared;

namespace Disney.iDash.iDashController
{
    public partial class ControllerForm : DevExpress.XtraEditors.XtraForm
    { 

        private DataSources.DSSRSMSG _dssrsmsg = new DataSources.DSSRSMSG();
        private DataSources.DSSRUML _dssruml = new DataSources.DSSRUML();
        private DataSources.DSSRLCK _dssrlck = new DataSources.DSSRLCK();
        private DataSources.DSAILOK _dsialok = new DataSources.DSAILOK();

        public ControllerForm()
        {
            InitializeComponent();
            browserDSSRSMSG.Setup(_dssrsmsg);
            browserDSSRUML.Setup(_dssruml);
            browserDSSRLCK.Setup(_dssrlck);
            browserDSIALOK.Setup(_dsialok);
        }

        private void RefreshActiveGrid()
        {
            if (xtraTabControl1.SelectedTabPage == tpDSSRSMSG && _dssrsmsg.DataSource == null)
                browserDSSRSMSG.RefreshGrid();
            else if (xtraTabControl1.SelectedTabPage == tpDSSRLCK && _dssrlck.DataSource == null)
                browserDSSRLCK.RefreshGrid();
            else if (xtraTabControl1.SelectedTabPage == tpDSSRUML && _dssruml.DataSource == null)
                browserDSSRUML.RefreshGrid();
            else if (xtraTabControl1.SelectedTabPage == tpDSIALOK && _dsialok.DataSource == null)
                browserDSIALOK.RefreshGrid();
        }

        private void ControllerForm_Shown(object sender, EventArgs e)
        {
            RefreshActiveGrid();
            lblEnvironment.Text = Session.Environment.EnvironmentName;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            RefreshActiveGrid();
        }

        private void ControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var ctrl in new List<BrowserControl>{browserDSSRSMSG, browserDSSRLCK, browserDSSRUML, browserDSIALOK})
                if (ctrl.DataSource != null && ctrl.DataSource.IsDirty)
                    switch (Question.YesNoCancel("Changes have been made and not saved.  Save them", ctrl.Caption))
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            if (!ctrl.DataSource.Save())
                                e.Cancel = true;
                            break;

                        case System.Windows.Forms.DialogResult.No:
                            break;

                        case System.Windows.Forms.DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }

        }
    }
}