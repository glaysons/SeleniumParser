using SeleniumParser.Models;

namespace SeleniumParser.Driver
{
	public interface ICommand
	{
		void Perform(SeleniumSideModel testes, SeleniumTestModel teste, SeleniumCommandModel comando);
	}
}
