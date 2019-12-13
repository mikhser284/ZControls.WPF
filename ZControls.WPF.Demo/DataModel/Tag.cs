using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{

    public class Tag : ITagTreeItem, INotifyPropertyChanged
    {
        public Boolean? _isSelected { get; set; }

        public Boolean? IsSelected
        {
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public String _name { get; set; }
        
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(_name));
            }
        }

        public ETagState _state { get; set; }

        public ETagState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public ITagTreeItem Parent { get; set; }

        public Tag(String name, ETagState state = ETagState.Undefined)
        {
            _name = name;
            _state = state;
            _isSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
