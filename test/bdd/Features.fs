module Features

open TickSpec
open Xunit

/// Represents a set of Step Definitions available within a given Assembly
type AssemblyStepDefinitionsSource(assembly : System.Reflection.Assembly) =
    let definitions = StepDefinitions(assembly)    
    /// Yields Scenarios generated by parsing the supplied Resource Name and binding the steps to their Step Definitions
    member __.ScenariosFromEmbeddedResource resourceName : Scenario seq =
        let stream = assembly.GetManifestResourceStream(resourceName)
        definitions.GenerateScenarios(resourceName, stream)

/// Adapts a Scenario Sequence to match the required format for an xUnit MemberData attribute
module MemberData =
    let ofScenarios xs = xs |> Seq.map (fun x -> [| x |])
let x =     System.Reflection.Assembly.GetExecutingAssembly()
let source = AssemblyStepDefinitionsSource(x)
let scenarios resourceName = source.ScenariosFromEmbeddedResource resourceName |> MemberData.ofScenarios

[<Theory; MemberData("scenarios", "CreateBlocksInBlockchain.feature")>]
let CreateBlocksInBlockchain (scenario : Scenario) = scenario.Action.Invoke()
