using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.Commands;
using WarOfEmpires.Utilities.Extensions;

namespace WarOfEmpires.CommandHandlers.Tests {
    public class CommandResultAssertions<TCommand> : ReferenceTypeAssertions<CommandResult<TCommand>, CommandResultAssertions<TCommand>> where TCommand : ICommand {
        protected override string Identifier => "CommandResult";

        public CommandResultAssertions(CommandResult<TCommand> subject) {
            Subject = subject;
        }

        public AndConstraint<CommandResultAssertions<TCommand>> HaveError<TProperty>(Expression<Func<TCommand, TProperty>> expression, string message) {
            Execute.Assertion
                .ForCondition(Subject.Errors.Count == 1)
                .FailWith("Expected {context:CommandResult} to contain an error, but found {0} errors.", Subject.Errors.Count)
                .Then
                .ForCondition(Subject.Errors.Single().Expression?.GetExpressionText() == expression?.GetExpressionText())
                .FailWith("Expected {context:CommandResult} to contain an error with expression {0}, but found expression {1}.", expression, Subject.Errors.Single().Expression)
                .Then
                .ForCondition(Subject.Errors.Single().Message == message)
                .FailWith("Expected {context:CommandResult} to contain an error with message {0}, but found message {1}.", message, Subject.Errors.Single().Message);

            return new AndConstraint<CommandResultAssertions<TCommand>>(this);
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

        public AndConstraint<CommandResultAssertions<TCommand>> HaveError(string message) {
            Execute.Assertion
                .ForCondition(Subject.Errors.Count == 1)
                .FailWith("Expected {context:CommandResult} to contain an error, but found {0} errors.", Subject.Errors.Count)
                .Then
                .ForCondition(Subject.Errors.Single().Expression == null)
                .FailWith("Expected {context:CommandResult} to contain an error without an expression, but found expression {0}.", Subject.Errors.Single().Expression)
                .Then
                .ForCondition(Subject.Errors.Single().Message == message)
                .FailWith("Expected {context:CommandResult} to contain an error with message {0}, but found message {1}.", message, Subject.Errors.Single().Message);

            return new AndConstraint<CommandResultAssertions<TCommand>>(this);
        }
    }
}