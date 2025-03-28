//Fabio, Luiz Gustavo, Daniel Oliveira, Lucas Litieri, João Andrade
namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter code to tokenize (or -1 to exit):");
                string input = Console.ReadLine();
                if (input == "-1")
                    break;

                try
                {
                    var lexer = new Lexer(input);
                    List<Token> tokens = lexer.Tokenize();
                    
                    var parser = new Parser(tokens);
                    parser.Parse();

                    Console.WriteLine("\nTokens:");
                    foreach (var token in tokens)
                    {
                        Console.WriteLine(token);
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\n");
                }
            }
        }
    }
}