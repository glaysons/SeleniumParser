using OpenQA.Selenium;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.IO;

namespace SeleniumParser
{
	public class Parser
	{

		public void ParseTests(string sideFile, IWebDriver driver)
		{
			var tests = ConvertSideFileToModel(sideFile);
			foreach (var test in tests.Tests)
			{
				var context = new Context(driver);
				foreach (var command in test.Commands)
					PerformCommand(tests, context, test, command);
			}
		}

		private void PerformCommand(SeleniumSideModel tests, Context context, SeleniumTestModel test, SeleniumCommandModel command)
		{
			var current = CommandFactory.Create(context, command.Command);
			if (!(current is INextCommand))
				current.Perform(tests, test, command);
			if (context.LastCommand is INextCommand)
				context.LastCommand.Perform(tests, test, command);
			context.LastCommand = current;
		}

		public void ParseOneTestByBrowserInstance(string sideFile, Func<IWebDriver> driverConstructor)
		{
			var tests = ConvertSideFileToModel(sideFile);
			foreach (var test in tests.Tests)
			{
				using (var driver = driverConstructor())
				{
					var context = new Context(driver);
					foreach (var command in test.Commands)
						PerformCommand(tests, context, test, command);
				}
			}
		}

		private SeleniumSideModel ConvertSideFileToModel(string sideFile)
		{
			using (var arquivo = new StreamReader(sideFile))
				return Newtonsoft.Json.JsonConvert.DeserializeObject<SeleniumSideModel>(arquivo.ReadToEnd());
		}

		public void ParseAllTestsOnSameBrowserInstance(string sideFile, Func<IWebDriver> driverConstructor)
		{
			var tests = ConvertSideFileToModel(sideFile);
			using (var driver = driverConstructor())
			{
				var context = new Context(driver);
				foreach (var test in tests.Tests)
				{
					foreach (var command in test.Commands)
						PerformCommand(tests, context, test, command);
				}
			}
		}

	}
}
