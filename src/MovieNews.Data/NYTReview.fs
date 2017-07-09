module MovieNews.Data.NYTReview
open FSharp.Data

type NYT = JsonProvider<"http://api.nytimes.com/svc/movies/v2/reviews/search.json?api-key=2ef8ce1a9c93a64599d9d00f80555ff3:8:72646066&query=paddington">

let baseUrl = "http://api.nytimes.com/svc/movies/v2/reviews/search.json"
let apiKey = "2ef8ce1a9c93a64599d9d00f80555ff3:8:72646066"

/// As in 'Netflix.fs', the following function takes the
/// response as input and so it can be easily tested.

let tryPickReviewByName name response =
  let nyt = NYT.Parse(response)
  let reviewOpt = nyt.Results |> Seq.tryFind (fun r ->
    System.String.Equals
      ( r.DisplayTitle, name, 
        System.StringComparison.InvariantCultureIgnoreCase) )

  reviewOpt |> Option.map (fun r ->
    { Published = r.PublicationDate
      Summary = r.SummaryShort
      Link = r.Link.Url
      LinkText = r.Link.SuggestedLinkText } )

let tryDownloadReviewByNme name = 
  let q = ["api-key", apiKey; "query", name]
  let response = Http.RequestString(baseUrl, q)
  tryPickReviewByName name response
