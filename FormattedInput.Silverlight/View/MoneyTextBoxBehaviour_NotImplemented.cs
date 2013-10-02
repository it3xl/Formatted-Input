using System;

namespace It3xl.FormattedInput.View
{
	/// <summary>
	/// It's not implemented. See remarks here.
	/// </summary>
	/// <remarks>
	/// This project was started as a Silverlight Behaviour (when it was ported from JavaScript).
	/// But now I decided to see it in a form of some TextBox's extention as is the (<see cref="MoneyTextBox"/>).
	/// Nothing blocks to port it to a Behaviour at any time.
	/// But now I block it as an additional feature.
	/// </remarks>
	public class MoneyTextBoxBehaviour_NotImplemented
		//: Behavior<TextBox>
	{
		public MoneyTextBoxBehaviour_NotImplemented()
		{
			throw new NotImplementedException(
				String.Format("The {0} class is not implemented. See the remarks at the class defenition.",
				GetType().Name
				));
		}
	}
}
