using System;
using System.Linq;
using System.Threading.Tasks;
using Grammophone.DataAccess.QueryExtensions;
using Grammophone.DataAccess.Tests.Domain;
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
		/// Tests expression include with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void Include_expression_with_standard_terminal_loads_collection()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = domainContainer.Albums
					.Include(a => a.Tracks)
					.Single(a => a.Name == TestData.AlbumName);

				Assert.IsTrue(
					domainContainer.Entry(album)
						.Collection(a => a.Tracks)
						.IsLoaded);
			}
		}

		/// <summary>
		/// Tests expression include with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task Include_expression_with_async_terminal_loads_collection()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = await domainContainer.Albums
					.Include(a => a.Tracks)
					.SingleAsync(a => a.Name == TestData.AlbumName);

				Assert.IsTrue(
					domainContainer.Entry(album)
						.Collection(a => a.Tracks)
						.IsLoaded);
			}
		}

		/// <summary>
		/// Tests string include with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void Include_string_with_standard_terminal_loads_collection()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = domainContainer.Albums
					.Include(nameof(Album.Tracks))
					.Single(a => a.Name == TestData.AlbumName);

				Assert.IsTrue(
					domainContainer.Entry(album)
						.Collection(a => a.Tracks)
						.IsLoaded);
			}
		}

		/// <summary>
		/// Tests string include with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task Include_string_with_async_terminal_loads_collection()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = await domainContainer.Albums
					.Include(nameof(Album.Tracks))
					.SingleAsync(a => a.Name == TestData.AlbumName);

				Assert.IsTrue(
					domainContainer.Entry(album)
						.Collection(a => a.Tracks)
						.IsLoaded);
			}
		}

		/// <summary>
		/// Tests nested eager loading with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void ThenInclude_with_standard_terminal_loads_nested_reference()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = domainContainer.Albums
					.Include(a => a.Tracks)
					.ThenInclude(t => t.Genre)
					.Single(a => a.Name == TestData.AlbumName);

				AssertTracksAndGenresAreLoaded(domainContainer, album);
			}
		}

		/// <summary>
		/// Tests nested eager loading with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task ThenInclude_with_async_terminal_loads_nested_reference()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				domainContainer.IsLazyLoadingEnabled = false;

				var album = await domainContainer.Albums
					.Include(a => a.Tracks)
					.ThenInclude(t => t.Genre)
					.SingleAsync(a => a.Name == TestData.AlbumName);

				AssertTracksAndGenresAreLoaded(domainContainer, album);
			}
		}

		/// <summary>
		/// Tests no-tracking queries with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void AsNoTracking_with_standard_terminal_returns_detached_entity()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = domainContainer.Albums
					.AsNoTracking()
					.Single(a => a.Name == TestData.AlbumName);

				Assert.AreEqual(TrackingState.Detached, domainContainer.Entry(album).State);
			}
		}

		/// <summary>
		/// Tests no-tracking queries with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task AsNoTracking_with_async_terminal_returns_detached_entity()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var album = await domainContainer.Albums
					.AsNoTracking()
					.SingleAsync(a => a.Name == TestData.AlbumName);

				Assert.AreEqual(TrackingState.Detached, domainContainer.Entry(album).State);
			}
		}

		/// <summary>
		/// Tests the portable Like function with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void Like_with_standard_terminal_matches_pattern()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var albums = domainContainer.Albums
					.Where(a => QueryFunctions.Like(a.Name, "%Integration%"))
					.ToList();

				Assert.AreEqual(1, albums.Count);
				Assert.AreEqual(TestData.AlbumName, albums[0].Name);
			}
		}

		/// <summary>
		/// Tests the portable Like function with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task Like_with_async_terminal_matches_pattern()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var albums = await domainContainer.Albums
					.Where(a => QueryFunctions.Like(a.Name, "%Integration%"))
					.ToListAsync();

				Assert.AreEqual(1, albums.Count);
				Assert.AreEqual(TestData.AlbumName, albums[0].Name);
			}
		}

		/// <summary>
		/// Tests portable date difference with a standard terminal method.
		/// </summary>
		[TestMethod]
		public void DiffDays_with_standard_terminal_filters_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var comparisonDate = TestData.AlbumReleaseDate.AddDays(3);

				var albums = domainContainer.Albums
					.Where(a => QueryFunctions.DiffDays(a.ReleaseDate, comparisonDate) == 3)
					.ToList();

				Assert.AreEqual(1, albums.Count);
			}
		}

		/// <summary>
		/// Tests portable date difference with an async terminal method.
		/// </summary>
		[TestMethod]
		public async Task DiffDays_with_async_terminal_filters_results()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var comparisonDate = TestData.AlbumReleaseDate.AddDays(3);

				var albums = await domainContainer.Albums
					.Where(a => QueryFunctions.DiffDays(a.ReleaseDate, comparisonDate) == 3)
					.ToListAsync();

				Assert.AreEqual(1, albums.Count);
			}
		}

		#endregion

		#region Private methods

		private static void AssertTracksAndGenresAreLoaded(IMusicDomainContainer domainContainer, Album album)
		{
			Assert.IsTrue(
				domainContainer.Entry(album)
					.Collection(a => a.Tracks)
					.IsLoaded);

			foreach (var track in album.Tracks)
			{
				Assert.IsTrue(
					domainContainer.Entry(track)
						.Reference(t => t.Genre)
						.IsLoaded);
			}
		}

		#endregion
	}
}
