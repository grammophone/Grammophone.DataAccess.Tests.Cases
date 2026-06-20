using System.Linq;
using System.Threading.Tasks;
using Grammophone.DataAccess.QueryExtensions;
using Grammophone.DataAccess.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base test cases for set-based query operations.
	/// </summary>
	[TestClass]
	public abstract class SetOperationsTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// Tests asynchronous set-based deletion.
		/// </summary>
		[TestMethod]
		public async Task ExecuteDeleteAsync_deletes_matching_tracks()
		{
			SetOperationSeed seed;

			using (var domainContainer = CreateDomainContainer())
			{
				seed = MusicTestDataSeeder.SeedSetOperationData(domainContainer, nameof(ExecuteDeleteAsync_deletes_matching_tracks));
			}

			using (var domainContainer = CreateDomainContainer())
			{
				var deleted = await domainContainer.Tracks
					.Where(t => t.Album.Name == seed.AlbumName)
					.ExecuteDeleteAsync();

				Assert.AreEqual(2, deleted);
			}

			using (var domainContainer = CreateDomainContainer())
			{
				var remaining = domainContainer.Tracks
					.Count(t => t.Album.Name == seed.AlbumName);

				Assert.AreEqual(0, remaining);
			}
		}

		/// <summary>
		/// Tests asynchronous set-based update.
		/// </summary>
		[TestMethod]
		public async Task ExecuteUpdateAsync_updates_matching_tracks()
		{
			SetOperationSeed seed;

			using (var domainContainer = CreateDomainContainer())
			{
				seed = MusicTestDataSeeder.SeedSetOperationData(domainContainer, nameof(ExecuteUpdateAsync_updates_matching_tracks));
			}

			int targetGenreID;

			using (var domainContainer = CreateDomainContainer())
			{
				targetGenreID = domainContainer.Genres
					.Where(g => g.Name == seed.TargetGenreName)
					.Select(g => g.ID)
					.Single();

				var updated = await domainContainer.Tracks
					.Where(t => t.Album.Name == seed.AlbumName)
					.ExecuteUpdateAsync(setters => setters
						.SetProperty(t => t.GenreID, targetGenreID));

				Assert.AreEqual(2, updated);
			}

			using (var domainContainer = CreateDomainContainer())
			{
				var updatedCount = domainContainer.Tracks
					.Count(t => t.Album.Name == seed.AlbumName && t.Genre.Name == seed.TargetGenreName);

				Assert.AreEqual(2, updatedCount);
			}
		}

		#endregion
	}
}
