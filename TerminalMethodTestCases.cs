using System.Linq;
using System.Threading.Tasks;
using Grammophone.DataAccess.QueryExtensions;
using Grammophone.DataAccess.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base terminal method test cases.
	/// </summary>
	[TestClass]
	public abstract class TerminalMethodTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// Tests asynchronous materialization to a list.
		/// </summary>
		[TestMethod]
		public async Task ToListAsync_materializes_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var albums = await domainContainer.Albums
					.Where(a => a.Name == TestData.AlbumName)
					.ToListAsync();

				Assert.AreEqual(1, albums.Count);
				Assert.AreEqual(TestData.AlbumName, albums[0].Name);
			}
		}

		/// <summary>
		/// Tests asynchronous materialization to an array.
		/// </summary>
		[TestMethod]
		public async Task ToArrayAsync_materializes_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var artists = await domainContainer.Artists
					.Where(a => a.Name == TestData.ArtistName)
					.ToArrayAsync();

				Assert.AreEqual(1, artists.Length);
				Assert.AreEqual(TestData.ArtistName, artists[0].Name);
			}
		}

		/// <summary>
		/// Tests asynchronous retrieval of the first result.
		/// </summary>
		[TestMethod]
		public async Task FirstAsync_returns_first_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = await domainContainer.Albums
					.OrderBy(a => a.ID)
					.FirstAsync();

				Assert.AreEqual(TestData.AlbumName, album.Name);
			}
		}

		/// <summary>
		/// Tests asynchronous retrieval of the first matching result or default value.
		/// </summary>
		[TestMethod]
		public async Task FirstOrDefaultAsync_returns_null_for_missing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = await domainContainer.Albums
					.FirstOrDefaultAsync(a => a.Name == "Missing Album");

				Assert.IsNull(album);
			}
		}

		/// <summary>
		/// Tests asynchronous retrieval of the single matching result.
		/// </summary>
		[TestMethod]
		public async Task SingleAsync_returns_single_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var artist = await domainContainer.Artists
					.SingleAsync(a => a.Name == TestData.ArtistName);

				Assert.AreEqual(TestData.ArtistName, artist.Name);
			}
		}

		/// <summary>
		/// Tests asynchronous retrieval of the single matching result or default value.
		/// </summary>
		[TestMethod]
		public async Task SingleOrDefaultAsync_returns_null_for_missing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var genre = await domainContainer.Genres
					.SingleOrDefaultAsync(g => g.Name == "Missing Genre");

				Assert.IsNull(genre);
			}
		}

		/// <summary>
		/// Tests asynchronous existence checks.
		/// </summary>
		[TestMethod]
		public async Task AnyAsync_returns_true_for_existing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var exists = await domainContainer.Tracks
					.AnyAsync(t => t.Name == TestData.TrackName);

				Assert.IsTrue(exists);
			}
		}

		/// <summary>
		/// Tests asynchronous count aggregation.
		/// </summary>
		[TestMethod]
		public async Task CountAsync_returns_number_of_matching_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var count = await domainContainer.Tracks
					.CountAsync(t => t.Album.Name == TestData.AlbumName);

				Assert.AreEqual(1, count);
			}
		}

		/// <summary>
		/// Tests asynchronous sum aggregation on a non-empty result set.
		/// </summary>
		[TestMethod]
		public async Task SumAsync_returns_total_for_non_empty_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var totalDuration = await domainContainer.Tracks
					.Where(t => t.Album.Name == TestData.AlbumName)
					.SumAsync(t => t.DurationSeconds);

				Assert.AreEqual(TestData.TrackDurationSeconds, totalDuration);
			}
		}

		/// <summary>
		/// Tests asynchronous nullable sum aggregation on an empty result set.
		/// </summary>
		/// <remarks>
		/// SQL Server and providers differ on the final empty-set value. Projecting to a nullable value type
		/// allows the provider to materialize the result instead of failing for a non-nullable value type.
		/// </remarks>
		[TestMethod]
		public async Task Nullable_sum_async_allows_empty_result_set()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var totalDuration = await domainContainer.Tracks
					.Where(t => t.Name == "Missing Track")
					.SumAsync(t => (int?)t.DurationSeconds);

				Assert.IsTrue(totalDuration == null || totalDuration == 0);
			}
		}

		/// <summary>
		/// Tests dictionary materialization with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task ToDictionaryAsync_with_key_selector_materializes_dictionary()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var tracks = await domainContainer.Tracks.ToListAsync();
				var tracksByName = await domainContainer.Tracks.ToDictionaryAsync(t => t.Name);

				Assert.AreEqual(tracks.Count, tracksByName.Count);
				Assert.IsTrue(tracksByName.ContainsKey(TestData.TrackName));

				foreach (var track in tracks)
				{
					Assert.IsTrue(tracksByName.ContainsKey(track.Name));
					Assert.AreEqual(track.ID, tracksByName[track.Name].ID);
				}

				var seededTrack = tracksByName[TestData.TrackName];

				Assert.AreEqual(TestData.TrackName, seededTrack.Name);
				Assert.AreEqual(TestData.TrackDurationSeconds, seededTrack.DurationSeconds);
			}
		}

		/// <summary>
		/// Tests dictionary materialization with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task ToDictionaryAsync_with_key_and_value_selectors_materializes_dictionary()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var tracks = await domainContainer.Tracks.ToListAsync();
				var trackDurationsByName = await domainContainer.Tracks.ToDictionaryAsync(t => t.Name, t=> t.DurationSeconds);

				Assert.AreEqual(tracks.Count, trackDurationsByName.Count);
				Assert.IsTrue(trackDurationsByName.ContainsKey(TestData.TrackName));

				var seededTrackDuration = trackDurationsByName[TestData.TrackName];

				Assert.AreEqual(TestData.TrackDurationSeconds, seededTrackDuration);
			}
		}

		#endregion
	}
}
