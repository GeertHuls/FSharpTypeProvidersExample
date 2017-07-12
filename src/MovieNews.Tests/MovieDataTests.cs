using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieNews.Data;
using System;

namespace MovieNews.Tests
{
    [TestClass]
    public class MovieDataTests
    {
        [TestMethod]
        public void CanParseNetflexFeed()
        {
	        var rss = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
				<rss version=""2.0"">
				  <channel >
					<title>Netflix Top 100</title>
					<description>Top 100 Netflix movies, published every 2 weeks.</description>
					<item>
					  <title>The Imitation Game</title>
					  <link>http://dvd.netflix.com/Movie/The-Imitation-Game/70295172</link>
					  <description>&lt;a href=&quot;http://dvd.netflix.com/Movie/The-Imitation-Game/70295172&quot;&gt;&lt;img src=&quot;http://cdn-0.nflximg.com/us/boxshots/small/70295172.jpg&quot;/&gt;&lt;/a&gt;&lt;br&gt;Chronicling mathematical wizard Alan Turing's key role in Britain's successful effort to crack Germany's Enigma code during World War II, this historical biopic also recounts how his groundbreaking work helped launch the computer age.</description>
					</item>
					<item>
					  <title>Game of Thrones</title>
					  <link>http://dvd.netflix.com/Movie/Game-of-Thrones/70177064</link>
					  <description>&lt;a href=&quot;http://dvd.netflix.com/Movie/Game-of-Thrones/70177064&quot;&gt;&lt;img src=&quot;http://cdn-0.nflximg.com/us/boxshots/small/70177064.jpg&quot;/&gt;&lt;/a&gt;&lt;br&gt;Originally airing on HBO, this live-action fantasy series -- based on George R.R. Martin's &quot;A Song of Ice and Fire&quot; novels -- charts the violent efforts of competing noble families to gain control of the vacant Westeros throne.</description>
					</item>
				  </channel>
				</rss>";
			var feed = Netflix.parseTop100(rss);

			Assert.AreEqual(2, feed.Count());

	        var first = feed.First();
	        Assert.AreEqual("The Imitation Game", first.Title);
	        Assert.AreEqual(
		        "http://cdn-0.nflximg.com/us/boxshots/small/70295172.jpg",
		        first.Thumbnail.Value);
		}

	    [TestMethod]
	    public void CanParseNYTReviews()
	    {
		    var response = @"{ 
        ""status"": ""OK"",
        ""copyright"": ""Copyright (c) 2015 The New 
            York Times Company.  All Rights Reserved."",
        ""num_results"": 1,
        ""results"": [
            { ""nyt_movie_id"": 451296,
              ""display_title"": ""Paddington"",
              ""sort_name"": ""Paddington"",
              ""mpaa_rating"": ""PG"",
              ""headline"": ""Adventures of a Peruvian Immigrant (the Furry Variety)"",
              ""summary_short"": ""In stark contrast to their furry, blundering star, the makers of &ldquo;Paddington have colored so carefully inside the lines that any possibility of surprise or subversion is effectively throttled."",
              ""publication_date"": ""2014-01-16"",
              ""opening_date"": ""2015-01-16"",
              ""date_updated"": ""2015-01-15 17:19:27"",
              ""link"": {
                  ""type"": ""article"",
                  ""url"": ""http://movies.nytimes.com/movies/paddington.html"",
                  ""suggested_link_text"": ""Read the New York Times Review of Paddington""
                }
            } ] 
        }";

		    var actual = NYTReview.tryPickReviewByName(
				"Paddington", response).Value;

		    var expected = new Review(
				new DateTime(2014, 01, 16),
			    "In stark contrast to their furry, blundering star, " +
			    "the makers of &ldquo;Paddington have colored so " +
			    "carefully inside the lines that any possibility of " +
			    "surprise or subversion is effectively throttled.",
			    "http://movies.nytimes.com/movies/paddington.html",
			    "Read the New York Times Review of Paddington");

		    Assert.AreEqual(expected, actual);
	    }

	    [TestMethod]
	    public void CanGetMovieData()
	    {	
		    var info = Movies.GetMovieInfo("Interstellar");
		    if (info.HasMovie)
		    {
			    Console.WriteLine
			    ($"Name: {info.Movie.Movie.Title}\n" +
			     $"Poster: {info.Movie.Details.Poster}");
		    }
		}
    }
}
