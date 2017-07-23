module MovieNews.Data.Netflix
open FSharp.Data
open System.Text.RegularExpressions
open System.Configuration

// Original code: get data from RSS feed:

//let regexThumb = 
//  Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

//type Netflix = 
//  XmlProvider<"http://dvd.netflix.com/Top100RSS">

///// The function takes a response from Netflix and returns the 
///// parsed data as a sequence (IEnuemerable). This makes it
///// easy to test from C# unit test project.
//let parseTop100 response =
//  let top = Netflix.Parse(response)
//  seq {
//    for it in top.Channel.Items ->
//      let m = regexThumb.Match(it.Description)
//      let descr, thumb = 
//        if m.Success then
//          m.Groups.[2].Value,
//          Some(m.Groups.[1].Value)
//        else it.Description, None
//      { Title = it.Title 
//        Summary = descr
//        Thumbnail = thumb } }

//let getTop100 () = async {
//    let! response = 
//      Http.AsyncRequestString("http://dvd.netflix.com/Top100RSS")
//    return parseTop100 response
//  }


// The following connection string is only used at compile time by the type provider
// to check the structure of SQL commands (e.g. to ensure that column names are valid)
[<Literal>]
let ConnStr =
    "Data Source=(localdb)\\mssqllocaldb;database=MovieNews;trusted_connection=yes"

// The select command that we're using to read data from the database.
// Select top @Count movies (with all columns) from the LatestMovies table.
type SelectCmd = SqlCommandProvider<"""
  SELECT * FROM LatestMovies WHERE SortOrder < @Count
  ORDER BY SortOrder""", ConnStr>


let getTop100 response = async {
  // Get the runtime connection string from web.config and use it to
  // execute the 'SelectCmd' defined earlier. We execute the command
  // asynchronously, because communication with database is an I/O
  // operation too (though less expensive than connecting to the web
  let conn = ConfigurationManager.ConnectionStrings.["MovieDb"]
  use cmd = SelectCmd.Create(conn.ConnectionString)
  let! res = cmd.AsyncExecute(20)
  return
    [ for it in res ->
      // Note that the type provider inferred the names and types of 
      // the columns and so we can access title, etc. using just '.'
      { Title = it.Title 
        Summary = it.Description
        Thumbnail = it.Thumbnail } ] }
