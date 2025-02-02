// See https://aka.ms/new-console-template for more information
using TrainerizeMigrate.Migrations;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainerizeMigrate;
using TrainerizeMigrate.DataManagers;

Console.WriteLine("Hello, World!");

ApplicationDbContext db = new ApplicationDbContext();
Config config = new Config();

BodyWeightManager bodyWeightManager = new BodyWeightManager(config);
bodyWeightManager.ExtractData();