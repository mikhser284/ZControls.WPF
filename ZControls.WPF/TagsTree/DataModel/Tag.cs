using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZControls.WPF.TagsTree.DataModel
{
    public class Tag : TagsTreeItem
    {
        private ObservableCollection<ITaggedObject> _taggedObjects = new ObservableCollection<ITaggedObject>();
        public ObservableCollection<ITaggedObject> TaggedObjects
        {
            get => _taggedObjects;
            set { _taggedObjects = value; OnPropertyChanged(nameof(TaggedObjects)); }
        }

        public Tag(String name, TagsDir parentDir) : base(name, parentDir) { }
    }
}
