using System.Windows.Input;

namespace AMS.Tool
{
    public static class CustomCommand
    {
        public static readonly RoutedUICommand Save = new RoutedUICommand
        (
            "Save",
            "Save",
            typeof(CustomCommand),
            new InputGestureCollection()
            {
                 new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Check = new RoutedUICommand
        (
            "Check",
            "Check",
            typeof(CustomCommand),
            new InputGestureCollection()
            {
                new KeyGesture(Key.C, ModifierKeys.Control)
            }
        );
    }
}
