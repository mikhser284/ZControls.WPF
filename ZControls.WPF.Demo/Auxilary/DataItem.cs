using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.Aux
{
    public class DataItem : INotifyPropertyChanged
    {
        private Boolean _isSelected { get; set; }
        public Boolean IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private String _name { get; set; }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public HashSet<Int32> _tagsIds { get; set; }

        private String _tagsNames { get; set; }
        public String TagsNames
        {
            get { return _tagsNames; }
            set
            {
                _tagsNames = value;
                OnPropertyChanged(nameof(TagsNames));
            }
        }

        public DataItem(String name)
        {
            _name = name;
            _tagsIds = new HashSet<int>();
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
