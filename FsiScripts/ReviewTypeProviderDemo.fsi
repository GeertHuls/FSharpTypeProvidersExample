#r "./packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

type NYT = JsonProvider<"http://api.nytimes.com/svc/movies/v2/reviews/search.json?api-key=e53d8b02b86648909bcaf2f680753896&query=paddington">

let baseUrl = "http://api.nytimes.com/svc/movies/v2/reviews/search.json"
let apiKey = "e53d8b02b86648909bcaf2f680753896"
let query = "sniper"

type Review =
  { Published : System.DateTime
    Summary : string
    Link : string
    LinkText : string }

let tryPickReviewByName query  = 
    let q = ["api-key", apiKey; "query", query]
    let response = Http.RequestString(baseUrl, q)

    //Let the type provider parse the string result:
    let nyt = NYT.Parse(response)
    for res in nyt.Results do
        printfn "%s" res.DisplayTitle

    let reviewOpt = nyt.Results |> Seq.tryFind (fun r -> 
        System.String.Equals
            (r.DisplayTitle, query,
                System.StringComparison.InvariantCultureIgnoreCase) )

    reviewOpt |> Option.map (fun r ->
        { Published = r.PublicationDate
          Summary = r.SummaryShort
          Link = r.Link.Url
          LinkText = r.Link.SuggestedLinkText } )

tryPickReviewByName "Interstellar"
tryPickReviewByName "NonExistentMovieTitle"