using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{

    public class Tag : ITagTreeItem, INotifyPropertyChanged
    {
        private Int32 _id;
        public Int32 Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private Boolean? _isSelected;
        public Boolean? IsSelected
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

        private ETagState _state;
        public ETagState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        private Int32 _relatedItemsCount = 0;
        public Int32 RelatedItemsCount
        {
            get { return _relatedItemsCount; }
            set
            {
                _relatedItemsCount = value;
                OnPropertyChanged(nameof(RelatedItemsCount));
            }
        }

        private Int32 _availableItemsCount = 0;
        public Int32 AvailableItemsCount
        {
            get { return _availableItemsCount; }
            set
            {
                _relatedItemsCount = value;
                OnPropertyChanged(nameof(AvailableItemsCount));
            }
        }

        private ITagTreeItem _parent;
        public ITagTreeItem Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        internal Tag(Int32 id, String name, ETagState state = ETagState.Undefined)
        {
            _id = id;
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
