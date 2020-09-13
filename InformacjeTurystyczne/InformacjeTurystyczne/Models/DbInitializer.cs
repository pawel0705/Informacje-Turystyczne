using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models
{
    // wypełnia bazę danych danymi początkowymi
    public static class DbInitializer
    {

        // Metoda Seed służy do inicjowania bazy danych danymi jeśli nie będzie miała żadnych danych
        public static void Seed(AppDbContext context)
        {
            // REGION
            if (!context.Regions.Any())
            {
                context.AddRange(
                    new Region
                    {
                        Name = "Gliwice"
                    },
                    new Region
                    {
                        Name = "Katowice"
                    },
                    new Region
                    {
                        Name = "Chorzów"
                    },
                    new Region
                    {
                        Name = "Zabrze"
                    },
                    new Region
                    {
                        Name = "Bytom"
                    },
                    new Region
                    {
                        Name = "Warszawa"
                    }
                    );
            }

            context.SaveChanges();

            // ATRAKCJE
            if (!context.Attractions.Any())
            {
                context.AddRange(
                    new Attraction
                    {
                        AttractionType = "Muzeum",
                        Name = "Muzeum programistyczne w Gliwicach",
                        Description = "Muzealna wystawa przedstawia młodych ludzi pełnych nadziei na zostanie programistą, a nie koderem",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Attraction
                    {
                        AttractionType = "Restauracja",
                        Name = "LaBanda",
                        Description = "Pięcio gwiazdkowa restauracja. Tel 123-123-123. Nasza strona www.labanda.pl",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Attraction
                    {
                        AttractionType = "Aquapark",
                        Name = "Aquapark w Katowicach",
                        Description = "Otwarty na zewnątrz aquapark dla dzieci i dorosłych.",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Chorzów").IdRegion
                    },
                    new Attraction
                    {
                        AttractionType = "Muzeum",
                        Name = "Muzeum - Ayy old school!",
                        Description = "Muzeum z tematem wystawy lat XX!",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    },
                    new Attraction
                    {
                        AttractionType = "Restauracja",
                        Name = "Le delicje",
                        Description = "Czterogwiazdoa restauracja w Katowicach na ulicy 3 Maja.",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    }
                    );
            }

            context.SaveChanges();

            // KATEGORIE
            if (!context.Categories.Any())
            {
                context.AddRange(
                new Category
                {
                    Name = "Zamknięcie szlaku"
                },
                new Category
                {
                    Name = "Otwarcie szlaku"
                },
                new Category
                {
                    Name = "Nowa atrakcja"
                },
                new Category
                {
                    Name = "Nowe wydarzenie"
                },
                new Category
                {
                    Name = "Otwarcie schroniska"
                }
                );

            }

            context.SaveChanges();


            // WIADOMOSCI
            if (!context.Messages.Any())
            {
                context.AddRange(
                    new Message
                    {
                        PostingDate1 = DateTime.UtcNow,
                        Description = "Wielki dzień, bo szlak Kubusia Puchatka otwarty!",
                        Name = "Otwarcie nowego szlaku!",
                        IdCategory = context.Categories.FirstOrDefault(a => a.Name == "Otwarcie szlaku").IdCategory,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Message
                    {
                        PostingDate1 = new DateTime(2020, 2, 15),
                        Description = "Nowa restauracja w twoim regionie! Myszka Mika zaprasza!",
                        Name = "Restauracja u Myszki Miki",
                        IdCategory = context.Categories.FirstOrDefault(a => a.Name == "Nowa atrakcja").IdCategory,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Message
                    {
                        PostingDate1 = DateTime.UtcNow,
                        Description = "Szlak zamknięty. Zapraszamy kiedyś indziej, albo nigdy :(",
                        Name = "Zamkniecie szlaku",
                        IdCategory = context.Categories.FirstOrDefault(a => a.Name == "Zamknięcie szlaku").IdCategory,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    },
                    new Message
                    {
                        PostingDate1 = new DateTime(2017, 1, 18),
                        Description = "Zamknięcie szlaku Kubusia Puchatka na czas nieokreślony. Za utrudnienia przepraszamy.",
                        Name = "Zamkniecie szlaku",
                        IdCategory = context.Categories.FirstOrDefault(a => a.Name == "Zamknięcie szlaku").IdCategory,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    }
                    );
            }

            context.SaveChanges();

            // IMPREZY
            if (!context.Partys.Any())
            {
                context.AddRange(
                    new Party
                    {
                        Name = "Balujemy tutaj!",
                        PlaceDescription = "Za garażem ojca Kacpra",
                        Description = "Robimy impre na 18 uro!!!",
                        UpToDate = false,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Chorzów").IdRegion
                    },
                    new Party
                    {
                        Name = "Karnawał powstańczy",
                        PlaceDescription = "Główny rynek miasta",
                        Description = "Wydarzenie rozpoczyna się o 15:00 w sobotę!",
                        UpToDate = true,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Chorzów").IdRegion
                    },
                    new Party
                    {
                        Name = "IEM 2020",
                        PlaceDescription = "Katowice spodek",
                        Description = "9:00 piątek - 19:00 niedziela",
                        UpToDate = true,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    },
                    new Party
                    {
                        Name = "Koncert pod płotką",
                        PlaceDescription = "Gdzieś daleko nie mam pomysłów lmao",
                        Description = "10:00 sobota - 19:00 niedziela",
                        UpToDate = true,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Party
                    {
                        Name = "Zjazd bogów internetu xddd",
                        PlaceDescription = "McDonald na ulicy Poznańczej 7",
                        Description = "Wszystkie dzieci z całej polski zapraszamy!! Sobota 9:00 - 16:00",
                        UpToDate = true,
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Chorzów").IdRegion
                    }
                    );
            }

            context.SaveChanges();

            // SCHRONISKA
            if (!context.Shelters.Any())
            {
                context.AddRange(
                    new Shelter
                    {
                        Name = "Schronisko dla biednych studentów AEI",
                        MaxPlaces = 220,
                        Places = 25,
                        IsOpen = true,
                        PhoneNumber = "666-666-666",
                        Description = "Schronisko dla ofiar sesji.",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Gliwice").IdRegion
                    },
                    new Shelter
                    {
                        Name = "Strażnicy galaktyki",
                        MaxPlaces = 100,
                        Places = 90,
                        IsOpen = false,
                        PhoneNumber = "123123123",
                        Description = "Schronisko. Zapraszamy do recepcji w godzinach 8:00 - 22:00 we wszystkie dni w tygodniu",
                        IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                    },
                     new Shelter
                     {
                         Name = "Schronisko młodzieżowe",
                         MaxPlaces = 50,
                         Places = 6,
                         IsOpen = true,
                         PhoneNumber = "532123554",
                         Description = "Schronisko. Zapraszamy do recepcji w godzinach 8:00 - 22:00 we wszystkie dni w tygodniu",
                         IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Chorzów").IdRegion
                     },
                     new Shelter
                     {
                         Name = "Schronisko dla każdego!",
                         MaxPlaces = 45,
                         Places = 2,
                         IsOpen = true,
                         PhoneNumber = "453-123-432",
                         Description = "Więcej na naszej stronie www.schroniskodlakazdego.pl",
                         IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                     },
                     new Shelter
                     {
                         Name = "Schronisko pod Zieloną Górą",
                         MaxPlaces = 60,
                         Places = 12,
                         IsOpen = true,
                         PhoneNumber = "432-534-123",
                         Description = "Zapraszamy do schroniska młodzieżowego, szczególnie kolonie. Więcej na stronie www.schrzielgor.pl",
                         IdRegion = context.Regions.FirstOrDefault(a => a.Name == "Katowice").IdRegion
                     }
                    );
            }

            context.SaveChanges();


            // SZLAKI
            if (!context.Trails.Any())
            {
                context.AddRange(
                    new Trail
                    {
                        Name = "Szlak bajkowy",
                        Open = true,
                        Feedback = "-",
                        Length = 100,
                        Difficulty = 2,
                        Description = "Szlak dla średnio - zaawansowanych",
                        Colour = "żółty"
                    },
                    new Trail
                    {
                        Name = "Szlak kubusia pichatka",
                        Open = false,
                        Feedback = "Zawalenie drogi",
                        Length = 1000,
                        Difficulty = 3,
                        Description = "Trudny szlak dla zaawansowanych osób",
                        Colour = "czerwony",

                    },
                    new Trail
                    {
                        Name = "Szlak przełęczny",
                        Open = false,
                        Feedback = "Uszkodzony most",
                        Length = 500,
                        Difficulty = 1,
                        Description = "Jakiś łatwy szlak",
                        Colour = "zielony"
                    },
                    new Trail
                    {
                        Name = "Szlak przyjazny",
                        Open = true,
                        Feedback = "-",
                        Length = 150,
                        Difficulty = 2,
                        Description = "Szlak z pięknymi widokami",
                        Colour = "zielony"
                    },
                    new Trail
                    {
                        Name = "Szlak bohaterski",
                        Open = true,
                        Feedback = "-",
                        Length = 2000,
                        Difficulty = 4,
                        Description = "Szlak bardzo trudny. Wyzwanie dla prawdziwych znawców",
                        Colour = "czerwony"
                    }
                    );
            }

            // zapisujemy zmiany
            context.SaveChanges();
        }
    }
}
