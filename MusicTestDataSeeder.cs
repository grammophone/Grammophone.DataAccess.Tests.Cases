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

		#endregion
	}
}
