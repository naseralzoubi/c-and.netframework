using System;

// Main program class that serves as the entry point
class Program
{
    static void Main(string[] args)
    {
        // Create command invoker and receiver
        var receiver = new ShippingQuoteReceiver();
        var invoker = new CommandInvoker();

        // Set up command chain
        invoker.SetCommand(new WelcomeCommand(receiver));
        invoker.ExecuteCommand();

        // Get and validate weight
        invoker.SetCommand(new GetWeightCommand(receiver));
        if (!invoker.ExecuteCommand())
            return;

        // Get and validate dimensions
        invoker.SetCommand(new GetDimensionsCommand(receiver));
        if (!invoker.ExecuteCommand())
            return;

        // Calculate and display quote
        invoker.SetCommand(new CalculateQuoteCommand(receiver));
        invoker.ExecuteCommand();
    }
}

// Base command interface
interface ICommand
{
    bool Execute();
}

// Command invoker class
class CommandInvoker
{
    private ICommand _command;

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public bool ExecuteCommand()
    {
        return _command.Execute();
    }
}

// Receiver class that performs the actual operations
class ShippingQuoteReceiver
{
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }

    public void ShowWelcome()
    {
        Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");
    }

    public bool GetAndValidateWeight()
    {
        Console.WriteLine("Please enter the package weight:");
        if (!double.TryParse(Console.ReadLine(), out double weight))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value.");
            return GetAndValidateWeight();
        }

        if (weight > 50)
        {
            Console.WriteLine("Package too heavy to be shipped via Package Express. Have a good day.");
            return false;
        }

        Weight = weight;
        return true;
    }

    public bool GetAndValidateDimensions()
    {
        // Get width
        Console.WriteLine("Please enter the package width:");
        if (!double.TryParse(Console.ReadLine(), out double width))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value.");
            return GetAndValidateDimensions();
        }
        Width = width;

        // Get height
        Console.WriteLine("Please enter the package height:");
        if (!double.TryParse(Console.ReadLine(), out double height))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value.");
            return GetAndValidateDimensions();
        }
        Height = height;

        // Get length
        Console.WriteLine("Please enter the package length:");
        if (!double.TryParse(Console.ReadLine(), out double length))
        {
            Console.WriteLine("Invalid input. Please enter a numeric value.");
            return GetAndValidateDimensions();
        }
        Length = length;

        // Validate total dimensions
        if (Width + Height + Length > 50)
        {
            Console.WriteLine("Package too big to be shipped via Package Express.");
            return false;
        }

        return true;
    }

    public void CalculateAndDisplayQuote()
    {
        var quote = (Width * Height * Length * Weight) / 100;
        Console.WriteLine($"Your estimated total for shipping this package is: ${quote:F2}");
        Console.WriteLine("Thank you!");
    }
}

// Concrete command for showing welcome message
class WelcomeCommand : ICommand
{
    private readonly ShippingQuoteReceiver _receiver;

    public WelcomeCommand(ShippingQuoteReceiver receiver)
    {
        _receiver = receiver;
    }

    public bool Execute()
    {
        _receiver.ShowWelcome();
        return true;
    }
}

// Concrete command for getting weight
class GetWeightCommand : ICommand
{
    private readonly ShippingQuoteReceiver _receiver;

    public GetWeightCommand(ShippingQuoteReceiver receiver)
    {
        _receiver = receiver;
    }

    public bool Execute()
    {
        return _receiver.GetAndValidateWeight();
    }
}

// Concrete command for getting dimensions
class GetDimensionsCommand : ICommand
{
    private readonly ShippingQuoteReceiver _receiver;

    public GetDimensionsCommand(ShippingQuoteReceiver receiver)
    {
        _receiver = receiver;
    }

    public bool Execute()
    {
        return _receiver.GetAndValidateDimensions();
    }
}

// Concrete command for calculating quote
class CalculateQuoteCommand : ICommand
{
    private readonly ShippingQuoteReceiver _receiver;

    public CalculateQuoteCommand(ShippingQuoteReceiver receiver)
    {
        _receiver = receiver;
    }

    public bool Execute()
    {
        _receiver.CalculateAndDisplayQuote();
        return true;
    }
}