module MovieNews.Data.Netflix
open FSharp.Data
open System.Text.RegularExpressions

let regexThumb = 
  Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

type Netflix = 
  XmlProvider<"http://dvd.netflix.com/Top100RSS">

/// The function takes a response from Netflix and returns the 
/// parsed data as a sequence (IEnuemerable). This makes it
/// easy to test from C# unit test project.
let parseTop100 response =
  let top = Netflix.Parse(response)
  seq {
    for it in top.Channel.Items ->
      let m = regexThumb.Match(it.Description)
      let descr, thumb = 
        if m.Success then
          m.Groups.[2].Value,
          Some(m.Groups.[1].Value)
        else it.Description, None
      { Title = it.Title 
        Summary = descr
        Thumbnail = thumb } }

let getTop100 () = async {
    let! response = 
      Http.AsyncRequestString("http://dvd.netflix.com/Top100RSS")
    return parseTop100 response
  }


