module ExceptionwiseErrorHandling

module Apis =
    exception Failure1 
    type Input1 = Input1 of unit
    type Output1 = Output1
    /// just imagine that I might purposely throw Failure1 instead of returning an Async<Output1>
    type Api1 =
        abstract member GetAsync : Input1 -> Async<Output1>
        
    exception Failure2
    type Input2 = Input2 of Output1
    type Output2 = Output2
    /// just imagine that I might purposely throw Failure2 instead of returning an Async<Output2>
    type Api2 =
        abstract member GetAsync : Input2 -> Async<Output2>
    
    exception Failure3
    type Input3 = Input3 of Output2
    type Output3 = Output3
    /// just imagine that I might purposely throw Failure3 instead of returning an Async<Output3>
    type Api3 =
        abstract member GetAsync : Input3 -> Async<Output3>

module Workflows =
    open Apis

    let workflow (api1: Api1) (api2: Api2) (api3: Api3) =
        async {
            try
                let! output1 = api1.GetAsync (Input1 ())
                let! output2 = api2.GetAsync (Input2 output1)
                let! output3 = api3.GetAsync (Input3 output2)
                return 1
            with
                | Failure1 ->
                    printfn "This is how you deal with failure 1"
                    return -1
                | Failure2 ->
                    printfn "This is how you deal with failure 2"
                    return -2
                | Failure3 ->
                    printfn "This is how you deal with failure 3"
                    return -3
        }