module OhNulls

let boxedNull = box null

let nullInt = unbox<int> boxedNull

let print (x:obj) =
    System.Console.WriteLine(x)

let run () =
    nullInt + 5 |> print
