using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class MyViewModel : INotifyPropertyChanged
    {
        private string _versionText;
        private string _subVersionText;
        public string VersionText
        {
            get { return _versionText; }
            set
            {
                _versionText = value;
                OnPropertyChanged(nameof(VersionText));
            }
        }


        public string SubVersionText
        {
            get { return _subVersionText; }
            set
            {
                _subVersionText = value;
                OnPropertyChanged(nameof(SubVersionText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
