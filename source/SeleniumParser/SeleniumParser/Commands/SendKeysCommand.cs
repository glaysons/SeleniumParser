using FluentAssertions;
using OpenQA.Selenium;
using System.Text;
using SeleniumParser.Driver;
using SeleniumParser.Models;

namespace SeleniumParser.Commands
{
	public class SendKeysCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel comand)
		{
			var element = SearchElement(comand);

			element
				.Should()
				.NotBeNull();

			element.SendKeys(FindKeys(comand));
		}

		private string FindKeys(SeleniumCommandModel sender)
		{
			var teclas = new StringBuilder();

			if (sender.Value.ContainsText("${KEY_ENTER}"))
				teclas.Append(Keys.Enter);

			return teclas.ToString();
		}

	}
}
