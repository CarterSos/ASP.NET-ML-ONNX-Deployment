# C# / ASP.NET-ML-ONNX-Deployment
This is a C# / ASP.NET app showing how to save an ML model with the ONNX library, and use it in an application to make single predictions and run predictions on a whole database.n entire database.

## Note:
This app is in .Net Core 3.1
In .NET Entity Framwork Core 8.0, you will load the model (from its path) in the Program.cs file
Then in HomeController.cs you can bring it into InferenceSession _session; similar to this example, and run _session.Run(inputs) to get a prediction
Just ask ChatGPT or read specific documentation on how to load ONNX model in .NET 8
