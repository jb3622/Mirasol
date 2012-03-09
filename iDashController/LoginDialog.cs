using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Disney.iDash.Framework.Forms;
using System.Deployment.Application;
using System.Reflection;

namespace Disney.iDash.iDashController
{
    public partial class LoginDialog : Disney.iDash.Framework.Forms.LoginDialog
    {
        const int VERSION_PROD = 1;        
        const int VERSION_QA = 2;
        const int VERSION_ITG = 3;
        Version version;

        public LoginDialog()
        {
            InitializeComponent();
        }

        protected override void ShowVersionNumber()
        {
            base.ShowVersionNumber();

            if (ApplicationDeployment.IsNetworkDeployed)
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            else
                version = Assembly.GetEntryAssembly().GetName().Version;

            if (version.Build == VERSION_QA)
            {
                base.LoginStatusMessage.Text = LoginStatusMessage.Text + " [" + "QA" + "]";
            }

            if (version.Build == VERSION_ITG)
            {
                base.LoginStatusMessage.Text = LoginStatusMessage.Text + " [" + "ITG" + "]";
            }
        }

        public override bool ShowForm()
        {
            base.Text = "Login - iDashController";

            bool result = base.ShowForm();

            return (result);
        }

        protected override void btnLogin_Click(object sender, EventArgs e)
        {
            string sql;
            bool loginOK = false;
            string msg = "";
            string msg2 = "";

            base._factory.OpenConnection();
            sql = "Select * from DSSRADM where UserID ='" + txtNetworkId.Text.ToString().ToUpper() + "'";
            DataTable userTable = base._factory.CreateTable(sql);

            if (userTable.Rows.Count > 0)
            {
                if (base.cboEnvironment.Text.Contains("(Production") && version.Build == VERSION_PROD)
                {
                    loginOK = true;
                }
                else if (base.cboEnvironment.Text.Contains("(QA") && version.Build == VERSION_QA)
                {
                    loginOK = true;
                }
                else if (base.cboEnvironment.Text.Contains("(Integration") && version.Build == VERSION_ITG)
                {
                    loginOK = true;
                }
                else
                {
                    msg = "This user is not authorised to access this environment.\n";
                    if (version.Build == VERSION_QA)
                    {
                        msg2 = "Please try logging onto the QA system";
                    }
                    if (version.Build == VERSION_ITG)
                    {
                        msg2 = "Please try logging onto the ITG system.";
                    }
                    msg += msg2;
                }
            }
            else
            {
                msg = "This user is not authorised to use this application.";
            }

            if (loginOK == true)
            {
                base.btnLogin_Click(sender, e);
            }
            else
            {
                MessageBox.Show(msg, "Login - iDashController", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    
    
    
    
    
    }


}
