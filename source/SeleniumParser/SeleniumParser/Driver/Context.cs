using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumParser.Driver
{
	public class Context
	{

		public IWebDriver Driver { get; }

		public WebDriverWait Wait { get; }

		public Actions Actions { get; }

		public string UltimoAlerta { get; set; }

		public Context(IWebDriver driver)
		{
			Driver = driver;
			Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
			Actions = new Actions(driver);
		}

	}
}
