using System.Collections.Generic;
using System.ComponentModel;

namespace CraftConf16.Shared.ViewModels
{
    public class TalkViewModel : INotifyPropertyChanged
    {
        public TalkViewModel()
        {
            _talks = new List<Talk>();
            _stage = string.Empty;
        }

        private List<Talk> _talks;
        private string _stage;

        public List<Talk> Talks
        {
            get { return _talks; }
            set
            {
                _talks = value;
                OnPropertyChanged("Talks");
            }
        }

        public string Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;
                OnPropertyChanged("Stage");
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
