using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace PRN212_FinalProject
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of Register window
            Register registerWindow = new Register();
            registerWindow.Show(); // Show the Register window
            this.Close(); // Optionally close the Login window

        }

       
    }
}
