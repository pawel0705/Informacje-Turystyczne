using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using InformacjeTurystyczne.Models;
using InformacjeTurystyczne.Models.ViewModels;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Repository;
using System.Dynamic;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.IO;
using System.Text;

namespace InformacjeTurystyczne.Controllers
{
    public class TouristInformationController : Controller
    {
        private readonly IConverter _converter;


        private readonly IAttractionRepository _attractionRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IShelterRepository _shelterRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly IRegionRepository _regionRepository;


        public TouristInformationController(
            IAttractionRepository attractionRepository,
            IPartyRepository partyRepository,
            IShelterRepository shelterRepository,
            ITrailRepository trailRepository,
            IRegionRepository regionRepository,
            IConverter converter
            )
        {
            _attractionRepository = attractionRepository;
            _partyRepository = partyRepository;
            _regionRepository = regionRepository;
            _shelterRepository = shelterRepository;
            _trailRepository = trailRepository;

            _converter = converter;
        }
        
        public IActionResult Index()
        {
            var viewModel = new TouristInformationVM();
            viewModel.attractions = _attractionRepository;
            viewModel.parties = _partyRepository;
            viewModel.shelters = _shelterRepository;
            viewModel.trails = _trailRepository;
            viewModel.regions = _regionRepository;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GeneratePDF()
        {
            var paper = new GlobalSettings
            {
                ColorMode = ColorMode.Grayscale,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DocumentTitle = "Raport z regionu"
            };

            var text = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = PrepareDocument(),
                // tutaj ścieżka do pliku css nie chce coś łapać (zła ścieżka???) TODO
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "stylesheet", @"~/css/site.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 10, Right = "Strona [page] z [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 10, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = paper,
                Objects = { text }
            };

            var pdfFile = _converter.Convert(pdf);

            return File(pdfFile, "application/pdf", "RegionRaport.pdf");
        }

        private string PrepareDocument()
        {
            var document = new StringBuilder();

            document.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Raport z regionów</h1></div>
                                <table class='table' align='center'>
                                    <tr>
                                        <th>Typ atrakcji</th>
                                        <th>Nazwa atrakcji</th>
                                        <th>Opis atrakcji</th>
                                        <th>Region</th>
                                    </tr>");

            foreach (var attraction in _attractionRepository.GetAllAttractionToUser())
            {
                document.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", 
                                  attraction.AttractionType,
                                  attraction.Name,
                                  attraction.Description,
                                  attraction.Region.Name);
            }

            document.Append(@"
                                </table>
                            <table align='center'>
                                    <tr>
                                        <th>Nazwa imprezy</th>
                                        <th>Lokalizacja</th>
                                        <th>Opis</th>
                                        <th>Aktualna</th>
                                        <th>Region</th>
                                    </tr>");

            foreach (var party in _partyRepository.GetAllPartyToUser())
            {
                document.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                  </tr>",
                                  party.Name,
                                  party.PlaceDescription,
                                  party.Description,
                                  party.UpToDate,
                                  party.Region.Name);
            }

            document.Append(@"
                                </table>
                            <table class='table' align='center'>
                                    <tr>
                                        <th>Nazwa schroniska</th>
                                        <th>Maksymalna ilość miejsc</th>
                                        <th>Wolne miejsca</th>
                                        <th>Otwarte</th>
                                        <th>Numer telefonu</th>
                                        <th>Opis</th>
                                        <th>Region</th>
                                    </tr>");

            foreach (var shelter in _shelterRepository.GetAllShelterToUser())
            {
                document.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>",
                                  shelter.Name,
                                  shelter.MaxPlaces,
                                  shelter.Places,
                                  shelter.IsOpen,
                                  shelter.PhoneNumber,
                                  shelter.Description,
                                  shelter.Region.Name);
            }

            document.Append(@"
                                </table>
                            <table class='table' align='center'>
                                    <tr>
                                        <th>Nazwa szlaku</th>
                                        <th>Kolor</th>
                                        <th>Otwarty</th>
                                        <th>Przyczyna zamknięcia</th>
                                        <th>Długość szlaku (m)</th>
                                        <th>Trudność</th>
                                        <th>Opis</th>
                                    </tr>");

            foreach (var trail in _trailRepository.GetAllTrailToUser())
            { 
                document.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>",
                                  trail.Name,
                                  trail.Colour,
                                  trail.Open,
                                  trail.Feedback,
                                  trail.Length,
                                  trail.Difficulty,
                                  trail.Description);
            }

            document.Append(@"
                                </table>
                            </body>
                        </html>");

            return document.ToString();
        }
    }
}