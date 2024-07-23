using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SeleniumParser.Commands
{
    public class StoreWindowHandleCommand : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            var value = Current.Driver.CurrentWindowHandle;


            if (command.Variables.ContainsKey(command.Target))
                command.Variables[command.Target] = value;
            else
                command.Variables.Add(command.Target, value);
        }
    }
}
