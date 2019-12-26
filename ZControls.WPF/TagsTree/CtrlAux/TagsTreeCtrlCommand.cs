using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ZControls.WPF.TagsTree.CtrlAux
{
    public static class TagsTreeCtrlCommands
    {
        private static RoutedUICommand NewCommand(String commandName, String uiText, params KeyGesture[] keyGestures)
            => new RoutedUICommand(uiText, commandName, typeof(TagsTreeCtrlCommands), new InputGestureCollection(keyGestures));

        // ▬▬▬▬▬

        public static readonly RoutedUICommand CollapseAllButThis = NewCommand(nameof(CollapseAllButThis)
                , "Свернуть все кроме этой ветки"
                , new KeyGesture(Key.Add, ModifierKeys.Control));

        public static readonly RoutedUICommand CollapseAll = NewCommand(nameof(CollapseAll)
                , "Свернуть все"
                , new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedUICommand ExpandAll = NewCommand(nameof(ExpandAll)
                , "Развернуть все"
                , new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift));

        // -----

        public static readonly RoutedUICommand AutoClearCheckMarks = NewCommand(nameof(AutoClearCheckMarks)
                , "Снимать метки автоматически"
                , new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt));

        public static readonly RoutedUICommand ClearAllCheckMarks = NewCommand(nameof(ClearAllCheckMarks)
                , "Снять все метки"
                , new KeyGesture(Key.Space, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt));

        public static readonly RoutedUICommand InvertCheckMark = NewCommand(nameof(InvertCheckMark)
                , "Обратить метку"
                , new KeyGesture(Key.Space));

        public static readonly RoutedUICommand SetCheckMark = NewCommand(nameof(SetCheckMark)
                , "Установить метку"
                , new KeyGesture(Key.Space, ModifierKeys.Control));

        public static readonly RoutedUICommand RemoveCheckMark = NewCommand(nameof(RemoveCheckMark)
                , "Снять метку"
                , new KeyGesture(Key.Space, ModifierKeys.Control | ModifierKeys.Shift));

        // -----

        public static readonly RoutedUICommand InfiniteSearch = NewCommand(nameof(InfiniteSearch)
                , "Непрерывный поиск"
                , new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt));

        public static readonly RoutedUICommand MarkCheckedAsIncludable = NewCommand(nameof(MarkCheckedAsIncludable)
                , "Отмеченные как включаемые при поиске"
                , new KeyGesture(Key.Multiply, ModifierKeys.Control));

        public static readonly RoutedUICommand MarkCheckedAsExcludable = NewCommand(nameof(MarkCheckedAsExcludable)
                , "Отмеченные как исключаемые при поиске"
                , new KeyGesture(Key.Multiply, ModifierKeys.Control| ModifierKeys.Shift));

        public static readonly RoutedUICommand MarkAllAsUndefined = NewCommand(nameof(MarkAllAsUndefined)
                , "Сбросить настройки поиска"
                , new KeyGesture(Key.Multiply, ModifierKeys.Control| ModifierKeys.Shift));

        // -----

        public static readonly RoutedUICommand Bind = NewCommand(nameof(Bind)
                , "Связать"
                , new KeyGesture(Key.B, ModifierKeys.Control));

        public static readonly RoutedUICommand Unbind = NewCommand(nameof(Unbind)
                , "Отвязать"
                , new KeyGesture(Key.B, ModifierKeys.Control | ModifierKeys.Shift));

        // -----

        public static readonly RoutedUICommand AddTag = NewCommand(nameof(AddTag)
                , "Добавить тег"
                , new KeyGesture(Key.Q, ModifierKeys.Control));

        public static readonly RoutedUICommand AddDir = NewCommand(nameof(AddDir)
                , "Добавить папку"
                , new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift));

        public static readonly RoutedUICommand RenameSelected = NewCommand(nameof(RenameSelected)
                , "Переименовать"
                , new KeyGesture(Key.F2));

        public static readonly RoutedUICommand DeleteSelected = NewCommand(nameof(DeleteSelected)
                , "Удалить выделеный елемент"
                , new KeyGesture(Key.Delete, ModifierKeys.Control));

        public static readonly RoutedUICommand DeleteChecked = NewCommand(nameof(DeleteChecked)
                , "Удалить отмеченные елементы"
                , new KeyGesture(Key.Delete, ModifierKeys.Control | ModifierKeys.Shift));

    }
}
