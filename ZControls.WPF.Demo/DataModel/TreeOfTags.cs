using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{
    public class TreeOfTags : INotifyPropertyChanged
    {
        private Int32 _lastId;


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

        public TreeOfTags()
        {
            _items = new ObservableCollection<ITagTreeItem>();
            _lastId = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void SelectItem(ITagTreeItem treeItem, Boolean originSeleciton)
        {
            treeItem.IsSelected = originSeleciton;
            TagsDir originDir = treeItem as TagsDir;
            if(originDir != null) SelectDescendants(originDir, originSeleciton);
            TagsDir parentDir = treeItem.Parent as TagsDir;
            if (parentDir != null) SelectAscendants(parentDir, originSeleciton);
        }

        private void SelectDescendants(TagsDir originDir, Boolean originSelection)
        {
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(originDir.Items);
            while(stack.Count > 0)
            {
                ITagTreeItem curentItem = stack.Pop();
                curentItem.IsSelected = originSelection;
                TagsDir dir = curentItem as TagsDir;
                if (dir == null) continue;
                foreach (var dirItem in dir.Items) stack.Push(dirItem);
            }
        }

        private void SelectAscendants(TagsDir parentDir, Boolean originSelection)
        {
            Boolean? selection = originSelection;
            while(parentDir != null)
            {
                if(selection != null)
                {
                    Boolean thereAreCheckedItems = false;
                    Boolean thereAreUncheckedItems = false;
                    Boolean thereAreUndefinedItems = false;

                    foreach (var item in parentDir.Items)
                    {
                        switch (item.IsSelected)
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
                        if (diferentItemsCount > 1)
                        {
                            selection = null;
                            break;
                        }
                    }
                }
                parentDir.IsSelected = selection;
                parentDir = parentDir.Parent as TagsDir;
            }
        }

        public List<Tag> GetSelectedTags()
        {
            List<Tag> selectedTags = new List<Tag>();
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(Items);
            while(stack.Count > 0)
            {
                ITagTreeItem treeItem = stack.Pop();
                if (treeItem is Tag tag && treeItem.IsSelected == true)
                {
                    selectedTags.Add(tag);
                    continue;
                }
                if(treeItem is TagsDir dir && IsTreeItemSelected(dir)) foreach(var dirItem in dir.Items) if (IsTreeItemSelected(dirItem)) stack.Push(dirItem);
            }
            return selectedTags;            
        }

        public List<Tag> ChangeStateOfSelectedTags(ETagState state)
        {
            List<Tag> selectedTags = GetSelectedTags();
            selectedTags.ForEach(x => x.State = state);
            return selectedTags;
        }

        public void ApplyToSelectedItems(Action<ITagTreeItem> action)
        {
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(Items);
            while (stack.Count > 0)
            {
                ITagTreeItem treeItem = stack.Pop();
                if (treeItem is TagsDir dir && IsTreeItemSelected(dir)) foreach (var dirItem in dir.Items) if (IsTreeItemSelected(dirItem)) stack.Push(dirItem);
                action(treeItem);
            }            
        }

        public void ForEach(Action<ITagTreeItem> action)
        {
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(Items);
            while (stack.Count > 0)
            {
                ITagTreeItem treeItem = stack.Pop();
                if (treeItem is TagsDir dir) foreach (var dirItem in dir.Items) stack.Push(dirItem);
                action(treeItem);
            }
        }

        private static Boolean IsTreeItemSelected(ITagTreeItem treeItem) => treeItem.IsSelected == true || treeItem.IsSelected == null;

        public TagsDir AddDir(String dirName, TagsDir parentDir = null)
        {
            TagsDir newDir = new TagsDir(dirName);
            if(parentDir != null)
            {
                newDir.Parent = parentDir;
                parentDir.Items.Add(newDir);
            }
            else
            {
                newDir.Parent = null;
                Items.Add(newDir);
            }
            return newDir;
        }

        public Tag AddTag(String tagName, TagsDir parentDir = null)
        {
            Tag newTag = new Tag(++_lastId, tagName);
            if(parentDir != null)
            {
                newTag.Parent = parentDir;
                parentDir.Items.Add(newTag);
            }
            else
            {
                newTag.Parent = null;
                Items.Add(newTag);
            }
            return newTag;
        }
    }
}
