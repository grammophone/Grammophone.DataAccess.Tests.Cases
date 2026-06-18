using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base exception translation test cases.
	/// </summary>
	[TestClass]
	public abstract class ExceptionTranslationTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// Placeholder for provider-independent exception translation tests.
		/// </summary>
		[TestMethod]
		[Ignore("Database setup and constraint scenarios will be added in the next test iteration.")]
		public void Exception_translation_tests_are_pending()
		{
		}

		#endregion
	}
}
