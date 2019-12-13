using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{
    public class TagsTree : INotifyPropertyChanged
    {
        public ObservableCollection<ITagTreeItem> _items { get; set; }

        public ObservableCollection<ITagTreeItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public TagsTree()
        {
            _items = new ObservableCollection<ITagTreeItem>();
        }

        
        public TagsTree AddDir(String name)
        {
            TagsDir tagsDir = new TagsDir(name);
            Items.Add(tagsDir);
            tagsDir.Parent = null;
            return this;
        }

        public TagsDir AddDirAndDown(String name)
        {
            TagsDir tagsDir = new TagsDir(name);
            Items.Add(tagsDir);
            tagsDir.Parent = null;
            return tagsDir;
        }

        public TagsTree AddTag(String name)
        {
            Tag tag = new Tag(name);
            Items.Add(tag);
            tag.Parent = null;
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
