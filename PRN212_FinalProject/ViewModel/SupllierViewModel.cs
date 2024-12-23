﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using PRN212_FinalProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    internal class SupplierViewModel : BaseViewModel
    {
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ICommand AddButton { get; set; }
        public ICommand UpdateButton { get; set; }
        public ICommand DeleteButton { get; set; }

        public void AddSupplier(Object Parameter)
        {
            DBContext dBContext = new DBContext();
            Supplier Addsupplier;
            Addsupplier = new Supplier();
            Addsupplier.Id = getNewSupplierId();
            Addsupplier.Name = _supplierName;
            Addsupplier.ContactInfo = _supplierContactInfo;
            Addsupplier.Address = _supplierAddress;
            dBContext.Suppliers.Add(Addsupplier);
            dBContext.SaveChanges();
            LoadData();
            SupplierId = "";
            SupplierName = "";
            SupplierContactInfo = "";
            SupplierAddress = "";


        }
        public void UpdateSupplier(Object Parameter)
        {
            DBContext dBContext = new DBContext();
            if (SelectedItem != null)
            {
                var existingSupllier = dBContext.Suppliers.Where(s => s.Id == SelectedItem.Id).FirstOrDefault();
                if (existingSupllier != null)
                {
                    existingSupllier.Name = SupplierName;
                    existingSupllier.ContactInfo = SupplierContactInfo;
                    existingSupllier.Address = SupplierAddress;
                    dBContext.SaveChanges();
                    LoadData();
                    OnPropertyChanged(nameof(Suppliers));
                    SupplierId = "";
                    SupplierName = "";
                    SupplierContactInfo = "";
                    SupplierAddress = "";
                }
            } 
        }
        public void DeleteSupplier(Object Parameter)
        {
            DBContext dBContext = new DBContext();
            if (SelectedItem != null)
            {
                var existingSupllier = dBContext.Suppliers.Where(s => s.Id == SelectedItem.Id).FirstOrDefault();
                if (existingSupllier != null)
                {
                    dBContext.Suppliers.Remove(existingSupllier);
                    dBContext.SaveChanges() ;
                    LoadData(); OnPropertyChanged(nameof(Suppliers));
                    SupplierId = "";
                    SupplierName = "";
                    SupplierContactInfo = "";
                    SupplierAddress = "";
                }
            }
        }
        public SupplierViewModel()

        {
            LoadData();
            AddButton = new RelayCommand(AddSupplier);
            UpdateButton = new RelayCommand(UpdateSupplier);
            DeleteButton = new RelayCommand(DeleteSupplier);
        }
        void LoadData()
        {
            DBContext dBContext = new DBContext();
            Suppliers = new ObservableCollection<Supplier>(dBContext.Suppliers.ToList());
            OnPropertyChanged(nameof(Suppliers));
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
                OnPropertyChanged(nameof(SupplierId));
            }
        }
        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                OnPropertyChanged(nameof(SupplierName));
            }
        }
        public string SupplierContactInfo
        {
            get { return _supplierContactInfo; }
            set
            {
                _supplierContactInfo = value; OnPropertyChanged(nameof(SupplierContactInfo));
            }
        }
        public string SupplierAddress
        {
            get { return _supplierAddress; }
            set
            {
                _supplierAddress = value;
                OnPropertyChanged(nameof(SupplierAddress));
            }
        }

        private Supplier _selectedItem;
        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                OnPropertyChanged(nameof(SelectedItem));
                if (SelectedItem != null)
                {
                    SupplierId = _selectedItem.Id;
                    SupplierName = _selectedItem.Name;
                    SupplierContactInfo = _selectedItem.ContactInfo;
                    SupplierAddress = _selectedItem.Address;
                }   
            }





        }
        
        public string getNewSupplierId()
        {
            DBContext dbContext = new DBContext();
            string lastId = dbContext.Suppliers
                     .OrderByDescending(a => a.Id)
                     .Select(a => a.Id)
                     .FirstOrDefault();
            if (lastId == null)
            {
                return "S0000001";
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
    }
}
