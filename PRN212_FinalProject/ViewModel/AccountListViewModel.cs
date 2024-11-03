using Microsoft.EntityFrameworkCore;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System.Security.Cryptography;
using PRN212_FinalProject.Entities;

namespace PRN212_FinalProject.ViewModel
{
    public class AccountListViewModel : BaseViewModel
    {
        Entities.DBContext db;

        public ObservableCollection<Entities.Account> Accounts { get; set; }
        public ObservableCollection<Entities.RoleName> AccountRoles { get; set; }
        public ObservableCollection<Entities.AccountState> AccountStates { get; set; }
       

        public Entities.Account Accountt { get; set; }


        public ICommand GetAccCommand { get; set; }
        public ICommand Adds { get; set; }
        public ICommand Updates { get; }
        public ICommand Deletes { get; }


        public AccountListViewModel()
        {
            db = new Entities.DBContext();
            LoadAccountss();
            Adds = new RelayCommand(Add);
            //Accountt = new Entities.Account();
            //db = new Entities.DBContext();

            Updates = new RelayCommand(Update);
            Deletes = new RelayCommand(Delete);
        }
        

        private void LoadAccountss()
        {
            using (var context = new Entities.DBContext())
            {
                Accounts = new ObservableCollection<Entities.Account>(context.Accounts.ToList());
                AccountRoles = new ObservableCollection<Entities.RoleName>(context.RoleNames.ToList());
                AccountStates = new ObservableCollection<Entities.AccountState>(context.AccountStates.ToList());

                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged(nameof(AccountRoles));
                OnPropertyChanged(nameof(AccountStates));
            }
        }

        public void Add(object parameter)
        {
            // Kiểm tra nếu username đã tồn tại trong cơ sở dữ liệu
            var existingUser = db.Accounts.FirstOrDefault(a => a.Username == Username);
            if (existingUser != null)
            {
                MessageBox.Show("Username đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.");
                return;
            }
            string hashedPassword = HashPasswordWithMD5(Password);


            var newAccount = new Entities.Account
            {
                Id = GetNewId(),
                Username = Username,
                Password = hashedPassword,
                Name = Username,
                Phone = Phone,
                Email = Email,
                Address = Adress,
                RoleId = "Role0002",
                StateId = "S0000001"

            };


            db.Accounts.Add(newAccount);
            db.SaveChanges();
            var accountWithRoleAndState = db.Accounts
                .Include(a => a.Role)
                .Include(a => a.State)
                .FirstOrDefault(a => a.Id == newAccount.Id);

            if (accountWithRoleAndState != null)
            {
                // Thêm tài khoản mới vào ObservableCollection để cập nhật giao diện
                Accounts.Add(newAccount);

                // Hiển thị thông báo thành công
                MessageBox.Show("Tài khoản đã được thêm thành công.");
            }

        }
        public void Update(object parameter)
        {
            if (select != null)
            {
                // Kiểm tra xem bản ghi có còn tồn tại không
                var existingAccount = db.Accounts.Find(select.Id);
                if (existingAccount == null)
                {
                    MessageBox.Show("Tài khoản này không còn tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                try
                {
                    // Cập nhật các thuộc tính
                    existingAccount.Username = Username;
                    existingAccount.Phone = Phone;
                    existingAccount.Address = Adress;
                    existingAccount.Email = Email;
                    existingAccount.RoleId = RoleId;
                    existingAccount.StateId = StateId;

                    db.SaveChanges();
                    // Cập nhật ObservableCollection
                    Accounts.Clear();
                    LoadAccountss();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi không xác định: " + ex.Message);
                }
            }
        }



        public void Delete(object parameter)
        {
            if (select != null) // Kiểm tra tài khoản đã chọn
            {
                var accountToDelete = db.Accounts.Find(select.Id);

                if (accountToDelete != null)
                {
                    db.Accounts.Remove(accountToDelete);  // Xóa tài khoản khỏi cơ sở dữ liệu
                    db.SaveChanges();  // Lưu thay đổi

                    Accounts.Remove(select);  // Xóa tài khoản khỏi ObservableCollection để cập nhật giao diện
                    select = null; // Đặt lại chọn để tránh xóa tài khoản cũ
                }
            }
        }


        private Entities.Account _select;
        public Entities.Account select
        {
            get { return _select; }
            set
            {
                _select = value;
                OnPropertyChanged(nameof(select));
                if (_select != null)
                {
                    ID = _select.Id;
                    Username = _select.Username;
                    Phone = _select.Phone;
                    Adress = _select.Address;
                    Email = _select.Email;
                    RoleId = _select.Role.Id;
                    StateId = _select.State.Id;
                }
            }

        }
        


        public string GetNewId()
        {
            string lastId = db.Accounts
                     .OrderByDescending(a => a.Id)
                     .Select(a => a.Id)
                     .FirstOrDefault();
            if (lastId == null)
            {
                return "A0000001";
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
        public string HashPasswordWithMD5(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển mật khẩu thành byte array
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi byte array thành chuỗi hexa
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                // Trả về chuỗi hash MD5
                return builder.ToString();
            }
        }

        public dynamic GetRole()
        {
            // Lấy danh sách RoleName từ DB và trả về Id và Name cho mỗi Role
            var result = db.RoleNames.Select(p => new RoleName
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return result;
        }

        public dynamic GetState()
        {
            // Lấy danh sách AccountState từ DB, bao gồm Id và Name của mỗi trạng thái
            var result = db.AccountStates.Select(p => new AccountState
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return result;
        }

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _Adress;
        public string Adress
        {
            get { return _Adress; }
            set
            {
                _Adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _Role;
        public string Role
        {
            get { return _Role; }
            set
            {
                _Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        private string _State;
        public string State
        {
            get { return _State; }
            set
            {
                _State = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _RoleId;
        public string RoleId
        {
            get { return _RoleId; }
            set
            {
                _RoleId = value;
                OnPropertyChanged(nameof(RoleId));
            }
        }

        private string _StateId;
        public string StateId
        {
            get { return _StateId; }
            set
            {
                _StateId = value;
                OnPropertyChanged(nameof(StateId));
            }
        }



    }
}
