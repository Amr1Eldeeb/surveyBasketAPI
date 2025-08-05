using surveyBasket.Api.Abstractions;

namespace surveyBasket.Api.Abstractions
{
    public class Result
    {
         

        public Result(bool Isscuccess , Error error)
        {
            if( (Isscuccess &&error!=Error.None) ||(!Isscuccess && error==Error.None) )
            {
                throw new InvalidOperationException("fy exception");
            }
            IsSuccess = Isscuccess;
            Error = error;
        }
        public bool IsSuccess { get;}
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } = default!;
        public static Result Success()=> new Result(true , Error.None);
        public static Result Failure(Error error)=> new Result(false , error);

public static Result <TValue> Success<TValue>(TValue value)=>new(value, true , Error.None);
public static Result <TValue> Failure<TValue>(Error error)=>new(default! , false , error);
 



    }
}
public class Result<TValue> : Result
{
      

    private readonly TValue? _Value;
    public Result(TValue value , bool Isscuccess, Error error) :base(Isscuccess, error) 
    {
        _Value = value; 
    }

    public TValue value => IsSuccess ?
        _Value!
        : throw new InvalidOperationException("Failure Results cannot have Value");

}