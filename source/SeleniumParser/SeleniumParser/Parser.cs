using OpenQA.Selenium;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.IO;

namespace SeleniumParser
{
	public class Parser
	{

		public void ParseOneTestByBrowserInstance(string sideFile, Func<IWebDriver> driverConstructor)
		{
			var tests = ConvertSideFileToModel(sideFile);
			foreach (var test in tests.Tests)
			{
				using (var driver = driverConstructor())
				{
					var context = new Context(driver);
					foreach (var comando in test.Commands)
						CommandFactory.Criar(context, comando.Command)
							.Perform(tests, test, comando);
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
					foreach (var comando in test.Commands)
						CommandFactory.Criar(context, comando.Command)
							.Perform(tests, test, comando);
				}
			}
		}

	}
}
