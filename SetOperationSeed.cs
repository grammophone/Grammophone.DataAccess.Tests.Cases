namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Names of entities seeded for set-operation tests.
	/// </summary>
	public sealed class SetOperationSeed
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="albumName">The album name.</param>
		/// <param name="originalGenreName">The original genre name.</param>
		/// <param name="targetGenreName">The target genre name.</param>
		public SetOperationSeed(string albumName, string originalGenreName, string targetGenreName)
		{
			this.AlbumName = albumName;
			this.OriginalGenreName = originalGenreName;
			this.TargetGenreName = targetGenreName;
		}

		#endregion

		#region Public properties

		/// <summary>The album name.</summary>
		public string AlbumName { get; }

		/// <summary>The original genre name.</summary>
		public string OriginalGenreName { get; }

		/// <summary>The target genre name.</summary>
		public string TargetGenreName { get; }

		#endregion
	}
}
