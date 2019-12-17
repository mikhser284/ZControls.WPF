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

    public partial class TagsTree : UserControl, INotifyPropertyChanged
    {
        private TreeOfTags tTree = new TreeOfTags();

        public void GetTagsByState(out HashSet<Int32> redTags, out HashSet<Int32> greenTags, out HashSet<Int32> blueTags)
            => tTree.GetTagsByState(out redTags, out greenTags, out blueTags);


        public ObservableCollection<TagableObject> TagableObjectsCollection { get; set; }

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
            var dirA = tTree.AddDir("Dir A");
            var dirA_A = tTree.AddDir("Dir A-A", dirA);
            var tag007 = tTree.AddTag("Tag 007", dirA_A);
            var tag008 = tTree.AddTag("Tag 008", dirA_A);
            var tag009 = tTree.AddTag("Tag 009", dirA_A);
            var dirA_B = tTree.AddDir("Dir A-B", dirA);
            var dirA_B_A = tTree.AddDir("Dir A-B", dirA_B);
            var tag013 = tTree.AddTag("Tag 007", dirA_B_A);
            var tag014 = tTree.AddTag("Tag 008", dirA_B_A);
            var tag015 = tTree.AddTag("Tag 009", dirA_B_A);
            var tag010 = tTree.AddTag("Tag 007", dirA_B);
            var tag011 = tTree.AddTag("Tag 008", dirA_B);
            var tag012 = tTree.AddTag("Tag 009", dirA_B);
            var dirA_C = tTree.AddDir("Dir A-C", dirA);
            var tag004 = tTree.AddTag("Tag 004", dirA);
            var tag005 = tTree.AddTag("Tag 005", dirA);
            var tag006 = tTree.AddTag("Tag 006", dirA);
            var dirB = tTree.AddDir("Dir B");
            var dirC = tTree.AddDir("Dir C");
            var dirD = tTree.AddDir("Dir D");
            var tag001 = tTree.AddTag("Tag 001");
            var tag002 = tTree.AddTag("Tag 002");
            var tag003 = tTree.AddTag("Tag 003");
            //
            Ctrl_TagsTree.ItemsSource = tTree.Items;
        }

        


        private void TagsTreeItem_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;
            StackPanel stackPanel = checkBox.Parent as StackPanel;
            if (stackPanel == null) return;
            ITagTreeItem tagsTreeItem = stackPanel.DataContext as ITagTreeItem;
            tTree.SelectItem(tagsTreeItem, (Boolean)checkBox.IsChecked);
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

            foreach(var item in tTree.Items)
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
            TagsDir newDir = tTree.AddDir("New dir", tagsTreeItem as TagsDir);
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
            tTree.AddTag("New tag", tagsTreeItem as TagsDir);
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
            tTree.ForEach(x => x.IsSelected == true, (x) => { if (x.Parent is TagsDir dir) dir.Items.Remove(x); x.Parent = null; });
        }

        private void DeleteCheckedItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        private Boolean IsAnyItemChecked()
        {
            Boolean thereAreSelectedItems = false;
            foreach (var item in tTree.Items)
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
            List<TagableObject> selectedItems = TagableObjectsCollection.GetQueriedItems(x => x.IsSelected);
            List<Tag> selectedTags = tTree.GetSelectedTags().OrderBy(x => x.Name).ToList();
            selectedItems.ForEach(item => selectedTags.ForEach(tag => item.TagsIds.Add(tag.Id)));
            Dictionary<Int32, Tag> tagsDictionary = tTree.GetTagsDictionary();
            foreach(var item in selectedItems)
            {
                List<String> tagsNames = new List<string>();
                foreach (Int32 tagId in item.TagsIds) tagsNames.Add($"[#{tagsDictionary[tagId].Name}]");
                item.TagsNames = String.Join("; ", tagsNames.OrderBy(x => x));
            }
        }

        private void BindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ UnbindItem ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void UnbindItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void UnbindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ CheckedTagsStateAsIncluded ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsIncluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Include);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void CheckedTagsStateAsIncluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ CheckedTagsStateAsExcluded ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsExcluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Exclude);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void CheckedTagsStateAsExcluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ CheckedTagsStateAsNone ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void CheckedTagsStateAsNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Undefined);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void CheckedTagsStateAsNone_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsAnyItemChecked();
        }

        // ▬▬▬▬▬ SetCheckMark ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬

        private void SetCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            tTree.SelectItem(tagsTreeItem, true);
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
            tTree.SelectItem(tagsTreeItem, false);
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
            tTree.SelectItem(tagsTreeItem, !selection);
        }

        private void InvertCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Ctrl_TagsTree.SelectedItem != null;
        }
    }


    public static class Ext_Linq
    {
        public static List<T> GetQueriedItems<T>(this ObservableCollection<T> itemsSet, Func<T, Boolean> selector)
        {
            List<T> queriedItems = new List<T>();
            foreach(T item in itemsSet) if (selector(item)) queriedItems.Add(item);
            return queriedItems;
        }
    }
}
