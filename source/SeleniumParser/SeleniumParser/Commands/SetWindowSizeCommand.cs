using System.Drawing;
using SeleniumParser.Driver;
using SeleniumParser.Models;

namespace SeleniumParser.Commands
{
	public class SetWindowSizeCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			var size = command.Target.Split('x');
			if (size.Length > 1)
				Current.Driver.Manage().Window.Size = new Size(size[0].ToInt(), size[1].ToInt());
		}
	}
}
