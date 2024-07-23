using SeleniumParser.Driver;
using SeleniumParser.Models;

namespace SeleniumParser.Commands
{
	public class OpenCommand : Command
	{

		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			Current.Driver.Navigate().GoToUrl(tests.Url + command.Target);
		}

	}
}
