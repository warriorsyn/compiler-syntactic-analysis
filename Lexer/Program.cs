var code = "if ( hero  {move_down} else {move_down}";
var code2 = "while(trap) {move_left}";
var code3 = "for(hero;enemy;treasure) {move_left}";

var code4 = @"
// Comentario diferente
if(hero) {while(trap) {move_left}} else {for(hero;enemy;treasure) {move_left}}  // ola

// Comentario
";

var lexer = new Lexer.Lexer(code4);
var tokens = lexer.Tokenize();

var parser = new Lexer.Parser(tokens);

parser.Parse();

foreach (var token in tokens)
{
    Console.WriteLine(token);
}