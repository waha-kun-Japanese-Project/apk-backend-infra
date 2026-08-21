using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanLib.Result
{
    public class Result
    {
        protected readonly List<Error> _errors = [];

        public IReadOnlyList<Error> Errors => _errors;

        public bool IsSuccess => _errors.Count == 0;

        public bool IsFailure => !IsSuccess;

        protected Result() { }

        protected Result(Error error)
        {
            _errors.Add(error);
        }

        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }


        public static Result Ok() => new();

        public static Result Fail(Error error) => new(error);

        public static Result Fail(List<Error> errors) => new(errors);
    }


    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(TValue value) : base()
        {
            _value = value;
        }

        private Result(Error error) : base(error)
        {
            _value = default!;
        }

        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }


        public static Result<TValue> Ok(TValue value) => new(value);

        public static new Result<TValue> Fail(Error error) => new(error);

        public static new Result<TValue> Fail(List<Error> errors) => new(errors);


        public static implicit operator Result<TValue>(TValue value) => Ok(value);

        public static implicit operator Result<TValue>(Error error) => Fail(error);

        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
