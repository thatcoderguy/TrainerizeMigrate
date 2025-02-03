// See https://aka.ms/new-console-template for more information
using TrainerizeMigrate.Migrations;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainerizeMigrate;
using TrainerizeMigrate.DataManagers;



ApplicationDbContext db = new ApplicationDbContext();
Config config = new Config();
Console.WriteLine(db.DbPath);
db.Database.Migrate();

//build a menu

Console.WriteLine("Retreiving body weight data");


BodyWeightManager bodyWeightManager = new BodyWeightManager(config, db);
bodyWeightManager.ExtractAndStoreData();


Console.WriteLine("Importing body weight data");


bodyWeightManager.ImportExtractedData();

//excersize manager

//workout manager

//training session manager


Console.WriteLine("Done. Press Any Key");


Console.ReadKey();