using System.Collections.Generic;

namespace SeleniumParser.Models
{
	public class SeleniumCommandModel
	{

		public string Id { get; set; }

		public string Comment { get; set; }

		public string Command { get; set; }

		public string Target { get; set; }

		public IEnumerable<string[]> Targets { get; set; }

		public string Value { get; set; }
        public IDictionary<string, object> Variables { get; set; }

		public bool OpensWindow { get; set; }
        public string WindowHandleName { get; set; }
		public int WindowTimeout { get; set; }

    }
}