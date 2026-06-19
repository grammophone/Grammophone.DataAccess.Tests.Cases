using System.Linq;
using Grammophone.DataAccess.Tests.Domain;
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
		/// Tests translation of unique constraint violations.
		/// </summary>
		[TestMethod]
		public void Duplicate_artist_name_throws_unique_constraint_violation()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var artist = domainContainer.Create<Artist>();
				artist.Name = TestData.ArtistName;

				domainContainer.Artists.Add(artist);

				Assert.ThrowsException<UniqueConstraintViolationException>(() => domainContainer.SaveChanges());
			}
		}

		/// <summary>
		/// Tests translation of referential constraint violations.
		/// </summary>
		[TestMethod]
		public void Deleting_album_with_tracks_throws_referential_constraint_violation()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = domainContainer.Albums
					.Single(a => a.Name == TestData.AlbumName);

				domainContainer.Albums.Remove(album);

				Assert.ThrowsException<ReferentialConstraintViolationException>(() => domainContainer.SaveChanges());
			}
		}

		#endregion
	}
}
