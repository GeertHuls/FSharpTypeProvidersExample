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
let GetMovieInfo name = async {
    // run the request in parallel to make the requests faster
    // using Async.StartChild.
    let! infoWork =
      TheMovieDb.getMovieInfoByName name
      |> Async.StartChild

    let! reviewWork =
      NYTReview.tryDownloadReviewByNme name
      |> Async.StartChild

    // wait when all requests are finished:
    let! top100 = Netflix.getTop100()
    let! review = reviewWork
    let! info = infoWork

    let basics = top100 |> Seq.tryFind (fun m -> m.Title = name)
    match basics, info with
    | Some(basics), Some(details, cast) ->
      return
        { Movie = basics 
          Details = details
          Cast = cast
          Review = review } |> Some |> MovieSearchResult 
    | _ -> return None |> MovieSearchResult } |> Async.StartAsTask // backets span over pattern match branches??

/// The function that returns top 20 movies from Netflix
let GetLatestMovies() = async {
    let! top100 = Netflix.getTop100()
    return top100 |> Seq.take 20 } |> Async.StartAsTask