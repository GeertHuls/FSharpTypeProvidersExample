#r "../packages/ExcelProvider/lib/ExcelProvider.dll"
open FSharp.ExcelProvider

// Let the type provider do it's work
type DataTypesTest = ExcelFile<"Cars.xlsx">

let file = new DataTypesTest()

let row = file.Data |> Seq.head
row.Name
