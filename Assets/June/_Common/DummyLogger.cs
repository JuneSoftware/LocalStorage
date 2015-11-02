using UnityEngine;
using System.Collections;

namespace June {
	
	/// <summary>
	/// Dummy logger, this will never print anything to the console.
	/// </summary>
	public class DummyLogger {
		
		/// <summary>
		/// Calls to all DummyLogger logging methods will be omitted depending on whether this symbol is defined.
		/// </summary>
		private const string DUMMY_LOGGER_FLAG = "ENABLE_DUMMY_LOGGER";
		
		/// <summary>
		/// Log the specified message and context.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void Log (System.Object message, UnityEngine.Object context) { }
		
		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void Log (System.Object message) { }
		
		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogError (System.Object message, UnityEngine.Object context) { }
		
		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogError (System.Object message) { }
		
		/// <summary>
		/// Logs the exception.
		/// </summary>
		/// <param name="exception">Exception.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogException (System.Exception exception, UnityEngine.Object context) { }
		
		/// <summary>
		/// Logs the exception.
		/// </summary>
		/// <param name="exception">Exception.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogException (System.Exception exception) { }
		
		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogWarning (System.Object message, UnityEngine.Object context) { }
		
		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DUMMY_LOGGER_FLAG)]
		public static void LogWarning (System.Object message) { }
	}
}