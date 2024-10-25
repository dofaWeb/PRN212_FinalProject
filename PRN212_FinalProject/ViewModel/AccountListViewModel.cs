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
using BCrypt.Net; // Thêm namespace này để sử dụng BCrypt
using System.Security.Cryptography;

namespace PRN212_FinalProject.ViewModel
{
    public class AccountListViewModel : BaseViewModel
    {
        Entities.DBContext db;

        public ObservableCollection<Entities.Account> Accounts { get; set; }

        public Entities.Account Accountt { get; set; }


        public ICommand GetAccCommand { get; set; }
        public ICommand Adds {  get; set; }
        public ICommand Updates {  get; }
        public ICommand Deletes { get;  }


        public AccountListViewModel()
        {
            db = new Entities.DBContext();
            LoadAccounts();
            Adds = new RelayCommand(Add);
            Accountt = new Entities.Account();
            db = new Entities.DBContext();

            Updates = new RelayCommand(Update);
            Deletes = new RelayCommand(Delete);
        }
        private void LoadAccounts()
        {
            // Lấy danh sách tài khoản từ cơ sở dữ liệu, bao gồm cả Role và State
            // Sử dụng select new để chọn các thuộc tính cần thiết và khởi tạo Entities.Account
            var accountList = db.Accounts
                                .Include(a => a.Role)   // Nạp thông tin từ bảng Role_Name
                                .Include(a => a.State)  // Nạp thông tin từ bảng Account_State
                                .Select(a => new Entities.Account
                                {
                                    Id = a.Id,
                                    Username = a.Username,
                                    Password = a.Password,
                                    Name = a.Name,
                                    Phone = a.Phone,
                                    Email = a.Email,
                                    Address = a.Address,
                                    RoleId = a.RoleId,
                                    StateId = a.StateId,
                                    Role = new Entities.RoleName
                                    {
                                        Id = a.Role.Id,
                                        Name = a.Role.Name
                                    },
                                    State = new Entities.AccountState
                                    {
                                        Id = a.State.Id,
                                        Name = a.State.Name
                                    }
                                })
                                .ToList();

            // Chuyển đổi sang ObservableCollection để sử dụng cho giao diện
            Accounts = new ObservableCollection<Entities.Account>(accountList);
        }
       

        public void Update(object parameter)
        {
            if (select != null) // Kiểm tra nếu có tài khoản được chọn
            {
                // Cập nhật thông tin từ view model vào đối tượng 'select' (Account đã chọn)
                select.Username = Username;
                select.Phone = Phone;
                select.Address = Adress;
                select.Email = Email;
                select.Role.Name = Role; // Cập nhật Role nếu cần thiết
                select.State.Name = State; // Cập nhật State nếu cần thiết

                // Cập nhật đối tượng 'select' trong DB
                db.Accounts.Update(select);
                db.SaveChanges(); // Lưu thay đổi vào DB

                // Cập nhật lại danh sách khách hàng hiển thị (ObservableCollection)
                int index = Accounts.IndexOf(select);
                if (index >= 0)
                {
                    Accounts[index] = select; // Cập nhật danh sách hiển thị
                   
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
                    Role = _select.Role.Name;
                    State = _select.State.Name;
                }
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

            // Tạo tài khoản mới
            var newAccount = new Entities.Account
            {
                Id = GetNewId(),         // Lấy ID mới theo quy tắc tạo ID đã có
                Username = Username,     // Sử dụng thông tin từ ViewModel
                Password = hashedPassword, // Bạn có thể cập nhật logic mật khẩu theo yêu cầu
                Name = Username,         // Sử dụng tạm thời tên là username
                Phone = Phone,
                Email = Email,
                Address = Adress,
                RoleId = "Role0002",
                StateId = "S0000001"

            };

            // Thêm tài khoản mới vào cơ sở dữ liệu
            db.Accounts.Add(newAccount);
            db.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
                               // Tải lại tài khoản vừa thêm từ cơ sở dữ liệu cùng với Role và State
            var accountWithRoleAndState = db.Accounts
                .Include(a => a.Role)  // Nạp thông tin Role từ cơ sở dữ liệu
                .Include(a => a.State) // Nạp thông tin State từ cơ sở dữ liệu
                .FirstOrDefault(a => a.Id == newAccount.Id);

            if (accountWithRoleAndState != null)
            {
                 // Thêm tài khoản mới vào ObservableCollection để cập nhật giao diện
                Accounts.Add(newAccount);

            // Hiển thị thông báo thành công
            MessageBox.Show("Tài khoản đã được thêm thành công.");
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

       


    }
}
