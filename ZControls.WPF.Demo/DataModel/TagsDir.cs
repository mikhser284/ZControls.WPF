﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZControls.WPF.Demo.DataModel
{
    public enum Move
    {
        Up = -1,
        No = 0,
        Down = 1
    }

    public class TagsDir : ITagTreeItem, INotifyPropertyChanged
    {
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


        private ObservableCollection<ITagTreeItem> _items;

        public ObservableCollection<ITagTreeItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
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


        public TagsDir(String name)
        {
            Items = new ObservableCollection<ITagTreeItem>();
            Name = name;
            IsSelected = false;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
