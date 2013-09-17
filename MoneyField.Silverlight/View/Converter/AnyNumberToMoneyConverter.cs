using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MoneyField.Silverlight.NullAndEmptyHandling;

namespace MoneyField.Silverlight.View.Converter
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AnyNumberToMoneyConverter : IValueConverter
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

		private static readonly Action<string> WriteLogDummyAction = el => {};
		private static Action<string> _writeLogAction;
		/// <summary>
		/// The simple debug log writer.
		/// </summary>
		public static Action<String> WriteLogAction
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
				WriteLogAction(String.Format("Convert. return = {0}", "null"));

				return String.Empty;
			}

			var doubleValue = doubleNullableValue.Value;

			var unformattedValue = GetCustomSerialisationFromDouble(doubleValue);

			String formatteValue;
			var selectionStartDummy = 0;
			FormatDoubleManagePosition(unformattedValue, null, 0, out formatteValue, ref selectionStartDummy);

			WriteLogAction(String.Format("Convert. unformattedValue = {0}", unformattedValue));
			WriteLogAction(String.Format("Convert. formatteValue = {0}", formatteValue));

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
				WriteLogAction(String.Format("ConvertBack. return = {0}", "null"));

				// Only a string is available.
				return null;
			}

			var doubleValue = GetDoubleFromCustomSerialisation(stringValue);

			return doubleValue;
		}


		/// <summary>
		/// Отформатирует строковое представление Double и сообщит, какая должна быть позиция курсора, относильено начальной.
		/// Fucking russian, yeah? For UK with love... :)
		/// TODO.it3xl.com: Make the translation.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="formatteValue"></param>
		/// <param name="caretPosition"></param>
		public void FormatDoubleManagePosition(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			out String formatteValue,
			ref Int32 caretPosition)
		{
			try
			{
				formatteValue = unformattedValue;

				if (unformattedValue == String.Empty)
				{
					// Пустое поле не правим и считаем корректным.

					return;
				}

				var formattingType = SetFormattingType(unformattedValue, textBeforeChanging);

				var state = new FormatterState
					{
						FormatteValue = formatteValue,
						CaretPosition = caretPosition,
						FormattingType = formattingType,
					};

				// Заменим алтернативный десятичный разделитель на основной.
				AlternativeInputDecimalSeparator.InvokeIfNotDefault(el =>
					{
						if (AlternativeInputDecimalSeparator == DecimalSeparator)
						{
							return;
						}

						state.FormatteValue = state.FormatteValue.Replace(AlternativeInputDecimalSeparator, DecimalSeparator);
					});

				// Обработаем удаление одного символа.
				if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
				{
					state.DeletionType = state.CaretPosition < lastCaretPosition
						? DeletionDirection.BackspaceButton
						: DeletionDirection.DeleteButton;

					// Restoring of the Decimal Separator and carret promotion.
					// The Decimal Separator delition means onle caret promotion.
					var wasDeletedSeparator = state.FormatteValue.Contains(DecimalSeparator) == false;
					if (wasDeletedSeparator)
					{
						state.FormatteValue = state.FormatteValue.Insert(state.CaretPosition, DecimalSeparatorChar);
						if (state.DeletionType == DeletionDirection.DeleteButton)
						{
							state.CaretPosition++;
						}
					}
				}

				// Excessive Decimal Separator processing.
				var separatorCount = state.FormatteValue.Count(el => el == DecimalSeparator);
				if (separatorCount == 0)
				{
					state.FormatteValue += DecimalSeparator;
				}
				if (1 < separatorCount)
				{
					// Пока не знаю как правильно и для простоты оставляю только самый первый разделитель.
					var index = state.FormatteValue.IndexOf(DecimalSeparator);
					state.FormatteValue = state.FormatteValue
						// Удалим все разделители вообще.
						.Replace(DecimalSeparatorChar, String.Empty)
						// Вернем только первый разделитель.
						.Insert(index, DecimalSeparatorChar);

					if(index + 1 < state.CaretPosition)
					{
						state.CaretPosition -= separatorCount - 1;
					}
				}

				// Удалить символы, которые не относятся к цифрам.
				var stringForIteraction = state.FormatteValue;
				foreach (var @char in stringForIteraction)
				{
					if (CustomSerialilzationChars.Contains(@char))
					{
						continue;
					}

					// Удалим только один символ (даже, если есть такие же повторяющиеся), чтобы избежать сложных вычислений.
					var index = state.FormatteValue.IndexOf(@char);
					state.FormatteValue = state.FormatteValue.Remove(index, 1);

					if (index <= (state.CaretPosition - 1))
					{
						state.CaretPosition -= 1;
					}
				}

	

				var digit = state.FormatteValue.Split(DecimalSeparator);

				// Integral part processing.
				var integer = digit.First();
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

				// The group separator processing.
				GroupSeparator.InvokeIfNotDefault(el =>
					{
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
						if (0 < digitsAfterCaretWithoutSeparator
							&& digitsAfterCaretWithoutSeparator % 3 == 0)
						{
							separarotAmountAfterCaret--;
						}
						if(state.DeletionType == DeletionDirection.BackspaceButton
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
					}
				);


				// Processing of the fractional part of a number.
				var partial = digit.Last();
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
					var positionAfterFirstPartialDigit = integer.Length + 1 + partial.Length - 2 == state.CaretPosition;
					var needCutSecondPartialDigit = partial.Length == 3 && positionAfterFirstPartialDigit;
					if (needCutSecondPartialDigit)
					{
						partial = partial.Remove(1,1);
					}
					else
					{
						partial = partial.Substring(0, 2);
					}
				}

				state.FormatteValue = integer + DecimalSeparator + partial;


				// Полученную сериализацию числа конвертируем в число и получаем из него автоматическкую сериализацию.
				// Это нужно из-за ограничений точности хранения чисел в .NET Framework. Слишком большие числа округляются.
				// Если этого не делать здесь, то получим отличное значение из ViewModel
				//  и неуправляемую передвижку курсора в начальную позицию.
				state.FormatteValue = FormatByPrecisionForDouble(state.FormatteValue);


				formatteValue = state.FormatteValue;
				caretPosition = state.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				formatteValue = String.Empty;
				caretPosition = 0;
			}

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
		/// The formatting for <see cref="Double"/> precision.
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
		private String GetCustomSerialisationFromDouble(double doubleValue)
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

			WriteLogAction(String.Format("ConvertBack. return = {0}", cSharpDigitalSerialisation));

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
