﻿using System;
using System.Globalization;
using System.Linq;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		public const Char NonBreakingSpaceChar = (Char)160;
		private const Char BreakingSpaceChar = (Char)32;

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


		/// <summary>
		/// See <see cref="AlternativeInputDecimalSeparator"/>.
		/// </summary>
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
		/// Available chars of a number of a custom format.
		/// </summary>
		private Char[] CustomSerialilzationChars
		{
			get
			{
				return new[] { DecimalSeparator, '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			}
		}
	}
}