using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumParser.Commands
{
    public class SelectWindowCommand : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            var window = command.Target.Replace("handle=","");
            Current.Driver.SwitchTo().Window(window);
        }
    }
}
