open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent

type App() =
    inherit Application()
    let name = "Reactive Draw"

    override this.Initialize() =
        this.Styles.Add <| FluentTheme(baseUri = null, Mode = FluentThemeMode.Light)
        this.Name <- name

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- ReactiveDraw.view name
            base.OnFrameworkInitializationCompleted()
        | _ ->
            ()

module Program =
    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
