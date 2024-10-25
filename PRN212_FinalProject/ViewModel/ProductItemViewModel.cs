using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    internal class ProductItemViewModel : BaseViewModel
    {
        public ObservableCollection<Entities.ProductItem> ProductItems { get; set; }

        private DBContext db;

        public ICommand AddProductItemCommand { get; }
        public ICommand EditProductItemCommand { get; }
        public ICommand DeleteProductItemCommand { get; }

        public ProductItemViewModel() { 
            db = new DBContext();
            AddProductItemCommand = new RelayCommand(AddProductItem);
            EditProductItemCommand = new RelayCommand(EditProductItem);
            DeleteProductItemCommand = new RelayCommand(DeleteProductItem);
        }

        public void LoadProductItem()
        {
            
            ProductItems = new ObservableCollection<Entities.ProductItem>()
            {
                
            };
        }

        public void AddProductItem(object paramenter)
        {

        }

        public void EditProductItem(object paramenter)
        {

        }

        public void DeleteProductItem(object paramenter)
        {

        }
    }
}
