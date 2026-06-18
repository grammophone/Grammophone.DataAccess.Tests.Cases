using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base query extension test cases.
	/// </summary>
	[TestClass]
	public abstract class QueryExtensionTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// Placeholder for provider-independent include tests.
		/// </summary>
		[TestMethod]
		[Ignore("Database setup and seed data will be added in the next test iteration.")]
		public void Include_tests_are_pending()
		{
		}

		#endregion
	}
}
