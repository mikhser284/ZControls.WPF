using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.TagsTree.DataModel
{
    public abstract class TagsTreeItem : INotifyPropertyChanged
    {
        private Boolean? _checkMark;
        public Boolean? CheckMark
        {
            get => _checkMark;
            set { _checkMark = value; OnPropertyChanged(nameof(CheckMark)); }
        }

        private String _name;
        public String Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private TagsDir _parentDir;
        public TagsDir ParentDir
        {
            get => _parentDir;
            set { _parentDir = value; OnPropertyChanged(nameof(ParentDir)); }
        }

        public TagsTreeItem(String name, TagsDir parentDir = null)
        {
            CheckMark = false;
            ParentDir = parentDir;
            if(ParentDir != null) parentDir.ChildItems.Add(this);
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") { if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop)); }

        public override String ToString() => $"[{(CheckMark == null ? "■" : CheckMark == true ? "✔" : "✖")}] {Name}";
    }
}
