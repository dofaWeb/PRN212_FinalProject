using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        private DBContext db;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<Entities.Category> Categories { get; set; }


        public CategoryViewModel()
        {
            db = new DBContext();
            AddCommand = new RelayCommand(AddCategory);
            EditCommand = new RelayCommand(EditCategory);
            DeleteCommand = new RelayCommand(DeleteCategory);
            LoadCategory();
        }

        public dynamic GetCategory()
        {
            var result = db.Categories.Select(p => new Category
            {
                Id = p.Id,
                Name = p.Name,
                CategoryType = (p.Id.StartsWith("C0")) ? "Laptop" : "Phone"
            }).ToList();
            return result;
        }

        private string _idInfo;
        public string IdInfo
        {
            get => _idInfo;
            set { _idInfo = value; OnPropertyChanged(nameof(IdInfo)); }
        }

        private string _name;
        public string NameInfor
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(NameInfor)); }
        }

        private string _categoryType;
        public string CategoryTypeInfor
        {
            get => _categoryType;
            set { _categoryType = value; OnPropertyChanged(nameof(CategoryTypeInfor)); }
        }

        private Category _selectedItem;
        public Category SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    // Update input fields with the selected item's properties
                    IdInfo = _selectedItem.Id;
                    NameInfor = _selectedItem.Name;
                    CategoryTypeInfor = _selectedItem.CategoryType;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public void LoadCategory()
        {
            var query = GetCategory();
            Categories = new ObservableCollection<Entities.Category>(query);
            OnPropertyChanged(nameof(Categories));
        }

        public string GetCategoryId(string Prefix)
        {
            string lastId = db.Categories
                .OrderByDescending(a => a.Id)
                .Where(c => c.Id.StartsWith(Prefix))
                .Select(a => a.Id)
                .FirstOrDefault();

            if (lastId == null)
            {
                if (Prefix == "C0")
                    return "C0000001";
                else
                    return "C1000001";
            }

            string prefix = lastId.Substring(0, 1);
            int number = int.Parse(lastId.Substring(1));

            int newNumber = number + 1;
            string newId = $"{prefix}{newNumber:D7}";

            return newId;
        }

        public void AddCategory(object parameter)
        {
            string Id = GetCategoryId((CategoryTypeInfor == "Laptop") ? "C0" : "C1");
            var newCategory = new Category
            {
                Id = Id,
                Name = NameInfor,
                CategoryType = CategoryTypeInfor,
            };

            db.Categories.Add(newCategory);
            db.SaveChanges();
            Categories.Add(newCategory);
            OnPropertyChanged(nameof(Categories));
            NameInfor = "";
            IdInfo = "";
            CategoryTypeInfor = "";
        }

        public void EditCategory(object parameter)
        {
            if(SelectedItem != null)
            {
                var existingCategory = db.Categories.FirstOrDefault(p => p.Id == SelectedItem.Id);
                if (existingCategory != null)
                {
                    existingCategory.Name = NameInfor;
                    db.SaveChanges();
                    LoadCategory();
                    NameInfor = "";
                    IdInfo = "";
                    CategoryTypeInfor = "";
                }
            }
        }

        public void DeleteCategory(object parameter)
        {
            if(SelectedItem != null)
            {
                var existingCategory = db.Categories.FirstOrDefault(p=> p.Id ==SelectedItem.Id);
                if (existingCategory != null)
                {
                    db.Categories.Remove(existingCategory);
                    db.SaveChanges();
                    Categories.Remove(existingCategory);
                    OnPropertyChanged(nameof(Categories));
                    LoadCategory();
                    NameInfor = "";
                    IdInfo = "";
                    CategoryTypeInfor = "";
                }
            }
        }
    }
}
