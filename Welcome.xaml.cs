using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using TeslaCamTheater.Exceptions;
using TeslaCamTheater.Services;
using MessageBox = System.Windows.MessageBox;

namespace TeslaCamTheater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
        }
        
        private void Browse_Click(object s, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                AutoUpgradeEnabled = true,
                Description = "Select Your TeslaCam Folder",
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = false,
                UseDescriptionForTitle = true
            };

            var w32handle = new NativeWindow();
            w32handle.AssignHandle(new WindowInteropHelper(this).Handle);

            var dialogRes = fbd.ShowDialog(w32handle);

            if (dialogRes == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    FileService.IsTeslaCamFolder(fbd.SelectedPath);

                    new Theater(fbd.SelectedPath).Show();
                    Close();
                }
                catch (ErrorMessageException em)
                {
                    MessageBox.Show(em.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
