using System;
using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;
using WeakClosureProject;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		public const Char NonBreakingSpaceChar = (Char)160;
		public const Char BreakingSpaceChar = (Char)32;

		private const String ZeroString = "0";
		private const Char ZeroChar = '0';
		private const String ZerosPartialString = "00";

		private const String NumberStandartFormattingKey = "n";

		private readonly Type _typeDouble = typeof(Double);
		private readonly Type _typeDoubleNullabe = typeof(Double?);
		private readonly Type _typeDecimal = typeof(Decimal);
		private readonly Type _typeDecimalNullabe = typeof(Decimal?);

		private Double? _lastViewDouble;
		private Decimal? _lastViewDecimal;

		private Boolean _viewModelValueChanged;
		private Boolean _jumpCaretToEndOfIntegerOnNextProcessing;

		private WeakClosure<MoneyTextBox> _moneyBox;

		/// <summary>
		/// Will be executed in case of a exception.
		/// </summary>
		public static Action<Exception> ShowExeptionAction { get; set; }

		/// <summary>
		/// Do-nothing log writer.
		/// </summary>
		private static readonly Action<Func<String>> WriteLogDummyAction = el => { };
		/// <summary>
		/// See <see cref="WriteLogAction"/>.
		/// </summary>
		private static Action<Func<String>> _writeLogAction;
		/// <summary>
		/// The simple debug log writer's entry for debug purposes.
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
		/// See <see cref="GroupSeparator"/>.
		/// </summary>
		private Char? _groupSeparator;
		/// <summary>
		/// The desirable Group Separator char for a view.
		/// </summary>
		public Char GroupSeparator
		{
			get
			{
				if (_groupSeparator == null)
				{
					return CultureInfo.CurrentCulture.NumberFormat
						.NumberGroupSeparator
						.ToCharFirst();
				}

				return _groupSeparator.Value;
			}
			set
			{
				ConvertSpaceToNonBreaking(ref value);

				_groupSeparator = value;
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

		/// <summary>
		/// See <see cref="DecimalSeparator"/>.
		/// </summary>
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
						.ToCharFirst();
				}

				return _decimalSeparator;
			}
			set
			{
				ConvertSpaceToNonBreaking(ref value);

				_decimalSeparator = value;
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

		/// <summary>
		/// Hides the partial part.
		/// </summary>
		public Boolean PartialDisabled { get; set; }

		/// <summary>
		/// Hides the partial part on a text input (on focus).
		/// </summary>
		public Boolean PartialDisabledOnInput { get; set; }

		/// <summary>
		/// The input focus state for a target text-element.
		/// </summary>
		public FocusState FocusState { get; set; }

		/// <summary>
		/// The current state for disabling of the partila part.
		/// </summary>
		private Boolean PartialDisabledCurrent
		{
			get
			{
				const Boolean disabled = true;
				const Boolean none = false;

				if (PartialDisabled)
				{
					return disabled;
				}

				if(PartialDisabledOnInput)
				{
					if(FocusState == FocusState.No)
					{
						return none;
					}

					return disabled;
				}

				return none;
			}
		}


		/// <summary>
		/// See <see cref="DecimalSeparatorAlternative"/>.
		/// </summary>
		private char _decimalSeparatorAlternative;

		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or past time.
		/// </summary>
		public Char DecimalSeparatorAlternative
		{
			get
			{
				return _decimalSeparatorAlternative;
			}
			set
			{
				ConvertSpaceToNonBreaking(ref value);

				_decimalSeparatorAlternative = value;
			}
		}

		private string _textBeforeChangingNotNull;
		/// <summary>
		/// The text that was before the formatting.
		/// </summary>
		public String TextBeforeChangingNotNull
		{
			get { return _textBeforeChangingNotNull ?? String.Empty; }
			private set { _textBeforeChangingNotNull = value ?? String.Empty; }
		}

		/// <summary>
		/// The last caret position before the formatting.
		/// </summary>
		private Int32 CaretPositionBeforeTextChanging { get; set; }
		/// <summary>
		/// Sets the <see cref="CaretPositionBeforeTextChanging"/> property.
		/// </summary>
		/// <param name="caretPosition">The caret position.</param>
		public void SetCaretPositionBeforeTextChanging(Int32 caretPosition)
		{
			CaretPositionBeforeTextChanging = caretPosition;
		}

		/// <summary>
		/// Available chars of a number of a custom format.
		/// </summary>
		private Char[] CustomSerialilzationChars
		{
			get
			{
				if(PartialDisabledCurrent)
				{
					return new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
				}

				return new[] { DecimalSeparator, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			}
		}

	}
}
