using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieNews.Data;

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
    }
}
