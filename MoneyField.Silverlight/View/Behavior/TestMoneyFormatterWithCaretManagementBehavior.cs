//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using MoneyField.Silverlight.Code;

//namespace MoneyField.Silverlight.View.Behavior
//{
//    /// <summary>
//    /// Поведение Silverlight, задающее коррекнтую позицию курсора при форматировании строки типа Money.
//    /// MoneyFormatterWithCaretManagementBehavior
//    /// </summary>
//    public sealed class TestMoneyFormatterWithCaretManagementBehavior : Behavior<TextBox>
//    {

//        protected override void OnAttached()
//        {
//            base.OnAttached();

//            AssociatedObject.Loaded += Loaded;
//            AssociatedObject.Unloaded += Unloaded;
//        }

//        protected override void OnDetaching()
//        {
//            base.OnDetaching();

//            AssociatedObject.Loaded -= Loaded;
//            AssociatedObject.Unloaded -= Unloaded;
//        }

//        private void Loaded(object sender, RoutedEventArgs routedEventArgs)
//        {


//            var textBox = AssociatedObject;
//            var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);

//            var binding = new Binding(bindingExpression.ParentBinding)
//                {
//                    // it3xl.ru: Только сами определяем, когда обновлять источник.
//                    UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
//                    Mode = BindingMode.TwoWay,

//                    // it3xl.ru: Все конверторы удаляем, т.к. они только мешают.
//                    Converter = null
//                };
//            textBox.SetBinding(TextBox.TextProperty, binding);

//            textBox.TextChanged += textBox_TextChanged;

//            textBox.SelectionChanged += textBox_SelectionChanged;

//            textBox.TextInput += textBox_TextInput;
//            textBox.TextInputStart += textBox_TextInputStart;
//            textBox.TextInputUpdate += textBox_TextInputUpdate;

//        }

//        void textBox_TextChanged(object sender, TextChangedEventArgs e)
//        {
//            var textBox = AssociatedObject;
//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("+textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}", textBox.SelectionStart, textBox.SelectionLength, textBox.Text));


//            var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
//            bindingExpression.UpdateSource();
//        }


//        void textBox_SelectionChanged(object sender, RoutedEventArgs e)
//        {
//            var textBox = AssociatedObject;
//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("+textBox_SelectionChanged. SelectionStart = {0}. SelectionLength = {1}", textBox.SelectionStart, textBox.SelectionLength));
//        }


//        void textBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
//        {
//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("+textBox_TextInput. Text = {0}. TextComposition = {1}", e.Text, e.TextComposition.CompositionText));
//        }

//        void textBox_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
//        {
//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("+textBox_TextInputStart. Text = {0}. TextComposition = {1}", e.Text, e.TextComposition.CompositionText));
//        }

//        void textBox_TextInputUpdate(object sender, System.Windows.Input.TextCompositionEventArgs e)
//        {
//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("+textBox_TextInputUpdate. Text = {0}. TextComposition = {1}", e.Text, e.TextComposition.CompositionText));
//        }





























//        private void Unloaded(object sender, RoutedEventArgs e)
//        {

//        }





//    }
//}
