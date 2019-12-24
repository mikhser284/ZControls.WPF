using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ZControls.WPF.TagsTree.CtrlAux
{
    public static class TagsTreeCtrlCommands
    {
        private static RoutedUICommand NewCommand(String commandName, params KeyGesture[] keyGestures)
            => new RoutedUICommand(commandName, commandName, typeof(TagsTreeCtrlCommands), new InputGestureCollection(keyGestures));

        // ▬▬▬▬▬

        public static readonly RoutedCommand AddDir
            = NewCommand(nameof(AddDir), new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedCommand AddTag
            = NewCommand(nameof(AddTag), new KeyGesture(Key.Add, ModifierKeys.Control));

        public static readonly RoutedCommand EditSelected
            = NewCommand(nameof(EditSelected), new KeyGesture(Key.F2));

        public static readonly RoutedCommand DeleteSelected
            = NewCommand(nameof(DeleteSelected), new KeyGesture(Key.Delete, ModifierKeys.Control));

        public static readonly RoutedCommand DeleteChecked
            = NewCommand(nameof(DeleteChecked), new KeyGesture(Key.Delete, ModifierKeys.Control | ModifierKeys.Shift));

        // -----

        public static readonly RoutedCommand InvertCheckMark
            = NewCommand(nameof(InvertCheckMark), new KeyGesture(Key.Space));

        public static readonly RoutedCommand SetCheckMark
            = NewCommand(nameof(SetCheckMark), new KeyGesture(Key.Space, ModifierKeys.Control));

        public static readonly RoutedCommand RemoveCheckMark
            = NewCommand(nameof(RemoveCheckMark), new KeyGesture(Key.Space, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedCommand ClearAllCheckMark
            = NewCommand(nameof(ClearAllCheckMark), new KeyGesture(Key.Space, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt));

        // -----

        public static readonly RoutedCommand MarkCheckedAsIncludable
            = NewCommand(nameof(MarkCheckedAsIncludable), new KeyGesture(Key.Multiply, ModifierKeys.Control));

        public static readonly RoutedCommand MarkCheckedAsExcludable
            = NewCommand(nameof(MarkCheckedAsExcludable), new KeyGesture(Key.Multiply, ModifierKeys.Control| ModifierKeys.Shift));

        public static readonly RoutedCommand MarkAllAsUndefined
            = NewCommand(nameof(MarkAllAsUndefined), new KeyGesture(Key.Multiply, ModifierKeys.Control| ModifierKeys.Shift));

        // -----

        public static readonly RoutedCommand Bind
            = NewCommand(nameof(Bind), new KeyGesture(Key.B, ModifierKeys.Control));

        public static readonly RoutedCommand Unbind
            = NewCommand(nameof(Unbind), new KeyGesture(Key.B, ModifierKeys.Control | ModifierKeys.Shift));

        // -----

        public static readonly RoutedCommand ExpandAll
            = NewCommand(nameof(ExpandAll), new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedCommand CollapseAll
            = NewCommand(nameof(CollapseAll), new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedCommand CollapseAllButThis
            = NewCommand(nameof(CollapseAllButThis), new KeyGesture(Key.Add, ModifierKeys.Control));
    }
}
