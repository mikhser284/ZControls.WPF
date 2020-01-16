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
        public void OnPropertyChanged(string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public override String ToString() => $"[{(CheckMark == null ? "■" : CheckMark == true ? "✔" : "✖")}] {Name}";

        // ----

        public static void SetItemCheckMark(TagsTreeItem treeItem, Boolean originCheckMark)
        {
            treeItem.CheckMark = originCheckMark;
            TagsDir originDir = treeItem as TagsDir;
            if(originDir != null) SetItemDescendantsCheckMarks(originDir, originCheckMark);
            TagsDir parentDir = treeItem.ParentDir;
            if(parentDir != null) SetItemAscendantsCheckMarks(parentDir, originCheckMark);
        }

        private static void SetItemDescendantsCheckMarks(TagsDir originDir, Boolean originCheckMark)
        {
            Stack<TagsTreeItem> stack = new Stack<TagsTreeItem>(originDir.ChildItems);
            while(stack.Count > 0)
            {
                TagsTreeItem curentItem = stack.Pop();
                curentItem.CheckMark = originCheckMark;
                TagsDir dir = curentItem as TagsDir;
                if(dir == null) continue;
                foreach(var dirItem in dir.ChildItems) stack.Push(dirItem);
            }
        }

        private static void SetItemAscendantsCheckMarks(TagsDir parentDir, Boolean originCheckMark)
        {
            Boolean? selection = originCheckMark;
            while(parentDir != null)
            {
                if(selection != null)
                {
                    Boolean thereAreCheckedItems = false;
                    Boolean thereAreUncheckedItems = false;
                    Boolean thereAreUndefinedItems = false;

                    foreach(var item in parentDir.ChildItems)
                    {
                        switch(item.CheckMark)
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
                            selection = null;
                            break;
                        }
                    }
                }
                parentDir.CheckMark = selection;
                parentDir = parentDir.ParentDir as TagsDir;
            }
        }
    }
}
