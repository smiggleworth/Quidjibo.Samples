﻿using System.ComponentModel;
using System.Configuration.Install;

namespace Quidjibo.Windows.Service.Sample
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e) {}

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e) {}
    }
}