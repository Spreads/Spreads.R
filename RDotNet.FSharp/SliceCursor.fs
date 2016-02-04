namespace Spreads.R

open System
open System.Linq
open System.Collections.Generic

open Spreads
open Spreads.Collections

open RDotNet


// TODO often we want to treat table as matrix
// we could do the same - populate R matrix from 
// Spreads without intermediate buffers



type internal RNumericDataFrmae(length:int, width:int, names:string[]) =
  
//  let df = R.data_frame(paramArray=args)
//            df.SetAttribute("names", frame.ColumnKeys |> Seq.map convertKey |> engine.CreateCharacterVector)
//            df.SetAttribute("row.names", frame.RowKeys |> Seq.map convertKey |> engine.CreateCharacterVector)
//            df }

  member private this.CreateVectors(engine) =
    new NumericVector(engine, length)

  interface IDisposable with
    member __.Dispose() =()

type RUtils() =
  
  static member CreateTable(panel:Panel<DateTime, string, double>) =

    failwith "TODO"