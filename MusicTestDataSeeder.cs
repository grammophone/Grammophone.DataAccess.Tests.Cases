using Grammophone.DataAccess.Tests.Domain;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Seeds the music test domain.
	/// </summary>
	public static class MusicTestDataSeeder
	{
		#region Public methods

		/// <summary>
		/// Seed test data into a music domain container.
		/// </summary>
		/// <param name="domainContainer">The domain container to seed.</param>
		public static void Seed(IMusicDomainContainer domainContainer)
		{
			var artist = domainContainer.Create<Artist>();
			artist.Name = TestData.ArtistName;

			var genre = domainContainer.Create<Genre>();
			genre.Name = TestData.GenreName;

			var album = domainContainer.Create<Album>();
			album.Name = TestData.AlbumName;
			album.ReleaseDate = TestData.AlbumReleaseDate;
			album.Artist = artist;
			album.Genre = genre;

			var track = domainContainer.Create<Track>();
			track.Name = TestData.TrackName;
			track.Number = 1;
			track.DurationSeconds = TestData.TrackDurationSeconds;
			track.Album = album;
			track.Genre = genre;

			domainContainer.Artists.Add(artist);
			domainContainer.Genres.Add(genre);
			domainContainer.Albums.Add(album);
			domainContainer.Tracks.Add(track);

			domainContainer.SaveChanges();
		}

		/// <summary>
		/// Seed additional data for set-operation tests.
		/// </summary>
		/// <param name="domainContainer">The domain container to seed.</param>
		/// <param name="suffix">A suffix used to keep unique values per test.</param>
		/// <returns>Returns the seeded data names.</returns>
		public static SetOperationSeed SeedSetOperationData(IMusicDomainContainer domainContainer, string suffix)
		{
			var artist = domainContainer.Create<Artist>();
			artist.Name = $"{TestData.SetOperationArtistNamePrefix} {suffix}";

			var originalGenre = domainContainer.Create<Genre>();
			originalGenre.Name = $"{TestData.SetOperationOriginalGenreNamePrefix} {suffix}";

			var targetGenre = domainContainer.Create<Genre>();
			targetGenre.Name = $"{TestData.SetOperationTargetGenreNamePrefix} {suffix}";

			var album = domainContainer.Create<Album>();
			album.Name = $"{TestData.SetOperationAlbumNamePrefix} {suffix}";
			album.ReleaseDate = TestData.AlbumReleaseDate;
			album.Artist = artist;
			album.Genre = originalGenre;

			var trackOne = domainContainer.Create<Track>();
			trackOne.Name = $"{TestData.SetOperationTrackOneNamePrefix} {suffix}";
			trackOne.Number = 1;
			trackOne.DurationSeconds = TestData.TrackDurationSeconds;
			trackOne.Album = album;
			trackOne.Genre = originalGenre;

			var trackTwo = domainContainer.Create<Track>();
			trackTwo.Name = $"{TestData.SetOperationTrackTwoNamePrefix} {suffix}";
			trackTwo.Number = 2;
			trackTwo.DurationSeconds = TestData.TrackDurationSeconds;
			trackTwo.Album = album;
			trackTwo.Genre = originalGenre;

			domainContainer.Artists.Add(artist);
			domainContainer.Genres.Add(originalGenre);
			domainContainer.Genres.Add(targetGenre);
			domainContainer.Albums.Add(album);
			domainContainer.Tracks.Add(trackOne);
			domainContainer.Tracks.Add(trackTwo);

			domainContainer.SaveChanges();

			return new SetOperationSeed(album.Name, originalGenre.Name, targetGenre.Name);
		}

		#endregion
	}
}
