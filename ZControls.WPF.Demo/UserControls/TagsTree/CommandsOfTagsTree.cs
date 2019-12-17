using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ZControls.WPF.Demo.UserControls
{
    public static class CommandsOfTagsTree
    {
        public static readonly RoutedCommand ExpandAll
            = new RoutedUICommand("Развернуть Все"
            , nameof(ExpandAll)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand ExpandTree
            = new RoutedUICommand("Развернуть это дерево"
            , nameof(ExpandTree)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Add, ModifierKeys.Control) });

        public static readonly RoutedCommand ColapseAll
            = new RoutedUICommand("Свернуть все"
            , nameof(ColapseAll)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand ColapseAllButThisTree
            = new RoutedUICommand("Свернуть все кроме этого дерева"
            , nameof(ColapseAllButThisTree)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Subtract, ModifierKeys.Alt) });

        public static readonly RoutedCommand ColapseAllButThisBranch
            = new RoutedUICommand("Свернуть все кроме этой ветки"
            , nameof(ColapseAllButThisBranch)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Subtract, ModifierKeys.Alt | ModifierKeys.Shift) });

        public static readonly RoutedCommand ColapseTree
            = new RoutedUICommand("Свернуть это дерево"
            , nameof(ColapseTree)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Subtract, ModifierKeys.Control) });

        public static readonly RoutedCommand AddDir
            = new RoutedUICommand("Добавить папку"
            , nameof(AddDir)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.End, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand AddTag
            = new RoutedUICommand("Добавить тег"
            , nameof(AddTag)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.End, ModifierKeys.Control) });

        public static readonly RoutedCommand EditItems
            = new RoutedUICommand("Редактировать елемент"
            , nameof(EditItems)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.F2) });

        public static readonly RoutedCommand DeleteCheckedItems
            = new RoutedUICommand("Удалить елемент"
            , nameof(DeleteCheckedItems)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Delete, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand BindItem
            = new RoutedUICommand("Связать"
            , nameof(BindItem)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control) });

        public static readonly RoutedCommand UnbindItem
            = new RoutedUICommand("Отвязать"
            , nameof(UnbindItem)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand CheckedTagsStateAsIncluded
            = new RoutedUICommand("Включать"
            , nameof(CheckedTagsStateAsIncluded)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });

        public static readonly RoutedCommand CheckedTagsStateAsExcluded
            = new RoutedUICommand("Исключать"
            , nameof(CheckedTagsStateAsExcluded)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift) });

        public static readonly RoutedCommand CheckedTagsStateAsNone
            = new RoutedUICommand("Не задано"
            , nameof(CheckedTagsStateAsNone)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Alt) });

        public static readonly RoutedCommand SetCheckMark
            = new RoutedUICommand("Установить метку выделения"
            , nameof(SetCheckMark)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Space) });

        public static readonly RoutedCommand ClearCheckMark
            = new RoutedUICommand("Снять метку выделения"
            , nameof(ClearCheckMark)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Space, ModifierKeys.Control) });

        public static readonly RoutedCommand InvertCheckMark
            = new RoutedUICommand("Инвертировать метку выделения"
            , nameof(InvertCheckMark)
            , typeof(CommandsOfTagsTree)
            , new InputGestureCollection() { new KeyGesture(Key.Space, ModifierKeys.Control | ModifierKeys.Shift) });
    }
}
