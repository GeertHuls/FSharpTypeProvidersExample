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

type DeleteCmd = SqlCommandProvider<"
    DELETE FROM LatestMovies", ConnStr>

let cmd = new DeleteCmd(ConnStr)
cmd.Execute()

open FSharp.Data
open System.Text.RegularExpressions

let regexThumb = 
  Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

type Netflix = 
  XmlProvider<"http://dvd.netflix.com/Top100RSS">

/// This function updates the data in the LatestMovies table. We first delete
/// all the old rows and then insert new rows by calling 'AddRow'. At the end,
/// we commit the changes by invoking the 'Update' method.
let updateLatest () =
  use cmd = new DeleteCmd(ConnStr)
  cmd.Execute() |> ignore

  let latest = new MovieNews.dbo.Tables.LatestMovies()
  let top = Netflix.GetSample()

  top.Channel.Items |> Seq.iteri (fun idx it ->
    let m = regexThumb.Match(it.Description)
    let descr, thumb = 
      if m.Success then
        m.Groups.[2].Value,
        Some(m.Groups.[1].Value)
      else it.Description, None
    latest.AddRow(it.Title, descr, idx, thumb) )

  latest.Update()


// Run the following line in F# interactive to update the database!
updateLatest ()