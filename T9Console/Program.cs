using T9Decoder.Services;

Console.WriteLine("Enter your input, ending with a # and I'll translate the T9 for you! (Non-keypad chars are ignored, feel free to spam)");
Console.WriteLine("Press Enter with an EMPTY INPUT to Terminate program.\n\n");

bool terminate = false;
while (!terminate)
{
    Console.Write("Input: ");
    string? input = Console.ReadLine();

    if (string.IsNullOrEmpty(input)) terminate = true;
    else Console.WriteLine($"'{input}' => '{T9TextService.T9ToString(input)}'");
}

Console.WriteLine("\nHappy texting!\n");