using System;
using System.Globalization;

namespace rooms
{
	public class ChatMessage
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string Message { get; set; }

		/// <summary>
		/// The ID stores the date and time, so let's extract that
		/// info and return it as a nicely formatted date/time string.
		/// </summary>
		/// <value>The date time.</value>
		public string DateTimeFormatted {
			get {
				string [] splitString = ID.Split ('-');
				string dateTimeStr = splitString [0] + "-" + splitString [1];
				DateTime dateTime = DateTime.ParseExact (dateTimeStr, "yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
				return dateTime.ToString ("dd-MMM-yyyy, hh:mm tt");

			}
		}

		public ChatMessage ()
		{
		}
	}
}

