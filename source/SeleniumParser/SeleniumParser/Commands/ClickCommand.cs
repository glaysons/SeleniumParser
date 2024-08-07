using FluentAssertions;
using SeleniumParser.Delegates;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using System.Threading;

namespace SeleniumParser.Commands
{
	public class ClickCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
            try
            {
                PerformClick(tests,test,command);
            }
            catch
            {
                Thread.Sleep(3000);
                PerformClick(tests, test, command);
            }
			
		}

			private void PerformClick(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
			{
            var element = SearchElement(command);

            element
                .Should()
                .NotBeNull();

            var preventDefault = false;

            var customEvent = GetCustomEvent<ClickCommandDelegate>();

            customEvent?.Invoke(tests, test, command, element, ref preventDefault);

            if (!preventDefault)
                element.Click();
        }
	}
}
