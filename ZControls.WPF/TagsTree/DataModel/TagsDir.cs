using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZControls.WPF.TagsTree.DataModel
{
    public class TagsDir : TagsTreeItem
    {
        private ObservableCollection<TagsTreeItem> _childItems = new ObservableCollection<TagsTreeItem>();
        public ObservableCollection<TagsTreeItem> ChildItems
        {
            get => _childItems;
            set { _childItems = value; OnPropertyChanged(nameof(ChildItems)); }
        }

        public TagsDir(String name, TagsDir parentDir = null) : base(name, parentDir) { }
    }
}
