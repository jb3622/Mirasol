using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Disney.iDash.Shared;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace Disney.iDash.iDashController
{
    public partial class BrowserControl : DevExpress.XtraEditors.XtraUserControl
    {
        private DataSources.DataSourceBase _dataSource = null;

        public BrowserControl()
        {
            InitializeComponent();
        }

        public DataSources.DataSourceBase DataSource
        {
            get { return _dataSource; }
        }

        public string Caption
        {
            get { return viewItems.ViewCaption; }
            set
            {
                viewItems.ViewCaption = value;
                viewItems.OptionsView.ShowViewCaption = value != string.Empty;
            }
        }

        public void Setup(DataSources.DataSourceBase dataSource)
        {
            _dataSource = dataSource;
            if (_dataSource.CanAdd)
                viewItems.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            else
                viewItems.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            bindingSource.AllowNew = _dataSource.CanAdd;

            if (_dataSource.CanDelete)
                viewItems.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            else
                viewItems.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            viewItems.OptionsSelection.MultiSelect = _dataSource.CanDelete;

            viewItems.OptionsBehavior.Editable = _dataSource.CanEdit;
        }

        public void RefreshGrid()
        {
            bindingSource.DataSource = null;
            if (_dataSource.Refresh())
            {
                bindingSource.DataSource= _dataSource.DataSource;
                _dataSource.FormatColumns(viewItems);
                viewItems.OptionsView.ColumnAutoWidth = false;
                viewItems.BestFitColumns();
                btnDelete.Enabled = _dataSource.CanDelete && viewItems.RowCount > 0;
                btnSave.Enabled = false;
            }
        }
       
        private void viewItems_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            btnSave.Enabled = _dataSource.IsDirty;
        }

        private void viewItems_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle == -1 || e.FocusedRowHandle == GridControl.InvalidRowHandle || e.FocusedRowHandle == GridControl.AutoFilterRowHandle || e.FocusedRowHandle == GridControl.NewItemRowHandle)
                btnDelete.Enabled = false;
            else
                btnDelete.Enabled = _dataSource.CanDelete;
        }

        private void viewItems_RowCountChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = _dataSource.IsDirty;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            viewItems.CloseEditor();
            viewItems.UpdateCurrentRow();

            if (_dataSource.Save())
                RefreshGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ArrayList rows = new ArrayList();

            if (Question.YesNo("Mark/Unmark selected rows as deleted?", this.Text))
            {
                for (int i = 0; i < viewItems.SelectedRowsCount; i++)
                {
                    if (viewItems.GetSelectedRows()[i] >= 0)
                    {
                        rows.Add(viewItems.GetDataRow(viewItems.GetSelectedRows()[i]));
                    }
                }

                try
                {
                    viewItems.BeginUpdate();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataRow row = rows[i] as DataRow;
                        if ((bool)row["DELFLAG"] == true)
                        {
                            row["DELFLAG"] = false;
                        }
                        else
                        {
                            row["DELFLAG"] = true;
                        }
                    }
                }
                finally
                {
                    viewItems.EndUpdate();
                }

                _dataSource.IsDirty = true;
                btnSave.Enabled = true;
                viewItems.ClearSelection();
                gridItems.RefreshDataSource();
            }
        }

        private void viewItems_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            var row = viewItems.GetDataRow(e.RowHandle);

            if (row != null)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        if ((bool)row[_dataSource.colDelFlag])
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
                        }
                        else
                        {
                            e.Appearance.ForeColor = Color.Blue;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                        }
                        break;

                    case DataRowState.Modified:
                        if (_dataSource.CanDelete && (bool)row[_dataSource.colDelFlag])
                        {
                            e.Appearance.ForeColor = Color.Red;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Strikeout);
                        }
                        else
                        {
                            e.Appearance.ForeColor = Color.Green;
                            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                        }
                        break;

                    default:
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                        break;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var refresh = false;
            if (_dataSource.IsDirty)
                switch (Question.YesNoCancel("Changes have been made but not saved.  Save them?", "Refresh"))
                {
                    case DialogResult.Yes:
                        refresh = _dataSource.Save();
                        break;

                    case DialogResult.No:
                        refresh = true;
                        break;

                    case DialogResult.Cancel:
                        break;
                }
            else
                refresh = true;

            if (refresh)
                RefreshGrid();
        }

        private void viewItems_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void viewItems_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            _dataSource.IsDirty = true;
            btnSave.Enabled = true;
        }
    }
}
