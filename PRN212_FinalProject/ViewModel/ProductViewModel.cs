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
        public ObservableCollection<Category> CategoryList { get; set; }
        public ObservableCollection<Supplier> SupplierList { get; set; }
        private Entities.DBContext db;

        public ProductViewModel()
        {
            db = new DBContext();
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            EditProductCommand = new RelayCommand(UpdateProduct);
            LoadProducts();
            LoadCategory();
            LoadSupplier();
        }

        public void LoadCategory()
        {
            var query = db.Categories.ToList();
            CategoryList = new ObservableCollection<Category>(query);
        }

        public void LoadSupplier()
        {
            var query = db.Suppliers.ToList();
            SupplierList = new ObservableCollection<Supplier>(query);
        }

        private string _IdInfo;
        public string IdInfo
        {
            get { return _IdInfo; }
            set { _IdInfo = value; OnPropertyChanged(nameof(IdInfo)); }
        }

        private string _NameInfo;
        public string NameInfo
        {
            get { return _NameInfo; }
            set { _NameInfo = value; OnPropertyChanged(nameof(NameInfo)); }
        }

        private string _DescriptionInfo;
        public string DescriptionInfo
        {
            get { return _DescriptionInfo; }
            set { _DescriptionInfo = value; OnPropertyChanged(nameof(DescriptionInfo)); }
        }

        private Category _categoryInfo;
        public Category CategoryInfo
        {
            get { return _categoryInfo; }
            set { _categoryInfo = value; OnPropertyChanged(nameof(CategoryInfo)); }
        }

        private Supplier _supplierInfo;
        public Supplier SupplierInfo
        {
            get { return _supplierInfo; }
            set
            {
                _supplierInfo = value; OnPropertyChanged(nameof(SupplierInfo));
            }
        }

        private string _pictureInfo;
        public string PictureInfo
        {
            get { return _pictureInfo; }
            set
            {
                _pictureInfo = value;
                OnPropertyChanged(nameof(PictureInfo));
                OnPropertyChanged(nameof(PictureFilePath)); // Notify when PictureInfo changes
            }
        }

        public string PictureFilePath
        {
            get
            {
                string basePath = "/img/"; // Adjust path to match your project structure
                return Path.Combine(basePath, PictureInfo); // Combines folder with filename
            }
        }

        private Product _selectItem;
        public Product SelectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                if (_selectItem != null)
                {
                    IdInfo = _selectItem.Id;
                    NameInfo = _selectItem.Name;
                    DescriptionInfo = _selectItem.Description;
                    CategoryInfo = _selectItem.Category;
                    SupplierInfo = _selectItem.Supplier;
                    PictureInfo = _selectItem.Picture;  // Set filename for Picture
                }
                OnPropertyChanged(nameof(SelectItem));
            }
        }


        private void LoadProducts()
        {
            var query = from p in db.Products
                        join c in db.Categories on p.CategoryId equals c.Id
                        join s in db.Suppliers on p.SupplierId equals s.Id
                        select new Product
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Picture = p.Picture??"",
                            Description = p.Description,
                            Category = c,
                            Supplier = s,
                        };
            Products = new ObservableCollection<Product>(query.ToList());
        }

        public string GetNewProductId()
        {
            string lastId = db.Products.OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
            if (lastId == null) return "P0000001";

            string prefix = lastId.Substring(0, 1);
            int number = int.Parse(lastId.Substring(1));
            return $"{prefix}{(number + 1):D7}";
        }

        private void AddProduct(object parameter)
        {
            var newProduct = new Product
            {
                Id = GetNewProductId(),
                Name = NameInfo,
                Description = DescriptionInfo,
                CategoryId = CategoryInfo?.Id,
                SupplierId = SupplierInfo?.Id,
                Picture = PictureInfo
            };

            db.Products.Add(newProduct);
            db.SaveChanges();
            Products.Add(newProduct);

            ClearInputFields();
            OnPropertyChanged(nameof(Products));
        }

        private void UpdateProduct(object parameter)
        {
            if (SelectItem != null)
            {
                var existingProduct = db.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefault(p => p.Id == SelectItem.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = NameInfo;
                    existingProduct.Description = DescriptionInfo;
                    existingProduct.CategoryId = CategoryInfo?.Id;
                    existingProduct.SupplierId = SupplierInfo?.Id;
                    existingProduct.Picture = PictureInfo;

                    db.SaveChanges();

                    Products[Products.IndexOf(SelectItem)] = existingProduct;
                    LoadProducts();
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private void DeleteProduct(object parameter)
        {
            var productToDelete = db.Products.FirstOrDefault(p => p.Id == IdInfo);
            if (productToDelete != null)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                Products.Remove(SelectItem);

                ClearInputFields();
                LoadProducts();
                OnPropertyChanged(nameof(Products));
            }
        }

       

        private void ClearInputFields()
        {
            IdInfo = string.Empty;
            NameInfo = string.Empty;
            DescriptionInfo = string.Empty;
            PictureInfo = string.Empty;
        }
    }
}
