using FluentAssertions;
using OpenQA.Selenium;
using System.Text;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using SeleniumParser.Delegates;

namespace SeleniumParser.Commands
{
	public class SendKeysCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			var element = SearchElement(command);

			element
				.Should()
				.NotBeNull();

			var preventDefault = false;

			var customEvent = GetCustomEvent<SendKeysCommandDelegate>();

			var value = FindKeys(command);

			customEvent?.Invoke(tests, test, command, element, ref value, ref preventDefault);

			if (!preventDefault)
				element.SendKeys(value);
		}

		private string FindKeys(SeleniumCommandModel sender)
		{
			var teclas = new StringBuilder();

			teclas.Append(sender.Value.Replace("${KEY_ENTER}", ""));

         //   if (sender.Value.ContainsText("${KEY_ENTER}"))
			teclas.Append(Keys.Enter);

			return teclas.ToString();
		}

	}
}
