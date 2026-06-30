using System.Linq;
using System.Threading.Tasks;
using Grammophone.DataAccess.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base test cases for standard LINQ operations.
	/// </summary>
	[TestClass]
	public abstract class StandardLinqTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// Tests materialization to a list.
		/// </summary>
		[TestMethod]
		public void ToList_materializes_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var albums = domainContainer.Albums
					.Where(a => a.Name == TestData.AlbumName)
					.ToList();

				Assert.AreEqual(1, albums.Count);
				Assert.AreEqual(TestData.AlbumName, albums[0].Name);
			}
		}

		/// <summary>
		/// Tests materialization to an array.
		/// </summary>
		[TestMethod]
		public void ToArray_materializes_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var artists = domainContainer.Artists
					.Where(a => a.Name == TestData.ArtistName)
					.ToArray();

				Assert.AreEqual(1, artists.Length);
				Assert.AreEqual(TestData.ArtistName, artists[0].Name);
			}
		}

		/// <summary>
		/// Tests retrieval of the first result.
		/// </summary>
		[TestMethod]
		public void First_returns_first_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = domainContainer.Albums
					.OrderBy(a => a.ID)
					.First();

				Assert.AreEqual(TestData.AlbumName, album.Name);
			}
		}

		/// <summary>
		/// Tests retrieval of the first matching result or default value.
		/// </summary>
		[TestMethod]
		public void FirstOrDefault_returns_null_for_missing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = domainContainer.Albums
					.FirstOrDefault(a => a.Name == "Missing Album");

				Assert.IsNull(album);
			}
		}

		/// <summary>
		/// Tests retrieval of the single matching result.
		/// </summary>
		[TestMethod]
		public void Single_returns_single_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var artist = domainContainer.Artists
					.Single(a => a.Name == TestData.ArtistName);

				Assert.AreEqual(TestData.ArtistName, artist.Name);
			}
		}

		/// <summary>
		/// Tests retrieval of the single matching result or default value.
		/// </summary>
		[TestMethod]
		public void SingleOrDefault_returns_null_for_missing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var genre = domainContainer.Genres
					.SingleOrDefault(g => g.Name == "Missing Genre");

				Assert.IsNull(genre);
			}
		}

		/// <summary>
		/// Tests existence checks.
		/// </summary>
		[TestMethod]
		public void Any_returns_true_for_existing_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var exists = domainContainer.Tracks
					.Any(t => t.Name == TestData.TrackName);

				Assert.IsTrue(exists);
			}
		}

		/// <summary>
		/// Tests count aggregation.
		/// </summary>
		[TestMethod]
		public void Count_returns_number_of_matching_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var count = domainContainer.Tracks
					.Count(t => t.Album.Name == TestData.AlbumName);

				Assert.AreEqual(1, count);
			}
		}

		/// <summary>
		/// Tests sum aggregation on a non-empty result set.
		/// </summary>
		[TestMethod]
		public void Sum_returns_total_for_non_empty_result()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var totalDuration = domainContainer.Tracks
					.Where(t => t.Album.Name == TestData.AlbumName)
					.Sum(t => t.DurationSeconds);

				Assert.AreEqual(TestData.TrackDurationSeconds, totalDuration);
			}
		}

		/// <summary>
		/// Tests nullable sum aggregation on an empty result set.
		/// </summary>
		/// <remarks>
		/// SQL Server and providers differ on the final empty-set value. Projecting to a nullable value type
		/// allows the provider to materialize the result instead of failing for a non-nullable value type.
		/// </remarks>
		[TestMethod]
		public void Nullable_sum_allows_empty_result_set()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var totalDuration = domainContainer.Tracks
					.Where(t => t.Name == "Missing Track")
					.Sum(t => (int?)t.DurationSeconds);

				Assert.IsTrue(totalDuration == null || totalDuration == 0);
			}
		}

		/// <summary>
		/// Tests dictionary materialization with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void ToDictionary_with_key_selector_materializes_dictionary()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var tracks = domainContainer.Tracks.ToList();
				var tracksByName = domainContainer.Tracks.ToDictionary(t => t.Name);

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
		/// Tests dictionary materialization with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void ToDictionary_with_key_and_value_selectors_materializes_dictionary()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var tracks = domainContainer.Tracks.ToList();
				var trackDurationsByName = domainContainer.Tracks.ToDictionary(t => t.Name, t => t.DurationSeconds);

				Assert.AreEqual(tracks.Count, trackDurationsByName.Count);
				Assert.IsTrue(trackDurationsByName.ContainsKey(TestData.TrackName));

				var seededTrackDuration = trackDurationsByName[TestData.TrackName];

				Assert.AreEqual(TestData.TrackDurationSeconds, seededTrackDuration);
			}
		}

		#endregion
	}
}
