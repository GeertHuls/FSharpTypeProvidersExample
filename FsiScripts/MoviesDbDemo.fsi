#r "./packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

// ------------------------------------------------------------------
// NOTE: In order to use The Movie Database API, you'll need to 
// register and obtain an API key. Please follow the instructions at:
//  * https://www.themoviedb.org/documentation/api
// The API key used in the samples below is just for demonstration.
// ------------------------------------------------------------------

let apiKey = "6ce0ef5b176501f8c07c634dfa933cff"

// Functions that generate URL for calling the three end-points
// (search for a movie, get movie details & get movie cast)

let searchUrl =
  "http://api.themoviedb.org/3/search/movie"
let detailsUrl id = 
  sprintf "http://api.themoviedb.org/3/movie/%d" id
let creditsUrl id = 
  sprintf "http://api.themoviedb.org/3/movie/%d/credits" id


Http.RequestString
 ( searchUrl,
   query = ["api_key",apiKey; "query", "Interstellar"],
   headers = [HttpRequestHeaders.Accept "application/json"])


// Does not work, string too long:
//type Search = JsonProvider<"""{"page":1,"results":[{"adult":false,"backdrop_path":"/xu9zaAevzQ5nnrsXN6JcahLnG4i.jpg","genre_ids":[18,878],"id":157336,"original_language":"en","original_title":"Interstellar","overview":"Interstellar chronicles the adventures of a group of explorers who make use of a newly discovered wormhole to surpass the limitations on human space travel and conquer the vast distances involved in an interstellar voyage.","release_date":"2014-11-05","poster_path":"/nBNZadXqJSdt05SHLqgT0HuC5Gm.jpg","popularity":17.592125,"title":"Interstellar","video":false,"vote_average":8.4,"vote_count":3101},{"adult":false,"backdrop_path":"/bT5jpIZE50MI0COE8pOeq0kMpQo.jpg","genre_ids":[99],"id":301959,"original_language":"en","original_title":"Interstellar: Nolan's Odyssey","overview":"Behind the scenes of Christopher Nolan's sci-fi drama, which stars Matthew McConaughey and Anne Hathaway","release_date":"2014-11-05","poster_path":"/xZwUIPqBHyJ2QIfMPANOZ1mAld6.jpg","popularity":1.376865,"title":"Interstellar: Nolan's Odyssey","video":false,"vote_average":7.9,"vote_count":44},{"adult":false,"backdrop_path":"/mgb6tVEieDYLpQt666ACzGz5cyE.jpg","genre_ids":[10770,35],"id":287954,"original_language":"en","original_title":"Lolita from Interstellar Space","overview":"An undeniably beautiful alien is sent to Earth to study the complex mating rituals of human beings, which leads to the young interstellar traveler experiencing the passion that surrounds the centuries-old ritual of the species.","release_date":"2014-03-08","poster_path":"/buoq7zYO4J3ttkEAqEMWelPDC0G.jpg","popularity":1.514079,"title":"Lolita from Interstellar Space","video":false,"vote_average":0.0,"vote_count":0},{"adult":false,"backdrop_path":null,"genre_ids":[99],"id":336592,"original_language":"en","original_title":"The Science of Interstellar","overview":"The science of Christopher Nolan's sci-fi, Interstellar.","release_date":"2014-11-25","poster_path":"/nuzfkvikb6TXT5bwIMxxXenvvJI.jpg","popularity":1.052512,"title":"The Science of Interstellar","video":false,"vote_average":0.0,"vote_count":0},{"adult":false,"backdrop_path":"/5ism2HNUGuQi5a3ajYaN9ypMQMf.jpg","genre_ids":[878,28],"id":47662,"original_language":"en","original_title":"Trancers 4: Jack of Swords","overview":"Jack is now back in the future. He had since lost Lena, and finds out that he's lost his other wife Alice to none other than Harris. While heading out for another assignment, something goes awry with the TCL chamber. Jack finds himself in a whole new dimension. He also runs across a different version of trancers. These guys seem to be in control of this planet. Jack manages to assist a rebel group known as the \"Tunnel Rats\" crush the rule of the evil Lord Calaban.","release_date":"1994-02-02","poster_path":"/69yr3oxBpSgua26RJkFmsm7plTG.jpg","popularity":1.000192,"title":"Trancers 4: Jack of Swords","video":false,"vote_average":5.8,"vote_count":4},{"adult":false,"backdrop_path":"/an0xpUEX1P1BI80sCpkU1pSoREx.jpg","genre_ids":[28,53,878],"id":47663,"original_language":"en","original_title":"Trancers V","overview":"Jack Deth is back for one more round with the trancers. Jack must attempt to find his way home from the other-dimensional world of Orpheus, where magic works and the trancers were the ruling class (before Trancers IV, that is). Unfortunately, Jack's quest to find the mystical Tiamond in the Castle of Unrelenting Terror may be thwarted by the return of Caliban, king of the trancers who was thought dead.","release_date":"1994-11-04","poster_path":"/epMaTjPDMbgC8TbW1ZToh4RNv0i.jpg","popularity":1.000192,"title":"Trancers V","video":false,"vote_average":5.0,"vote_count":2},{"adult":false,"backdrop_path":"/u4JBwlGZN8hGeLxwu7Q0WmibACp.jpg","genre_ids":[878],"id":261443,"original_language":"en","original_title":"Angry Planet","overview":"A criminal sentenced to life on a prison planet reveals his true purpose: to extract revenge on the killers who murdered his family.","release_date":"2008-01-01","poster_path":"/ie5luS87ess1c5VgFhbGECJTQVK.jpg","popularity":1.000963,"title":"Angry Planet","video":false,"vote_average":4.5,"vote_count":1}],"total_pages":1,"total_results":7}""">
type MovieSearch = JsonProvider<"MovieSearch.json">
type MovieCast = JsonProvider<"MovieCast.json">
type MovieDetails = JsonProvider<"MovieDetails.json">

let sample = MovieSearch.GetSample()

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


let id = sample.Results.[0].Id
printfn "%i" id


/// Get the ID of a movie with a specified name. To do this, we call the
/// "/search" function and then find the result with the matching name
/// (this can fail and so the function returns 'option<int>')
let tryGetMovieId title = 
  let jsonResponse = 
    Http.RequestString
     ( searchUrl,
       query = ["api_key",apiKey; "query", title],
       headers = [HttpRequestHeaders.Accept "application/json"])
  // use the Type provider as an example how to parse the 
  // json response from the api:
  let searchRes = MovieSearch.Parse(jsonResponse)
  searchRes.Results
  |> Seq.tryFind (fun res -> 
      res.Title = title)
  |> Option.map (fun res -> res.Id)

tryGetMovieId "Interstellar"


/// Get movie details for a movie with a given ID. We assume that the ID
/// (obtained using the previous function) is correct, and so this just
/// loads data into the 'Details' record. 
let getMovieDetails id =
  let jsonResponse =
    Http.RequestString
     ( detailsUrl id, 
       query = ["api_key",apiKey],
       headers = [HttpRequestHeaders.Accept "application/json"])
  // use the Type provider as an example how to parse the 
  // json response from the api:
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

getMovieDetails 157336


/// Get cast for a movie with a given ID. We assume that the ID
/// (obtained using the earlier function) is correct, and so this just
/// loads data into a list of 'Cast' values
let getMovieCast id = 
  let jsonResponse = 
    Http.RequestString
     ( creditsUrl id, 
       query = ["api_key",apiKey],
       headers = [HttpRequestHeaders.Accept "application/json"])
  // use the Type provider as an example how to parse the 
  // json response from the api:
  let cast = MovieCast.Parse(jsonResponse)
  [ for c in cast.Cast ->
      { Actor = c.Name
        Character = c.Character } ]

getMovieCast 157336
