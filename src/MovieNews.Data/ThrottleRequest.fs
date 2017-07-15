module MovieNews.Data.Utils
open FSharp.Data

/// The type represents a request for the throttling agent. 
/// A request consists of URL and query string to download,
/// together with a channel used for sending replies back
/// to the caller
type ThrottleMessage =
   { Url : string
     Query : seq<string * string>
     Reply : AsyncReplyChannel<string> }


// The purpose of the throttling agent is to limit the numer of
// requests sent to the different services. Next all donwloads
// are going to be asynchronous to avoid blocking any threads
// during the calls.
// This agent will be used for the New York times reviews and
// the movie database.

/// This function takes a 'delay'. It initializes a new 
/// instance of the agent, wraps it in a 'download' function
/// and then returns this new function as a result - check
/// out the type of 'createThrottler'! It is a function 
/// that returns another function as a result.
let createThrottle delay =
  let agent = MailboxProcessor<ThrottleMessage>.Start(fun inbox ->
    async {
      while true do
        let! req = inbox.Receive()
        let sw = System.Diagnostics.Stopwatch.StartNew()
        let! res =
          Http.AsyncRequestString
            ( req.Url, List.ofSeq req.Query,
              [ HttpRequestHeaders.Accept HttpContentTypes.Json ])
        req.Reply.Reply(res)
        let sleep = delay - (int sw.ElapsedMilliseconds)
        if sleep > 0 then do! Async.Sleep(sleep)
    })
  
  /// This creates a new message and sends it to the agent.
  /// The result is a workflow that waits until the message
  /// is processed and then it returns the downloaded string.
  let download url query = 
    agent.PostAndAsyncReply(fun reply ->
      { Url = url; Query = query; Reply = reply })

  download
