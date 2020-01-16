using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

    [TemplatePart(Name = nameof(Part_TagsTree) , Type = typeof(TreeView))]
    [TemplatePart(Name = nameof(Part_CollapseAllButThis_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_CollapseAllButThis_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_CollapseAll_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_CollapseAll_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_ExpandAll_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_AutoClearCheckMarks_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_AutoClearCheckMarks_Btn), Type = typeof(ToggleButton))]
    [TemplatePart(Name = nameof(Part_ClearAllCheckMarks_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_ClearAllCheckMarks_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_InvertCheckMark_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_SetCheckMark_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_RemoveCheckMark_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_InfiniteSearch_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_InfiniteSearch_Btn), Type = typeof(ToggleButton))]
    [TemplatePart(Name = nameof(Part_MarkCheckedAsIncludable_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_MarkCheckedAsIncludable_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_MarkCheckedAsExcludable_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_MarkCheckedAsExcludable_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_MarkAllAsUndefined_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_MarkAllAsUndefined_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_Bind_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_Bind_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_Unbind_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_Unbind_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_AddTag_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_AddDir_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_AddDir_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_AddTag_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_RenameSelected_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_RenameSelected_Btn), Type = typeof(Button))]
    [TemplatePart(Name = nameof(Part_DeleteSelected_Mnu), Type = typeof(MenuItem))]
    [TemplatePart(Name = nameof(Part_DeleteChecked_Mnu), Type = typeof(MenuItem))]
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

            BindCommand(TagsTreeCtrlCommands.CollapseAllButThis, CollapseAllButThis_Executed, CollapseAllButThis_CanExecute);
            BindCommand(TagsTreeCtrlCommands.CollapseAll, CollapseAll_Executed, CollapseAll_CanExecute);
            BindCommand(TagsTreeCtrlCommands.ExpandAll, ExpandAll_Executed, ExpandAll_CanExecute);
            // -----
            BindCommand(TagsTreeCtrlCommands.AutoClearCheckMarks, AutoClearCheckMarks_Executed, AutoClearCheckMarks_CanExecute);
            BindCommand(TagsTreeCtrlCommands.ClearAllCheckMarks, ClearAllCheckMark_Executed, ClearAllCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.InvertCheckMark, InvertCheckMark_Executed, InvertCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.SetCheckMark, SetCheckMark_Executed, SetCheckMark_CanExecute);
            BindCommand(TagsTreeCtrlCommands.RemoveCheckMark, RemoveCheckMark_Executed, RemoveCheckMark_CanExecute);
            // -----
            BindCommand(TagsTreeCtrlCommands.InfiniteSearch, InfiniteSearch_Executed, InfiniteSearch_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkCheckedAsIncludable, MarkCheckedAsIncludable_Executed, MarkCheckedAsIncludable_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkCheckedAsExcludable, MarkCheckedAsExcludable_Executed, MarkCheckedAsExcludable_CanExecute);
            BindCommand(TagsTreeCtrlCommands.MarkAllAsUndefined, MarkAllAsUndefined_Executed, MarkAllAsUndefined_CanExecute);
            // -----
            BindCommand(TagsTreeCtrlCommands.Bind, Bind_Executed, Bind_CanExecute);
            BindCommand(TagsTreeCtrlCommands.Unbind, Unbind_Executed, Unbind_CanExecute);
            // -----
            BindCommand(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute);
            BindCommand(TagsTreeCtrlCommands.AddDir, AddDir_Executed, AddDir_CanExecute);
            BindCommand(TagsTreeCtrlCommands.RenameSelected, RenameSelected_Executed, RenameSelected_CanExecute);
            BindCommand(TagsTreeCtrlCommands.DeleteSelected, DeleteSelected_Executed, DeleteSelected_CanExecute);
            BindCommand(TagsTreeCtrlCommands.DeleteChecked, DeleteChecked_Executed, DeleteChecked_CanExecute);

            #endregion ————— Commands registration
        }

        private static void BindCommand(RoutedCommand command, ExecutedRoutedEventHandler executedHandler, CanExecuteRoutedEventHandler canExecuteHandler)
        {
            CommandManager.RegisterClassCommandBinding(typeof(TagsTreeCtrl), new CommandBinding(command, executedHandler, canExecuteHandler));
        }

        
    }
    #endregion ■■■■■ Base



    #region ■■■■■ ControlParts ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        private TreeView        Part_TagsTree;
        //
        private MenuItem        Part_CollapseAllButThis_Mnu;
        private Button          Part_CollapseAllButThis_Btn;
        private MenuItem        Part_CollapseAll_Mnu;
        private Button          Part_CollapseAll_Btn;
        private MenuItem        Part_ExpandAll_Mnu;
        //
        private MenuItem        Part_AutoClearCheckMarks_Mnu;
        private ToggleButton    Part_AutoClearCheckMarks_Btn;
        private MenuItem        Part_ClearAllCheckMarks_Mnu;
        private Button          Part_ClearAllCheckMarks_Btn;
        private MenuItem        Part_InvertCheckMark_Mnu;
        private MenuItem        Part_SetCheckMark_Mnu;
        private MenuItem        Part_RemoveCheckMark_Mnu;
        //
        private MenuItem        Part_InfiniteSearch_Mnu;
        private ToggleButton    Part_InfiniteSearch_Btn;
        private MenuItem        Part_MarkCheckedAsIncludable_Mnu;
        private Button          Part_MarkCheckedAsIncludable_Btn;
        private MenuItem        Part_MarkCheckedAsExcludable_Mnu;
        private Button          Part_MarkCheckedAsExcludable_Btn;
        private MenuItem        Part_MarkAllAsUndefined_Mnu;
        private Button          Part_MarkAllAsUndefined_Btn;
        //
        private MenuItem        Part_Bind_Mnu;
        private Button          Part_Bind_Btn;
        private MenuItem        Part_Unbind_Mnu;
        private Button          Part_Unbind_Btn;
        //
        private MenuItem        Part_AddTag_Mnu;
        private Button          Part_AddTag_Btn;
        private MenuItem        Part_AddDir_Mnu;
        private Button          Part_AddDir_Btn;
        private MenuItem        Part_RenameSelected_Mnu;
        private Button          Part_RenameSelected_Btn;
        private MenuItem        Part_DeleteSelected_Mnu;
        private MenuItem        Part_DeleteChecked_Mnu;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Part_TagsTree = FindTemplatePart<TreeView>(nameof(Part_TagsTree));
            //
            Part_CollapseAllButThis_Mnu = FindTemplatePart<MenuItem>(nameof(Part_CollapseAllButThis_Mnu));
            Part_CollapseAllButThis_Btn = FindTemplatePart<Button>(nameof(Part_CollapseAllButThis_Btn));
            Part_CollapseAll_Mnu = FindTemplatePart<MenuItem>(nameof(Part_CollapseAll_Mnu));
            Part_CollapseAll_Btn = FindTemplatePart<Button>(nameof(Part_CollapseAll_Btn));
            Part_ExpandAll_Mnu = FindTemplatePart<MenuItem>(nameof(Part_ExpandAll_Mnu));
            //
            Part_AutoClearCheckMarks_Mnu = FindTemplatePart<MenuItem>(nameof(Part_AutoClearCheckMarks_Mnu));
            Part_AutoClearCheckMarks_Btn = FindTemplatePart<ToggleButton>(nameof(Part_AutoClearCheckMarks_Btn));
            Part_ClearAllCheckMarks_Mnu = FindTemplatePart<MenuItem>(nameof(Part_ClearAllCheckMarks_Mnu));
            Part_ClearAllCheckMarks_Btn = FindTemplatePart<Button>(nameof(Part_ClearAllCheckMarks_Btn));
            Part_InvertCheckMark_Mnu = FindTemplatePart<MenuItem>(nameof(Part_InvertCheckMark_Mnu));
            Part_SetCheckMark_Mnu = FindTemplatePart<MenuItem>(nameof(Part_SetCheckMark_Mnu));
            Part_RemoveCheckMark_Mnu = FindTemplatePart<MenuItem>(nameof(Part_RemoveCheckMark_Mnu));
            //
            Part_InfiniteSearch_Mnu = FindTemplatePart<MenuItem>(nameof(Part_InfiniteSearch_Mnu));
            Part_InfiniteSearch_Btn = FindTemplatePart<ToggleButton>(nameof(Part_InfiniteSearch_Btn));
            Part_MarkCheckedAsIncludable_Mnu = FindTemplatePart<MenuItem>(nameof(Part_MarkCheckedAsIncludable_Mnu));
            Part_MarkCheckedAsIncludable_Btn = FindTemplatePart<Button>(nameof(Part_MarkCheckedAsIncludable_Btn));
            Part_MarkCheckedAsExcludable_Mnu = FindTemplatePart<MenuItem>(nameof(Part_MarkCheckedAsExcludable_Mnu));
            Part_MarkCheckedAsExcludable_Btn = FindTemplatePart<Button>(nameof(Part_MarkCheckedAsExcludable_Btn));
            Part_MarkAllAsUndefined_Mnu = FindTemplatePart<MenuItem>(nameof(Part_MarkAllAsUndefined_Mnu));
            Part_MarkAllAsUndefined_Btn = FindTemplatePart<Button>(nameof(Part_MarkAllAsUndefined_Btn));
            //
            Part_Bind_Mnu = FindTemplatePart<MenuItem>(nameof(Part_Bind_Mnu));
            Part_Bind_Btn = FindTemplatePart<Button>(nameof(Part_Bind_Btn));
            Part_Unbind_Mnu = FindTemplatePart<MenuItem>(nameof(Part_Unbind_Mnu));
            Part_Unbind_Btn = FindTemplatePart<Button>(nameof(Part_Unbind_Btn));
            //
            Part_AddTag_Mnu = FindTemplatePart<MenuItem>(nameof(Part_AddTag_Mnu));
            Part_AddTag_Btn = FindTemplatePart<Button>(nameof(Part_AddTag_Btn));
            Part_AddDir_Mnu = FindTemplatePart<MenuItem>(nameof(Part_AddDir_Mnu));
            Part_AddDir_Btn = FindTemplatePart<Button>(nameof(Part_AddDir_Btn));
            Part_RenameSelected_Mnu = FindTemplatePart<MenuItem>(nameof(Part_RenameSelected_Mnu));
            Part_RenameSelected_Btn = FindTemplatePart<Button>(nameof(Part_RenameSelected_Btn));
            Part_DeleteSelected_Mnu = FindTemplatePart<MenuItem>(nameof(Part_DeleteSelected_Mnu));
            Part_DeleteChecked_Mnu = FindTemplatePart<MenuItem>(nameof(Part_DeleteChecked_Mnu));
            //
            //
            SetUpTemplateParts();
        }

        private T FindTemplatePart<T>(String templatePartName) where T : DependencyObject
            => (GetTemplateChild(templatePartName) as T) ?? throw new NullReferenceException(templatePartName);

        private String GetToolTipText(RoutedUICommand command)
        {
            KeyGesture keyGesture = command.InputGestures[0] as KeyGesture;
            return keyGesture == null ? command.Text : $"{command.Text} [{keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture)}]";
        }

        private void SetUpTemplateParts()
        {
            Part_CollapseAllButThis_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.CollapseAllButThis));
            Part_CollapseAllButThis_Mnu.Header = TagsTreeCtrlCommands.CollapseAllButThis.Text;
            Part_CollapseAllButThis_Mnu.CommandTarget = this;
            Part_CollapseAllButThis_Mnu.Command = TagsTreeCtrlCommands.CollapseAllButThis;
            Part_CollapseAllButThis_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAllButThis, AddTag_Executed, AddTag_CanExecute));
            //
            Part_CollapseAllButThis_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.CollapseAllButThis));
            Part_CollapseAllButThis_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.CollapseAllButThis);
            Part_CollapseAllButThis_Btn.Command = TagsTreeCtrlCommands.CollapseAllButThis;
            Part_CollapseAllButThis_Btn.CommandTarget = Part_TagsTree;
            Part_CollapseAllButThis_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAllButThis, CollapseAllButThis_Executed, CollapseAllButThis_CanExecute));
            //
            Part_CollapseAll_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.CollapseAll));
            Part_CollapseAll_Mnu.Header = TagsTreeCtrlCommands.CollapseAll.Text;
            Part_CollapseAll_Mnu.CommandTarget = this;
            Part_CollapseAll_Mnu.Command = TagsTreeCtrlCommands.CollapseAll;
            Part_CollapseAll_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAll, AddTag_Executed, AddTag_CanExecute));
            //
            Part_CollapseAll_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.CollapseAll));
            Part_CollapseAll_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.CollapseAll);
            Part_CollapseAll_Btn.Command = TagsTreeCtrlCommands.CollapseAll;
            Part_CollapseAll_Btn.CommandTarget = Part_TagsTree;
            Part_CollapseAll_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.CollapseAll, ExpandAll_Executed, ExpandAll_CanExecute));
            //
            Part_ExpandAll_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.ExpandAll));
            Part_ExpandAll_Mnu.Header = TagsTreeCtrlCommands.ExpandAll.Text;
            Part_ExpandAll_Mnu.CommandTarget = this;
            Part_ExpandAll_Mnu.Command = TagsTreeCtrlCommands.ExpandAll;
            Part_ExpandAll_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ExpandAll, AddTag_Executed, AddTag_CanExecute));
            //
            Part_AutoClearCheckMarks_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.AutoClearCheckMarks));
            Part_AutoClearCheckMarks_Mnu.Header = TagsTreeCtrlCommands.AutoClearCheckMarks.Text;
            Part_AutoClearCheckMarks_Mnu.CommandTarget = this;
            Part_AutoClearCheckMarks_Mnu.Command = TagsTreeCtrlCommands.AutoClearCheckMarks;
            Part_AutoClearCheckMarks_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AutoClearCheckMarks, AddTag_Executed, AddTag_CanExecute));
            //
            Part_AutoClearCheckMarks_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.AutoClearCheckMarks));
            Part_AutoClearCheckMarks_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.AutoClearCheckMarks);            
            Part_AutoClearCheckMarks_Btn.IsChecked = true;
            //
            Part_ClearAllCheckMarks_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.ClearAllCheckMarks));
            Part_ClearAllCheckMarks_Mnu.Header = TagsTreeCtrlCommands.ClearAllCheckMarks.Text;
            Part_ClearAllCheckMarks_Mnu.CommandTarget = this;
            Part_ClearAllCheckMarks_Mnu.Command = TagsTreeCtrlCommands.ClearAllCheckMarks;
            Part_ClearAllCheckMarks_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ClearAllCheckMarks, AddTag_Executed, AddTag_CanExecute));
            //
            Part_ClearAllCheckMarks_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.ClearAllCheckMarks));
            Part_ClearAllCheckMarks_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.ClearAllCheckMarks);
            Part_ClearAllCheckMarks_Btn.Command = TagsTreeCtrlCommands.ClearAllCheckMarks;
            Part_ClearAllCheckMarks_Btn.CommandTarget = Part_TagsTree;
            Part_ClearAllCheckMarks_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.ClearAllCheckMarks, ClearAllCheckMark_Executed, ClearAllCheckMark_CanExecute));
            //            
            Part_InvertCheckMark_Mnu.Header = TagsTreeCtrlCommands.InvertCheckMark.Text;
            Part_InvertCheckMark_Mnu.CommandTarget = this;
            Part_InvertCheckMark_Mnu.Command = TagsTreeCtrlCommands.InvertCheckMark;
            Part_InvertCheckMark_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.InvertCheckMark, AddTag_Executed, AddTag_CanExecute));
            //
            Part_SetCheckMark_Mnu.Header = TagsTreeCtrlCommands.SetCheckMark.Text;
            Part_SetCheckMark_Mnu.CommandTarget = this;
            Part_SetCheckMark_Mnu.Command = TagsTreeCtrlCommands.SetCheckMark;
            Part_SetCheckMark_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.SetCheckMark, AddTag_Executed, AddTag_CanExecute));
            //
            Part_RemoveCheckMark_Mnu.Header = TagsTreeCtrlCommands.RemoveCheckMark.Text;
            Part_RemoveCheckMark_Mnu.CommandTarget = this;
            Part_RemoveCheckMark_Mnu.Command = TagsTreeCtrlCommands.RemoveCheckMark;
            Part_RemoveCheckMark_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.RemoveCheckMark, AddTag_Executed, AddTag_CanExecute));
            //
            Part_InfiniteSearch_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.InfiniteSearch));
            Part_InfiniteSearch_Mnu.Header = TagsTreeCtrlCommands.InfiniteSearch.Text;
            Part_InfiniteSearch_Mnu.CommandTarget = this;
            Part_InfiniteSearch_Mnu.Command = TagsTreeCtrlCommands.InfiniteSearch;
            Part_InfiniteSearch_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.InfiniteSearch, AddTag_Executed, AddTag_CanExecute));
            //
            Part_InfiniteSearch_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.InfiniteSearch));
            Part_InfiniteSearch_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.AutoClearCheckMarks);
            Part_InfiniteSearch_Btn.IsChecked = true;
            //
            Part_MarkCheckedAsIncludable_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.MarkCheckedAsIncludable));
            Part_MarkCheckedAsIncludable_Mnu.Header = TagsTreeCtrlCommands.MarkCheckedAsIncludable.Text;
            Part_MarkCheckedAsIncludable_Mnu.CommandTarget = this;
            Part_MarkCheckedAsIncludable_Mnu.Command = TagsTreeCtrlCommands.MarkCheckedAsIncludable;
            Part_MarkCheckedAsIncludable_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsIncludable, AddTag_Executed, AddTag_CanExecute));
            //
            Part_MarkCheckedAsIncludable_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.MarkCheckedAsIncludable));
            Part_MarkCheckedAsIncludable_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.MarkCheckedAsIncludable);
            Part_MarkCheckedAsIncludable_Btn.Command = TagsTreeCtrlCommands.MarkCheckedAsIncludable;
            Part_MarkCheckedAsIncludable_Btn.CommandTarget = Part_TagsTree;
            Part_MarkCheckedAsIncludable_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsIncludable, MarkCheckedAsIncludable_Executed, MarkCheckedAsIncludable_CanExecute));
            //
            Part_MarkCheckedAsExcludable_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.MarkCheckedAsExcludable));
            Part_MarkCheckedAsExcludable_Mnu.Header = TagsTreeCtrlCommands.MarkCheckedAsExcludable.Text;
            Part_MarkCheckedAsExcludable_Mnu.CommandTarget = this;
            Part_MarkCheckedAsExcludable_Mnu.Command = TagsTreeCtrlCommands.MarkCheckedAsExcludable;
            Part_MarkCheckedAsExcludable_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsExcludable, AddTag_Executed, AddTag_CanExecute));
            //
            Part_MarkCheckedAsExcludable_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.MarkCheckedAsExcludable));
            Part_MarkCheckedAsExcludable_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.MarkCheckedAsExcludable);
            Part_MarkCheckedAsExcludable_Btn.Command = TagsTreeCtrlCommands.MarkCheckedAsExcludable;
            Part_MarkCheckedAsExcludable_Btn.CommandTarget = Part_TagsTree;
            Part_MarkCheckedAsExcludable_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkCheckedAsExcludable, MarkCheckedAsExcludable_Executed, MarkCheckedAsExcludable_CanExecute));
            //
            Part_MarkAllAsUndefined_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.MarkAllAsUndefined));
            Part_MarkAllAsUndefined_Mnu.Header = TagsTreeCtrlCommands.MarkAllAsUndefined.Text;
            Part_MarkAllAsUndefined_Mnu.CommandTarget = this;
            Part_MarkAllAsUndefined_Mnu.Command = TagsTreeCtrlCommands.MarkAllAsUndefined;
            Part_MarkAllAsUndefined_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkAllAsUndefined, AddTag_Executed, AddTag_CanExecute));
            //
            Part_MarkAllAsUndefined_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.MarkAllAsUndefined));
            Part_MarkAllAsUndefined_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.MarkAllAsUndefined);
            Part_MarkAllAsUndefined_Btn.Command = TagsTreeCtrlCommands.MarkAllAsUndefined;
            Part_MarkAllAsUndefined_Btn.CommandTarget = Part_TagsTree;
            Part_MarkAllAsUndefined_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.MarkAllAsUndefined, MarkAllAsUndefined_Executed, MarkAllAsUndefined_CanExecute));
            //
            Part_Bind_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.Bind));
            Part_Bind_Mnu.Header = TagsTreeCtrlCommands.Bind.Text;
            Part_Bind_Mnu.CommandTarget = this;
            Part_Bind_Mnu.Command = TagsTreeCtrlCommands.Bind;
            Part_Bind_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Bind, AddTag_Executed, AddTag_CanExecute));
            //
            Part_Bind_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.Bind));
            Part_Bind_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.Bind);
            Part_Bind_Btn.Command = TagsTreeCtrlCommands.Bind;
            Part_Bind_Btn.CommandTarget = Part_TagsTree;
            Part_Bind_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Bind, Bind_Executed, Bind_CanExecute));
            //
            Part_Unbind_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.Unbind));
            Part_Unbind_Mnu.Header = TagsTreeCtrlCommands.Unbind.Text;
            Part_Unbind_Mnu.CommandTarget = this;
            Part_Unbind_Mnu.Command = TagsTreeCtrlCommands.Unbind;
            Part_Unbind_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Unbind, AddTag_Executed, AddTag_CanExecute));
            //
            Part_Unbind_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.Unbind));
            Part_Unbind_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.Unbind);
            Part_Unbind_Btn.Command = TagsTreeCtrlCommands.Unbind;
            Part_Unbind_Btn.CommandTarget = Part_TagsTree;
            Part_Unbind_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.Unbind, Unbind_Executed, Unbind_CanExecute));
            //
            Part_AddTag_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.AddTag));
            Part_AddTag_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.AddTag);
            Part_AddTag_Btn.Command = TagsTreeCtrlCommands.AddTag;
            Part_AddTag_Btn.CommandTarget = this;
            Part_AddTag_Btn.Command = TagsTreeCtrlCommands.AddTag;
            Part_AddTag_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute));
            //
            Part_AddTag_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.AddTag));
            Part_AddTag_Mnu.Header = TagsTreeCtrlCommands.AddTag.Text;
            Part_AddTag_Mnu.CommandTarget = this;
            Part_AddTag_Mnu.Command = TagsTreeCtrlCommands.AddTag;
            Part_AddTag_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddTag, AddTag_Executed, AddTag_CanExecute));
            //
            Part_AddDir_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.AddDir));
            Part_AddDir_Mnu.Header = TagsTreeCtrlCommands.AddDir.Text;
            Part_AddDir_Mnu.CommandTarget = this;
            Part_AddDir_Mnu.Command = TagsTreeCtrlCommands.AddDir;
            Part_AddDir_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddDir, AddTag_Executed, AddTag_CanExecute));
            //
            Part_AddDir_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.AddDir));
            Part_AddDir_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.AddDir);
            Part_AddDir_Btn.Command = TagsTreeCtrlCommands.AddDir;
            Part_AddDir_Btn.CommandTarget = Part_TagsTree;
            Part_AddDir_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.AddDir, AddDir_Executed, AddDir_CanExecute));
            //
            Part_RenameSelected_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.RenameSelected));
            Part_RenameSelected_Mnu.Header = TagsTreeCtrlCommands.RenameSelected.Text;
            Part_RenameSelected_Mnu.CommandTarget = this;
            Part_RenameSelected_Mnu.Command = TagsTreeCtrlCommands.RenameSelected;
            Part_RenameSelected_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.RenameSelected, AddTag_Executed, AddTag_CanExecute));
            //
            Part_RenameSelected_Btn.Content = GetImage(nameof(TagsTreeCtrlCommands.RenameSelected));
            Part_RenameSelected_Btn.ToolTip = GetToolTipText(TagsTreeCtrlCommands.RenameSelected);
            Part_RenameSelected_Btn.Command = TagsTreeCtrlCommands.RenameSelected;
            Part_RenameSelected_Btn.CommandTarget = Part_TagsTree;
            Part_RenameSelected_Btn.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.RenameSelected, RenameSelected_Executed, RenameSelected_CanExecute));
            //
            Part_DeleteSelected_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.DeleteSelected));
            Part_DeleteSelected_Mnu.Header = TagsTreeCtrlCommands.DeleteSelected.Text;
            Part_DeleteSelected_Mnu.CommandTarget = this;
            Part_DeleteSelected_Mnu.Command = TagsTreeCtrlCommands.DeleteSelected;
            Part_DeleteSelected_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.DeleteSelected, AddTag_Executed, AddTag_CanExecute));
            //
            Part_DeleteChecked_Mnu.Icon = GetImage(nameof(TagsTreeCtrlCommands.DeleteChecked));
            Part_DeleteChecked_Mnu.Header = TagsTreeCtrlCommands.DeleteChecked.Text;
            Part_DeleteChecked_Mnu.CommandTarget = this;
            Part_DeleteChecked_Mnu.Command = TagsTreeCtrlCommands.DeleteChecked;
            Part_DeleteChecked_Mnu.CommandBindings.Add(new CommandBinding(TagsTreeCtrlCommands.DeleteChecked, AddTag_Executed, AddTag_CanExecute));
            //
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
        #region ————— CollapseAllButThis ——————————————————————————————————————————————————————————————————————————————

        private static void CollapseAllButThis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void CollapseAllButThis_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— CollapseAllButThis

        #region ————— CollapseAll —————————————————————————————————————————————————————————————————————————————————————

        private static void CollapseAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void CollapseAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— CollapseAll

        #region ————— ExpandAll ———————————————————————————————————————————————————————————————————————————————————————

        private static void ExpandAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeView treeView = (sender as TagsTreeCtrl)?.Part_TagsTree;
            var items = treeView?.ItemsSource;
            if(items == null) return;
            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();

            foreach(var item in items)
            {
                TreeViewItem treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if(treeViewItem != null) treeViewItem.ExpandSubtree();
            }
        }

        private static void ExpandAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion ————— ExpandAll

        // -----

        #region ————— AutoClearCheckMarks —————————————————————————————————————————————————————————————————————————————

        private static void AutoClearCheckMarks_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void AutoClearCheckMarks_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— AutoClearCheckMarks

        #region ————— ClearAllCheckMark ———————————————————————————————————————————————————————————————————————————————

        private static void ClearAllCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void ClearAllCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— ClearAllCheckMark

        #region ————— InvertCheckMark —————————————————————————————————————————————————————————————————————————————————

        private static void InvertCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TagsTreeItem selectedItem = (sender as TagsTreeCtrl)?.Part_TagsTree?.SelectedItem as TagsTreeItem;
            if(selectedItem == null) return;
            TagsTreeItem.SetItemCheckMark(selectedItem, !(selectedItem.CheckMark == true));
        }

        private static void InvertCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion ————— InvertCheckMark

        #region ————— SetCheckMark ————————————————————————————————————————————————————————————————————————————————————

        private static void SetCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TagsTreeItem selectedItem = (sender as TagsTreeCtrl)?.Part_TagsTree?.SelectedItem as TagsTreeItem;
            if(selectedItem == null) return;
            TagsTreeItem.SetItemCheckMark(selectedItem, true);
        }

        private static void SetCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion ————— SetCheckMark

        #region ————— RemoveCheckMark —————————————————————————————————————————————————————————————————————————————————

        private static void RemoveCheckMark_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TagsTreeItem selectedItem = (sender as TagsTreeCtrl)?.Part_TagsTree?.SelectedItem as TagsTreeItem;
            if(selectedItem == null) return;
            TagsTreeItem.SetItemCheckMark(selectedItem, false);
        }

        private static void RemoveCheckMark_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion ————— RemoveCheckMark

        // -----

        #region ————— InfiniteSearch ——————————————————————————————————————————————————————————————————————————————————

        private static void InfiniteSearch_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void InfiniteSearch_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— InfiniteSearch

        #region ————— MarkCheckedAsIncludable —————————————————————————————————————————————————————————————————————————

        private static void MarkCheckedAsIncludable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkCheckedAsIncludable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— MarkCheckedAsIncludable

        #region ————— MarkCheckedAsExcludable —————————————————————————————————————————————————————————————————————————

        private static void MarkCheckedAsExcludable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkCheckedAsExcludable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— MarkCheckedAsExcludable

        #region ————— MarkAllAsUndefined ——————————————————————————————————————————————————————————————————————————————

        private static void MarkAllAsUndefined_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void MarkAllAsUndefined_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— MarkAllAsUndefined

        // -----

        #region ————— Bind ————————————————————————————————————————————————————————————————————————————————————————————

        private static void Bind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void Bind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— Bind

        #region ————— Unbind ——————————————————————————————————————————————————————————————————————————————————————————

        private static void Unbind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void Unbind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— Unbind

        // -----

        #region ————— AddTag ——————————————————————————————————————————————————————————————————————————————————————————

        private static void AddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void AddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— AddTag

        #region ————— AddDir ——————————————————————————————————————————————————————————————————————————————————————————

        private static void AddDir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void AddDir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— AddDir

        #region ————— RenameSelected ——————————————————————————————————————————————————————————————————————————————————

        private static void RenameSelected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void RenameSelected_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— RenameSelected

        #region ————— DeleteSelected ——————————————————————————————————————————————————————————————————————————————————

        private static void DeleteSelected_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void DeleteSelected_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— DeleteSelected

        #region ————— DeleteChecked ———————————————————————————————————————————————————————————————————————————————————

        private static void DeleteChecked_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show($"Command {((RoutedUICommand)e.Command).Text} not implemented");
        }

        private static void DeleteChecked_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        #endregion ————— DeleteChecked
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

        //private static Dictionary<String, Image> ImageResources = new Dictionary<string, Image>();

        private static Image GetImage(String imageName)
        {
            
            BitmapImage bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/ZControls.WPF;component/Resources/Icons/TagsTreeCtrlCommands_{imageName}_16x.png", UriKind.RelativeOrAbsolute));
            Image image = new Image();
            image.Source = bitmapImage;
            //ImageResources.Add(imageName, image);
            return image;
        }
    }
    #endregion ■■■■■ Temp


    
}
