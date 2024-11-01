using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        DBContext dbContext;
        public string UserId { get; }
        public UserViewModel(string userId)
        {
            this.UserId = userId;
            LoadProductsData();
        }

        bool isProAvailable(string proId)
        {
            using (dbContext = new DBContext())
            {
                var totalQuantity = (from p in dbContext.Products
                                     join pi in dbContext.ProductItems on p.Id equals pi.ProductId
                                     where p.Id == proId
                                     select pi.Quantity).Sum();

                if (totalQuantity > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void LoadProductsData()
        {
            using (var dbContext = new DBContext())
            {
                var products = dbContext.Products.ToList();
                Products = new ObservableCollection<Product>();
                // Thêm "Images/" vào đường dẫn Picture
                foreach (var product in products)
                {
                    product.VirtualPicture = $"pack://application:,,,/Images/{product.Picture}";
                    if (isProAvailable(product.Id))
                    {
                        Products.Add(product);
                    }
                }
                OnPropertyChanged(nameof(Products));
            }
        }

    }
}
