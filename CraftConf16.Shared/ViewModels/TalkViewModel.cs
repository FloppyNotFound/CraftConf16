using System.Collections.Generic;
using System.ComponentModel;

namespace CraftConf16.Shared.ViewModels
{
    public class TalkViewModel : INotifyPropertyChanged
    {
        public TalkViewModel()
        {
            _talks = new List<Talk>();
        }

        private List<Talk> _talks;

        public List<Talk> Talks
        {
            get { return _talks; }
            set
            {
                _talks = value;
                OnPropertyChanged("Talks");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }



}
