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
	public sealed class MoneyTextBox : TextBox
	{
		public AnyNumberToMoneyConverter Converter { get; private set; }



		/// <summary>
		/// The desirable Group Separator char for a view.
		/// </summary>
		public Char GroupSeparator
		{
			get
			{
				return Converter.GetSafe(el => el.GroupSeparator);
			}
			set
			{
				Converter.SetSafe(el => el.GroupSeparator = value);
			}
		}



		/// <summary>
		/// The desirable Decimal Separator char for a view.
		/// </summary>
		public Char DecimalSeparator
		{
			get
			{
				return Converter.GetSafe(el => el.DecimalSeparator);
			}
			set
			{
				Converter.SetSafe(el => el.DecimalSeparator = value);
			}
		}

		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or past time.
		/// </summary>
		public Char AlternativeDecimalSeparator
		{
			get
			{
				return Converter.GetSafe(el => el.AlternativeInputDecimalSeparator);
			}
			set
			{
				Converter.SetSafe(el => el.AlternativeInputDecimalSeparator = value);
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
			Loaded += LoadedHandler;
			Unloaded += UnloadedHandler;
		}

		private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
		{
			var textBox = this;
			Converter = new AnyNumberToMoneyConverter
				{
					GroupSeparator = GroupSeparator,
					DecimalSeparator = DecimalSeparator,
					AlternativeInputDecimalSeparator = AlternativeDecimalSeparator,
				};

			CorrectBinding(textBox, Converter);

			textBox.TextChanged += textBox_TextChanged;
			textBox.SelectionChanged += textBox_SelectionChanged;
		}

		/// <summary>
		/// Проверит и настроит биндинг свойства <see cref="TextBox.TextProperty"/>, чтоб удовлетворял логике форматирования.
		/// </summary>
		/// <param name="textBox">The targer <see cref="TextBox"/>.</param>
		/// <param name="converter">The converter with a custom formatting logic.</param>
		private static void CorrectBinding(TextBox textBox, AnyNumberToMoneyConverter converter)
		{
			textBox
				.GetBindingExpression(TextBox.TextProperty)
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

					textBox.SetBinding(TextBox.TextProperty, binding);
				});
		}

		void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = this;


			AnyNumberToMoneyConverter.WriteLogAction(() => String.Format("textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
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

		void textBox_SelectionChanged(object sender, RoutedEventArgs e)
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
			Converter = null;

			var textBox = this;
			// Unsubscribe from all events for sake of leaks.
			textBox.TextChanged -= textBox_TextChanged;
			textBox.SelectionChanged -= textBox_SelectionChanged;
		}



	}
}
