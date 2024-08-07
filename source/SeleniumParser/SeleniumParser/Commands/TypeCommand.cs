using FluentAssertions;
using SeleniumParser.Delegates;
using SeleniumParser.Driver;
using SeleniumParser.Models;

namespace SeleniumParser.Commands
{
	public class TypeCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			var element = SearchElement(command);

			element
				.Should()
				.NotBeNull();

			var preventDefault = false;

			var customEvent = GetCustomEvent<TypeCommandDelegate>();

			var value = command.Value;

			customEvent?.Invoke(tests, test, command, element, ref value, ref preventDefault);

			if (!preventDefault)
				element.SendKeys(value);
		}
	}
}
