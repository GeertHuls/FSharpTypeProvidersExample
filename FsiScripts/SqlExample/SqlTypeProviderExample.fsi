#r "../packages/FSharp.Data.SqlClient/lib/net40/FSharp.Data.SqlClient.dll"
#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "System.Xml.Linq.dll"
open FSharp.Data

[<Literal>]
let ConnStr =
    "Data Source=(localdb)\\mssqllocaldb;database=MovieNews;trusted_connection=yes"

type MovieNews = SqlProgrammabilityProvider<ConnStr>

let latest = new MovieNews.dbo.Tables.LatestMovies()
latest.AddRow("Test title", "Test description", 1)
latest.Update()
