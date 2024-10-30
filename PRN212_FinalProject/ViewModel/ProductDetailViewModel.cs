using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    public class ProductDetailViewModel : BaseViewModel
    {
        public ObservableCollection<ProductItem> ProductItems { get; set; }
        public Product Product { get; set; }

        public ProductItem ProductItemSelected { get; set; }

        DBContext dbContext;
        public ProductDetailViewModel(string productId)
        {
            LoadData(productId);
        }

        void LoadData(string productId)
        {
            using (dbContext = new DBContext())
            {
                Product = dbContext.Products.Where(p=> p.Id == productId).FirstOrDefault();
                Product.Picture = $"pack://application:,,,/Images/{Product.Picture}";
                OnPropertyChanged(nameof(Product));
                ProductItems = new ObservableCollection<ProductItem>(dbContext.ProductItems.Where(p=>p.ProductId==productId).ToList());
            }
        }
    }
}
