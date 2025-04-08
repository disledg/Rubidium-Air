using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rubidium.UI.UC
{
    public partial class sideCategoryButton : UserControl
    {
        // Стандартное свойство Content (переопределяем)
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(object),
                typeof(sideCategoryButton),
                new PropertyMetadata(null));

        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        // Свойство Command
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(sideCategoryButton),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Свойство CommandParameter
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(sideCategoryButton),
                new PropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public sideCategoryButton()
        {
            InitializeComponent();
        }
    }
}