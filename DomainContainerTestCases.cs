using Grammophone.DataAccess.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base class for provider-independent data access test cases.
	/// </summary>
	public abstract class DomainContainerTestCases
	{
		#region Protected methods

		/// <summary>
		/// Create a domain container for a test case.
		/// </summary>
		/// <returns>Returns the test domain container.</returns>
		protected abstract IMusicDomainContainer CreateDomainContainer();

		/// <summary>
		/// Verifies that a provider-specific test project can create a test domain container.
		/// </summary>
		[TestMethod]
		public void Can_create_domain_container()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				Assert.IsNotNull(domainContainer);
			}
		}

		#endregion
	}
}
