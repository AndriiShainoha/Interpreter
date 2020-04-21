using Interpreter.Expressions;
using Interpreter.Expressions.Keywords;
using Interpreter.Expressions.Operators;
using Interpreter.Expressions.Terminals;
using Interpreter.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Interpreter.Parser
{
    class Parser : IParser
    {
        private Queue<Token> _tokens;

        public IExpression Parse(Queue<Token> tokens)
        {
            _tokens = tokens;
            var statements = new List<IExpression>();
            while (_tokens.Count != 0)
            {
                statements.Add(GetStatement());
            }
            return statements.Count == 1 ? statements[0] : new BlockExpression(statements);
        }

        private IExpression GetStatement()
        {
            IExpression statement;
            switch (_tokens.Peek().Type)
            {
                case TokenType.LeftBracket:
                    return GetBlockStatement();
                case TokenType.Cycle:
                    return GetCycle();
                case TokenType.If:
                    return GetConditional();
                case TokenType.Variable:
                    statement = GetAssignStatement();
                    if (_tokens.Dequeue().Type != TokenType.EndOfOperation)
                        throw new InvalidOperationException();
                    return statement;
                case TokenType.RightBracket:
                case TokenType.EndOfOperation:
                    return null;
                default:
                    statement = GetBasicFuncExpression();
                    if (_tokens.Dequeue().Type != TokenType.EndOfOperation)
                        throw new InvalidOperationException();
                    return statement;
            }
        }

        private IExpression GetExpression()
        {
            var node = GetAddMinus();

            if (_tokens.Peek().Type == TokenType.Less)
            {
                _tokens.Dequeue();
                var nextNode = GetAddMinus();
                node = new LessExpression(node, nextNode);
            }
            else if (_tokens.Peek().Type == TokenType.Greater)
            {
                _tokens.Dequeue();
                var nextNode = GetAddMinus();
                node = new GreaterExpression(node, nextNode);
            }
            else if (_tokens.Peek().Type == TokenType.Equal)
            {
                _tokens.Dequeue();
                var nextNode = GetAddMinus();
                node = new EqualExpression(node, nextNode);
            }
            else if (_tokens.Peek().Type == TokenType.NotEqual)
            {
                _tokens.Dequeue();
                var nextNode = GetAddMinus();
                node = new NotEqualExpression(node, nextNode);
            }

            return node;
        }

        private IExpression GetAddMinus()
        {
            var node = GetMultiplyDivide();

            while (_tokens.Peek().Type == TokenType.Add || _tokens.Peek().Type == TokenType.Subtract)
            {
                var currentToken = _tokens.Dequeue();
                var nextNode = GetMultiplyDivide();

                if (currentToken.Type == TokenType.Add)
                {
                    node = new AddExpression(node, nextNode);
                }
                else if (currentToken.Type == TokenType.Subtract)
                {
                    node = new SubtractExpression(node, nextNode);
                }
            }

            return node;
        }

        private IExpression GetMultiplyDivide()
        {
            var node = GetOperand();

            while (_tokens.Peek().Type == TokenType.Multiply || _tokens.Peek().Type == TokenType.Divide)
            {
                var currentToken = _tokens.Dequeue();
                var nextNode = GetOperand();

                if (currentToken.Type == TokenType.Multiply)
                {
                    node = new MultiplyExpression(node, nextNode);
                }
                else if (currentToken.Type == TokenType.Divide)
                {
                    node = new DivideExpression(node, nextNode);
                }
            }

            return node;
        }

        private IExpression GetOperand()
        {
            Token token = _tokens.Dequeue();
            switch (token.Type)
            {
                case TokenType.Variable:
                    return new VariableExpression(token.Value);
                case TokenType.Integer:
                    return new IntegerExpression(int.Parse(token.Value));
                case TokenType.Double:
                    return new DoubleExpression(double.Parse(token.Value, new NumberFormatInfo { NumberDecimalSeparator = "." }));
                case TokenType.String:
                    return new StringExpression(token.Value.Replace("'", ""));
                case TokenType.Bool:
                    return new BoolExpression(bool.Parse(token.Value));
                case TokenType.LeftParentness:
                    var node = GetExpression();
                    if (_tokens.Dequeue().Type != TokenType.RightParentness)
                    {
                        throw new ArithmeticException();
                    }

                    return node;
                default:
                    throw new ArgumentException();
            }
        }

        private BlockExpression GetBlockStatement()
        {
            var expressions = new List<IExpression>();

            if (_tokens.Dequeue().Type != TokenType.LeftBracket)
                throw new InvalidOperationException();
            var exp = GetStatement();
            if (exp == null & _tokens.Peek().Type == TokenType.RightBracket)
            {
                _tokens.Dequeue();
                return new BlockExpression(expressions);
            }

            expressions.Add(exp);
            while (_tokens.Peek().Type != TokenType.RightBracket)
            {
                var expression = GetStatement();
                if (expression == null) continue;
                expressions.Add(expression);
            }
            if (_tokens.Dequeue().Type != TokenType.RightBracket)
                throw new InvalidOperationException();

            return new BlockExpression(expressions);
        }

        private CycleExpression GetCycle()
        {
            if (_tokens.Dequeue().Type != TokenType.Cycle)
                throw new InvalidOperationException();

            var condition = GetExpression();

            if (_tokens.Dequeue().Type != TokenType.Colon)
                throw new InvalidOperationException();

            var body = GetStatement();

            return new CycleExpression(condition, body);
        }

        private ConditionalExpression GetConditional()
        {
            if (_tokens.Dequeue().Type != TokenType.If)
                throw new InvalidOperationException();

            var condition = GetExpression();

            if (_tokens.Dequeue().Type != TokenType.Colon)
                throw new InvalidOperationException();

            var trueBody = GetStatement();

            if (_tokens.Count == 0 || _tokens.Peek().Type != TokenType.Else)
                return new ConditionalExpression(condition, trueBody, null);
            if (_tokens.Dequeue().Type != TokenType.Else && _tokens.Dequeue().Type != TokenType.Colon)
                throw new InvalidOperationException();

            var falseBody = GetStatement();

            return new ConditionalExpression(condition, trueBody, falseBody);
        }

        private AssignExpression GetAssignStatement()
        {
            var variable = _tokens.Dequeue();
            if (variable.Type != TokenType.Variable & _tokens.Dequeue().Type != TokenType.Assign)
                throw new InvalidOperationException();
            var expression = GetExpression();

            return new AssignExpression(variable.Value, expression);
        }

        private IExpression GetBasicFuncExpression()
        {
            var func = _tokens.Dequeue();
            var funcExpressionType = Type.GetType("Interpreter.Expressions.Keywords." + Enum.GetName(typeof(TokenType), func.Type) + "Expression");

            if (funcExpressionType == null) throw new InvalidOperationException();

            var argsCount = funcExpressionType?.GetConstructors()[0].GetParameters().Length ?? 0;

            var args = new object[argsCount];
            var ind = 0;
            while (_tokens.Peek().Type != TokenType.EndOfOperation)
            {
                args[ind++] = GetExpression();
                if (ind > argsCount) throw new ArgumentException();
                if (_tokens.Peek().Type == TokenType.EndOfOperation) break;
                if (_tokens.Dequeue().Type != TokenType.Coma)
                    throw new InvalidOperationException();
            }

            if (ind != argsCount) throw new ArgumentException();

            return (IExpression)funcExpressionType?.GetConstructors()[0].Invoke(args);
        }

    }
}
