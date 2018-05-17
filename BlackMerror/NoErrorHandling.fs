module NoErrorHandling

module Apis =
    type Input1 = Input1 of unit
    type Output1 = Output1
    type Api1 =
        abstract member GetAsync : Input1 -> Async<Output1>
        
    type Input2 = Input2 of Output1
    type Output2 = Output2
    type Api2 =
        abstract member GetAsync : Input2 -> Async<Output2>
    
    type Input3 = Input3 of Output2
    type Output3 = Output3
    type Api3 =
        abstract member GetAsync : Input3 -> Async<Output3>
        
module Workflows =
    let workflow (api1: Apis.Api1) (api2: Apis.Api2) (api3: Apis.Api3) =
        async {
            let! output1 = api1.GetAsync (Apis.Input1 ())
            let! output2 = api2.GetAsync (Apis.Input2 output1)
            let! output3 = api3.GetAsync (Apis.Input3 output2)
            return 1
        }