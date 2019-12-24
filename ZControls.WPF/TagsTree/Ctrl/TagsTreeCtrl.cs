using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZControls.WPF.TagsTree.CtrlAux;
using ZControls.WPF.TagsTree.DataModel;

namespace ZControls.WPF.TagsTree.Ctrl
{
    #region ■■■■■ Base ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    [TemplatePart(Name = "Part_TagsTree", Type = typeof(TreeView))]
    public partial class TagsTreeCtrl : Control
    {
        public TagsTreeCtrl()
        {

        }

        static TagsTreeCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagsTreeCtrl), new FrameworkPropertyMetadata(typeof(TagsTreeCtrl)));

            #region ————— Dependency property registration ————————————————————————————————————————————————————————————

            StringPropProperty = DependencyProperty.Register(nameof(StringProp), typeof(String), typeof(TagsTreeCtrl),
                new FrameworkPropertyMetadata(default(String), new PropertyChangedCallback(OnDependencyPropChanged_StringProp)));

            #endregion ————— Dependency property registration

            #region ————— Routed events registraiton ——————————————————————————————————————————————————————————————————

            StringPropChangedEvent = EventManager.RegisterRoutedEvent(nameof(StringPropChanged), RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventArgs<String>), typeof(TagsTreeCtrl));

            #endregion ————— Routed events registraiton

            #region ————— Commands registration ———————————————————————————————————————————————————————————————————————

            BindCommand(TagsTreeCtrlCommands.AddDir, AddDir_Executed, AddDir_CanExecute);
            BindCommand(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute);
            BindCommand(TagsTreeCtrlCommands.EditSelected, EditSelected_Executed, EditSelected_CanExecute);
            BindCommand(TagsTreeCtrlCommands.DeleteSelected, DeleteSelected_Executed, DeleteSelected_CanExecute);
            BindCommand(TagsTreeCtrlCommands.DeleteChecked, DeleteChecked_Executed, DeleteChecked_CanExecute);
            BindCommand(TagsTreeCtrlCommands.InvertCheckMark, InvertCheckMark_Executed, InvertCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.SetCheckMark, SetCheckMark_Executed, SetCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.RemoveCheckMark, RemoveCheckMark_Executed, RemoveCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.ClearAllCheckMark, ClearAllCheckMark_Executed, ClearAllCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkCheckedAsIncludable, MarkCheckedAsIncludable_Executed, MarkCheckedAsIncludable_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkCheckedAsExcludable, MarkCheckedAsExcludable_Executed, MarkCheckedAsExcludable_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkAllAsUndefined, MarkAllAsUndefined_Executed, MarkAllAsUndefined_CanExecute);
            BindCommand(TagsTreeCtrlCommands.Bind, Bind_Executed, Bind_CanExecute);
            BindCommand(TagsTreeCtrlCommands.Unbind, Unbind_Executed, Unbind_CanExecute);
            BindCommand(TagsTreeCtrlCommands.ExpandAll, ExpandAll_Executed, ExpandAll_CanExecute);
            BindCommand(TagsTreeCtrlCommands.CollapseAll, CollapseAll_Executed, CollapseAll_CanExecute);
            BindCommand(TagsTreeCtrlCommands.CollapseAllButThis, CollapseAllButThis_Executed, CollapseAllButThis_CanExecute);

            #endregion ————— Commands registration
        }        

        private static void BindCommand(RoutedCommand command, ExecutedRoutedEventHandler executedHandler, CanExecuteRoutedEventHandler canExecuteHandler)
        {
            CommandManager.RegisterClassCommandBinding(typeof(TagsTreeCtrl), new CommandBinding(command, executedHandler, canExecuteHandler));
        }

        
    }
    #endregion ■■■■■ Base



    #region ■■■■■ ControlParts ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        private TreeView Part_TagsTree;
        private ToggleButton Part_AutoClearSelection;
        private Button Part_CollapseAllButThis;
        private Button Part_ExpandAll;
        private Button Part_ClearSelection;
        private Button Part_MarkAllAsUndefined;
        private ToggleButton Part_InfiniteSearch;
        private Button Part_MarkCheckedAsIncludable;
        private Button Part_MarkCheckedAsExcludable;
        private Button Part_EditSelected;
        private Button Part_AddFolder;
        private Button Part_AddTag;
        private Button Part_Bind;
        private Button Part_Unbind;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Part_TagsTree = FindTemplatePart<TreeView>(nameof(Part_TagsTree));
            Part_AutoClearSelection = FindTemplatePart<ToggleButton>(nameof(Part_AutoClearSelection));
            Part_CollapseAllButThis = FindTemplatePart<Button>(nameof(Part_CollapseAllButThis));
            Part_ExpandAll = FindTemplatePart<Button>(nameof(Part_ExpandAll));
            Part_ClearSelection = FindTemplatePart<Button>(nameof(Part_ClearSelection));
            Part_MarkAllAsUndefined = FindTemplatePart<Button>(nameof(Part_MarkAllAsUndefined));
            Part_InfiniteSearch = FindTemplatePart<ToggleButton>(nameof(Part_InfiniteSearch));
            Part_MarkCheckedAsIncludable = FindTemplatePart<Button>(nameof(Part_MarkCheckedAsIncludable));
            Part_MarkCheckedAsExcludable = FindTemplatePart<Button>(nameof(Part_MarkCheckedAsExcludable));
            Part_EditSelected = FindTemplatePart<Button>(nameof(Part_EditSelected));
            Part_AddFolder = FindTemplatePart<Button>(nameof(Part_AddFolder));
            Part_AddTag = FindTemplatePart<Button>(nameof(Part_AddTag));
            Part_Bind = FindTemplatePart<Button>(nameof(Part_Bind));
            Part_Unbind = FindTemplatePart<Button>(nameof(Part_Unbind));
            //
            SetUpTemplateParts();
        }

        private T FindTemplatePart<T>(String templatePartName) where T : DependencyObject
            => (GetTemplateChild(templatePartName) as T) ?? throw new NullReferenceException(templatePartName);

        private void SetUpTemplateParts()
        {
            //
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAllButThis, CollapseAllButThis_Executed, CollapseAllButThis_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ExpandAll, ExpandAll_Executed, ExpandAll_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ClearAllCheckMark, ClearAllCheckMark_Executed, ClearAllCheckMark_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkAllAsUndefined, MarkAllAsUndefined_Executed, MarkAllAsUndefined_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsIncludable, MarkCheckedAsIncludable_Executed, MarkCheckedAsIncludable_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsExcludable, MarkCheckedAsExcludable_Executed, MarkCheckedAsExcludable_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.EditSelected, EditSelected_Executed, EditSelected_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddDir, AddDir_Executed, AddDir_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Bind, Bind_Executed, Bind_CanExecute));
            Part_TagsTree.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Unbind, Unbind_Executed, Unbind_CanExecute));
            //
            Part_CollapseAllButThis.CommandTarget = Part_TagsTree;
            Part_CollapseAllButThis.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAllButThis, CollapseAllButThis_Executed, CollapseAllButThis_CanExecute));
            //
            Part_ExpandAll.CommandTarget = Part_TagsTree;
            Part_ExpandAll.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ExpandAll, ExpandAll_Executed, ExpandAll_CanExecute));
            //
            Part_ClearSelection.CommandTarget = Part_TagsTree;
            Part_ClearSelection.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ClearAllCheckMark, ClearAllCheckMark_Executed, ClearAllCheckMark_CanExecute));
            //
            Part_MarkAllAsUndefined.CommandTarget = Part_TagsTree;
            Part_MarkAllAsUndefined.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkAllAsUndefined, MarkAllAsUndefined_Executed, MarkAllAsUndefined_CanExecute));
            //
            Part_MarkCheckedAsIncludable.CommandTarget = Part_TagsTree;
            Part_MarkCheckedAsIncludable.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsIncludable, MarkCheckedAsIncludable_Executed, MarkCheckedAsIncludable_CanExecute));
            //
            Part_MarkCheckedAsExcludable.CommandTarget = Part_TagsTree;
            Part_MarkCheckedAsExcludable.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsExcludable, MarkCheckedAsExcludable_Executed, MarkCheckedAsExcludable_CanExecute));
            //
            Part_EditSelected.CommandTarget = Part_TagsTree;
            Part_EditSelected.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.EditSelected, EditSelected_Executed, EditSelected_CanExecute));
            //
            Part_AddFolder.CommandTarget = Part_TagsTree;
            Part_AddFolder.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddDir, AddDir_Executed, AddDir_CanExecute));
            //
            Part_AddTag.CommandTarget = Part_TagsTree;
            Part_AddTag.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute));
            //
            Part_Bind.CommandTarget = Part_TagsTree;
            Part_Bind.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Bind, Bind_Executed, Bind_CanExecute));
            //
            Part_Unbind.CommandTarget = Part_TagsTree;
            Part_Unbind.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Unbind, Unbind_Executed, Unbind_CanExecute));
            //
            Temp_SetupItemsCollection();
        }

    }
    #endregion ■■■■■ ControlParts



    #region ■■■■■ Properties ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        #region ————— StringPropProperty ———————————————————————————————————————————————————————————————————————————

        public static DependencyProperty StringPropProperty;

        public String StringProp
        {
            get => (String)GetValue(StringPropProperty);
            set => SetValue(StringPropProperty, value);
        }

        private static void OnDependencyPropChanged_StringProp(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TagsTreeCtrl ctrl = sender as TagsTreeCtrl;
            if(ctrl == null) return;
            String oldValue = (String)e.OldValue;
            String newValue = (String)e.NewValue;
            RoutedPropertyChangedEventArgs<String> args = new RoutedPropertyChangedEventArgs<String>(oldValue, newValue);
            args.RoutedEvent = TagsTreeCtrl.StringPropChangedEvent;
            ctrl.RaiseEvent(args);
        }

        #endregion ————— StringPropProperty
    }
    #endregion ■■■■■ Properties



    #region ■■■■■ Events ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        #region ————— StringPropChanged ———————————————————————————————————————————————————————————————————————————————————

        public static readonly RoutedEvent StringPropChangedEvent;

        public event RoutedPropertyChangedEventHandler<String> StringPropChanged
        {
            add => AddHandler(StringPropChangedEvent, value);
            remove => RemoveHandler(StringPropChangedEvent, value);
        }

        #endregion ————— StringPropChanged
    }
    #endregion ■■■■■ Events



    #region ■■■■■ Commands ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        #region ————— AddDir ——————————————————————————————————————————————————————————————————————————————————————————

        private static void AddDir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void AddDir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— AddDir

        #region ————— AddTag ——————————————————————————————————————————————————————————————————————————————————————————

        private static void AddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void AddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            MessageBox.Show($"Command {e.Command} not implemented");
        }

        #endregion ————— AddTag

        #region ————— EditSelected ————————————————————————————————————————————————————————————————————————————————————

        private static void EditSelected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void EditSelected_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— EditSelected

        #region ————— DeleteSelected ——————————————————————————————————————————————————————————————————————————————————

        private static void DeleteSelected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void DeleteSelected_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— DeleteSelected

        #region ————— DeleteChecked ———————————————————————————————————————————————————————————————————————————————————

        private static void DeleteChecked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void DeleteChecked_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— DeleteChecked

        #region ————— InvertCheckMark —————————————————————————————————————————————————————————————————————————————————

        private static void InvertCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void InvertCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— InvertCheckMark

        #region ————— SetCheckMark ————————————————————————————————————————————————————————————————————————————————————

        private static void SetCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void SetCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— SetCheckMark

        #region ————— RemoveCheckMark —————————————————————————————————————————————————————————————————————————————————

        private static void RemoveCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void RemoveCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— RemoveCheckMark

        #region ————— ClearAllCheckMark ———————————————————————————————————————————————————————————————————————————————

        private static void ClearAllCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void ClearAllCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— ClearAllCheckMark

        #region ————— MarkCheckedAsIncludable —————————————————————————————————————————————————————————————————————————

        private static void MarkCheckedAsIncludable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkCheckedAsIncludable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— MarkCheckedAsIncludable

        #region ————— MarkCheckedAsExcludable —————————————————————————————————————————————————————————————————————————

        private static void MarkCheckedAsExcludable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkCheckedAsExcludable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— MarkCheckedAsExcludable

        #region ————— MarkAllAsUndefined ——————————————————————————————————————————————————————————————————————————————

        private static void MarkAllAsUndefined_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkAllAsUndefined_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— MarkAllAsUndefined

        #region ————— Bind ————————————————————————————————————————————————————————————————————————————————————————————

        private static void Bind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void Bind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— Bind

        #region ————— Unbind ——————————————————————————————————————————————————————————————————————————————————————————

        private static void Unbind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void Unbind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— Unbind

        #region ————— ExpandAll ———————————————————————————————————————————————————————————————————————————————————————

        private static void ExpandAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void ExpandAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— ExpandAll

        #region ————— CollapseAll —————————————————————————————————————————————————————————————————————————————————————

        private static void CollapseAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void CollapseAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— CollapseAll

        #region ————— CollapseAllButThis ——————————————————————————————————————————————————————————————————————————————

        private static void CollapseAllButThis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void CollapseAllButThis_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— CollapseAllButThis
    }
    #endregion ■■■■■ Commands



    #region ■■■■■ Temp ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        private void Temp_SetupItemsCollection()
        {
            TagsDir dirMain = new TagsDir("_main");
            Tag dirAllActiveUsers = new Tag("Работающие пользователи", dirMain);
            Tag dirAllInactiveUsers = new Tag("Уволенные пользователи", dirMain);
            Tag dirAllUsers = new Tag("Все пользователи", dirMain);

            TagsDir dirUserGrouping = new TagsDir("Пользовательские наборы");
            TagsDir dirUsersByPositions = new TagsDir("Пользьватели по должностям", dirUserGrouping);
            TagsDir economists = new TagsDir("Экономисты", dirUsersByPositions);
            TagsDir lavyers = new TagsDir("Юристы", dirUsersByPositions);
            TagsDir accountants = new TagsDir("Бухгалтеры", dirUsersByPositions);

            ObservableCollection<TagsDir> dirs = new ObservableCollection<TagsDir>
            {
                dirMain,
                dirUserGrouping
            };

            Part_TagsTree.ItemsSource = dirs;

        }
    }
    #endregion ■■■■■ Temp
}
