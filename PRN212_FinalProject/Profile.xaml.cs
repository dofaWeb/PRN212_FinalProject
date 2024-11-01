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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile(ProfileViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // 
        }
        public Profile(string customerId)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(customerId);
        }

        private void ProfileButton(object sender, RoutedEventArgs e)
        {

            MainFrame.Visibility = Visibility.Collapsed;

            ProfileTextBox.Visibility = Visibility.Visible;

        }

        private void BackButton(object sender, RoutedEventArgs e)
        {

            this.Close();

        }

        private void UserOrderHistoryButton(object sender, RoutedEventArgs e)
        {
            // Giả sử `DataContext` của User window là `UserViewModel`
            var profileViewModel = (ProfileViewModel)this.DataContext;

            // Khởi tạo ProfileViewModel với userId từ UserViewModel
            UserOrderHistoryViewModel userOrderHistoryViewModel = new UserOrderHistoryViewModel(profileViewModel.Profile);

            MainFrame.Content = new UserOrderHistoryPage(userOrderHistoryViewModel);

            MainFrame.Visibility = Visibility.Visible;

            ProfileTextBox.Visibility = Visibility.Collapsed;

        }
    }
}