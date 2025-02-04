// See https://aka.ms/new-console-template for more information
using TrainerizeMigrate.Migrations;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainerizeMigrate;
using TrainerizeMigrate.DataManagers;
using Spectre.Console;


internal class Program
{
    public async static Task Main(string[] args)
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Config config = new Config();
        db.Database.Migrate();

        AnsiConsole.Markup("[green]Database created successfully!\n[/]");

        AnsiConsole.Markup("[green]Database location: " + db.DbPath + "\n[/]");

        if (File.Exists("config.json"))
        {
            AnsiConsole.Markup("[green]config.json located successfully!\n[/]");
        }
        else
            Environment.Exit(1);

        if (File.Exists("trainerize_urls.json"))
        {
            AnsiConsole.Markup("[green]trainerize_urls.json located successfully!\n[/]");
        }
        else
            Environment.Exit(1);

        bool itemselected = false;
        BodyWeightManager bodyWeightManager = new BodyWeightManager(config, db);
        ExcersizeManager excersizeManager = new ExcersizeManager(config, db);
        WorkoutManager workoutManager = new WorkoutManager(config, db);
        TrainingSessionManager trainingSessionManager = new TrainingSessionManager(config, db);

        do
        {
            var userSelectionResult = await HandleUserSelection(bodyWeightManager, excersizeManager, workoutManager, trainingSessionManager);
            itemselected = userSelectionResult.IsExit;

        } while (!itemselected);

    }

    public static async Task<(bool IsExit, bool something)> HandleUserSelection(BodyWeightManager bodyWeightManager, ExcersizeManager excersizeManager, WorkoutManager workoutManager, TrainingSessionManager trainingSessionManager)
    {
        bool action = false;
        while (!action)
        {
            var mainMenuOptions = GetMainMenuOptions();

            var mainMenuSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(mainMenuOptions)
            );

            switch (mainMenuSelection)
            {
                case "[red]Export and Store Body Weight Data[/]":
                    bodyWeightManager.ExtractAndStoreData();
                    break;

                case "[red]Import Body Weight Data[/]":
                   bodyWeightManager.ImportExtractedData();
                    break;

                case "[red]Export and Store Custom Excersizes[/]":
                    excersizeManager.ExtractAndStoreData();
                    break;

                case "[red]Import Custom Excersizes[/]":
                    excersizeManager.ImportExtractedData();
                    break;
                case "[red]Export and Store Programs[/]":
                    break;
                case "[red]Import Programs[/]":
                    break;
                case "[red]Export and Store Phases[/]":
                    break;
                case "[red]Import Phases[/]":
                    break;
                case "[red]Export and Store Phased Workout Plans[/]":
                    break;
                case "[red]Phased Workout Plans[/]":
                    break;
                case "[red]Export and Store Workout Session Data[/]":
                    break;
                case "[red]Import Workout Session Data[/]":
                    break;
                case "[red]Exit[/]":
                    return (true, false); // Return false to indicate exit
            }
        }
        
        return (true, true);
    }

    public static List<string> GetMainMenuOptions()
    {

        return new List<string>
        {
            "[red]Export and Store Body Weight Data[/]",
            "[red]Import Body Weight Data[/]",
            "[red]Export and Store Custom Excersizes[/]",
            "[red]Import Custom Excersizes[/]",
            "[red]Export and Store Programs[/]",
            "[red]Import Programs[/]",
            "[red]Export and Store Phases[/]",
            "[red]Import Phases[/]",
            "[red]Export and Store Phased Workout Plans[/]",
            "[red]Import Phased Workout Plans[/]",
            "[red]Export and Store Workout Session Data[/]",
            "[red]Import Workout Session Data[/]",
            "[red]Exit[/]"
        };

    }
}

