using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.Aux
{
    public class DataSet : INotifyPropertyChanged
    {
        private Boolean _useTags;
        public Boolean UseTags
        {
            get { return _useTags; }
            set
            {
                _useTags = value;
                OnPropertyChanged(nameof(UseTags));
            }
        }

        private ObservableCollection<DataItem> _dataItems;
        public ObservableCollection<DataItem> DataItems
        {
            get { return _dataItems; }
            set
            {
                _dataItems = value;
                OnPropertyChanged(nameof(DataItems));
            }
        }

        public DataSet()
        {
            DataItems = new ObservableCollection<DataItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
