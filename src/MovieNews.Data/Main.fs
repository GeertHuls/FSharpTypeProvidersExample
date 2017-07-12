module MovieNews.Data.Movies

/// A wrapper that provides a C#-friendly API over a Movie
/// that may be missing. Rather than dealing with F# options,
/// the C# caller can use the 'HasMovie' and 'Movie' properties.
type MovieSearchResult(result:option<Movie>) = 
  member x.HasMovie = result <> None
  member x.Movie = 
    match result with
    | Some movie -> movie
    | None -> invalidOp "Movie not found!"

/// The method returns a value of type 'MovieSearchResult'.
/// Note that we also follow the PascalCase naming convention
/// to make the function appear as a normal static method.
let GetMovieInfo name = 
  let info =
    TheMovieDb.tryGetMovieId name
    |> Option.map (fun id ->
        TheMovieDb.getMovieDetails id,
        TheMovieDb.getMovieCast id )
  let review =
    NYTReview.tryDownloadReviewByNme name
  let basics = 
    Netflix.getTop100()
    |> Seq.tryFind (fun m -> m.Title = name)

  match basics, info with
  | Some(basics), Some(details, cast) ->
      { Movie = basics 
        Details = details
        Cast = cast
        Review = review } |> Some |> MovieSearchResult
  | _ -> None |> MovieSearchResult

/// The function that returns top 20 movies from Netflix
let GetLatestMovies() = 
  Netflix.getTop100() |> Seq.take 20
