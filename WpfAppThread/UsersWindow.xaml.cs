using Domain.Data;
using Infrastraction.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppThread
{
    /// <summary>
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        private ObservableCollection<UserViewModel> _users = new ();
        private readonly Stopwatch _stopwatch = new Stopwatch();


        public UsersWindow()
        {
            InitializeComponent();
            _stopwatch.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hello Peter");
            MyDataContext myDataContext = new MyDataContext();

            var model = myDataContext.Users
                .Select(x=> new UserViewModel
                {
                    Email = x.Email,
                    Name = x.LastName+ " "+ x.FistName,
                    Phone = x.PhoneNumber,
                })
                .ToList();
            _users = new(model);
            dgSimple.ItemsSource = _users;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _users.Add(new UserViewModel
            {
                Name="Test",
                Phone="+380 98 89 899",
                Email="email@test.com",
            });
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                _stopwatch.Restart();
                UserViewModel selectedUser = (UserViewModel)dgSimple.SelectedItem;
                _users.Remove(selectedUser);

                UpdateStatusBar($"User {selectedUser.Name} deleted successfully. Time taken: {_stopwatch.ElapsedMilliseconds} ms.");

            }
        }
        private void UpdateStatusBar(string text)
        {
            


        }
        private void dgSimple_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedUser = (UserViewModel)e.Row.Item;
                
                UpdateStatusBar($"User {editedUser.Name} updated successfully.");
                

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MyDataContext myDataContext = new MyDataContext();
            myDataContext.SaveChanges();
        }
    }
}
