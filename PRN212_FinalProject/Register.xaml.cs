using PRN212_FinalProject.ViewModel;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            var viewModel = new RegisterViewModel();
            this.DataContext = viewModel;

            // Đặt CloseWindowAction để mở LoginWindow và đóng RegisterWindow khi đăng ký thành công
            viewModel.CloseWindowAction = () =>
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            };
        }

       
    }
}
