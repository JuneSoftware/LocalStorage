using UnityEngine;
using System.Collections;

namespace June {

	/// <summary>
	/// Debug logger, this will only print messages to console if the DEBUG_FLAG is present in the build.
	/// </summary>
	public class DebugLogger {

		/// <summary>
		/// The Debug Flag
		/// </summary>
		private const string DEBUG_FLAG = "DEBUG";

		/// <summary>
		/// Log the specified message and context.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void Log (System.Object message, UnityEngine.Object context) {
			UnityEngine.Debug.Log(message, context);
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void Log (System.Object message) {
			UnityEngine.Debug.Log(message);
		}

		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogError (System.Object message, UnityEngine.Object context) {
			UnityEngine.Debug.LogError(message, context);
		}

		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogError (System.Object message) {
			UnityEngine.Debug.LogError(message);
		}

		/// <summary>
		/// Logs the exception.
		/// </summary>
		/// <param name="exception">Exception.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogException (System.Exception exception, UnityEngine.Object context) {
			UnityEngine.Debug.LogException(exception, context);
		}

		/// <summary>
		/// Logs the exception.
		/// </summary>
		/// <param name="exception">Exception.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogException (System.Exception exception) {
			UnityEngine.Debug.LogException(exception);
		}

		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="context">Context.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogWarning (System.Object message, UnityEngine.Object context) {
			UnityEngine.Debug.LogWarning(message, context);
		}

		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">Message.</param>
		[System.Diagnostics.Conditional(DEBUG_FLAG)]
		public static void LogWarning (System.Object message) {
			UnityEngine.Debug.LogWarning(message);
		}
	}
}