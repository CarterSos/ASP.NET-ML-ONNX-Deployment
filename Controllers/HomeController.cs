using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Zoo.Models;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Zoo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZooContext _context;
        private readonly InferenceSession _session;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ZooContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;

            // Initialize the InferenceSession here; ensure the path is correct.
            try
            {
                _session = new InferenceSession("C:/Users/carte/Documents-Local/ZooC#/Zoo/decision_tree_model.onnx");
                _logger.LogInformation("ONNX model loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading the ONNX model: {ex.Message}");
            }
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(int hair, int feathers, int eggs, int milk, int airborne, int aquatic, int predator, int toothed, int backbone, int breathes, int venomous, int fins, int legs, int tail, int domestic, int catsize)
        {
            // Dictionary mapping the numeric prediction to an animal type
            var class_type_dict = new Dictionary<int, string>
            {
                { 1, "mammal" },
                { 2, "bird" },
                { 3, "reptile" },
                { 4, "fish" },
                { 5, "amphibian" },
                { 6, "bug" },
                { 7, "invertebrate" }
            };

            try
            {
                var input = new List<float> { hair, feathers, eggs, milk, airborne, aquatic, predator, toothed, backbone, breathes, venomous, fins, legs, tail, domestic, catsize };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
        };

                using (var results = _session.Run(inputs)) // makes the prediction with the inputs from the form (i.e. class_type 1-7)
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    if (prediction != null && prediction.Length > 0)
                    {
                        // Use the prediction to get the animal type from the dictionary
                        var animalType = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                        ViewBag.Prediction = animalType;
                    }
                    else
                    {
                        ViewBag.Prediction = "Error: Unable to make a prediction.";
                    }
                }

                _logger.LogInformation("Prediction executed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during prediction: {ex.Message}");
                ViewBag.Prediction = "Error during prediction.";
            }

            return View("Index");
        }

        public IActionResult ShowPredictions()
        {
            var records = _context.zoo_animals.ToList();  // Fetch all records
            var predictions = new List<AnimalPrediction>();  // Your ViewModel for the view

            // Dictionary mapping the numeric prediction to an animal type
            var class_type_dict = new Dictionary<int, string>
            {
                { 1, "mammal" },
                { 2, "bird" },
                { 3, "reptile" },
                { 4, "fish" },
                { 5, "amphibian" },
                { 6, "bug" },
                { 7, "invertebrate" }
            };

            foreach (var record in records)
            {
                var input = new List<float>
        {
            record.Hair, record.Feathers, record.Eggs, record.Milk,
            record.Airborne, record.Aquatic, record.Predator, record.Toothed,
            record.Backbone, record.Breathes, record.Venomous, record.Fins,
            record.Legs, record.Tail, record.Domestic, record.Catsize
        };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
        };

                string predictionResult;
                using (var results = _session.Run(inputs))
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    predictionResult = prediction != null && prediction.Length > 0 ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
                }

                predictions.Add(new AnimalPrediction { Animal = record, Prediction = predictionResult });
            }

            return View(predictions);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
