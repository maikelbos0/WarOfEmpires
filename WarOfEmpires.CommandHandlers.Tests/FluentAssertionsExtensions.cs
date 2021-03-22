using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Linq;
using WarOfEmpires.Commands;

namespace WarOfEmpires.CommandHandlers.Tests {
    public static class FluentAssertionsExtensions {
        public static CommandResultAssertions<TCommand> Should<TCommand>(this CommandResult<TCommand> instance) where TCommand : ICommand {
            return new CommandResultAssertions<TCommand>(instance);
        }
    }

    public class CommandResultAssertions<TCommand> : ReferenceTypeAssertions<CommandResult<TCommand>, CommandResultAssertions<TCommand>> where TCommand : ICommand {
        protected override string Identifier => "CommandResult";

        public CommandResultAssertions(CommandResult<TCommand> instance) {
            Subject = instance;
        }

        // TODO refactor to accept expression type to get rid of dependency on MVC
        /* Ex:
            var result = new CommandResult<Commands.Markets.SellResourcesCommand>();
            result.AddError(c => c.Merchandise[0].Type, "test");

            System.Linq.Expressions.Expression<System.Func<Commands.Markets.SellResourcesCommand, string>> expression = c => c.Merchandise[0].Type;

            result.Errors[0].Expression.ToString().Should().Be(expression.ToString());
        */
        [Obsolete]
        public AndConstraint<CommandResultAssertions<TCommand>> HaveError(string expression, string message) {
            Execute.Assertion
                .ForCondition(Subject.Errors.Count == 1)
                .FailWith("Expected {context:CommandResult} to contain an error, but found {0} errors.", Subject.Errors.Count)
                .Then
                .ForCondition(GetExpressionText(Subject.Errors.Single()) == expression)
                .FailWith("Expected {context:CommandResult} to contain an error with expression {0}, but found expression {1}.", expression, GetExpressionText(Subject.Errors.Single()))
                .Then
                .ForCondition(Subject.Errors.Single().Message == message)
                .FailWith("Expected {context:CommandResult} to contain an error with message {0}, but found message {1}.", message, Subject.Errors.Single().Message);

            return new AndConstraint<CommandResultAssertions<TCommand>> (this);
        }

        public AndConstraint<CommandResultAssertions<TCommand>> HaveWarning(string message) {
            Execute.Assertion
                .ForCondition(Subject.Warnings.Count == 1)
                .FailWith("Expected {context:CommandResult} to contain a warning, but found {0} warnings.", Subject.Warnings.Count)
                .Then
                .ForCondition(Subject.Warnings.Single() == message)
                .FailWith("Expected {context:CommandResult} to contain a warning with message {0}, but found message {1}.", message, Subject.Warnings.Single());

            return new AndConstraint<CommandResultAssertions<TCommand>>(this);
        }

        // TODO refactor to get rid of dependency on MVC
        private string GetExpressionText(CommandError<TCommand> commandError) {
            if (commandError.Expression == null) {
                return null;
            }

            return null;
        }

        public AndConstraint<CommandResultAssertions<TCommand>> HaveError(string message) {
            return HaveError(null, message);
        }
    }
}