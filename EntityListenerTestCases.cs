using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grammophone.DataAccess.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grammophone.DataAccess.Tests.Cases
{
	/// <summary>
	/// Base entity listener test cases.
	/// </summary>
	[TestClass]
	public abstract class EntityListenerTestCases : DomainContainerTestCases
	{
		#region Public methods

		/// <summary>
		/// SaveChanges notifies listeners before and after adding an entity.
		/// </summary>
		[TestMethod]
		public void SaveChanges_notifies_listener_when_adding_entity()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var listener = new RecordingEntityListener();
				domainContainer.EntityListeners.Add(listener);

				var genre = CreateGenre(domainContainer, nameof(SaveChanges_notifies_listener_when_adding_entity));

				domainContainer.Genres.Add(genre);
				domainContainer.SaveChanges();

				CollectionAssert.Contains(listener.AddingEntities, genre);
				CollectionAssert.Contains(listener.AddedEntities, genre);
				Assert.AreEqual(0, listener.ChangingEntities.Count);
				Assert.AreEqual(0, listener.DeletingEntities.Count);
			}
		}

		/// <summary>
		/// SaveChangesAsync notifies listeners before and after adding an entity.
		/// </summary>
		[TestMethod]
		public async Task SaveChangesAsync_notifies_listener_when_adding_entity()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var listener = new RecordingEntityListener();
				domainContainer.EntityListeners.Add(listener);

				var genre = CreateGenre(domainContainer, nameof(SaveChangesAsync_notifies_listener_when_adding_entity));

				domainContainer.Genres.Add(genre);
				await domainContainer.SaveChangesAsync();

				CollectionAssert.Contains(listener.AddingEntities, genre);
				CollectionAssert.Contains(listener.AddedEntities, genre);
				Assert.AreEqual(0, listener.ChangingEntities.Count);
				Assert.AreEqual(0, listener.DeletingEntities.Count);
			}
		}

		/// <summary>
		/// SaveChanges notifies listeners before changing and deleting entities.
		/// </summary>
		[TestMethod]
		public void SaveChanges_notifies_listener_when_changing_and_deleting_entities()
		{
			using (var domainContainer = CreateDomainContainer())
			{
				var genreToChange = AddGenre(domainContainer, nameof(SaveChanges_notifies_listener_when_changing_and_deleting_entities), "Change");
				var genreToDelete = AddGenre(domainContainer, nameof(SaveChanges_notifies_listener_when_changing_and_deleting_entities), "Delete");

				var listener = new RecordingEntityListener();
				domainContainer.EntityListeners.Add(listener);

				genreToChange.Name = CreateUniqueName(nameof(SaveChanges_notifies_listener_when_changing_and_deleting_entities), "Changed");
				domainContainer.Genres.Remove(genreToDelete);
				domainContainer.SaveChanges();

				CollectionAssert.Contains(listener.ChangingEntities, genreToChange);
				CollectionAssert.Contains(listener.DeletingEntities, genreToDelete);
				Assert.AreEqual(0, listener.AddingEntities.Count);
				Assert.AreEqual(0, listener.AddedEntities.Count);
			}
		}

		/// <summary>
		/// Materialization notifies listeners when reading an entity from storage.
		/// </summary>
		[TestMethod]
		public void Query_materialization_notifies_listener_when_reading_entity()
		{
			string genreName;

			using (var domainContainer = CreateDomainContainer())
			{
				var genre = AddGenre(domainContainer, nameof(Query_materialization_notifies_listener_when_reading_entity), "Read");
				genreName = genre.Name;
			}

			using (var domainContainer = CreateDomainContainer())
			{
				var listener = new RecordingEntityListener();
				domainContainer.EntityListeners.Add(listener);

				var genre = domainContainer.Genres.Single(g => g.Name == genreName);

				CollectionAssert.Contains(listener.ReadEntities, genre);
			}
		}

		#endregion

		#region Private methods

		private static Genre AddGenre(IMusicDomainContainer domainContainer, string testName, string suffix)
		{
			var genre = CreateGenre(domainContainer, testName, suffix);

			domainContainer.Genres.Add(genre);
			domainContainer.SaveChanges();

			return genre;
		}

		private static Genre CreateGenre(IMusicDomainContainer domainContainer, string testName, string suffix = null)
		{
			var genre = domainContainer.Create<Genre>();

			genre.Name = CreateUniqueName(testName, suffix);

			return genre;
		}

		private static string CreateUniqueName(string testName, string suffix)
		{
			return $"{testName} {suffix} {Guid.NewGuid():N}";
		}

		#endregion

		#region Private classes

		private sealed class RecordingEntityListener : IEntityListener
		{
			public List<object> AddingEntities { get; } = new List<object>();

			public List<object> DeletingEntities { get; } = new List<object>();

			public List<object> ChangingEntities { get; } = new List<object>();

			public List<object> ReadEntities { get; } = new List<object>();

			public List<object> AddedEntities { get; } = new List<object>();

			public void OnAdding(object entity) => AddingEntities.Add(entity);

			public void OnDeleting(object entity) => DeletingEntities.Add(entity);

			public void OnChanging(object entity) => ChangingEntities.Add(entity);

			public void OnRead(object entity) => ReadEntities.Add(entity);

			public void OnAdded(object entity) => AddedEntities.Add(entity);
		}

		#endregion
	}
}
