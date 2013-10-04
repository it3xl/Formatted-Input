using System;
using System.Globalization;
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
		public String GroupSeparatorChar
		{
			get
			{
				return Converter.GroupSeparator.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				Converter.GroupSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFromFirst());
			}
		}

		/// <summary>
		/// The desirable Decimal Separator char for a view.
		/// </summary>
		public String DecimalSeparatorChar
		{
			get
			{
				return Converter.DecimalSeparator.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				Converter.DecimalSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFromFirst());
			}
		}

		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or past time.
		/// </summary>
		public String DecimalSeparatorAlternativeChar
		{
			get
			{
				return Converter.DecimalSeparatorAlternative.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				Converter.DecimalSeparatorAlternative = value.InvokeNotNullOrEmpty(el => el.ToCharFromFirst());
			}
		}

		/// <summary>
		/// The bitness of the integer part.
		/// </summary>
		public UInt16? IntegerBitness
		{
			get
			{
				return Converter.IntegerBitness;
			}
			set
			{
				Converter.IntegerBitness = value;
			}
		}


		public MoneyTextBox()
		{
			Converter = new NumberToMoneyConverter();

			Loaded += LoadedHandler;
		}

		private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
		{
			CorrectBinding(Converter);

			var textBox = this;
			textBox.TextChanged += textBox_TextChanged;
			textBox.SelectionChanged += textBox_SelectionChanged;
			textBox.GotFocus += textBox_GotFocus;
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

			ProcessText(FocusEnum.HasNoState);
		}

		void textBox_GotFocus(object sender, RoutedEventArgs e)
		{
			var textBox = this;

			NumberToMoneyConverter.WriteLogAction(() => String.Format("textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
				textBox.SelectionStart,
				textBox.SelectionLength,
				textBox.Text)
			);

			ProcessText(FocusEnum.JustGotten);
		}

		private void ProcessText(FocusEnum focusState)
		{
			// TODO.it3xl.com: Check a whip out recursion and break it up.

			var textBox = this;

			Boolean textFormatted;
			FormatTextAndManageCaretInRecursion(focusState, out textFormatted);

			// Helps to ignore the excessive TextChanged event triggered by formatting.
			// It breaks the recursion.
			if (textFormatted == false)
			{
				textBox
					.GetBindingExpression(TextProperty)
					.InvokeNotNull(el => el.UpdateSource());
			}
		}

		/// <summary>
		/// Formats the text and manages the caret's position.<para/>
		/// Starts a recursion if it invoked from the TextChangent event handler.
		/// </summary>
		private void FormatTextAndManageCaretInRecursion(FocusEnum focusState, out Boolean textFormatted)
		{
			var textBox = this;

			var selectionStart = textBox.SelectionStart;
			var unformattedValue = textBox.Text ?? String.Empty;

			String formattedValue;
			Converter.FormatAndManageCaret(unformattedValue, focusState, out formattedValue, ref selectionStart);

			textFormatted = unformattedValue != formattedValue;
			if (textFormatted)
			{
				// It starts the recursion.
				textBox.Text = formattedValue;
			}

			textBox.SelectionStart = selectionStart;
		}

		private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			// It watchs for the caret position changed by an user.
			Converter.SetCaretPositionBeforeTextChanging(SelectionStart);
		}


	}
}
