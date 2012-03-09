using System;
using System.Windows.Forms;
using DevExpress.UserSkins;
using Disney.iDash.Framework.Forms;
using Disney.iDash.Shared;
using System.Deployment.Application;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Disney.iDash.LocalData;

namespace Disney.iDash.iDashController
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Allow only one instance to run.
            var singleInstance = new SingleInstance();

            if (singleInstance.IsRunning)
                singleInstance.Activate();
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                OfficeSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.Skins.SkinManager.EnableMdiFormSkins();

                Application.ThreadException += (sender, e) =>
                {
                    ErrorDialog.Show(e.Exception, "1: Application Exception");
                };

                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                {
                    ErrorDialog.Show("Exception in: " + e.ExceptionObject.ToString(), "2: Application Exception");
                };

                CreateTempFolder();

                if (SetLocalDataLocation())
                {
                    SetSkinColour();
                    var exit = false;
                    while (!exit && ShowLogin())
                    {
                        var mainForm = new ControllerForm();

                        if (System.Diagnostics.Debugger.IsAttached)
                            Application.Run(mainForm);
                        else
                            try
                            {
                                Application.Run(mainForm);
                            }
                            catch (Exception ex)
                            {
                                ErrorDialog.Show(ex, "Unhandled exception");
                            }
                    }

                    Environment.Exit(0);
                }
                else if (Session.LastException != null)
                    ErrorDialog.Show(Session.LastException, "Startup failed");
            }

        }

        static void CreateTempFolder()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\iDash";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }

        static bool SetLocalDataLocation()
        {
            var result = false;
            var remoteDataFilename = string.Empty;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var path = ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri;
                remoteDataFilename = path.Substring(0, path.LastIndexOf('/') + 1) + Properties.Settings.Default.LocalDataFilename;
            }
            else
                remoteDataFilename = Properties.Settings.Default.LocalDataPath + Properties.Settings.Default.LocalDataFilename;

            result = Session.CopyRemoteConfigFile(remoteDataFilename) && Session.TestConnection(Session.GetLocalConfigFileName(), Properties.Settings.Default.LocalDataFileVersionRequired);

            return result;
        }

        static void SetSkinColour()
        {
            var skinColor = Disney.iDash.Shared.RegUtils.GetCustomSetting("Skin", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName);
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skinColor);
        }

        static bool ShowLogin()
        {
            var login = new Disney.iDash.iDashController.LoginDialog();
            //var login = new Disney.iDash.Framework.Forms.LoginDialog();
            return login.ShowForm();
        }
    }
}
