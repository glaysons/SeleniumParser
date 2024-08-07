using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumParser.Commands
{
    public class StoreCommand : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            if (command.Variables.ContainsKey(command.Value))
                command.Variables[command.Value] = command.Target;
            else
                command.Variables.Add(command.Value,command.Target);
        }
    }
}
