// dotnet .\packages\xunit.runner.console\tools\netcoreapp2.0\xunit.console.dll .\test\blockchain.test\bin\Debug\netcoreapp2.0\blockchain.test.dll
//
// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#if !FAKE
#r "./packages/FAKE/tools/FakeLib.dll"
#endif

open Fake
open System
open Fake.Testing
open Fake.EnvironmentHelper
// --------------------------------------------------------------------------------------
// Build variables
// --------------------------------------------------------------------------------------

let buildDir  = "./build/"
let appReferences = !! "/**/*.fsproj"
let dotnetcliVersion = "2.0.2"
let testDir = "./test/"
let xUnitConsole = "./packages/xunit.runner.console/tools/net452/xunit.console.exe"
let mutable dotnetExePath = "dotnet"


// --------------------------------------------------------------------------------------
// Helpers
// --------------------------------------------------------------------------------------

let run' timeout cmd args dir =
    if execProcess (fun info ->
        info.FileName <- cmd
        if not (String.IsNullOrWhiteSpace dir) then
            info.WorkingDirectory <- dir
        info.Arguments <- args
    ) timeout |> not then
        failwithf "Error while running '%s' with args: %s" cmd args

let run = run' System.TimeSpan.MaxValue

let runDotnet workingDir args =
    let result =
        ExecProcess (fun info ->
            info.FileName <- dotnetExePath
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if result <> 0 then failwithf "dotnet %s failed" args

// --------------------------------------------------------------------------------------
// Targets
// --------------------------------------------------------------------------------------

Target "Clean" (fun _ ->
    CleanDirs [buildDir]
)

Target "InstallDotNetCLI" (fun _ ->
    if isWindows then 
        dotnetExePath <- DotNetCli.InstallDotNetSDK dotnetcliVersion
    else 
        dotnetExePath <- "mono"    
)

Target "Restore" (fun _ ->
    appReferences
    |> Seq.iter (fun p ->
        let dir = System.IO.Path.GetDirectoryName p
        runDotnet dir "restore"
    )
)

Target "Build" (fun _ ->
    appReferences
    |> Seq.iter (fun p ->
        let dir = System.IO.Path.GetDirectoryName p
        runDotnet dir "build"
    )
)

Target "Test" (fun _ ->
    // don't work with .netcore 2.0
    !! (testDir @@"/**/bin/**/*.test.dll")
     |> xUnit2 (fun p -> { p with HtmlOutputPath = Some (testDir @@ "unit.html"); ToolPath = xUnitConsole })
    // run in the cli
    //run dotnetExePath "./packages/xunit.runner.console/tools/net452/xunit.console.exe \"./test/blockchain.test/bin/Debug/net461/blockchain.test.dll\" -parallel none -xml \"./test/xunit.xml\" " "."
)

Target "BDD" (fun _ ->
    !! (testDir @@"/**/bin/**/bdd.dll")
     |> xUnit2 (fun p -> { p with HtmlOutputPath = Some (testDir @@ "bdd.html"); ToolPath = xUnitConsole}) 
)

// --------------------------------------------------------------------------------------
// Build order
// --------------------------------------------------------------------------------------

"Clean"
  ==> "InstallDotNetCLI"
  ==> "Restore"
  ==> "Build"

RunTargetOrDefault "Build"
