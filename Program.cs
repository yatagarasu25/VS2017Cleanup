using Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_cleanup
{
	class Program
	{
		static void Main(string[] args)
		{
			var json = JsonParser.FromJson(File.ReadAllText("Catalog.json"));
			HashSet<string> directories = new HashSet<string>(Directory.EnumerateDirectories("."));
			HashSet<string> systemDirectories = new HashSet<string>(directories);

			foreach (Dictionary<string, object> package in (List<object>)json["packages"])
			{
				string packageId = (string)package["id"];
				string packageName = packageId;
				packageName += ",version=" + (string)package["version"];
				if (package.ContainsKey("chip")) packageName += ",chip=" + (string)package["chip"];
				if (package.ContainsKey("language")) packageName += ",language=" + (string)package["language"];

				directories.Remove(".\\" + packageName);
				systemDirectories.RemoveWhere((s) => s.StartsWith(".\\" + packageId));
			}

			foreach (var sysstem in systemDirectories)
			{
				directories.Remove(sysstem);
			}

			if (args.Length > 0 && args[0] == "--delete")
			{
				foreach (var obsolete in directories)
				{
					Directory.Delete(obsolete, true);
				}
			}
			else
			{
				foreach (var obsolete in directories)
				{
					Console.WriteLine("{0}", obsolete);
				}
			}
		}
	}
}
