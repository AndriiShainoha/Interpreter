using Interpreter.Expressions;
using Interpreter.Expressions.Keywords;
using Interpreter.Expressions.Operators;
using Interpreter.Expressions.Terminals;
using System.Linq;

namespace Interpreter.Optimizer
{
    class Optimizer : IOptimizer
    {
        public IExpression Optimize(AddExpression add)
        {
            add.Expression1.AcceptOptimizer(this);
            add.Expression2.AcceptOptimizer(this);
            if (add.Expression1 is StringExpression stringOperand)
            {
                switch (add.Expression2)
                {
                    case StringExpression operand2:
                        return new StringExpression(stringOperand.Value + operand2.Value);
                    case IntegerExpression operand2:
                        return new StringExpression(stringOperand.Value + operand2.Value);
                    case DoubleExpression operand2:
                        return new StringExpression(stringOperand.Value + operand2.Value);
                }
            }
            else if (add.Expression1 is DoubleExpression doubleOperand)
            {
                switch (add.Expression2)
                {
                    case StringExpression operand2:
                        return new StringExpression(doubleOperand.Value + operand2.Value);
                    case IntegerExpression operand2:
                        return new DoubleExpression(doubleOperand.Value + operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(doubleOperand.Value + operand2.Value);
                }
            }
            else if (add.Expression1 is IntegerExpression intOperand)
            {
                switch (add.Expression2)
                {
                    case StringExpression operand2:
                        return new StringExpression(intOperand.Value + operand2.Value);
                    case IntegerExpression operand2:
                        return new IntegerExpression(intOperand.Value + operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(intOperand.Value + operand2.Value);
                }
            }

            return add;
        }

        public IExpression Optimize(SubtractExpression subtract)
        {
            subtract.Expression1.AcceptOptimizer(this);
            subtract.Expression2.AcceptOptimizer(this);
            if (subtract.Expression1 is DoubleExpression doubleOperand)
            {
                switch (subtract.Expression2)
                {
                    case IntegerExpression operand2:
                        return new DoubleExpression(doubleOperand.Value - operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(doubleOperand.Value - operand2.Value);
                }
            }
            else if (subtract.Expression1 is IntegerExpression intOperand)
            {
                switch (subtract.Expression2)
                {
                    case IntegerExpression operand2:
                        return new IntegerExpression(intOperand.Value - operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(intOperand.Value - operand2.Value);
                }
            }

            return subtract;
        }

        public IExpression Optimize(MultiplyExpression multiply)
        {
            multiply.Expression1.AcceptOptimizer(this);
            multiply.Expression2.AcceptOptimizer(this);
            if (multiply.Expression1 is DoubleExpression doubleOperand)
            {
                switch (multiply.Expression2)
                {
                    case IntegerExpression operand2:
                        return new DoubleExpression(doubleOperand.Value * operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(doubleOperand.Value * operand2.Value);
                }
            }
            else if (multiply.Expression1 is IntegerExpression intOperand)
            {
                switch (multiply.Expression2)
                {
                    case IntegerExpression operand2:
                        return new IntegerExpression(intOperand.Value * operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(intOperand.Value * operand2.Value);
                }
            }

            return multiply;
        }

        public IExpression Optimize(DivideExpression divide)
        {
            divide.Expression1.AcceptOptimizer(this);
            divide.Expression2.AcceptOptimizer(this);
            if (divide.Expression1 is DoubleExpression doubleOperand)
            {
                switch (divide.Expression2)
                {
                    case IntegerExpression operand2:
                        return new DoubleExpression(doubleOperand.Value / operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(doubleOperand.Value / operand2.Value);
                }
            }
            else if (divide.Expression1 is IntegerExpression intOperand)
            {
                switch (divide.Expression2)
                {
                    case IntegerExpression operand2:
                        return new IntegerExpression(intOperand.Value / operand2.Value);
                    case DoubleExpression operand2:
                        return new DoubleExpression(intOperand.Value / operand2.Value);
                }
            }

            return divide;
        }

        public IExpression Optimize(EqualExpression equal)
        {
            equal.Expression1.AcceptOptimizer(this);
            equal.Expression2.AcceptOptimizer(this);
            if (equal.Expression1 is StringExpression stringOperand)
            {
                return new BoolExpression(stringOperand.Value == (equal.Expression2 as StringExpression)?.Value);
            }
            else if (equal.Expression1 is DoubleExpression doubleOperand)
            {
                switch (equal.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(doubleOperand.Value == operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(doubleOperand.Value == operand2.Value);
                }
            }
            else if (equal.Expression1 is IntegerExpression intOperand)
            {
                switch (equal.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(intOperand.Value == operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(intOperand.Value == operand2.Value);
                }
            }

            return equal;
        }

        public IExpression Optimize(NotEqualExpression notEqual)
        {
            notEqual.Expression1.AcceptOptimizer(this);
            notEqual.Expression2.AcceptOptimizer(this);
            if (notEqual.Expression1 is StringExpression stringOperand)
            {
                return new BoolExpression(stringOperand.Value != (notEqual.Expression2 as StringExpression)?.Value);
            }
            else if (notEqual.Expression1 is DoubleExpression doubleOperand)
            {
                switch (notEqual.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(doubleOperand.Value != operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(doubleOperand.Value != operand2.Value);
                }
            }
            else if (notEqual.Expression1 is IntegerExpression intOperand)
            {
                switch (notEqual.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(intOperand.Value != operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(intOperand.Value != operand2.Value);
                }
            }

            return notEqual;
        }

        public IExpression Optimize(LessExpression less)
        {
            less.Expression1.AcceptOptimizer(this);
            less.Expression2.AcceptOptimizer(this);
            if (less.Expression1 is DoubleExpression doubleOperand)
            {
                switch (less.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(doubleOperand.Value < operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(doubleOperand.Value < operand2.Value);
                }
            }
            else if (less.Expression1 is IntegerExpression intOperand)
            {
                switch (less.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(intOperand.Value < operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(intOperand.Value < operand2.Value);
                }
            }

            return less;
        }

        public IExpression Optimize(GreaterExpression greater)
        {
            greater.Expression1.AcceptOptimizer(this);
            greater.Expression2.AcceptOptimizer(this);
            if (greater.Expression1 is DoubleExpression doubleOperand)
            {
                switch (greater.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(doubleOperand.Value > operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(doubleOperand.Value > operand2.Value);
                }
            }
            else if (greater.Expression1 is IntegerExpression intOperand)
            {
                switch (greater.Expression2)
                {
                    case IntegerExpression operand2:
                        return new BoolExpression(intOperand.Value > operand2.Value);
                    case DoubleExpression operand2:
                        return new BoolExpression(intOperand.Value > operand2.Value);
                }
            }

            return greater;
        }

        public IExpression Optimize(AssignExpression assign)
        {
            assign.Expression = assign.Expression.AcceptOptimizer(this);
            return assign;
        }

        public IExpression Optimize(CycleExpression cycle)
        {
            cycle.Condition = cycle.Condition.AcceptOptimizer(this);
            if (cycle.Expression != null)
                cycle.Expression = cycle.Expression.AcceptOptimizer(this);

            if (!(cycle.Condition is BoolExpression boolExpression)) return cycle;
            return boolExpression.Value ? cycle : null;
        }

        public IExpression Optimize(ConditionalExpression conditional)
        {
            conditional.Condition = conditional.Condition.AcceptOptimizer(this);
            conditional.TrueExpression = conditional.TrueExpression.AcceptOptimizer(this);
            if (conditional.FalseExpression != null)
                conditional.FalseExpression = conditional.FalseExpression.AcceptOptimizer(this);

            if (!(conditional.Condition is BoolExpression boolExpression)) return conditional;
            return boolExpression.Value ? conditional.TrueExpression : conditional.FalseExpression;
        }

        public IExpression Optimize(BlockExpression block)
        {
            for (var i = 0; i < block.Expressions.Count; ++i)
            {
                var optimizedExp = block.Expressions[i].AcceptOptimizer(this);
                if (optimizedExp == null)
                {
                    block.Expressions.RemoveAt(i);
                    --i;
                }
                else
                {
                    block.Expressions[i] = optimizedExp;
                }
            }
            return block?.Expressions.Count <= 1 ? block.Expressions.FirstOrDefault() : block;
        }

        public IExpression Optimize(IExpression expression)
        {
            if (!(expression.GetType().Namespace?.Contains("Keywords") ?? false)) return expression;
            var expressionType = expression.GetType();
            var expressionArgs = expressionType.GetProperties();
            foreach (var arg in expressionArgs)
            {
                var optimizedArg = (arg.GetValue(expression) as IExpression)?.AcceptOptimizer(this);
                arg.SetValue(expression, optimizedArg);
            }

            return expression;
        }
    }
}
