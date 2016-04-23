using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace FailTracker.Web.Infrastructure.Tasks
{
	public class TaskRegistry : Registry
	{
		public TaskRegistry()
		{
			Scan(scan =>
			{
				scan.AssembliesFromApplicationBaseDirectory(
					a => a.FullName.StartsWith("SmartAdminMvc"));
				scan.AddAllTypesOf<IRunAtInit>();
				scan.AddAllTypesOf<IRunAtStartup>();
				scan.AddAllTypesOf<IRunOnEachRequest>();
				scan.AddAllTypesOf<IRunOnError>();
				scan.AddAllTypesOf<IRunAfterEachRequest>();
			});
		}
	}
}