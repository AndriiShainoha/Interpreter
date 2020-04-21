namespace Interpreter.Tokens
{
    class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value = "")
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
