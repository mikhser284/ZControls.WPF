using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{
    public class TagsTree
    {
        public ObservableCollection<ITagTreeItem> Items { get; set; }

        public TagsTree()
        {
            Items = new ObservableCollection<ITagTreeItem>();
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
    }
}
