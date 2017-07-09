namespace MovieNews.Data

// Namespace can only contain types but not functions.
type MovieBasics =
    {   Title : string
        Summary : string
        Thumbnail : option<string> }

type Review =
  { Published : System.DateTime
    Summary : string
    Link : string
    LinkText : string }

type Cast =
  { Actor : string
    Character : string }

type Details = 
  { Homepage : string
    Genres : seq<string>
    Overview : string
    Companies : seq<string>
    Poster : string
    Countries : seq<string>
    Released : System.DateTime
    AverageVote : decimal }

/// Combines all available information about a movie
type Movie =
  { Movie : MovieBasics
    Details : Details
    Cast : seq<Cast>
    Review : option<Review> }
