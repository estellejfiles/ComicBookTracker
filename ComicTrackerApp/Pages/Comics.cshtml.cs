using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.ComponentModel;

namespace ComicTrackerApp.Pages
{
    public class ComicsModel : PageModel
    {
        [BindProperty]
        // list that will hold all comics
        public List<Comic> Comics {get; set;} = new List<Comic>();
        [BindProperty]
        public List<int> SelectedIds {get; set;} = new List<int>();
        // handles HTTP Get request; initializes ComicList from db
        public void OnGet()
        {
            LoadComicList();
        }

        // on post method that updates wishlist, wishlist, and reloads comic list
        public void OnPost()
        {
            UpdateInWishlist(SelectedIds);
            UpdateInCollection(SelectedIds);
            LoadComicList();
        }

        // helper method to load comics from db; only called within this class
        private void LoadComicList()
        {
            // create a connection to the SQLite database
            using (var connection = new SqliteConnection("Data Source=Comics.db"))
            {
                // open the connection
                connection.Open();

                // create SQL command and set up query to select all comics
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Comics";

                // store comics in list
                var comics = new List<Comic>();

                // reader
                using (var reader = command.ExecuteReader())
                {
                    // while reader is open
                    while (reader.Read())
                    {
                        // create new Comic object instance based on data being read in from db
                        var comic = new Comic
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Year = reader.GetInt32(2),
                            Description = reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            ImageFileName = reader.GetString(5),
                            InWishlist = reader.GetBoolean(6),
                            InCollection = reader.GetBoolean(7)
                        };
                        Comics.Add(comic);
                    }
                }
            }
        }

        // helper method that updates InWishlist attribute
        private void UpdateInWishlist(List<int> selectedIds)
        {
            using (var connection = new SqliteConnection("Data Source=Comics.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // First, set all to false
                command.CommandText = "UPDATE Comics SET InWishlist = 0";
                command.ExecuteNonQuery();
                // Then, set selected to true
                foreach (var id in selectedIds)
                {
                    command.CommandText = $"UPDATE Comics SET InWishlist = 1 WHERE Id = {id}";
                    command.ExecuteNonQuery();
                }
            }
        }
        
        // helper method to edit InCollection attribute
        private void UpdateInCollection(List<int> selectedIds)
        {
            using (var connection = new SqliteConnection("Data Source=Comics.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // First, set all to false
                command.CommandText = "UPDATE Comics SET InCollection = 0";
                command.ExecuteNonQuery();
                // Then, set selected to true
                foreach (var id in selectedIds)
                {
                    command.CommandText = $"UPDATE Comics SET InCollection = 1 WHERE Id = {id}";
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    // class representing a Comic object
    public class Comic
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public int Year {get; set;}
        public string Description {get; set;}
        public decimal Price {get; set;}
        public string ImageFileName {get; set;}
        public bool InWishlist {get; set;}
        public bool InCollection {get; set;}
    }
}