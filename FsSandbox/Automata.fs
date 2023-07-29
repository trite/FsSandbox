module Automata

open FShade
open Aardvark.Rendering
open Aardvark.GPGPU

//[<LocalSize(X = 64)>]
//let add (l : float32[]) (r : float32[]) =
//    compute {
//        let id = getGlobalId().X
//        l.[id] <- l.[id] + r.[id]
//    }

[<LocalSize(X = 64)>]
let add1 (n : float32[]) =
    compute {
        let id = getGlobalId().X
        n.[id] <- n.[id] + 1.0f
    }

//ComputeShader.ofFunction MaxLocalSize add
//ComputeShader.ofFunction 
//|> ComputeShader.toModule
//|> ModuleCompiler.compileGLSL430
//|> GLSL.code
//|> printfn "%s"

//let effect = Effect.ofFunction add1

//let computeShader = ComputeShader.ofFunction Aardvark.Base.V3i.MaxValue add1


//let test2 =
//    computeShader
//    |> Aardvark.Rendering.

//let config =
//    { EffectConfig.empty with
//        lastStage = FShade.ShaderStage.Compute
//        outputs = Map.ofList ["out", (typeof<float32[]>, 0)]
//    }


//let test =
//    effect
//    |> Effect.toModule config
//    |> ModuleCompiler.compileGLSL glsl430

let run () =
    //use app = new Aardvark.Application.WinForms.VulkanApplication(true)
    use app = new Aardvark.Application.Slim.VulkanApplication(true)
    let runtime = app.Runtime :> IRuntime

    use par = new ParallelPrimitives(runtime)

    let testArr = [| 1.0f .. 100.0f |]
    let input = runtime.CreateBuffer<float32>(testArr)

    let targetWriteShader = runtime.CreateComputeShader add1
    let targetWrite = runtime.CreateInputBinding targetWriteShader

    targetWrite.["n"] <- input

    targetWrite.Flush()

    let ceilDiv (v : int) (d : int) =
        if v % d = 0 then v / d
        else 1 + v / d

    let mk = [
        ComputeCommand.Bind(targetWriteShader)
        ComputeCommand.SetInput targetWrite
        ComputeCommand.Dispatch  (ceilDiv (int input.Count) 64)
    ]

    let program = runtime.CompileCompute mk

    program.Run()

    let result = input.Download()

    printfn "%A" result

//let run () =
//    test2.code
//    |> printfn "%s"

//|> GLSL.Interface.callFunction (Imperative.CFunctionSignature.ofFunction add1)
//|> GLSL.GLSLShaderInterface.

//let blah = test.iface.shaders.[ShaderSlot.Compute]




    //|> GLSL.Assembler.assemble glsl430

//let run () =
//    let l = [| 1.0f .. 100.0f |]
//    let l' = Effect.
//    //let l' = effect l
//    printfn "%A" l'

//let compile (slot : ShaderSlot) (shader : ComputeShader)




//let run () =
