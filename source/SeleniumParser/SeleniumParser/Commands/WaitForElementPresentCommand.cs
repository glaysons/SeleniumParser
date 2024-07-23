using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumParser.Commands
{
    public class WaitForElementPresentCommand : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            var waitTime = 2000;
            int.TryParse(command.Target, out waitTime);
            WaitElement(waitTime,command);

        }
    }
}
