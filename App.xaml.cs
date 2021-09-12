using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

namespace TeslaCamTheater
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (_, ex) =>
            {
                MessageBox.Show($"Unhandled app exception.\r\n\r\n{ex.Exception.GetType().FullName}: {ex.Exception.Message}\r\n\r\nThe application will now quit.");
                Environment.Exit(-1);
            };
            base.OnStartup(e);
        }
    }
}
