namespace Quidjibo.Windows.Service.Sample
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sampleServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.sampleServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // sampleServiceProcessInstaller
            // 
            this.sampleServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.sampleServiceProcessInstaller.Password = null;
            this.sampleServiceProcessInstaller.Username = null;
            this.sampleServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // sampleServiceInstaller
            // 
            this.sampleServiceInstaller.Description = "Quidjibo Sample Service";
            this.sampleServiceInstaller.DisplayName = "Quidjibo";
            this.sampleServiceInstaller.ServiceName = "SampleService";
            this.sampleServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.sampleServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.sampleServiceProcessInstaller,
            this.sampleServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller sampleServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller sampleServiceInstaller;
    }
}