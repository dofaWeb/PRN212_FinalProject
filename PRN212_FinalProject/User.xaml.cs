using PRN212_FinalProject.Entities;
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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        private readonly string userId;
        public User(UserViewModel viewModel, string userId)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.userId = userId;
        }

        private void ViewProDetail(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)sender).DataContext;
            var productId = product.Id;

            // Create view model and page for the selected product
            var productDetailViewModel = new ProductDetailViewModel(productId, userId);
            MainFrame.Content = new ProductDetailPage(productDetailViewModel);

            // Toggle visibility
            ProductsControl.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
        }

        public void ShowProductsList()
        {
            ProductsControl.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;
            MainFrame.Content = null; // Clear the frame content to remove the ProductDetailPage
        }

        private void LogoutButton(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Giả sử `DataContext` của User window là `UserViewModel`
            var userViewModel = (UserViewModel)this.DataContext;

            // Khởi tạo ProfileViewModel với userId từ UserViewModel
            ProfileViewModel profileViewModel = new ProfileViewModel(userViewModel.UserId);

            // Tạo và hiển thị Profile window với DataContext là ProfileViewModel
            Profile profileWindow = new Profile(profileViewModel);

            profileWindow.Show();
        }
    }
}