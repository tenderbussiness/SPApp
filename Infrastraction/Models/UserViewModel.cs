using System.ComponentModel;

namespace Infrastraction.Models
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string? _name;

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                this.NotifyPropertyChanged("Name");
                
            }
        }

        private string? _email;

        public string? Email
        {
            get { return _email; }
            set 
            { 
                _email = value;
                this.NotifyPropertyChanged("Email");
            }
        }

        private string? _phone;

        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                this.NotifyPropertyChanged("Phone");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            //if (this.PropertyChanged != null)
            //    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
