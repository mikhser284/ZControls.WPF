using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        

        private 

        TreeOfTags tTree = new TreeOfTags();

        public TagsTree()
        {
            InitializeComponent();

            GenerateTestData();
            BindCommandsAndHandlers(Ctrl_TagsTree);
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

        private void BindCommandsAndHandlers(TreeView treeView)
        {
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ExpandAll, CommandExpandAll_Executed, CommandExpandAll_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ExpandTree, CommandExpandTree_Executed, CommandExpandTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAll, CommandColapseAll_Executed, CommandColapseAll_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAllButThisTree, CommandColapseAllButThisTree_Executed, CommandColapseAllButThisTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseAllButThisBranch, CommandColapseAllButThisBranch_Executed, CommandColapseAllButThisBranch_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ColapseTree, CommandColapseTree_Executed, CommandColapseTree_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.AddDir, CommandAddDir_Executed, CommandAddDir_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.AddTag, CommandAddTag_Executed, CommandAddTag_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.EditItems, CommandEditItems_Executed, CommandEditItems_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.DeleteCheckedItems, CommandDeleteCheckedItems_Executed, CommandDeleteCheckedItems_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.BindItem, CommandBindItem_Executed, CommandBindItem_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.UnbindItem, CommandUnbindItem_Executed, CommandUnbindItem_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsIncluded, CommandCheckedTagsStateAsIncluded_Executed, CommandCheckedTagsStateAsIncluded_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsExcluded, CommandCheckedTagsStateAsExcluded_Executed, CommandCheckedTagsStateAsExcluded_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.CheckedTagsStateAsNone, CommandCheckedTagsStateAsNone_Executed, CommandCheckedTagsStateAsNone_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.SetCheckMark, CommandSetCheckMark_Executed, CommandSetCheckMark_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.ClearCheckMark, CommandClearCheckMark_Executed, CommandClearCheckMark_CanExecute));
            treeView.CommandBindings.Add(new CommandBinding(CommandsOfTagsTree.InvertCheckMark, CommandInvertCheckMark_Executed, CommandInvertCheckMark_CanExecute));
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

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MarkAsIncludable_Click(object sender, RoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Include);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void MarkAsExcludable_Click(object sender, RoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Exclude);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void MarkAsNone_Click(object sender, RoutedEventArgs e)
        {
            tTree.ChangeStateOfSelectedTags(ETagState.Undefined);
            tTree.ApplyToSelectedItems(x => x.IsSelected = false);
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnLink_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }


    public partial class TagsTree
    {
        private void CommandExpandAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();

            foreach(var item in tTree.Items)
            {
                TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeViewItem != null) treeViewItem.ExpandSubtree();
            }
        }

        private void CommandExpandAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandExpandTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewItem treeViewItem = Ctrl_TagsTree.ItemContainerGenerator.ContainerFromItem(Ctrl_TagsTree.SelectedItem) as TreeViewItem;
            if (treeViewItem != null) treeViewItem.ExpandSubtree();
        }

        private void CommandExpandTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandColapseAll_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void CommandColapseAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandColapseAllButThisTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandColapseAllButThisTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandColapseAllButThisBranch_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandColapseAllButThisBranch_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandColapseTree_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandColapseTree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void CommandAddDir_Executed(object sender, ExecutedRoutedEventArgs e)
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


        private void CommandAddDir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandAddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            while (tagsTreeItem != null)
            {
                if (tagsTreeItem is TagsDir) break;
                tagsTreeItem = tagsTreeItem.Parent;
            }
            tTree.AddTag("New tag", tagsTreeItem as TagsDir);
        }

        private void CommandAddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandEditItems_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IsInEditMode = true;
        }

        private void CommandEditItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void CommandDeleteCheckedItems_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tTree.ForEach(x => x.IsSelected == true, (x) => { if (x.Parent is TagsDir dir) dir.Items.Remove(x); x.Parent = null; });
        }

        private void CommandDeleteCheckedItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Boolean thereAreSelectedItems = false;
            foreach(var item in tTree.Items)
            {
                if(item.IsSelected == false) continue;
                thereAreSelectedItems = true;
                break;
            }
            e.CanExecute = thereAreSelectedItems;
        }


        private void CommandBindItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandUnbindItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandUnbindItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandCheckedTagsStateAsIncluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandCheckedTagsStateAsIncluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandCheckedTagsStateAsExcluded_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandCheckedTagsStateAsExcluded_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandCheckedTagsStateAsNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandCheckedTagsStateAsNone_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        private void CommandSetCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            tTree.SelectItem(tagsTreeItem, true);
        }

        private void CommandSetCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            e.CanExecute = tagsTreeItem != null && tagsTreeItem.IsSelected != true;
        }


        private void CommandClearCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            tTree.SelectItem(tagsTreeItem, false);
        }

        private void CommandClearCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            e.CanExecute = tagsTreeItem != null && tagsTreeItem.IsSelected != false;
        }


        private void CommandInvertCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ITagTreeItem tagsTreeItem = Ctrl_TagsTree.SelectedItem as ITagTreeItem;
            Boolean selection = tagsTreeItem.IsSelected == null ? true : (Boolean)tagsTreeItem.IsSelected;
            tTree.SelectItem(tagsTreeItem, !selection);
        }

        private void CommandInvertCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Ctrl_TagsTree.SelectedItem != null;
        }
    }
}
