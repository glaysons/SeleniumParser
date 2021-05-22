using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumParser;
using System.IO;

namespace Sample.Tests
{
	[TestClass]
	public class MinhaPaginaTest
	{

		[TestMethod]
		public void AoInserirUmItemNaGradeAGradeDeveSerPreenchida()
		{
			var enderecoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "WebForms-Sample-1.side");
			new Parser().ParseAllTestsOnSameBrowserInstance(enderecoArquivo, CriarChrome);
		}

		private IWebDriver CriarChrome()
		{
			return new ChromeDriver(Directory.GetCurrentDirectory());
		}

	}
}
