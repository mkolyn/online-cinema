namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexForeignKey : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Cinemas", "CityID", "dbo.Cities", "ID");
            AddForeignKey("dbo.CinemaHalls", "CinemaID", "dbo.Cinemas", "ID");
            AddForeignKey("dbo.CinemaHallMovies", "CinemaHallID", "dbo.CinemaHalls", "ID");
            AddForeignKey("dbo.CinemaHallMovies", "MovieID", "dbo.Movies", "ID");
            AddForeignKey("dbo.CinemaHallMoviePlaces", "CinemaHallMovieID", "dbo.CinemaHallMovies", "ID");
            AddForeignKey("dbo.CinemaHallMoviePlaces", "CinemaHallPlaceID", "dbo.CinemaHallPlaces", "ID");
            AddForeignKey("dbo.CinemaHallPlaces", "CinemaHallID", "dbo.CinemaHalls", "ID");
            AddForeignKey("dbo.CinemaMovies", "CinemaID", "dbo.Cinemas", "ID");
            AddForeignKey("dbo.CinemaMovies", "MovieID", "dbo.Movies", "ID");
            AddForeignKey("dbo.Movies", "GenreID", "dbo.Genres", "ID");
            AddForeignKey("dbo.Users", "CinemaID", "dbo.Cinemas", "ID");
            AddForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders", "ID");
            AddForeignKey("dbo.OrderItems", "CinemaHallMovieID", "dbo.CinemaHallMovies", "ID");
            AddForeignKey("dbo.OrderItems", "CinemaHallPlaceID", "dbo.CinemaHallPlaces", "ID");

            CreateIndex("dbo.Cinemas", "CityID");
            CreateIndex("dbo.CinemaHalls", "CinemaID");
            CreateIndex("dbo.CinemaHallMovies", "CinemaHallID");
            CreateIndex("dbo.CinemaHallMovies", "MovieID");
            CreateIndex("dbo.CinemaHallMoviePlaces", "CinemaHallMovieID");
            CreateIndex("dbo.CinemaHallMoviePlaces", "CinemaHallPlaceID");
            CreateIndex("dbo.CinemaHallPlaces", "CinemaHallID");
            CreateIndex("dbo.CinemaMovies", "CinemaID");
            CreateIndex("dbo.CinemaMovies", "MovieID");
            CreateIndex("dbo.Movies", "GenreID");
            CreateIndex("dbo.Users", "CinemaID");
            CreateIndex("dbo.OrderItems", "OrderID");
            CreateIndex("dbo.OrderItems", "CinemaHallMovieID");
            CreateIndex("dbo.OrderItems", "CinemaHallPlaceID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cinemas", "CityID", "dbo.Cities");
            DropForeignKey("dbo.CinemaHalls", "CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.CinemaHallMovies", "CinemaHallID", "dbo.CinemaHalls");
            DropForeignKey("dbo.CinemaHallMovies", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.CinemaHallMoviePlaces", "CinemaHallMovieID", "dbo.CinemaHallMovies");
            DropForeignKey("dbo.CinemaHallMoviePlaces", "CinemaHallPlaceID", "dbo.CinemaHallPlaces");
            DropForeignKey("dbo.CinemaHallPlaces", "CinemaHallID", "dbo.CinemaHalls");
            DropForeignKey("dbo.CinemaMovies", "CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.CinemaMovies", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.Movies", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.Users", "CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "CinemaHallMovieID", "dbo.CinemaHallMovies");
            DropForeignKey("dbo.OrderItems", "CinemaHallPlaceID", "dbo.CinemaHallPlaces");

            DropIndex("dbo.Cinemas", new string[] { "CityID" });
            DropIndex("dbo.CinemaHalls", new string[] { "CinemaID" });
            DropIndex("dbo.CinemaHallMovies", new string[] { "CinemaHallID" });
            DropIndex("dbo.CinemaHallMovies", new string[] { "MovieID" });
            DropIndex("dbo.CinemaHallMoviePlaces", new string[] { "CinemaHallMovieID" });
            DropIndex("dbo.CinemaHallMoviePlaces", new string[] { "CinemaHallPlaceID" });
            DropIndex("dbo.CinemaHallPlaces", new string[] { "CinemaHallID" });
            DropIndex("dbo.CinemaMovies", new string[] { "CinemaID" });
            DropIndex("dbo.CinemaMovies", new string[] { "MovieID" });
            DropIndex("dbo.Movies", new string[] { "GenreID" });
            DropIndex("dbo.Users", new string[] { "CinemaID" });
            DropIndex("dbo.OrderItems", new string[] { "OrderID" });
            DropIndex("dbo.OrderItems", new string[] { "CinemaHallMovieID" });
            DropIndex("dbo.OrderItems", new string[] { "CinemaHallPlaceID" });
        }
    }
}
