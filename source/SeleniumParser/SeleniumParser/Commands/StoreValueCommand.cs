using FluentAssertions;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumParser.Commands
{
    public class StoreValue : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            var element = SearchElement(command);

            element
                .Should()
                .NotBeNull();

            var value = element.GetAttribute("value");
            

            if (command.Variables.ContainsKey(command.Value))
                command.Variables[command.Value] = value;
            else
                command.Variables.Add(command.Value, value);
        }
    }
}
