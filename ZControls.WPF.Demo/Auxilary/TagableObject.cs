using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.Aux
{
    public class TagableObject : INotifyPropertyChanged, ITagableObject
    {
        private Boolean _isSelected;
        public Boolean IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private HashSet<Int32> _tagsIds;
        public HashSet<Int32> TagsIds
        {
            get { return _tagsIds; }
            set
            {
                _tagsIds = value;
                OnPropertyChanged(nameof(TagsIds));
            }
        }

        private String _tagsNames;
        public String TagsNames
        {
            get { return _tagsNames; }
            set
            {
                _tagsNames = value;
                OnPropertyChanged(nameof(TagsNames));
            }
        }

        public TagableObject(String name)
        {
            _name = name;
            _tagsIds = new HashSet<Int32>();
            _tagsNames = "—";
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
