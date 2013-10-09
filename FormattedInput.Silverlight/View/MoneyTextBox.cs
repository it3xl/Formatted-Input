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
		/// <summary>
		/// Converter with the main logic.<para/>
		/// </summary>
		/// <remarks>
		/// TODO.it3xl.com: Possibly the Converter property should be performed as the WeakReference.<para/>
		/// I'll test this by the way.<para/>
		/// In theory, it has no the memory roots.<para/>
		/// In worse case it's fast to apply the WeakReference here.
		/// </remarks>
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
				Converter.GroupSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
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
				Converter.DecimalSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
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
				Converter.DecimalSeparatorAlternative = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
			}
		}

		/// <summary>
		/// Hides the partial part.
		/// </summary>
		public Boolean PartialDisabled
		{
			get { return Converter.PartialDisabled; }
			set { Converter.PartialDisabled = value; }
		}

		/// <summary>
		/// Hides the partial part on a text input.
		/// </summary>
		public Boolean PartialDisabledOnInput
		{
			get { return Converter.PartialDisabledOnInput; }
			set { Converter.PartialDisabledOnInput = value; }
		}

		/// <summary>
		/// Sets a current state of the focus.
		/// </summary>
		private void SetFocusState(FocusState focusState)
		{
			Converter.FocusState = focusState;
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
			textBox.LostFocus += textBox_LostFocus;
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

			ProcessText();
		}

		void textBox_GotFocus(object sender, RoutedEventArgs e)
		{
			var textBox = this;

			NumberToMoneyConverter.WriteLogAction(() => String.Format("textBox_TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
				textBox.SelectionStart,
				textBox.SelectionLength,
				textBox.Text)
			);

			SetFocusState(FocusState.JustGotten);
			ProcessText();
			SetFocusState(FocusState.Gotten);
		}

		void textBox_LostFocus(object sender, RoutedEventArgs e)
		{
			SetFocusState(FocusState.No);
			ProcessText();
		}

		private void ProcessText()
		{
			// TODO.it3xl.com: Check a whip out recursion and break it up.

			var textBox = this;

			Boolean textFormatted;
			FormatTextAndManageCaretInRecursion(out textFormatted);

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
		private void FormatTextAndManageCaretInRecursion(out Boolean textFormatted)
		{
			var textBox = this;

			var selectionStart = textBox.SelectionStart;
			var unformattedValue = textBox.Text ?? String.Empty;

			String formattedValue;
			Converter.Process(unformattedValue, out formattedValue, ref selectionStart);

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
