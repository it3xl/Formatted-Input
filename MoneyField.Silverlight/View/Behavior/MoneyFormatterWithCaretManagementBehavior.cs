//using System;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using MoneyField.Silverlight.Extention;
//using MoneyField.Silverlight.View.Converter;

//namespace MoneyField.Silverlight.View.Behavior
//{
//    /// <summary>
//    /// Поведение Silverlight, задающее коррекнтую позицию курсора при форматировании строки типа Money.
//    /// MoneyFormatterWithCaretManagementBehavior
//    /// </summary>
//    public sealed class MoneyFormatterWithCaretManagementBehavior : Behavior<TextBox>
//    {
//        private TextBox TextBoxControl
//        {
//            get
//            {
//                return AssociatedObject;
//            }
//        }

//        /// <summary>
//        /// Текущий отображаемый ползователю текст.
//        /// </summary>
//        public String CurrentText
//        {
//            get
//            {
//                return TextBoxControl.Text;
//            }
//        }


//        protected override void OnAttached()
//        {
//            base.OnAttached();

//            AssociatedObject.Loaded += LoadedHandler;
//            AssociatedObject.Unloaded += UnloadedHandler;
//        }

//        protected override void OnDetaching()
//        {
//            base.OnDetaching();

//            AssociatedObject.Loaded -= LoadedHandler;
//            AssociatedObject.Unloaded -= UnloadedHandler;
//        }

//        private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
//        {
//            CorrectBinding(TextBoxControl);

//            TextBoxControl.TextChanged += textBox_TextChanged;
//        }

//        /// <summary>
//        /// Проверит и настроит биндинг свойства <see cref="TextBox.TextProperty"/>, чтоб удовлетворял логики данного класса.
//        /// </summary>
//        /// <param name="textBox"></param>
//        private void CorrectBinding(TextBox textBox)
//        {
//            textBox
//                .GetBindingExpression(TextBox.TextProperty)
//                .InvokeNotNull(el =>
//                {
//                    var binding = new Binding(el.ParentBinding)
//                        {
//                            // it3xl.ru: Только сами определяем, когда обновлять источник.
//                            UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
//                            Mode = BindingMode.TwoWay,

//                            // it3xl.ru: Биндинг правильно может работать только с нашим конвертером.
//                            Converter = new NullableDoubleConverterTwoWay(),
//                        };

//                    textBox.SetBinding(TextBox.TextProperty, binding);
//                });
//        }

//        void textBox_TextChanged(object sender, TextChangedEventArgs e)
//        {






//            NullableDoubleConverterTwoWay.WriteLogAction(String.Format("textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
//                TextBoxControl.SelectionStart,
//                TextBoxControl.SelectionLength,
//                TextBoxControl.Text)
//            );



//            Boolean textFormatted;
//            FormatTextAndManageCaretRecursion(out textFormatted);

//            if (textFormatted == false)
//            {
//                // Если текст изменен из-за форматировани, то вызывать обновление не нужно,
//                //  т.к. следом будет повторное событие TextChanged и в нем обновим источник.

//                TextBoxControl
//                    .GetBindingExpression(TextBox.TextProperty)
//                    .InvokeNotNull(el => el.UpdateSource());
//            }
//        }


//        /// <summary>
//        /// Отформатируем значение и выставим каретку куросора в правильное положение.
//        /// ! Внимание, этот метод вызывает рекурсию, если текст в нем менялся через событие TextChanged.
//        /// </summary>
//        private void FormatTextAndManageCaretRecursion(out Boolean textFormatted)
//        {
//            var selectionStart = TextBoxControl.SelectionStart;
//            var unformattedValue = TextBoxControl.Text ?? String.Empty;

//            string formatteValue;
//            NullableDoubleConverterTwoWay.FormatDoubleManagePosition(unformattedValue, out formatteValue, ref selectionStart);

//            textFormatted = unformattedValue != formatteValue;
//            if (textFormatted)
//            {
//                TextBoxControl.Text = formatteValue;

//                // Задание позиции меньше 0 приведет к исключению.
//                // Задание позиции больше длины строки приведет к автоматическому изменению позиции к длине строки.
//                var correctedPosition = Math.Max(selectionStart, 0);

//                if (selectionStart < 0)
//                {
					
//                }
//                if (formatteValue.Length <= selectionStart)
//                {
					
//                }

//                TextBoxControl.SelectionStart = correctedPosition;
//            }
//        }


//        private void UnloadedHandler(object sender, RoutedEventArgs e)
//        {
//            var textBox = AssociatedObject;

//            // Отпишемся от всех событий, чтоб предотвратить возможные утечки.
//            textBox.TextChanged -= textBox_TextChanged;
//        }

//    }
//}
