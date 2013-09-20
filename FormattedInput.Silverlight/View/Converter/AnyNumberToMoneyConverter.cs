using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	/// <summary>
	/// The main logic of a formatting.
	/// </summary>
	public sealed partial class AnyNumberToMoneyConverter : IValueConverter
	{
		private const Char NonBreakingSpaceChar = (Char)160;
		private const Char BreakingSpaceChar = (Char)32;

		private Char _groupSeparator;

		/// <summary>
		/// The desirable Group Separator char for a view.
		/// </summary>
		public Char GroupSeparator
		{
			get
			{
				if (_groupSeparator == Char.MinValue)
				{
					return CultureInfo.CurrentCulture.NumberFormat
						.NumberGroupSeparator
						.ToCharArray()
						.First();
				}

				return _groupSeparator;
			}
			set
			{
				_groupSeparator = value;

				ConvertSpaceToNonBreaking(ref _groupSeparator);
			}
		}

		/// <summary>
		/// The string representation of the <see cref="GroupSeparator"/> char.
		/// </summary>
		public String GroupSeparatorChar
		{
			get
			{
				if (GroupSeparator == default(Char))
				{
					return null;
				}

				return GroupSeparator.ToString(CultureInfo.InvariantCulture);
			}
		}


		private Char _decimalSeparator;

		/// <summary>
		/// The desirable Decimal Separator char for a view.
		/// </summary>
		public Char DecimalSeparator
		{
			get
			{
				if (_decimalSeparator == Char.MinValue)
				{
					return CultureInfo.CurrentCulture.NumberFormat
						.NumberDecimalSeparator
						.ToCharArray()
						.First();
				}

				return _decimalSeparator;
			}
			set
			{
				_decimalSeparator = value;

				ConvertSpaceToNonBreaking(ref _groupSeparator);
			}
		}

		/// <summary>
		/// The string representation of the <see cref="DecimalSeparator"/> char.
		/// </summary>
		public String DecimalSeparatorChar
		{
			get
			{
				return DecimalSeparator.ToString(CultureInfo.InvariantCulture);
			}
		}


		private char _alternativeInputDecimalSeparator;
		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or past time.
		/// </summary>
		public Char AlternativeInputDecimalSeparator
		{
			get
			{
				return _alternativeInputDecimalSeparator;
			}
			set
			{
				_alternativeInputDecimalSeparator = value;

				ConvertSpaceToNonBreaking(ref _groupSeparator);
			}
		}

		/// <summary>
		/// Available chars of a number.
		/// </summary>
		private Char[] CustomSerialilzationChars
		{
			get
			{
				return new[] { DecimalSeparator, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			}
		}

		public static Action<Exception> ShowExeptionAction { get; set; }

		private static readonly Action<Func<String>> WriteLogDummyAction = el => { };
		private static Action<Func<String>> _writeLogAction;
		/// <summary>
		/// The simple debug log writer.
		/// </summary>
		public static Action<Func<String>> WriteLogAction
		{
			get
			{
				if (_writeLogAction == null)
				{
					return WriteLogDummyAction;
				}

				return _writeLogAction;
			}
			set { _writeLogAction = value; }
		}

		/// <summary>
		/// Converts from a ViewModel to a View.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var doubleNullableValue = value as Double?;

			if (doubleNullableValue == null)
			{
				WriteLogAction(() => String.Format("Convert. return = {0}", "null"));

				return String.Empty;
			}

			var doubleValue = doubleNullableValue.Value;

			var unformattedValue = GetCustomSerialisationFromDouble(doubleValue);

			String formatteValue;
			var selectionStartDummy = 0;
			FormatAndManageCaret(unformattedValue, null, 0, out formatteValue, ref selectionStartDummy);

			WriteLogAction(() => String.Format("Convert. unformattedValue = {0}", unformattedValue));
			WriteLogAction(() => String.Format("Convert. formattedValue = {0}", formatteValue));

			return formatteValue;
		}


		/// <summary>
		/// Converts from a View to a ViewModel.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var stringValue = value as String;
			if (String.IsNullOrEmpty(stringValue))
			{
				WriteLogAction(() => String.Format("ConvertBack. return = {0}", "null"));

				// Only a string is available.
				return null;
			}

			var doubleValue = GetDoubleFromCustomSerialisation(stringValue);

			return doubleValue;
		}


		/// <summary>
		/// The entry pont of the formatting and the caret management.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void FormatAndManageCaret(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			try
			{
				resultingFormattedValue = unformattedValue;

				if (NeedStopFormatting(unformattedValue))
				{
					return;
				}

				var state = InitProcessingStates(
					unformattedValue,
					textBeforeChanging,
					lastCaretPosition,
					resultingFormattedValue,
					caretPosition);

				FormatAndManageCaretRaw(state);

				resultingFormattedValue = state.FormattedValue;
				caretPosition = state.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				// Let's set default states, since we screwed up.
				resultingFormattedValue = String.Empty;
				caretPosition = 0;
			}

		}

		private static bool NeedStopFormatting(string unformattedValue)
		{
			if (unformattedValue == String.Empty)
			{
				// The empty value is correct and will stay unprocessed.

				return true;
			}
			return false;
		}

		/// <summary>
		/// The lower implementation of a formatting.
		/// </summary>
		/// <param name="state"></param>
		private void FormatAndManageCaretRaw(FormatterState state)
		{
			DecimalSeparatorAlternatingReplacing(state);
			DecimalSeparatorDeletedProcessingWithCaret(state);
			DecimalSeparatorMissed(state);
			DecimalSeparatorExcessiveProcessingWithCaret(state);

			GroupSeparatorProcessingWithCaret(state);

			NotDigitCharsProcessingWithCaret(state);

			var integer = IntegerPartProcessingWithCaret(state);

			var partial = PartialPartProcessingWithCaret(state, integer.Length);

			var preliminatyFormattedValue = integer + DecimalSeparator + partial;
			state.FormattedValue = FormatByPrecisionForDouble(preliminatyFormattedValue);
		}

		/// <summary>
		/// Manage and format for the Group Separator.
		/// </summary>
		/// <param name="state"></param>
		private void GroupSeparatorProcessingWithCaret(FormatterState state)
		{
			if (state.GroupSeparatorDeleted != true)
			{
				return;
			}

			switch (state.DeletionType)
			{
				case DeletionDirection.BackspaceButton:
					state.CaretPosition--;
					state.FormattedValue = state.FormattedValue.Remove(state.CaretPosition, 1);
					break;
				case DeletionDirection.DeleteButton:
					state.FormattedValue = state.FormattedValue.Remove(state.CaretPosition, 1);
					break;
			}
		}

		/// <summary>
		/// Initializes states for formatting.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging"></param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		/// <returns></returns>
		private FormatterState InitProcessingStates(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			String resultingFormattedValue,
			Int32 caretPosition)
		{
			var state = new FormatterState
				{
					FormattedValue = resultingFormattedValue,
					CaretPosition = caretPosition,
					FormattingType = SetFormattingType(unformattedValue, textBeforeChanging),
				};

			SetPreviousStates(state, textBeforeChanging);

			if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
			{
				state.DeletionType = state.CaretPosition < lastCaretPosition
					? DeletionDirection.BackspaceButton
					: DeletionDirection.DeleteButton;

			}

			state.GroupSeparatorDeleted = GetStateGroupSeparatorDeleted(state.FormattingType, unformattedValue, textBeforeChanging);

			return state;
		}

		/// <summary>
		/// Sets the previous number states.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="textBeforeChanging"></param>
		private void SetPreviousStates(FormatterState state, string textBeforeChanging)
		{
			if (textBeforeChanging.IsNullOrEmpty())
			{
				return;
			}

			if (textBeforeChanging.Contains(DecimalSeparator) == false)
			{
				return;
			}

			var number = textBeforeChanging.Split(DecimalSeparator);

			state.PreviousInteger = number.First();
			state.PreviousParatial = number.Last();
		}

		private Boolean? GetStateGroupSeparatorDeleted(FormattingAfter formattingType, String unformattedValue, String textBeforeChanging)
		{
			if (formattingType != FormattingAfter.OneSymbolDeleted)
			{
				return null;
			}

			for (var i = 0; i < unformattedValue.Length; i++)
			{
				if (unformattedValue[i] == textBeforeChanging[i])
				{
					continue;
				}

				return textBeforeChanging[i] == GroupSeparator;
			}

			return false;
		}

		/// <summary>
		/// Processing of the integer part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private string IntegerPartProcessingWithCaret(FormatterState state)
		{
			var number = state.FormattedValue.Split(DecimalSeparator);

			// Integral part processing.
			var integer = number.First();

			if (1 < integer.Length
				&& state.PreviousInteger == "0"
				&& integer.Last() == '0')
			{
				integer = integer.Remove(integer.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < integer.Length && integer.First() == '0')
			{
				integer = integer.Remove(0, 1);
				if (0 < state.CaretPosition)
				{
					state.CaretPosition--;
				}
			}

			// Before decimal separator should be the 0, if it's the first.
			if (integer.Length == 0)
			{
				integer = "0";

				state.CaretPosition++;
			}

			if (GroupSeparator.IsDefault())
			{
				return integer;
			}

			// The group separator processing.
			var lengthWithoutSeparator = integer.Length;
			var caretPositionWithoutSeparator = state.CaretPosition;

			var integerInvert = String.Join(null, integer.Reverse());
			for (var i = 0; i < lengthWithoutSeparator; i++)
			{
				if (i == 0)
				{
					continue;
				}
				if (0 < i % 3)
				{
					continue;
				}

				var offset = i / 3 - 1;
				integerInvert = integerInvert.Insert(i + offset, GroupSeparatorChar);

				state.CaretPosition++;
			}

			integer = String.Join(null, integerInvert.Reverse());

			// Actualizes caret position.
			var digitsAfterCaretWithoutSeparator = Math.Abs(lengthWithoutSeparator - caretPositionWithoutSeparator);
			var separarotAmountAfterCaret = digitsAfterCaretWithoutSeparator / 3;
			if (0 < digitsAfterCaretWithoutSeparator && digitsAfterCaretWithoutSeparator % 3 == 0)
			{
				separarotAmountAfterCaret--;
			}
			if (state.DeletionType == DeletionDirection.BackspaceButton
				&& 0 < digitsAfterCaretWithoutSeparator
				&& digitsAfterCaretWithoutSeparator % 3 == 0)
			{
				separarotAmountAfterCaret++;
			}
			//if(state.DeletionType == DeletionDirection.DeleteButton
			//	&& 0 < digitsAfterCaretWithoutSeparator % 3)
			//{
			//	separarotAmountAfterCaret++;
			//}

			state.CaretPosition -= separarotAmountAfterCaret;

			return integer;
		}

		/// <summary>
		/// Processing of the partial part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private string PartialPartProcessingWithCaret(FormatterState state, int integerLength)
		{
			var number = state.FormattedValue.Split(DecimalSeparator);
			// Processing of the fractional part of a number.
			var partial = number.Last();
			// After the decimal point should be two digits.
			if (partial.Length == 0)
			{
				partial = "00";
			}
			else if (partial.Length == 1)
			{
				partial += "0";
			}
			else if (2 < partial.Length)
			{
				var positionAfterFirstPartialDigit = integerLength + 1 + partial.Length - 2 == state.CaretPosition;
				var needCutSecondPartialDigit = partial.Length == 3 && positionAfterFirstPartialDigit;
				if (needCutSecondPartialDigit)
				{
					partial = partial.Remove(1, 1);
				}
				else
				{
					partial = partial.Substring(0, 2);
				}
			}
			return partial;
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(FormatterState state)
		{
			var stringForIteraction = state.FormattedValue;
			foreach (var @char in stringForIteraction)
			{
				if (CustomSerialilzationChars.Contains(@char))
				{
					continue;
				}

				// Let's delete only the first entry of that char for now.
				var index = state.FormattedValue.IndexOf(@char);
				state.FormattedValue = state.FormattedValue.Remove(index, 1);

				if (index <= (state.CaretPosition - 1))
				{
					state.CaretPosition -= 1;
				}
			}
		}

		/// <summary>
		/// Excessive Decimal Separator's processing.
		/// With caret managment.
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorExcessiveProcessingWithCaret(FormatterState state)
		{
			var separatorCount = state.FormattedValue.Count(el => el == DecimalSeparator);
			if (separatorCount < 2)
			{
				return;
			}

			// Let's stay just the first decimal separator.
			var index = state.FormattedValue.IndexOf(DecimalSeparator);
			state.FormattedValue = state.FormattedValue
				.Replace(DecimalSeparatorChar, String.Empty)
				.Insert(index, DecimalSeparatorChar);

			if (index + 1 < state.CaretPosition)
			{
				state.CaretPosition -= separatorCount - 1;
			}
		}

		/// <summary>
		/// Yet another missed Decimal Separator processing.
		/// <seealso cref="DecimalSeparatorDeletedProcessingWithCaret"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorMissed(FormatterState state)
		{
			var separatorCountV = state.FormattedValue.Count(el => el == DecimalSeparator);
			if (0 < separatorCountV)
			{
				return;
			}

			state.FormattedValue += DecimalSeparator;
		}

		/// <summary>
		/// Restores the Decimal Separator if it lost and manage caret position.
		/// <seealso cref="DecimalSeparatorMissed"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorDeletedProcessingWithCaret(FormatterState state)
		{
			if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
			{
				return;
			}

			// Restoring of the Decimal Separator and carret promotion.
			// The Decimal Separator delition means onle caret promotion.
			var wasDeletedSeparator = state.FormattedValue.Contains(DecimalSeparator) == false;
			if (wasDeletedSeparator == false)
			{
				return;
			}

			state.FormattedValue = state.FormattedValue.Insert(state.CaretPosition, DecimalSeparatorChar);

			if (state.DeletionType == DeletionDirection.DeleteButton)
			{
				state.CaretPosition++;
			}
		}

		/// <summary>
		/// Replaces an alternate Decimal Separator.
		/// <see cref="AlternativeInputDecimalSeparator"/>
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorAlternatingReplacing(FormatterState state)
		{
			if (AlternativeInputDecimalSeparator.IsDefault())
			{
				return;
			}

			if (AlternativeInputDecimalSeparator == DecimalSeparator)
			{
				return;
			}

			state.FormattedValue = state.FormattedValue
				.Replace(AlternativeInputDecimalSeparator, DecimalSeparator);
		}

		/// <summary>
		/// Defines the formatting type <see cref="FormattingAfter"/>.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging"></param>
		/// <returns>The value of the  <see cref="FormattingAfter"/></returns>
		private static FormattingAfter SetFormattingType(String unformattedValue, String textBeforeChanging)
		{
			FormattingAfter formattingAfter;
			if (String.IsNullOrEmpty(textBeforeChanging))
			{
				formattingAfter = FormattingAfter.EmptyStartValue;
			}
			else if (Math.Abs(unformattedValue.Length - textBeforeChanging.Length) != 1)
			{
				formattingAfter = FormattingAfter.GroupPastingOrDeletion;
			}
			else
			{
				var subtraction = unformattedValue.Length - textBeforeChanging.Length;
				formattingAfter = 0 < subtraction
					? FormattingAfter.OneSymbolAdded
					: FormattingAfter.OneSymbolDeleted;
			}

			return formattingAfter;
		}

		/// <summary>
		/// The formatting for <see cref="Double"/> precision.<para/>
		/// It's the workaround for a implicit formatting by the .NET Framework numbers rounding.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private String FormatByPrecisionForDouble(String value)
		{
			var currentDouble = GetDoubleFromCustomSerialisation(value);
			var serialisationWithPrecision = GetCustomSerialisationFromDouble(currentDouble);

			return serialisationWithPrecision;
		}


		/// <summary>
		/// The custom double serialization.
		/// </summary>
		/// <param name="doubleValue"></param>
		/// <returns></returns>
		private String GetCustomSerialisationFromDouble(Double doubleValue)
		{
			const String numberStandartFormattingKey = "n";
			var unformattedValue = doubleValue
				.ToString(numberStandartFormattingKey, CultureInfo.InvariantCulture);

			// The order is important! The GroupSeparatorChar replacing must go before the DecimalSeparatorChar replacing.
			if (GroupSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				unformattedValue = unformattedValue.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator,
					GroupSeparatorChar);
			}

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				unformattedValue = unformattedValue.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator,
					DecimalSeparatorChar);
			}

			return unformattedValue;
		}

		/// <summary>
		/// The custom double conversion.
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		private double GetDoubleFromCustomSerialisation(String stringValue)
		{
			var cSharpDigitalSerialisation = stringValue;

			// Remove the group delimiter.
			GroupSeparator.InvokeIfNotDefault(el =>
				{
					if (GroupSeparator == NonBreakingSpaceChar)
					{
						cSharpDigitalSerialisation.Replace(BreakingSpaceChar, NonBreakingSpaceChar);
					}

					cSharpDigitalSerialisation = cSharpDigitalSerialisation.Replace(GroupSeparatorChar, String.Empty);
				}
			);

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn to the CultureInfo.InvariantCulture.
				cSharpDigitalSerialisation = cSharpDigitalSerialisation.Replace(
					DecimalSeparatorChar,
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
			}

			WriteLogAction(() => String.Format("ConvertBack. return = {0}", cSharpDigitalSerialisation));

			Double doubleValue;
			Double.TryParse(
				cSharpDigitalSerialisation,
				NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture,
				out doubleValue
				);
			return doubleValue;
		}

		/// <summary>
		/// We must use non-breking space because of default value at the 
		/// CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.
		/// </summary>
		/// <param name="charValue"></param>
		public static void ConvertSpaceToNonBreaking(ref Char charValue)
		{
			if (charValue != BreakingSpaceChar)
			{
				return;
			}

			// It's the breaking space. Fix it!
			charValue = NonBreakingSpaceChar;
		}
	}
}
