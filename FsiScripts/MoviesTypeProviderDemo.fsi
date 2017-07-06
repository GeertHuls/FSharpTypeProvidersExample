#r "./packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "System.Xml.Linq.dll"

open FSharp.Data
open System.Text.RegularExpressions

// Regex that extracts the image and description.
// The regex skips the link element, finds the src attribute
// of the image and then skips the rest of the html 
// and extracts the description.

// This results into 2 groups:
// 1) the image url
// 2) the description text
let regexThumb = 
    Regex("<a[^>]*><img src=\"([^\"]*)\".*>(.*)")

// Test regex:
// let m = regexThumb.Match("""<a href="https://dvd.netflix.com/Movie/Batman-v-Superman-Dawn-of-Justice/80081793"><img src="//secure.netflix.com/us/boxshots/small/80081793.jpg"/></a><br>Superegos battle in superhero fashion while a new threat emerges in this sequel to "Man of Steel." With Batman decamping Gotham City for Metropolis to take on a Superman who's become too powerful, trouble brews for the rest of humanity.""")
// m.Success
// m.Groups.[1].Value
// m.Groups.[2].Value

type Netflix = 
    XmlProvider<"http://dvd.Netflix.com/Top100RSS">


type MovieBacis =
    {   Title : string
        Summary : string
        Thumbnail : option<string> }

let getTop100() =
    let top = Netflix.GetSample()
    let findMovies title description =
        let m = regexThumb.Match(description)
        let descr, thumb = 
            if m.Success then
                m.Groups.[2].Value,
                Some(m.Groups.[1].Value)
            else description, None
        {   Title = title
            Summary = descr
            Thumbnail = thumb }
    [for it in top.Channel.Items -> 
        findMovies it.Title it.Description]

getTop100()