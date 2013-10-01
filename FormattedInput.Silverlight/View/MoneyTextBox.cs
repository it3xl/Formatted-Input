using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Converter;

namespace It3xl.FormattedInput.View
{
	/// <summary>
	/// The TextBox with build-in money input-formatting.
	/// </summary>
	public class MoneyTextBox : TextBox
	{
		public NumberToMoneyConverter Converter { get; private set; }


		/// <summary>
		/// The desirable Group Separator char for a view.
		/// </summary>
		public Char GroupSeparator
		{
			get
			{
				return Converter.GroupSeparator;
			}
			set
			{
				Converter.GroupSeparator = value;
			}
		}

		private Char? _decimalSeparator;
		/// <summary>
		/// The desirable Decimal Separator char for a view.
		/// </summary>
		public Char DecimalSeparator
		{
			get
			{
				return Converter.DecimalSeparator;
			}
			set
			{
				Converter.DecimalSeparator = value;
			}
		}

		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or past time.
		/// </summary>
		public Char DecimalSeparatorAlternative
		{
			get
			{
				return Converter.DecimalSeparatorAlternative;
			}
			set
			{
				Converter.DecimalSeparatorAlternative = value;
			}
		}

		/// <summary>
		/// The text that was before the TextChanged event.
		/// </summary>
		private String LastText { get; set; }

		/// <summary>
		/// The text that was before the TextChanged event.
		/// </summary>
		private Int32 LastSelectionStart { get; set; }






		public MoneyTextBox()
		{
			Converter = new NumberToMoneyConverter();

			Loaded += LoadedHandler;
			Unloaded += UnloadedHandler;
		}

		private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
		{
			CorrectBinding(Converter);

			var textBox = this;
			textBox.TextChanged += textBox_TextChanged;
			textBox.SelectionChanged += textBox_SelectionChanged;
		}

		/// <summary>
		/// Проверит и настроит биндинг свойства <see cref="TextBox.TextProperty"/>, чтоб удовлетворял логике форматирования.
		/// </summary>
		/// <param name="converter">The converter with a custom formatting logic.</param>
		private void CorrectBinding(NumberToMoneyConverter converter)
		{
			GetBindingExpression(TextProperty)
				.InvokeNotNull(el =>
				{
					var binding = new Binding(el.ParentBinding)
					{
						// it3xl.com: Только сами определяем, когда обновлять источник.
						UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
						Mode = BindingMode.TwoWay,

						// it3xl.com: Биндинг правильно может работать только с нашим конвертером.
						Converter = converter,
					};

					SetBinding(TextProperty, binding);

					// TODO.it3xl.com: Cover lack of next row by a test when start value from ViewModel is 0.
					LastText = Text;
				});
		}

		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = this;


			NumberToMoneyConverter.WriteLogAction(() => String.Format("textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
				textBox.SelectionStart,
				textBox.SelectionLength,
				textBox.Text)
			);


			Boolean textFormatted;
			FormatTextAndManageCaretRecursion(out textFormatted);

			if (textFormatted == false)
			{
				// Если текст изменен из-за форматировани, то вызывать обновление не нужно,
				//  т.к. следом будет повторное событие TextChanged и в нем обновим источник.

				textBox
					.GetBindingExpression(TextBox.TextProperty)
					.InvokeNotNull(el => el.UpdateSource());
			}
		}

		private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			// It watchs for the caret position changed by user.
			LastSelectionStart = SelectionStart;
		}

		/// <summary>
		/// Отформатируем значение и выставим каретку куросора в правильное положение.
		/// ! Внимание, этот метод вызывает рекурсию, если текст в нем менялся через событие TextChanged.
		/// </summary>
		private void FormatTextAndManageCaretRecursion(out Boolean textFormatted)
		{
			var textBox = this;

			var selectionStart = textBox.SelectionStart;
			var unformattedValue = textBox.Text ?? String.Empty;

			String formatteValue;
			Converter.FormatAndManageCaret(unformattedValue, LastText, LastSelectionStart, out formatteValue, ref selectionStart);

			LastText = formatteValue;
			LastSelectionStart = selectionStart;

			textFormatted = unformattedValue != formatteValue;
			if (textFormatted)
			{
				textBox.Text = formatteValue;

				// Fix the impossible negative caret's position for sake of the unwanted exception.
				var correctedPosition = Math.Max(selectionStart, 0);

				// For debug purposes.
				if (selectionStart < 0)
				{
					// For the negative caret's position will throw an exception.
				}
				// For debug purposes.
				if (formatteValue.Length < selectionStart)
				{
					// The caret's position that more than a value's length will be changed to the end position.
				}

				textBox.SelectionStart = correctedPosition;
			}
		}


		private void UnloadedHandler(object sender, RoutedEventArgs e)
		{
			var textBox = this;
			// Unsubscribe from all events for sake of leaks.
			textBox.TextChanged -= textBox_TextChanged;
			textBox.SelectionChanged -= textBox_SelectionChanged;
		}



	}
}
