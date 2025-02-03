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

        //build a menu
        bool itemselected = false;
        BodyWeightManager bodyWeightManager = new BodyWeightManager(config, db);

        do
        {
            var userSelectionResult = await HandleUserSelection(bodyWeightManager);
            itemselected = userSelectionResult.IsExit;

        } while (!itemselected);

    }

    public static async Task<(bool IsExit, bool something)> HandleUserSelection(BodyWeightManager bodyWeightManager)
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
                case "[red]Extract Body Weight Data[/]":
                    AnsiConsole.Markup("[green]Retreiving body weight data\n[/]");
                    bodyWeightManager.ExtractAndStoreData();
                    break;

                case "[red]Import Body Weight Data[/]":
                    Console.WriteLine("Importing body weight data");
                    //bodyWeightManager.ImportExtractedData();

                    break;

                case "[red]Extract Custom Excersizes[/]":
                    break;
                case "[red]Import Custom Excersizes[/]":
                    break;
                case "[red]Import Phases and Workouts[/]":
                    break;
                case "[red]Export Workout Session Data[/]":
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
            "[red]Extract Body Weight Data[/]",
            "[red]Import Body Weight Data[/]",
            "[red]Extract Custom Excersizes[/]",
            "[red]Import Custom Excersizes[/]",
            "[red]Export Phases and Workouts[/]",
            "[red]Import Phases and Workouts[/]",
            "[red]Export Workout Session Data[/]",
            "[red]Import Workout Session Data[/]",
            "[red]Exit[/]"
        };

    }
}