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
    public class PauseCommand : Command
    {
        public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
        {
            if (int.TryParse(command.Target, out var pause))
            {
                Thread.Sleep(pause);
            }
            else throw new Exception("Pause interval not specified");
        }
    }
}
