module ManualErrorHandling

open System.Net.Http

module Apis =
    type Input1 = Input1 of unit
    type Output1 = Success1 | Failure1
    type Api1 =
        abstract member GetAsync : Input1 -> Async<Output1>
        
    type Input2 = Input2 of Output1
    type Output2 = Success2 | Failure2
    type Api2 =
        abstract member GetAsync : Input2 -> Async<Output2>
    
    type Input3 = Input3 of Output2
    type Output3 = Success3 | Failure3
    type Api3 =
        abstract member GetAsync : Input3 -> Async<Output3>
        
module Workflows =
    open Apis

    let workflow (api1: Api1) (api2: Api2) (api3: Api3) =    
        async {
            let! output1 = api1.GetAsync (Input1 ())

            match output1 with
            | Success1 as s1 ->
                let! output2 = api2.GetAsync (Input2 s1)

                match output2 with
                | Success2 as s2 ->
                    let! output3 = api3.GetAsync (Input3 s2)
                    
                    match output3 with
                    | Success3 as s3 ->
                        return 1
                    | Failure3 ->
                        return -3
                | Failure2 ->
                    return -2
            | Failure1 ->
                return -1
        }