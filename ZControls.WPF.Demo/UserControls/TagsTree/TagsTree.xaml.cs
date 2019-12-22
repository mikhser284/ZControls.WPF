using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZControls.WPF.Demo.Aux;
using ZControls.WPF.Demo.DataModel;

namespace ZControls.WPF.Demo.UserControls
{
    // Base
    public partial class TagsTree
    {
        private Int32 _lastId = 0;

        private Dictionary<Int32, Tag> _tagsDictionary = new Dictionary<int, Tag>();

        private ObservableCollection<ITagTreeItem> _items = new ObservableCollection<ITagTreeItem>();

        public ObservableCollection<ITagTreeItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public TagsDir AddDir(String dirName, TagsDir parentDir = null)
        {
            TagsDir newDir = new TagsDir(dirName);
            if (parentDir != null)
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
            if (parentDir != null)
            {
                newTag.Parent = parentDir;
                parentDir.Items.Add(newTag);
            }
            else
            {
                newTag.Parent = null;
                Items.Add(newTag);
            }
            _tagsDictionary.Add(newTag.Id, newTag);
            return newTag;
        }

        public void DeleteCheckedItems()
        {
            ForEach(x => x.IsSelected == true, (x) => 
            {
                if (x.Parent is TagsDir dir) dir.Items.Remove(x);
                x.Parent = null;
                if (x is Tag tag) _tagsDictionary.Remove(tag.Id);
            });
        }

        public void SelectItem(ITagTreeItem treeItem, Boolean originSeleciton)
        {
            treeItem.IsSelected = originSeleciton;
            TagsDir originDir = treeItem as TagsDir;
            if (originDir != null) SelectDescendants(originDir, originSeleciton);
            TagsDir parentDir = treeItem.Parent as TagsDir;
            if (parentDir != null) SelectAscendants(parentDir, originSeleciton);
        }

        private void SelectDescendants(TagsDir originDir, Boolean originSelection)
        {
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(originDir.Items);
            while (stack.Count > 0)
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
            while (parentDir != null)
            {
                if (selection != null)
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

        public void GetTagsByState(out HashSet<Int32> redTags, out HashSet<Int32> greenTags)
        {
            redTags = new HashSet<Int32>();
            greenTags = new HashSet<Int32>();
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(Items);
            while (stack.Count > 0)
            {
                ITagTreeItem treeItem = stack.Pop();
                if (treeItem is Tag tag)
                {
                    switch (tag.State)
                    {
                        case ETagState.Exclude:
                            redTags.Add(tag.Id);
                            break;
                        case ETagState.Include:
                            greenTags.Add(tag.Id);
                            break;
                    }
                }
                else if (treeItem is TagsDir tagsDir) foreach (var dirItem in tagsDir.Items) stack.Push(dirItem);
            }
        }

        public List<ITagTreeItem> GetItemsByQuery(Func<ITagTreeItem, Boolean> selector)
        {
            List<ITagTreeItem> tagItems = new List<ITagTreeItem>();
            Stack<ITagTreeItem> stack = new Stack<ITagTreeItem>(Items);
            while (stack.Count > 0)
            {
                ITagTreeItem treeItem = stack.Pop();
                if (treeItem is TagsDir dir) foreach (var dirItem in dir.Items) stack.Push(dirItem);
                if (selector(treeItem)) tagItems.Add(treeItem);
            }
            return tagItems;
        }

        public void ForEach(Func<ITagTreeItem, Boolean> selector, Action<ITagTreeItem> action)
        {
            GetItemsByQuery(selector).ForEach(item => action(item));
        }

        private static Boolean IsTreeItemSelected(ITagTreeItem treeItem) => treeItem.IsSelected == true || treeItem.IsSelected == null;

        public ItemsControl RelatedControl { get; set; }

        public ObservableCollection<T> PerformQuery<T>(IEnumerable<T> tagableObjects, out HashSet<Int32> queriedTags) where T : ITagableObject
        {
            ObservableCollection<T> queryResult = new ObservableCollection<T>();
            HashSet<Int32> redTags;
            HashSet<Int32> greenTags;
            GetTagsByState(out redTags, out greenTags);
            queriedTags = new HashSet<int>();
            foreach (T item in tagableObjects)
            {
                if (item.TagsIds.ContainsAnyFrom(redTags)) continue;
                if (item.TagsIds.ContainsAnyFrom(greenTags))
                {
                    queryResult.Add(item);
                    foreach (Int32 tagId in item.TagsIds) queriedTags.Add(tagId);
                    continue;
                }
                if (greenTags.Count > 0) continue;
                queryResult.Add(item);
                foreach (Int32 tagId in item.TagsIds) queriedTags.Add(tagId);
            }
            return queryResult;
        }
    }

    // Object construction
    public partial class TagsTree : UserControl, INotifyPropertyChanged
    {
        private IEnumerable<ITagableObject> _tagableObjectsCollection;

        public IEnumerable<ITagableObject> TagableObjectsCollection
        {
            get { return _tagableObjectsCollection; }
            set
            {
                _tagableObjectsCollection = value;
                QueriedItems = value;
            }
        }

        private IEnumerable<ITagableObject> QueriedItems { get; set; }

        public TagsTree()
        {
            InitializeComponent();

            GenerateTestData();
            BindCommandsAndHandlers(Ctrl_TagsTree);
            Ctrl_TagsTree.KeyDown += Ctrl_TagsTree_KeyDown;
        }

        private void Ctrl_TagsTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) UnselectTreeViewItem(Ctrl_TagsTree);
        }

        private void UnselectTreeViewItem(TreeView pTreeView)
        {
            if (pTreeView.SelectedItem == null)
                return;

            if (pTreeView.SelectedItem is TreeViewItem)
            {
                (pTreeView.SelectedItem as TreeViewItem).IsSelected = false;
            }
            else
            {
                TreeViewItem item = pTreeView.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
                if (item != null)
                {
                    item.IsSelected = true;
                    item.IsSelected = false;
                }
            }
        }

        private void GenerateTestData()
        {
            var dirA = AddDir("Dir A");
            var dirA_A = AddDir("Dir A-A", dirA);
            var tag007 = AddTag("Tag 007", dirA_A);
            var tag008 = AddTag("Tag 008", dirA_A);
            var tag009 = AddTag("Tag 009", dirA_A);
            var dirA_B = AddDir("Dir A-B", dirA);
            var dirA_B_A = AddDir("Dir A-B", dirA_B);
            var tag013 = AddTag("Tag 007", dirA_B_A);
            var tag014 = AddTag("Tag 008", dirA_B_A);
            var tag015 = AddTag("Tag 009", dirA_B_A);
            var tag010 = AddTag("Tag 007", dirA_B);
            var tag011 = AddTag("Tag 008", dirA_B);
            var tag012 = AddTag("Tag 009", dirA_B);
            var dirA_C = AddDir("Dir A-C", dirA);
            var tag004 = AddTag("Tag 004", dirA);
            var tag005 = AddTag("Tag 005", dirA);
            var tag006 = AddTag("Tag 006", dirA);
            var dirB = AddDir("Dir B");
            var dirC = AddDir("Dir C");
            var dirD = AddDir("Dir D");
            var tag001 = AddTag("Tag 001");
            var tag002 = AddTag("Tag 002");
            var tag003 = AddTag("Tag 003");
            //
            Ctrl_TagsTree.ItemsSource = Items;
        }

        private void TagsTreeItem_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;
            StackPanel stackPanel = checkBox.Parent as StackPanel;
            if (stackPanel == null) return;
            ITagTreeItem tagsTreeItem = stackPanel.DataContext as ITagTreeItem;
            SelectItem(tagsTreeItem, (Boolean)checkBox.IsChecked);
        }
    }

    // Item editing
    public partial class TagsTree
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private Boolean _isInEditMode = false;
        public Boolean IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnPropertyChanged(nameof(IsInEditMode));
            }
        }

        String oldText;

        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.IsVisible)
            {
                tb.Focus();
                tb.SelectAll();
                oldText = tb.Text;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) IsInEditMode = false;
            if (e.Key == Key.Escape)
            {
                var tb = sender as TextBox;
                tb.Text = oldText;
                IsInEditMode = false;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsInEditMode = false;
        }

        private void Ctrl_TagsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IsInEditMode = false;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && FindTreeItem(e.OriginalSource as DependencyObject).IsSelected)
            {
                IsInEditMode = true;
                e.Handled = true;
            }
        }


        static TreeViewItem FindTreeItem(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem)) source = VisualTreeHelper.GetParent(source);
            return source as TreeViewItem;
        }
    }

    // Commands
    public partial class TagsTree
    {
        private void BindCommandsAndHandlers(TreeView treeView)
        {
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ExpandAll, ExpandAll_Executed, ExpandAll_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ExpandTree, ExpandTree_Executed, ExpandTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAll, ColapseAll_Executed, ColapseAll_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAllButThisTree, ColapseAllButThisTree_Executed, ColapseAllButThisTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAllButThisBranch, ColapseAllButThisBranch_Executed, ColapseAllButThisBranch_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseTree, ColapseTree_Executed, ColapseTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.AddDir, AddDir_Executed, AddDir_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.AddTag, AddTag_Executed, AddTag_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.EditItems, EditItems_Executed, EditItems_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.DeleteCheckedItems, DeleteCheckedItems_Executed, DeleteCheckedItems_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.BindItem, BindItem_Executed, BindItem_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.UnbindItem, UnbindItem_Executed, UnbindItem_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsIncluded, CheckedTagsStateAsIncluded_Executed, CheckedTagsStateAsIncluded_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsExcluded, CheckedTagsStateAsExcluded_Executed, CheckedTagsStateAsExcluded_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsNone, CheckedTagsStateAsNone_Executed, CheckedTagsStateAsNone_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.SetCheckMark, SetCheckMark_Executed, SetCheckMark_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ClearCheckMark, ClearCheckMark_Executed, ClearCheckMark_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.InvertCheckMark, InvertCheckMark_Executed, InvertCheckMark_CanExecute));
        }

        // ▬▬▬▬▬ ExpandAll ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ExpandAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();

            foreach(var item in Items)
            {
                TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem != null) treeViewItem.ExpandSubtree();
            }
        }

        private void ExpandAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ ExpandTree ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ExpandTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(Ctrl_TagsTree.SelectedItem) as TreeViewItem;
            if (treeViewItem != null) treeViewItem.ExpandSubtree();
        }

        private void ExpandTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ ColapseAll ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ColapseAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(Ctrl_TagsTree.SelectedItem) as TreeViewItem;
            foreach(var item in treeViewItem.Items)
            {
                TreeViewItem child = item as TreeViewItem;
                
                if (child == null)
                    child = treeViewItem.ItemContainerGenerator.ContainerFromItem(treeViewItem) as TreeViewItem;
                if (child != null) child.IsExpanded = false;
            }
        }

        private void ColapseAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ ColapseAllButThisTree ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ColapseAllButThisTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ColapseAllButThisTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        // ▬▬▬▬▬ ColapseAllButThisBranch ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ColapseAllButThisBranch_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ColapseAllButThisBranch_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        // ▬▬▬▬▬ ColapseTree ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ColapseTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ColapseTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        // ▬▬▬▬▬ AddDir ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void AddDir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;            
            while(tagsTreeItem != null && tagsTreeItem.GetType() != typeof(TagsDir)) tagsTreeItem = tagsTreeItem.Parent;
            TagsDir newDir = AddDir("New dir", tagsTreeItem as TagsDir);
            //
            TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(tagsTreeItem) as TreeViewItem;

            var container = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(tagsTreeItem);
            if (treeViewItem != null)
            {
                treeViewItem.Tag = newDir;
                treeViewItem.IsExpanded = true;
                var obj = treeViewItem.DataContext;
                var tvi = treeViewItem.ItemContainerGenerator.ContainerFromItem(newDir) as TreeViewItem;
                //MessageBox.Show($"{tvi != null}");
                if (tvi != null)
                {
                    tvi.IsSelected = true;
                }
            }
            else
            {                
                var tvi = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(newDir) as TreeViewItem;
                if (tvi != null)
                {
                    tvi.IsSelected = true;
                    
                }
            }
        }
        
        private void AddDir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ AddTag ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void AddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            while (tagsTreeItem != null)
            {
                if (tagsTreeItem is TagsDir) break;
                tagsTreeItem = tagsTreeItem.Parent;
            }
            AddTag("New tag", tagsTreeItem as TagsDir);
        }

        private void AddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ EditItems ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void EditItems_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IsInEditMode = true;
        }

        private void EditItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ DeleteCheckedItems ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void DeleteCheckedItems_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCheckedItems();
        }

        private void DeleteCheckedItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        private Boolean IsAnyItemChecked()
        {
            Boolean thereAreSelectedItems = false;
            foreach (var item in Items)
            {
                if (item.IsSelected == false) continue;
                thereAreSelectedItems = true;
                break;
            }
            return thereAreSelectedItems;
        }

        // ▬▬▬▬▬ BindItem ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void BindItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<ITagableObject> selectedItems = TagableObjectsCollection.GetQueriedItems(x => x.IsSelected);
            if (selectedItems.Count == 0) return;
            List<Tag> selectedTags = GetItemsByQuery(x => IsTreeItemSelected(x)).Select(x => (Tag)x).OrderBy(x => x.Name).ToList();
            selectedItems.ForEach(item => selectedTags.ForEach(tag => item.TagsIds.Add(tag.Id)));

            foreach (var item in selectedItems)
            {
                if (item is TagableObject obj)
                {
                    List<String> tagsNames = new List<string>();
                    foreach (Int32 tagId in item.TagsIds) tagsNames.Add($"[#{_tagsDictionary[tagId].Name}]");
                    obj.TagsNames = String.Join("; ", tagsNames.OrderBy(x => x));
                }
            }

            ForEach(x => IsTreeItemSelected(x), x => x.IsSelected = false);
        }

        private void BindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ UnbindItem ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void UnbindItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<ITagableObject> selectedItems = TagableObjectsCollection.GetQueriedItems(x => x.IsSelected);
            if (selectedItems.Count == 0) return;
            List<Tag> selectedTags = GetItemsByQuery(x => IsTreeItemSelected(x)).Select(x => (Tag)x).OrderBy(x => x.Name).ToList();
            selectedItems.ForEach(item => selectedTags.ForEach(tag => item.TagsIds.Remove(tag.Id)));

            foreach (var item in selectedItems)
            {
                if(item is TagableObject obj)
                {
                    List<String> tagsNames = new List<string>();
                    foreach (Int32 tagId in item.TagsIds) tagsNames.Add($"[#{_tagsDictionary[tagId].Name}]");
                    obj.TagsNames = String.Join("; ", tagsNames.OrderBy(x => x));
                }                
            }

            ForEach(x => IsTreeItemSelected(x), x => x.IsSelected = false);
        }

        private void UnbindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ CheckedTagsStateAsIncluded ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsIncluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ForEach(x => IsTreeItemSelected(x), x => { if (x is Tag tag) tag.State = ETagState.Include; x.IsSelected = false; });
            HashSet<Int32> selectedTags = null;
            QueriedItems = PerformQuery(TagableObjectsCollection, out selectedTags);
            RelatedControl.ItemsSource = QueriedItems;
            //
            foreach(var item in _tagsDictionary.Keys)
            {
                if (selectedTags.Contains(item)) continue;
                _tagsDictionary[item].State = ETagState.Unavailable;
            }            
        }

        private void CheckedTagsStateAsIncluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ CheckedTagsStateAsExcluded ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsExcluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ForEach(x => IsTreeItemSelected(x), x => { if (x is Tag tag) tag.State = ETagState.Exclude; x.IsSelected = false; });
            HashSet<Int32> selectedTags = null;
            QueriedItems = PerformQuery(TagableObjectsCollection, out selectedTags);
            RelatedControl.ItemsSource = QueriedItems;
            //
            foreach (var item in _tagsDictionary.Keys)
            {
                if (selectedTags.Contains(item)) continue;
                _tagsDictionary[item].State = ETagState.Unavailable;
            }
        }

        private void CheckedTagsStateAsExcluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ CheckedTagsStateAsNone ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ForEach(x => IsTreeItemSelected(x), x => { if (x is Tag tag) tag.State = ETagState.Undefined; x.IsSelected = false; });
            HashSet<Int32> selectedTags = null;
            QueriedItems = PerformQuery(TagableObjectsCollection, out selectedTags);
            RelatedControl.ItemsSource = QueriedItems;
            //
            foreach (var item in _tagsDictionary.Keys)
            {
                if (selectedTags.Contains(item)) continue;
                _tagsDictionary[item].State = ETagState.Unavailable;
            }
        }

        private void CheckedTagsStateAsNone_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // ▬▬▬▬▬ SetCheckMark ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void SetCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            SelectItem(tagsTreeItem, true);
        }

        private void SetCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            e.CanExecute = tagsTreeItem != null && tagsTreeItem.IsSelected != true;
        }

        // ▬▬▬▬▬ ClearCheckMark ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void ClearCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            SelectItem(tagsTreeItem, false);
        }

        private void ClearCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            e.CanExecute = tagsTreeItem != null && tagsTreeItem.IsSelected != false;
        }

        // ▬▬▬▬▬ InvertCheckMark ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void InvertCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            Boolean selection = tagsTreeItem.IsSelected == null ? true : (Boolean)tagsTreeItem.IsSelected;
            SelectItem(tagsTreeItem, !selection);
        }

        private void InvertCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Ctrl_TagsTree.SelectedItem != null;
        }
    }

    // Helpers
    public static class Ext_Linq
    {
        public static List<T> GetQueriedItems<T>(this IEnumerable<T> itemsSet, Func<T, Boolean> selector)
        {
            List<T> queriedItems = new List<T>();
            foreach(T item in itemsSet) if (selector(item)) queriedItems.Add(item);
            return queriedItems;
        }
    }
}