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
		/// In theory, it has no the memory's roots.<para/>
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
				return Converter.GroupSeparatorChar;
			}
			set
			{
				Converter.GroupSeparatorChar = value;
			}
		}

		/// <summary>
		/// The desirable Decimal Separator char for a view.
		/// </summary>
		public String DecimalSeparatorChar
		{
			get
			{
				return Converter.DecimalSeparatorChar;
			}
			set
			{
				Converter.DecimalSeparatorChar = value;
			}
		}

		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or at a copy/paste time.
		/// </summary>
		public String DecimalSeparatorAlternativeChar
		{
			get
			{
				return Converter.DecimalSeparatorAlternativeChar;
			}
			set
			{
				Converter.DecimalSeparatorAlternativeChar = value;
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
			var textBox = this;
			textBox.TextChanged += textBox_TextChanged;
			textBox.SelectionChanged += textBox_SelectionChanged;
			textBox.GotFocus += textBox_GotFocus;
			textBox.LostFocus += textBox_LostFocus;

			CorrectBinding(Converter);
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

			NumberToMoneyConverter.WriteLogAction(() => String.Format("* TextChanged. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
				textBox.SelectionStart,
				textBox.SelectionLength,
				textBox.Text)
			);

			ProcessText();
		}

		void textBox_GotFocus(object sender, RoutedEventArgs e)
		{
			var textBox = this;

			NumberToMoneyConverter.WriteLogAction(() => String.Format("* GotFocus. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
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
			var textBox = this;

			NumberToMoneyConverter.WriteLogAction(() => String.Format("* LostFocus. SelectionStart = {0}. SelectionLength = {1}. Text = {2}",
				textBox.SelectionStart,
				textBox.SelectionLength,
				textBox.Text)
			);

			SetFocusState(FocusState.No);
			ProcessText();
		}

		private void ProcessText()
		{
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

			if(IsRecursion())
			{
				textFormatted = true;

				textBox.Text = String.Empty;
				textBox.SelectionStart = 0;

				return;
			}

			String formattedText;
			Converter.Process(unformattedValue, out formattedText, ref selectionStart);

			textFormatted = unformattedValue != formattedText;
			if (textFormatted)
			{
				NumberToMoneyConverter.WriteLogAction(() => String.Format(" = Text = {0}", formattedText));
				// It starts the recursion.
				textBox.Text = formattedText;
			}

			NumberToMoneyConverter.WriteLogAction(() => String.Format(" = SelectionStart = {0}", selectionStart));
			textBox.SelectionStart = selectionStart;
		}

		private DateTime _recursionMark;
		private Int32 _recursionCount;

		/// <summary>
		/// The recursion interrupter's logic.
		/// </summary>
		/// <returns></returns>
		private Boolean IsRecursion()
		{
			var lastMark = _recursionMark;
			_recursionMark = DateTime.Now;

			var span = _recursionMark - lastMark;

			if (TimeSpan.FromMilliseconds(200) < span)
			{
				_recursionCount = 0;

				return false;
			}

			_recursionCount++;

			if(_recursionCount < 70)
			{
				return false;
			}

			_recursionCount = 0;

			return true;
		}

		private void textBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			// It watchs for the caret position changed by an user.
			Converter.SetCaretPositionBeforeTextChanging(SelectionStart);
		}


	}
}
