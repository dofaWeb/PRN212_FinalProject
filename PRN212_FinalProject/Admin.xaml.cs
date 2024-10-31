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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Content = new SupllierPage();
        }
        private void Product_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Content = new ProductPage();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Content = new CategoryPage();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainPage.Content = new ListAcc();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Content = new OrderMan();
        }
    }
}
