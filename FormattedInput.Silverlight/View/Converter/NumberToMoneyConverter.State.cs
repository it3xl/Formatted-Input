using System;
using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;

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

		/// <summary>
		/// Typed formatting flag.
		/// </summary>
		private RuntimeType RuntimeType { get; set; }

		private Boolean ViewModelValueChanged
		{
			get { return _viewModelValueChanged; }
			set
			{
				if (value != _viewModelValueChanged)
				{
					if(value)
					{
						WriteLogAction(() => "  >> ViewModelValueChanged = true");
					}
					else
					{
						WriteLogAction(() => "  << ViewModelValueChanged = false");
					}
				}

				_viewModelValueChanged = value;
			}
		}

		private Boolean JumpCaretToEndOfIntegerOnNextProcessing
		{
			get { return _jumpCaretToEndOfIntegerOnNextProcessing; }
			set
			{
				if (value != _jumpCaretToEndOfIntegerOnNextProcessing)
				{
					if(value)
					{
						WriteLogAction(() => "  >>>> JumpCaretToEndOfIntegerOnNextProcessing = true");
					}
					else
					{
						WriteLogAction(() => "  <<<< JumpCaretToEndOfIntegerOnNextProcessing = false");
					}
				}

				_jumpCaretToEndOfIntegerOnNextProcessing = value;
			}
		}

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
		/// The value for the empty DecimalSeparator.
		/// </summary>
		public const Char DefaultDecimalSeparator = '.';

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
					_decimalSeparator = DefaultDecimalSeparator;
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
				return DecimalSeparator.InvokeNotDefault(el => el.ToString(CultureInfo.InvariantCulture), String.Empty);
			}
			set
			{
				DecimalSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
			}
		}


		/// <summary>
		/// See <see cref="DecimalSeparatorAlternative"/>.
		/// </summary>
		private char _decimalSeparatorAlternative;
		/// <summary>
		/// The additional decimal part's separator char, acceptable at the input or at a copy/paste time.
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

		/// <summary>
		/// The string representation of the <see cref="DecimalSeparator"/> char.
		/// </summary>
		public String DecimalSeparatorAlternativeChar
		{
			get
			{
				return DecimalSeparatorAlternative.InvokeNotDefault(el => el.ToString(CultureInfo.InvariantCulture), String.Empty);
			}
			set
			{
				DecimalSeparatorAlternative = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
			}
		}


		/// <summary>
		/// See <see cref="GroupSeparator"/>.
		/// </summary>
		private Char _groupSeparator;
		/// <summary>
		/// The desirable Group Separator char for a view.
		/// </summary>
		public Char GroupSeparator
		{
			get
			{
				return _groupSeparator;
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
				return GroupSeparator.InvokeNotDefault(el => el.ToString(CultureInfo.InvariantCulture), String.Empty);
			}
			set
			{
				GroupSeparator = value.InvokeNotNullOrEmpty(el => el.ToCharFirst());
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
		/// The GroupSeparator disabled on input state.
		/// </summary>
		public Boolean GroupSeparatorDisabledOnInput { get; set; }

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
		/// Dynamic value for the GropuSeparator.
		/// </summary>
		private Char GroupSeparatorDynamic
		{
			get
			{
				if(GroupSeparatorDisabledOnInput == false)
				{
					return GroupSeparator;
				}
				if(FocusState == FocusState.No)
				{
					return GroupSeparator;
				}

				return Char.MinValue;
			}
		}

		/// <summary>
		/// The string representation of the <see cref="GroupSeparatorDynamic"/> char.
		/// </summary>
		public String GroupSeparatorCharDyninic
		{
			get
			{
				return GroupSeparatorDynamic.InvokeNotDefault(el => el.ToString(CultureInfo.InvariantCulture), String.Empty);
			}
		}


		private string _textBeforeChangingNotNull;
		private bool _jumpCaretToEndOfIntegerOnNextProcessing;
		private bool _viewModelValueChanged;

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
				if (PartialDisabledCurrent)
				{
					return _registeredDigits;
				}

				return new[] { DecimalSeparator, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			}
		}

		private readonly Char[] _registeredDigits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

	}
}
