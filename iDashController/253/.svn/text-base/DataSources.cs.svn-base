using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Disney.iDash.DataLayer;
using Disney.iDash.Shared;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using System.Data.OleDb;
using Disney.iDash.LocalData;

namespace Disney.iDash.iDashController
{
    public class DataSources
    {
        public abstract class DataSourceBase
        {
            private DataTable _data = null;

            public bool CanAdd { get; set; }
            public bool CanEdit { get; set; }
            public bool CanDelete { get; set; }
            public bool IsDirty { get; set; }
            public string colDelFlag { get; internal set; }

            public ExceptionHandler ExceptionHandler = new ExceptionHandler();

            internal DB2Factory Factory {get; private set;}

            public DataSourceBase()
            {
                Factory = new DB2Factory();
            
                Factory.ExceptionHandler.ExceptionEvent += ((ex, extraInfo, terminateApplication) =>
                {
                    ExceptionHandler.RaiseException(ex, extraInfo, terminateApplication);
                });

                CanAdd = false;
                CanEdit = false;
                CanDelete = false;
                colDelFlag = "DELFLAG";
            }

            public abstract bool Refresh();      
            public abstract bool Save();

            public virtual void FormatColumns(GridView view)
            {
                var col = view.Columns.ColumnByFieldName(colDelFlag);

                if (col != null)
                {
                    col.Visible = false;
                    col.OptionsColumn.ShowInCustomizationForm = false;
                }
            }

            public bool SetDeletedFlag(int[] selectedRows)
            {
                var result = false;
                try
                {
                    foreach (var rowHandle in selectedRows)
                    {
                        var row = DataSource.Rows[rowHandle];
                        if (row != null)
                        {
                            if ((bool)row[colDelFlag])
                                row[colDelFlag] = false;
                            else
                                row[colDelFlag] = true;

                            IsDirty = true;
                        }
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.RaiseException(ex, "SetDeletedFlag");
                }
                return result;
            }

            public DataTable DataSource
            {
                get { return _data; }
                internal set 
                { 
                    _data = value;
                    if (_data != null)
                    {
                        _data.RowChanged += ((sender, e) =>
                            {
                                IsDirty = true;
                            });
                        _data.RowDeleted += ((sender, e) =>
                            {
                                IsDirty = true;
                            });
                    }

                }
            }

            public void AddDelFlagColumn()
            {
                if (DataSource != null)
                {
                    var colDelete = new DataColumn(colDelFlag, typeof(bool));
                    colDelete.AllowDBNull = true;
                    colDelete.DefaultValue = false;
                    DataSource.Columns.Add(colDelete);
                }
            }
        }

        public class DSSRSMSG : DataSourceBase
        {
            public const string colKey = "MGKEY";
            public const string colSystem = "MGSYSN";
            public const string colUser = "MGUSER";
            public const string colStartTime = "MGSTRT";
            public const string colEndTime = "MGENDT";
            public const string colMon = "MGMON";
            public const string colTue = "MGTUE";
            public const string colWed = "MGWED";
            public const string colThu = "MGTHU";
            public const string colFri = "MGFRI";
            public const string colSat = "MGSAT";
            public const string colSun = "MGSUN";
            public const string colMessage = "MGMSG";
            public const string colStatus = "MGSTS";
            public const string colTerminate = "MGTRM";

            public DSSRSMSG()
            {
                CanAdd = true;
                CanEdit = true;
                CanDelete = true;
            }

            public override bool Refresh()
            {
                var refreshed = false;
                if (Factory.OpenConnection())
                    try
                    {
                        DataSource = Factory.CreateTable(Properties.Resources.SQL_DSSRSMSG_Select);

                        if (DataSource != null)
                        {
                            base.AddDelFlagColumn();

                            DataSource.TableNewRow += ((sender, e) =>
                                {
                                    e.Row[colKey] = DataSource.Rows.Count + 1;
                                    e.Row[colSystem] = "SRR";
                                    e.Row[colUser] = "*ALL";
                                    e.Row[colStartTime] = 0;
                                    e.Row[colEndTime] = 0;
                                    e.Row[colMon] = 1;
                                    e.Row[colTue] = 1;
                                    e.Row[colWed] = 1;
                                    e.Row[colThu] = 1;
                                    e.Row[colFri] = 1;
                                    e.Row[colSat] = 1;
                                    e.Row[colSun] = 1;
                                    e.Row[colMessage] = "Please enter a message";
                                    e.Row[colStatus] = 1;
                                    e.Row[colTerminate] = 0;
                                });
                        }
                        refreshed = true;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Refresh");
                    }
                    finally
                    {
                        Factory.CloseConnection();
                    }
                return refreshed;
            }

            public override bool Save()
            {
                var saved = false;

                if (base.IsDirty && Factory.OpenConnection())
                {
                    var sqlInsertUpdate = new Dictionary<string, object>
                    {
                        {colEndTime, 0},
                        {colFri, 0},
                        {colKey, 0},
                        {colMessage, string.Empty},
                        {colMon, 0},
                        {colSat, 0},
                        {colStartTime, 0},
                        {colStatus, 0},
                        {colSun, 0},
                        {colSystem, string.Empty},
                        {colTerminate, 0},
                        {colThu, 0},
                        {colTue, 0},
                        {colUser, 0},
                        {colWed, 0}                     
                        
                    };

                    try
                    {
                        foreach (DataRow row in DataSource.Rows)
                        {
                            row.EndEdit();
                            switch (row.RowState)
                            {
                                case DataRowState.Added:
                                    {
                                        foreach (var key in sqlInsertUpdate.Keys.ToList())
                                            sqlInsertUpdate[key] = row[key].ToString().Replace("'", "''");
                                        
                                        var cmd = Factory.CreateCommand(Factory.ReplaceTokens(Properties.Resources.SQL_DSSRSMSG_Insert, sqlInsertUpdate, "<", ">"));
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;

                                case DataRowState.Modified:
                                    {
                                        OleDbCommand cmd = null;
                                        if ((bool)row[colDelFlag])
                                            cmd = Factory.CreateCommand(Properties.Resources.SQL_DSSRSMSG_Delete.Replace("<" + colKey + ">", row[colKey].ToString()));
                                        else
                                        {
                                            foreach (var key in sqlInsertUpdate.Keys.ToList())
                                                sqlInsertUpdate[key] = row[key].ToString().Replace("'", "''");

                                            cmd = Factory.CreateCommand(Factory.ReplaceTokens(Properties.Resources.SQL_DSSRSMSG_Update, sqlInsertUpdate, "<", ">"));
                                        }
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }
                        }

                        saved = true;
                        base.IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Save");
                     }

                }
                return saved;
            }

            public override void FormatColumns(GridView view)
            {
                var checkBoxEditor = new RepositoryItemCheckEdit();
                              
                checkBoxEditor.ValueChecked = "1";
                checkBoxEditor.ValueUnchecked = "0";

                foreach (GridColumn col in view.Columns)
                    switch (col.FieldName)
                    {
                        case colKey:
                            col.Caption = "Key";
                            col.VisibleIndex = 0;
                            break;

                        case colSystem:
                            col.Caption = "System";
                            col.VisibleIndex = 1;
                            break;

                        case colUser:
                            col.Caption = "User";
                            col.VisibleIndex = 2;
                            break;

                        case colMessage:
                            col.Caption = "Message";
                            col.VisibleIndex = 3;
                            break;

                        case colStatus:
                            col.Caption = "Active";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 4;
                            break;

                        case colStartTime:
                            col.Caption = "Start Time";
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.VisibleIndex = 5;
                            break;

                        case colEndTime:
                            col.Caption = "End Time";
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.VisibleIndex = 6;
                            break;

                        case colMon:
                            col.Caption = "Mon";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 7;
                            break;

                        case colTue:
                            col.Caption = "Tue";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 8;
                            break;

                        case colWed:
                            col.Caption = "Wed";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 9;
                            break;

                        case colThu:
                            col.Caption = "Thu";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 10;
                            break;

                        case colFri:
                            col.Caption = "Fri";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 11;
                            break;

                        case colSat:
                            col.Caption = "Sat";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 12;
                            break;

                        case colSun:
                            col.Caption = "Sun";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 13;
                            break;

                        case colTerminate:
                            col.Caption = "Terminate";
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 14;
                            break;
                    }

                base.FormatColumns(view);
            }            
        }

        public class DSSRUML : DataSourceBase
        {

            public const string colUMSMNA = "UMSMNA";
            public const string colUMSMAS = "UMSMAS";
            public const string colUMSFLG = "UMSFLG";
            public const string colUMSLID = "UMSLID";
            public const string colUMSLDT = "UMSLDT";
            public const string colUMSUID = "UMSUID";
            public const string colUMSUDT = "UMSUDT";

            public DSSRUML()
            {
                CanAdd = false;
                CanEdit = true;
                CanDelete = false;
            }

            public override bool Refresh()
            {
                var refreshed = false;
                if (Factory.OpenConnection())
                    try
                    {
                        DataSource = Factory.CreateTable(Properties.Resources.SQL_DSSRUML_Select);
                        refreshed = true;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Refresh");
                    }
                    finally
                    {
                        Factory.CloseConnection();
                    }
                return refreshed;
            }

            public override bool Save()
            {
                var saved = false;

                if (base.IsDirty && Factory.OpenConnection())
                {
                    try
                    {
                        foreach (DataRow row in DataSource.Rows)
                        {
                            row.EndEdit();
                            switch (row.RowState)
                            {
                                case DataRowState.Modified:
                                    {
                                        var cmd = Factory.CreateCommand(Properties.Resources.SQL_DSSRUML_Update
                                            .Replace("<" + colUMSUID + ">", Session.User.Fullname + " (" + Session.User.NetworkId + ")")
                                            .Replace("<" + colUMSUDT + ">", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
                                            .Replace("<" + colUMSFLG + ">", row[colUMSFLG].ToString())
                                            .Replace("<" + colUMSMNA + ">", row[colUMSMNA].ToString())
                                            .Replace("<" + colUMSMAS + ">", row[colUMSMAS].ToString()));
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }
                        }

                        saved = true;
                        base.IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Save");
                    }

                }
                return saved;
            }

            public override void FormatColumns(GridView view)
            {
                var checkBoxEditor = new RepositoryItemCheckEdit();

                checkBoxEditor.ValueChecked = "L";
                checkBoxEditor.ValueUnchecked = "U";

                foreach (GridColumn col in view.Columns)
                {
                    col.OptionsColumn.AllowEdit = false;
                    switch (col.FieldName)
                    {
                        case colUMSMNA:
                            col.Caption = "System";
                            col.VisibleIndex = 0;
                            break;

                        case colUMSMAS:
                            col.Caption = "Area";
                            col.VisibleIndex = 1;
                            break;

                        case colUMSFLG:
                            col.Caption = "Locked";
                            col.OptionsColumn.AllowEdit = true;
                            col.ColumnEdit = checkBoxEditor;
                            col.VisibleIndex = 2;
                            break;

                        case colUMSLID:
                            col.Caption = "Created By";
                            col.VisibleIndex = 3;
                            break;

                        case colUMSLDT:
                            col.Caption = "Created";
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            col.DisplayFormat.FormatString = "dd-MMM-yyyy HH:mm";
                            col.VisibleIndex = 4;
                            break;

                        case colUMSUID:
                            col.Caption = "Updated By";
                            col.VisibleIndex = 5;
                            break;

                        case colUMSUDT:
                            col.Caption = "End Time";
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            col.DisplayFormat.FormatString = "dd-MMM-yyyy HH:mm";
                            col.VisibleIndex = 6;
                            break;

                    }
                }
                base.FormatColumns(view);
            }            

        }

        public class DSAILOK : DataSourceBase
        {
            public const string colIACLS = "IACLS";
            public const string colIAVEN = "IAVEN";
            public const string colIASTY = "IASTY";
            public const string colIACLR = "IACLR";
            public const string colIASIZ = "IASIZ";
            public const string colIAUSER = "IAUSER";
            public const string colIAFGP = "IAFGP";
            public const string colIAJOB = "IAJOB";
            public const string colIADTTM = "IADTTM";

            public DSAILOK()
            {
                CanAdd = false;
                CanEdit = false;
                CanDelete = true;
            }

            public override bool Refresh()
            {
                var refreshed = false;
                if (Factory.OpenConnection())
                    try
                    {
                        DataSource = Factory.CreateTable(Properties.Resources.SQL_DSIALOK_Select);
                        base.AddDelFlagColumn();
                        refreshed = true;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Refresh");
                    }
                    finally
                    {
                        Factory.CloseConnection();
                    }
                return refreshed;
            }

            public override bool Save()
            {
                var saved = false;
                if (base.IsDirty && Factory.OpenConnection())
                {
                    try
                    {
                        foreach (DataRow row in DataSource.Rows)
                        {
                            row.EndEdit();
                            switch (row.RowState)
                            {
                                case DataRowState.Modified:
                                    if ((bool)row[colDelFlag])
                                    {
                                        var cmd = Factory.CreateCommand(Properties.Resources.SQL_DSIALOK_Delete
                                            .Replace("<" + colIACLS + ">", row[colIACLS].ToString())
                                            .Replace("<" + colIAVEN + ">", row[colIAVEN].ToString())
                                            .Replace("<" + colIASTY + ">", row[colIASTY].ToString())
                                            .Replace("<" + colIACLR + ">", row[colIACLR].ToString())
                                            .Replace("<" + colIASIZ + ">", row[colIASIZ].ToString())
                                            .Replace("<" + colIAUSER + ">", row[colIAUSER].ToString()));

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }
                        }

                        saved = true;
                        base.IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Save");
                    }
                }

                return saved;
            }

        }

        public class DSSRLCK : DataSourceBase
        {
            public const string colLOKEY = "LOKEY";
            public const string colLODEPT = "LODEPT";
            public const string colLOCLS = "LOCLS";
            public const string colLOVEN = "LOVEN";
            public const string colLOSTY = "LOSTY";
            public const string colLOSTR = "LOSTR";
            public const string colLOUSR = "LOUSR";
            public const string colLOMBR = "LOMBR";
            public const string colLOTYP = "LOTYP";
            public const string colLOMOD = "LOMOD";
            public const string colLODAT = "LODAT";
            public const string colLOGUID = "LOGUID";

            public DSSRLCK()
            {
                CanAdd = false;
                CanEdit = false;
                CanDelete = true;
            }

            public override bool Refresh()
            {
                var refreshed = false;
                if (Factory.OpenConnection())
                    try
                    {
                        DataSource = Factory.CreateTable(Properties.Resources.SQL_DSSRLCK_Select);
                        base.AddDelFlagColumn();
                        refreshed = true;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Refresh");
                    }
                    finally
                    {
                        Factory.CloseConnection();
                    }
                return refreshed;
            }

            public override bool Save()
            {
                var saved = false;
                if (base.IsDirty && Factory.OpenConnection())
                {
                    try
                    {
                        foreach (DataRow row in DataSource.Rows)
                        {
                            row.EndEdit();
                            switch (row.RowState)
                            {
                                case DataRowState.Modified:
                                    if ((bool)row[colDelFlag])
                                    {                                       
                                        var sql = Properties.Resources.SQL_DSSRLCK_Delete;
                                        foreach (DataColumn col in DataSource.Columns)
                                            sql = sql.Replace("<" + col.ColumnName + ">", row[col].ToString());
                                        var cmd = Factory.CreateCommand(sql);                                      
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }
                        }

                        saved = true;
                        base.IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.RaiseException(ex, "Save");
                    }
                }

                return saved;
            }

        }
     }
}
