using System.ComponentModel;
using Var3.Model;

namespace Var3.ViewModel
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private Клиент _user;
        public Клиент User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public ProfileViewModel(Клиент user)
        {
            User = user;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}