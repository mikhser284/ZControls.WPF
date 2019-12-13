using System;
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
                ChangeDescendantsSelection();
                ChangeAscendantsSelection();
                OnPropertyChanged(nameof(IsSelected));
            }
        }


        public void ChangeDescendantsSelection()
        {
            var children = Items;
            if (Items == null || Items.Count == 0) return;
            foreach(var item in children) item.IsSelected = IsSelected;
        }
         
        public void ChangeAscendantsSelection()
        {
            TagsDir curentDir = this;
            TagsDir parentDir = curentDir.Parent as TagsDir;
            while(curentDir != null && parentDir != null)
            {
                Boolean? selection = curentDir.IsSelected;
                if(selection != null)
                {
                    Boolean thereAreCheckedItems = false;
                    Boolean thereAreUncheckedItems = false;
                    Boolean thereAreUndefinedItems = false;
                    
                    foreach(var item in parentDir.Items)
                    {
                        switch(item.IsSelected)
                        {
                            case true:
                                thereAreCheckedItems = true;
                                break;
                            case false:
                                thereAreUncheckedItems = true;
                                break;
                            case null:
                                thereAreUndefinedItems = true;
                                break;
                        }
                        Int32 diferentItemsCount = (thereAreCheckedItems ? 1 : 0) + (thereAreUncheckedItems ? 1 : 0) + (thereAreUndefinedItems ? 1 : 0);
                        if(diferentItemsCount > 1)
                        {
                            parentDir.IsSelected = null;
                            break;
                        }
                    }
                }
                parentDir.IsSelected = selection;
                //
                curentDir = curentDir.Parent as TagsDir;                
                parentDir = curentDir?.Parent as TagsDir;
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

        public TagsDir AddDir(String name)
        {
            TagsDir tagsDir = new TagsDir(name);
            Items.Add(tagsDir);
            tagsDir.Parent = this;
            return this;
        }

        public TagsDir AddDirAndDown(String name)
        {
            TagsDir tagsDir = new TagsDir(name);
            Items.Add(tagsDir);
            tagsDir.Parent = this;
            return tagsDir;
        }

        public TagsDir AddTag(String name)
        {
            Tag tag = new Tag(name);
            Items.Add(tag);
            tag.Parent = this;
            return this;
        }

        public TagsDir AddTagAndUp(String name)
        {
            Tag tag = new Tag(name);
            Items.Add(tag);
            tag.Parent = this;
            TagsDir parentDir = this.Parent as TagsDir;
            if (parentDir == null) throw new InvalidOperationException("Parent item is null or root");
            return parentDir;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
