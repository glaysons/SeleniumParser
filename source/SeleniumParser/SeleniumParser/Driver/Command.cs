using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumParser.Delegates;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace SeleniumParser.Driver
{
	public abstract class Command : ICommand
	{

		public Context Current { get; set; }

		public abstract void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command);

		public void PerformInternal(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
            var eventArgs = new CommandCompleteEventArgs()
            {
                StartDate = DateTime.Now,
                Success = true
            };

            var customEvent = GetCustomEvent<CommandCompleteDelegate>();

            var vars = new Dictionary<string, object>();
            foreach (var key in command.Variables.Keys)
                vars.Add(key, command.Variables[key]);

            try
            {
                LoadWindowHandles(command);
                Perform(tests, test, command);
                if (command.OpensWindow)
                {
                    command.Variables[command.WindowHandleName] = WaitForWindow(command);
                }
                var newVars = new Dictionary<string, object>();
                foreach(var key in vars.Keys)
                {
                    if(!command.Variables.ContainsKey(key) || command.Variables[key] != vars[key])
                    {
                        newVars.Add(key, vars[key]);
                    }
                }
                eventArgs.EndDate = DateTime.Now;
                eventArgs.ChangedVariables = newVars;
            }
            catch (Exception ex)
            {
                eventArgs.EndDate = DateTime.Now;
                eventArgs.Success = false;
                eventArgs.Exception = ex;
            }
            customEvent?.Invoke(tests, test, command, eventArgs);

            if (eventArgs.Exception != null)
                throw eventArgs.Exception;

        }


        protected IWebElement SearchElement(SeleniumCommandModel sender)
		{
			IWebElement element = null;

			if (TryGetElement(sender, out element))
				return element;

			var message = CreateMessage(sender);
			throw new Exception(message.ToString());
		}

		protected bool TryGetElement(SeleniumCommandModel sender, out IWebElement el)
		{
			IWebElement element = null;
            try
            {
                Current.Wait.Timeout = TimeSpan.FromSeconds(60);
                Current.Wait.PollingInterval = TimeSpan.FromSeconds(1);
                var elementFound = Current.Wait.Until(d =>
                {
                    return TryGetElementInternal(Current.Driver, sender, out element);
                });
                el = element;
                return elementFound;

            }
            catch
            {
                el = null;
                return false;

            }

        }

        private bool TryGetElementInternal(IWebDriver driver,SeleniumCommandModel sender, out IWebElement el)
        {
            IWebElement element = null;

            foreach (var target in sender.Targets)
            {
                if (TryGetTargetElement(driver,target, out element) && (element != null))
                {
                    el = element;
                    return true;
                }

            }

            if (!string.IsNullOrEmpty(sender.Target))
            {
                var target = CreateTarget(sender.Target);
                if ((target != null) && TryGetTargetElement(driver, target, out element) && (element != null))
                {
                    el = element;
                    return true;
                }
            }

            el = null;
            return false;
        }

        private bool TryGetTargetElement(IWebDriver driver,string[] target, out IWebElement element)
		{
			if ((target.Length < 2) || !target[0].ContainsText("="))
			{
				element = null;
				return false;
			}

			element = SearchWebElement(driver, target);
			return element != null;
		}

        protected int CountElements(SeleniumCommandModel sender)
        {
            List<IWebElement> elements = new List<IWebElement>();

            foreach (var target in sender.Targets)
            {
                if (!((target.Length < 2) || !target[0].ContainsText("=")))
                    elements.AddRange(SearchWebElements(Current.Driver, target));
            }

            if (!string.IsNullOrEmpty(sender.Target))
            {
                var target = CreateTarget(sender.Target);
                if (target != null) 
                {
                    elements.AddRange(SearchWebElements(Current.Driver, target));
                }
            }
            
            return elements.Distinct().Count();
        }

        private IList<IWebElement> SearchWebElements(IWebDriver driver, string[] target)
        {
            var targetType = target[1];
            var targetValue = target[0];
            targetValue = targetValue.Replace($"{targetValue.Split('=')[0]}=", "");

            try
            {
                switch (targetType)
                {
                    case "id":
                        return driver.FindElements(By.Id(targetValue));
                    case "name":
                        return driver.FindElements(By.Name(targetValue));
                    case "css:finder":
                        return driver.FindElements(By.CssSelector(targetValue));
                    case "xpath":
                    case "xpath:attributes":
                    case "xpath:idRelative":
                    case "xpath:position":
                        return driver.FindElements(By.XPath(targetValue));
                    case "linkText":
                        return driver.FindElements(By.LinkText(targetValue));
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private IWebElement SearchWebElement(IWebDriver driver,string[] target)
		{
			var targetType = target[1];
			var targetValue = target[0];
            targetValue = targetValue.Replace($"{targetValue.Split('=')[0]}=","");

			try
			{
                switch (targetType)
                {
                    case "id":
                        return driver.FindElement(By.Id(targetValue));
                    case "name":
                        return driver.FindElement(By.Name(targetValue));
                    case "css:finder":
                        return driver.FindElement(By.CssSelector(targetValue));
                    case "xpath":
                    case "xpath:attributes":
                    case "xpath:idRelative":
                    case "xpath:position":
                    case "xpath:innerText":
                        return driver.FindElement(By.XPath(targetValue));
                    case "linkText":
                        return driver.FindElement(By.LinkText(targetValue));
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

		private string[] CreateTarget(string target)
		{
			string targetType = null;

			if (target.StartsWithText("css="))
				targetType = "css:finder";

			else if (target.StartsWithText("id="))
				targetType = "id";

			else if (target.StartsWithText("name="))
				targetType = "name";

			else if(target.StartsWithText("linkText="))
                targetType = "linkText";
            else if (target.StartsWithText("xpath="))
                targetType = "xpath";

            if (targetType == null)
				return null;

			return new[] { target, targetType };
		}

		private StringBuilder CreateMessage(SeleniumCommandModel sender)
		{
			var message = new StringBuilder();

			message.Append("Could not find component: ");
			message.Append(sender.Targets.FirstOrDefault());

			if (!string.IsNullOrEmpty(sender.Comment))
			{
				message.AppendLine();
				message.Append(sender.Comment);
			}

			return message;
		}

		protected void Wait(double delay)
		{
			var now = DateTime.Now;
			var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
			wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
			wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
		}

        protected void WaitElement(double delay, SeleniumCommandModel sender)
		{
            var now = DateTime.Now;
            var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
			wait.Until(wd => TryGetElementInternal(wd,sender, out var element));

        }
        protected void WaitElementNotPresent(double delay, SeleniumCommandModel sender)
        {
            var now = DateTime.Now;
            var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            wait.Until(wd => !TryGetElementInternal(wd, sender, out var element));

        }

        protected void WaitElementVisible(double delay, SeleniumCommandModel sender)
        {
            var now = DateTime.Now;
            var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            wait.Until(wd => TryGetElementInternal(wd,sender, out var element) && element.Displayed);
        }

        protected void LoadWindowHandles(SeleniumCommandModel command)
		{
            command.Variables["WindowHandles"] = Current.Driver.WindowHandles;

        }
        protected string WaitForWindow(SeleniumCommandModel command)
        {
            try
            {
                Thread.Sleep(command.WindowTimeout);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            var whNow = ((IReadOnlyCollection<object>)Current.Driver.WindowHandles).ToList();
            var whThen = ((IReadOnlyCollection<object>)command.Variables["WindowHandles"]).ToList();
            if (whNow.Count > whThen.Count)
            {
                return whNow.Except(whThen).First().ToString();
            }
            else
            {
                return whNow.First().ToString();
            }
        }


        protected T GetCustomEvent<T>() where T : Delegate
		{
			if (Current.Events.TryGetValue(typeof(T), out Delegate customEvent))
				return customEvent as T;
			return null;
		}

	}
}
