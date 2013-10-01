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
		/// Checks and sets a data binding for the <see cref="TextBox.TextProperty"/>.
		/// </summary>
		/// <param name="converter">The converter with a custom formatting logic.</param>
		private void CorrectBinding(NumberToMoneyConverter converter)
		{
			GetBindingExpression(TextProperty)
				.InvokeNotNull(el =>
				{
					var binding = new Binding(el.ParentBinding)
					{
						// it3xl.com: Update Source Trigger is our job.
						UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
						Mode = BindingMode.TwoWay,

						// it3xl.com: Only our converter is appropriate. It's the main logic out here.
						Converter = converter,
					};

					SetBinding(TextProperty, binding);
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

			// Helps to ignore the excessive TextChanged event triggered by formatting.
			// It breaks the recursion.
			if (textFormatted == false)
			{
				textBox
					.GetBindingExpression(TextProperty)
					.InvokeNotNull(el => el.UpdateSource());
			}
		}

		private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			// It watchs for the caret position changed by user.
			LastSelectionStart = SelectionStart;
		}

		/// <summary>
		/// Formats the text and manages the caret's position.<para/>
		/// Starts a recursion if it invoked from the TextChangent event handler.
		/// </summary>
		private void FormatTextAndManageCaretRecursion(out Boolean textFormatted)
		{
			var textBox = this;

			var selectionStart = textBox.SelectionStart;
			var unformattedValue = textBox.Text ?? String.Empty;

			String formatteValue;
			Converter.FormatAndManageCaret(unformattedValue, LastSelectionStart, out formatteValue, ref selectionStart);

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
