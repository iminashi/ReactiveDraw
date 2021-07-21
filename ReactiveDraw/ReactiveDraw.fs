module ReactiveDraw

open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.Layout
open Avalonia.Media
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Builders

type PLine() =
    inherit Polyline(Points = ResizeArray())

    // Invalidate the geometry for the line to be updated while drawing
    // https://github.com/AvaloniaUI/Avalonia/issues/3623
    member this.AddPoint(point) =
        this.Points.Add point
        this.InvalidateGeometry()

let window = Window(Width = 800., Height = 600.)
let canvas = Canvas()
let clearButton =
    let b = Button(Content = "Clear", VerticalAlignment = VerticalAlignment.Top)
    b.Click.Add (fun _ -> canvas.Children.Clear())
    b

let lines =
    window.PointerPressed
    |> Observable.map (fun _ -> PLine(Stroke = Brushes.Black, StrokeThickness = 3.))

rxquery {
    for line in lines do
    canvas.Children.Add line
    for move in window.PointerMoved |> Observable.takeUntilOther window.PointerReleased do
    move.GetPosition window
    |> line.AddPoint }
|> Observable.repeat
|> Observable.subscribe ignore
|> ignore

let view title =
    let panel = Panel()
    panel.Children.AddRange [ canvas; clearButton ]
    window.Content <- panel
    window.Title <- title
    window
