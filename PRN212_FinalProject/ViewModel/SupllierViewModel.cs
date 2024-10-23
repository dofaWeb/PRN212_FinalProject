
using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using PRN212_FinalProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    internal class SupplierViewModel : BaseViewModel
    {
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ICommand AddButton {  get; set; }
        public void AddSupplier(Object Parameter)
        {
            DBContext dBContext = new DBContext();
            Supplier Addsupplier;
            Addsupplier = new Supplier();
            Addsupplier.Id = _supplierId;
            Addsupplier.Name = _supplierName;
            Addsupplier.ContactInfo = _supplierContactInfo;
            Addsupplier.Address = _supplierAddress;
            dBContext.Suppliers.Add(Addsupplier);
            dBContext.SaveChanges();
            LoadData();
        }
        public SupplierViewModel()

        {
            LoadData();
            AddButton = new RelayCommand(AddSupplier);
        }
        void LoadData()
        {
            DBContext dBContext = new DBContext();
            Suppliers = new ObservableCollection<Supplier>(dBContext.Suppliers.ToList());
        }

        private string _supplierId;
        private string _supplierName;
        private string _supplierContactInfo;
        private string _supplierAddress;

        public string SupplierId
        {
            get { return _supplierId; }
            set
            {
                _supplierId = value;
                OnPropertyChanged(SupplierId);
            }
        }
        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                OnPropertyChanged(SupplierName);
            }
        }
        public string SupplierContactInfo
        {
            get { return _supplierContactInfo; }
            set
            {
                _supplierContactInfo = value; OnPropertyChanged(SupplierContactInfo);
            }
        }
        public string SupplierAddress
        {
            get { return _supplierAddress; }
            set { _supplierAddress = value; 
                OnPropertyChanged(SupplierAddress); }
        }

        private Supplier _showItem;
        public Supplier ShowItem
        {
            get { return _showItem; }
            set
            {

            }




        }   
    }
}
