module MovieNews.Data.TheMovieDb
open FSharp.Data

type MovieSearch = JsonProvider<"MovieSearch.json">
type MovieCast = JsonProvider<"MovieCast.json">
type MovieDetails = JsonProvider<"MovieDetails.json">

let apiKey = "6ce0ef5b176501f8c07c634dfa933cff"
let searchUrl =
  "http://api.themoviedb.org/3/search/movie"
let detailsUrl id = 
  sprintf "http://api.themoviedb.org/3/movie/%d" id
let creditsUrl id = 
  sprintf "http://api.themoviedb.org/3/movie/%d/credits" id

let tryGetMovieId title = 
  let jsonResponse = 
    Http.RequestString
     ( searchUrl,
       query = ["api_key",apiKey; "query", title],
       headers = [HttpRequestHeaders.Accept "application/json"])
  let searchRes = MovieSearch.Parse(jsonResponse)
  searchRes.Results
  |> Seq.tryFind (fun res -> 
      res.Title = title)
  |> Option.map (fun res -> res.Id)

let getMovieDetails id = 
  let jsonResponse =
    Http.RequestString
     ( detailsUrl id, 
       query = ["api_key",apiKey],
       headers = [HttpRequestHeaders.Accept "application/json"])
  let details = MovieDetails.Parse(jsonResponse)
  { Homepage = details.Homepage
    Genres = [ for g in details.Genres -> g.Name ]
    Overview = details.Overview
    Companies = 
      [ for p in details.ProductionCompanies -> p.Name ]
    Poster = details.PosterPath
    Countries = 
      [ for c in details.ProductionCountries -> c.Name ]
    Released = details.ReleaseDate
    AverageVote = details.VoteAverage }

let getMovieCast id = 
  let jsonResponse = 
    Http.RequestString
     ( creditsUrl id, 
       query = ["api_key",apiKey],
       headers = [HttpRequestHeaders.Accept "application/json"])
  let cast = MovieCast.Parse(jsonResponse)
  [ for c in cast.Cast ->
      { Actor = c.Name
        Character = c.Character } ]
