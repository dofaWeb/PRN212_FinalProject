using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    public class RegisterViewModel
    {
        Entities.DBContext db;

        public Entities.Account Regis { get; set; }

        public ICommand RegisterCommand { get; set; }
        public Action CloseWindowAction { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            Regis = new Entities.Account();
            db = new Entities.DBContext();
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
        public void Register(object parameter)
        {
            string id = GetNewId();
            string UserName = Regis.Username;
            string Password = Regis.Password;
            string Name = Regis.Name;
            string Phone = Regis.Phone;
            string Email = Regis.Email;
            string Address = Regis.Address;
            string State = Regis.StateId;





            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Username và Password là bắt buộc!");
                return;
            }

            var existingUser = db.Accounts.Where(a => a.Username == UserName).FirstOrDefault();
            if (existingUser != null)
            {
                MessageBox.Show("Username đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
                return;
            }
            // Kiểm tra độ dài của mật khẩu
            if (Password.Length <= 6)
            {
                MessageBox.Show("Password phải có ít nhất 7 ký tự.");
                return;
            }

            // Kiểm tra độ dài của số điện thoại
            if (Phone.Length < 10)
            {
                MessageBox.Show("Số điện thoại phải có ít nhất 10 ký tự.");
                return;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Email không hợp lệ.");
                return;
            }
            

            Entities.Account account = new Entities.Account
            {
                Id = id,
                Username = UserName,
                Password = Password,
                Name = Name,
                Phone = Phone,
                Email = Email,
                Address = Address,
                RoleId = "Role0002", // Mặc định người dùng thường, có thể thay đổi theo yêu cầu
                StateId = "S0000001"

            };

            db.Accounts.Add(account);

            try
            {
                db.SaveChanges();
                MessageBox.Show("Đăng kí thành công");
                // Nếu đăng ký thành công, gọi CloseWindowAction
                CloseWindowAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không đăng kí được");
            }

        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}
