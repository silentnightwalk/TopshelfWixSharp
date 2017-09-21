using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WixSharp;
using WixSharp.CommonTasks;

namespace Installer
{
    class Setup
    {
        static void Main(string[] args)
        {
            Compiler.WixLocation = @"..\packages\WixSharp.wix.bin.3.11.0\tools\bin";
            var project = new ManagedProject("TopshelfWixSharp"
                , new Dir(
                    $@"{Environment.ExpandEnvironmentVariables("%programfiles(x86)%")}\TopshelfWixSharp"
                    , new File($@"..\TopshelfWixSharp\bin\Debug\TopshelfWixSharp.exe")
                    , new File($@"..\TopshelfWixSharp\bin\Debug\Topshelf.dll")
                    )
                );
            project.BeforeInstall += Project_BeforeInstall;
            project.AfterInstall += Project_AfterInstall;
            Compiler.BuildMsi(project);
        }

        private static void Project_BeforeInstall(SetupEventArgs e)
        {
            if (e.IsUninstalling)
            {
                Tasks.StopService("TopshelfWixSharp");

                Process proc = new Process();
                proc.StartInfo.FileName = $@"{Environment.ExpandEnvironmentVariables("%programfiles(x86)%")}\TopshelfWixSharp\TopshelfWixSharp.exe";
                proc.StartInfo.Arguments = "uninstall";
                proc.StartInfo.Verb = "runas";
                proc.Start();
            }
        }

        private static void Project_AfterInstall(SetupEventArgs e)
        {
            if (e.IsInstalling)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = $@"{Environment.ExpandEnvironmentVariables("%programfiles(x86)%")}\TopshelfWixSharp\TopshelfWixSharp.exe";
                proc.StartInfo.Arguments = "install start";
                proc.StartInfo.Verb = "runas";
                proc.Start();
            }
        }
    }
}
