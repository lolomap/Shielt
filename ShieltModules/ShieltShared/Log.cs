using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ShieltShared
{
	public static class Log
	{
		public static Serilog.Core.Logger Logger { get; private set; }

		private static readonly SystemConsoleTheme HighlightTheme = new(
			new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>()
			{
				[ConsoleThemeStyle.Text] = new() {Foreground = ConsoleColor.White},
				[ConsoleThemeStyle.SecondaryText] = new() {Foreground = ConsoleColor.Gray},
				[ConsoleThemeStyle.TertiaryText] = new() {Foreground = ConsoleColor.DarkGray},
				[ConsoleThemeStyle.Invalid] = new() {Foreground = ConsoleColor.Yellow},
				[ConsoleThemeStyle.Null] = new() {Foreground = ConsoleColor.Blue},
				[ConsoleThemeStyle.Name] = new() {Foreground = ConsoleColor.Gray},
				[ConsoleThemeStyle.String] = new() {Foreground = ConsoleColor.Cyan},
				[ConsoleThemeStyle.Number] = new() {Foreground = ConsoleColor.Magenta},
				[ConsoleThemeStyle.Boolean] = new() {Foreground = ConsoleColor.Blue},
				[ConsoleThemeStyle.Scalar] = new() {Foreground = ConsoleColor.Green},
				[ConsoleThemeStyle.LevelVerbose] = new() {Foreground = ConsoleColor.Gray},
				[ConsoleThemeStyle.LevelDebug] = new() {Foreground = ConsoleColor.Gray},
				[ConsoleThemeStyle.LevelInformation] = new() {Foreground = ConsoleColor.Green},
				[ConsoleThemeStyle.LevelWarning] = new() {Foreground = ConsoleColor.Yellow},
				[ConsoleThemeStyle.LevelError] = new() {Foreground = ConsoleColor.White, Background = ConsoleColor.Red},
				[ConsoleThemeStyle.LevelFatal] = new() {Foreground = ConsoleColor.White, Background = ConsoleColor.Red},
			});

		public static void Init()
		{
			Logger = new LoggerConfiguration()
				.WriteTo
				.Console(theme: HighlightTheme)
				.CreateLogger();
		}
	}
}