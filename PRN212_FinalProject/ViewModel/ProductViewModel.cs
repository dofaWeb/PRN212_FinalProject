using Microsoft.EntityFrameworkCore;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ObservableCollection<Entities.Product> Products { get; set; }
        Entities.DBContext db;

        public ProductViewModel()
        {

            db = new DBContext();
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            EditProductCommand = new RelayCommand(UpdateProduct);
            LoadProducts();
        }
        private string _IdInfo;
        public string IdInfo
        {
            get { return _IdInfo; }
            set
            {
                _IdInfo = value;
                OnPropertyChanged(nameof(IdInfo));
            }
        }

        private string _NameInfo;
        public string NameInfo
        {
            get { return _NameInfo; }
            set
            {
                _NameInfo = value;
                OnPropertyChanged(nameof(NameInfo));
            }
        }

        private string _DescriptionInfo;
        public string DescriptionInfo
        {
            get { return _DescriptionInfo; }
            set
            {
                _DescriptionInfo = value;
                OnPropertyChanged(nameof(DescriptionInfo));
            }
        }

        private string _categoryInfo;
        public string CategoryInfo
        {
            get { return _categoryInfo; }
            set
            {
                _categoryInfo = value;
                OnPropertyChanged(nameof(CategoryInfo));
            }
        }

        private string _supplierInfo;
        public string SupplierInfo
        {
            get { return _supplierInfo; }
            set
            {
                _supplierInfo = value;
                OnPropertyChanged(nameof(SupplierInfo));
            }
        }
        private Product _selectItem;
        public Product SelectItem
        {
            get { return _selectItem; }
            set
            {
                // Set the selected item and check if it is not null
                _selectItem = value;
                if (_selectItem != null)
                {
                    // Update the input fields when a product is selected
                    IdInfo = _selectItem.Id;          // Assuming IdInfo is a property in your ViewModel
                    NameInfo = _selectItem.Name;
                    DescriptionInfo = _selectItem.Description;
                    CategoryInfo = _selectItem.Category?.Name; // Assuming Category is a navigation property
                    SupplierInfo = _selectItem.Supplier?.Name; // Assuming Supplier is a navigation property
                }
                OnPropertyChanged(nameof(SelectItem));
            }
        }


        private void LoadProducts()
        {

            var querry = from p in db.Products
                         join c in db.Categories on p.CategoryId equals c.Id
                         join s in db.Suppliers on p.SupplierId equals s.Id
                         select new Entities.Product
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Picture = p.Picture,
                             Description = p.Description,
                             Category = p.Category,
                             Supplier = p.Supplier,
                         };
            Products = new ObservableCollection<Entities.Product>(querry.ToList());
        }
        public string getNewProductId()
        {
            string lastId = db.Products
                     .OrderByDescending(a => a.Id)
                     .Select(a => a.Id)
                     .FirstOrDefault();
            if (lastId == null)
            {
                return "P0000001";
            }
            // Tách phần chữ (A) và phần số (0000001)
            string prefix = lastId.Substring(0, 1); // Lấy ký tự đầu tiên
            int number = int.Parse(lastId.Substring(1)); // Lấy phần số và chuyển thành số nguyên

            // Tăng số lên 1
            int newNumber = number + 1;

            // Tạo ID mới với số đã tăng, định dạng lại với 7 chữ số
            string newId = $"{prefix}{newNumber:D7}";

            return newId;
        }
        private void AddProduct(object parameter)
        {
            var newProduct = new Entities.Product
            {
                Id = getNewProductId(),
                Name = NameInfo,
                Description = DescriptionInfo,
                CategoryId = db.Categories.FirstOrDefault(c => c.Name == CategoryInfo)?.Id,
                SupplierId = db.Suppliers.FirstOrDefault(s => s.Name == SupplierInfo)?.Id
            };

            db.Products.Add(newProduct);
            db.SaveChanges();

            // Add the new product directly to the ObservableCollection
            Products.Add(newProduct);

            // Clear input fields after adding
            IdInfo = string.Empty;
            NameInfo = string.Empty;
            DescriptionInfo = string.Empty;
            CategoryInfo = string.Empty;
            SupplierInfo = string.Empty;

            OnPropertyChanged(nameof(Products));
        }

        private void DeleteProduct(object parameter)
        {
            // Find the product to delete
            var productToDelete = db.Products.FirstOrDefault(p => p.Id == IdInfo);
            if (productToDelete != null)
            {
                Products.Remove(Products.FirstOrDefault(p => p.Id == IdInfo));
                db.Products.Remove(productToDelete);

                db.SaveChanges();
                IdInfo = string.Empty;
                NameInfo = string.Empty;
                DescriptionInfo = string.Empty;
                CategoryInfo = string.Empty;
                SupplierInfo = string.Empty;

                OnPropertyChanged(nameof(Products));
                // Reload products
                LoadProducts();
            }
        }
        private void UpdateProduct(object parameter)
        {
            if (SelectItem != null)
            {
                // Find the index of the selected product in the Products collection
                int index = Products.IndexOf(SelectItem);

               
                    // Update the product in the database
                    var existingProduct = db.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefault(p => p.Id == SelectItem.Id);
                    if (existingProduct != null)
                    {
                        // Update fields
                        existingProduct.Name = NameInfo;
                        existingProduct.Description = DescriptionInfo;

                        // Set Category and Supplier
                        existingProduct.CategoryId = db.Categories.FirstOrDefault(c => c.Name == CategoryInfo)?.Id;
                        existingProduct.SupplierId = db.Suppliers.FirstOrDefault(s => s.Name == SupplierInfo)?.Id;

                        // Save changes to the database
                        db.SaveChanges();

                    // Update the product in the ObservableCollection
                    var productToUpdate = Products.FirstOrDefault(p => p.Id == existingProduct.Id);
                    if (productToUpdate != null)
                    {
                        // Update the properties of the product in the ObservableCollection
                        productToUpdate.Name = existingProduct.Name;
                        productToUpdate.Description = existingProduct.Description;
                        productToUpdate.CategoryId = existingProduct.CategoryId; // Optional: If you want to keep category in the collection
                        productToUpdate.SupplierId = existingProduct.SupplierId; // Optional: If you want to keep supplier in the collection
                    }
                    // Refresh the ObservableCollection
                    Products.Remove(SelectItem);  // Temporarily remove the item
                    Products.Add(existingProduct); // Add the updated item back
                    OnPropertyChanged(nameof(Products));
                    LoadProducts();
                }
            }
          
        }

    }
}