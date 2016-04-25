using System.Collections.Generic;
using System.Web.Mvc;

namespace FailTracker.Web.Infrastructure.Alerts
{
	public static class AlertExtensions
	{
		const string Alerts = "_Alerts";

		public static List<Alert> GetAlerts(this TempDataDictionary tempData)
		{
			if (!tempData.ContainsKey(Alerts))
			{
				tempData[Alerts] = new List<Alert>();
			}

			return (List<Alert>) tempData[Alerts];
		}

		public static ActionResult WithSuccess(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, alertClass: "alert-success", message: message);
		}

		public static ActionResult WithInfo(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, alertClass: "alert-info", message: message);
		}

		public static ActionResult WithWarning(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, alertClass: "alert-warning", message: message);
		}

		public static ActionResult WithError(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, alertClass: "alert-danger", message: message);
		}
	}
}