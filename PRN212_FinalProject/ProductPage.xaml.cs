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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PRN212_FinalProject
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
        }
        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected product from the button's DataContext (which is bound to the current row's product)
            var product = (Product)((Button)sender).DataContext;

            // Navigate to the ProductDetailPage, passing the selected product
            NavigationService.Navigate(new ProductItemPage());
        }
    }
}
