using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    class UserViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        DBContext dbContext;
        public UserViewModel() {
            LoadProductsData();
        }

        public void LoadProductsData()
        {
            using (var dbContext = new DBContext())
            {
                var products = dbContext.Products.ToList();

                // Thêm "Images/" vào đường dẫn Picture
                foreach (var product in products)
                {
                    product.Picture = $"pack://application:,,,/Images/{product.Picture}";
                    ////Get Price
                    //var proItem = dbContext.ProductItems.Where(p => p.ProductId == product.Id).Order ToList();
                    //product.ProductItems = new ObservableCollection<ProductItem>(proItem);
                }
                
                Products = new ObservableCollection<Product>(products);
                OnPropertyChanged(nameof(Products));
            }
        }

    }
}
