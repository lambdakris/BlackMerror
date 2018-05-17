module MonadicErrorHandling

open Cvdm.ErrorHandling

module Apis =
    type Input1 = Input1 of unit
    type Output1 = Output1
    type Error1 = Error1
    type Api1 =
        abstract member GetAsync : Input1 -> Async<Result<Output1,Error1>>
        
    type Input2 = Input2 of Output1
    type Output2 = Output2
    type Error2 = Error2
    type Api2 =
        abstract member GetAsync : Input2 -> Async<Result<Output2,Error2>>
    
    type Input3 = Input3 of Output2
    type Output3 = Output3
    type Error3 = Error3
    type Api3 =
        abstract member GetAsync : Input3 -> Async<Result<Output3,Error3>>

module Workflows =
    open Apis

    type WorkflowError = Error1 of Error1 | Error2 of Error2 | Error3 of Error3

    let workflow (api1: Api1) (api2: Api2) (api3: Api3) =
        asyncResult {
            let! result1 = api1.GetAsync(Input1 ()) |> AsyncResult.mapError Error1
            let! result2 = api2.GetAsync(Input2 result1) |> AsyncResult.mapError Error2
            let! result3 = api3.GetAsync(Input3 result2) |> AsyncResult.mapError Error3
            return 1
        }