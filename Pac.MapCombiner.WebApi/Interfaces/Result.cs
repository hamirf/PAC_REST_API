namespace PAC_Map_Combiner_REST_API.Interfaces;

public static class ResultWithValue
{
  /// <summary>
  ///   Used for cannot execute command due to unknown reason.
  /// </summary>
  /// <typeparam name="T">Data type of the value.</typeparam>
  /// <param name="e">Exception.</param>
  /// <returns>Result with value.</returns>
  public static ResultWithValue<T> Exception<T>( System.Exception e )
  {
    return new ResultWithValue<T>( EResultCode.Error, e.Message, default );
  }

  /// <summary>
  ///   Used for cannot execute command due to known reason.
  /// </summary>
  /// <typeparam name="T">Data type of the value.</typeparam>
  /// <param name="format">Message format.</param>
  /// <param name="args">Arguments.</param>
  /// <returns>Result with value.</returns>
  public static ResultWithValue<T> Fail<T>( string format, params object[] args )
  {
    return new ResultWithValue<T>( EResultCode.Fail, string.Format( format, args ), default );
  }

  /// <summary>
  ///   Represent result as result with default value (value will be null for class data type).
  /// </summary>
  /// <typeparam name="T">Data type of the value.</typeparam>
  /// <param name="result">Result.</param>
  /// <returns>Result with value.</returns>
  public static ResultWithValue<T> FromResult<T>( IResult result )
  {
    return new ResultWithValue<T>( result.ResultCode, result.Description, default );
  }

  /// <summary>
  ///   Command is eligible to be executed.
  /// </summary>
  /// <typeparam name="T">Data type of the value.</typeparam>
  /// <param name="value">Value.</param>
  /// <returns>Result with value.</returns>
  public static ResultWithValue<T> Ok<T>( T value )
  {
    return new ResultWithValue<T>( EResultCode.Ok, value );
  }

  /// <summary>
  ///   Used for command is eligible to be executed, however some circumstances not meet expectations.
  /// </summary>
  /// <typeparam name="T">Data type of the value.</typeparam>
  /// <param name="value">Value.</param>
  /// <param name="warning">Message.</param>
  /// <returns>Result with value.</returns>
  public static ResultWithValue<T> Warning<T>( T value, string warning )
  {
    return new ResultWithValue<T>( EResultCode.Warning, warning, value );
  }
}

public class ResultWithValue<T> : Result, IResult<T>
{
  public ResultWithValue( EResultCode resultCode, string description, T? value ) : base( resultCode, description )
  {
    Value = value;
  }

  public ResultWithValue( EResultCode resultCode, T? value ) : base( resultCode )
  {
    Value = value;
  }

  public ResultWithValue()
  {
    Value = default;
  }

  public ResultWithValue( IResult<T> result )
  {
    Value = result.Value;
    Description = result.Description;
    ResultCode = result.ResultCode;
  }

  public T? Value { get; set; }
}

public class Result : IResult
{
  public Result( EResultCode resultCode, string description )
  {
    ResultCode = resultCode;
    Description = description;
  }

  public Result( IResult result )
  {
    ResultCode = result.ResultCode;
    Description = result.Description;
  }

  public Result( EResultCode resultCode )
  {
    ResultCode = resultCode;
  }

  public Result()
  {
    Description = string.Empty;
  }

  /// <summary>
  ///   Command is eligible to be executed.
  /// </summary>
  public static Result Ok => new(EResultCode.Ok);

  public EResultCode ResultCode { get; set; }
  public string Description { get; set; }

  public IResult Join( IResult other )
  {
    if( other.ResultCode > ResultCode )
    {
      return new Result( other.ResultCode, other.Description + Environment.NewLine + Description );
    }

    return new Result( ResultCode, Description + Environment.NewLine + other.Description );
  }

  public bool IsNotOK()
  {
    return ResultCode != EResultCode.Ok;
  }

  public bool IsNotOkOrWarning()
  {
    bool isOkOrWarning = ResultCode == EResultCode.Ok || ResultCode == EResultCode.Warning;
    return !isOkOrWarning;
  }

  public Result Clone()
  {
    return new Result( ResultCode, Description );
  }

  /// <summary>
  ///   Used for cannot execute command due to unknown reason.
  /// </summary>
  /// <param name="format">Message format.</param>
  /// <param name="args">Arguments.</param>
  /// <returns>Result.</returns>
  public static Result Error( string format, params object[] args )
  {
    return new Result( EResultCode.Error, string.Format( format, args ) );
  }

  /// <summary>
  ///   Used for cannot execute command due to unknown reason.
  /// </summary>
  /// <param name="e">Exception.</param>
  /// <returns>Result.</returns>
  public static Result Exception( System.Exception e )
  {
    return new Result( EResultCode.Error, e.Message );
  }

  /// <summary>
  ///   Used for cannot execute command due to known reason.
  /// </summary>
  /// <param name="format">Message format.</param>
  /// <param name="args">Arguments.</param>
  /// <returns>Result.</returns>
  public static Result Fail( string format, params object[] args )
  {
    return new Result( EResultCode.Fail, string.Format( format, args ) );
  }

  public override string ToString()
  {
    return $"Result {ResultCode} - {Description}";
  }

  /// <summary>
  ///   Used for command is eligible to be executed, however some circumstances not meet expectations.
  /// </summary>
  /// <param name="format">Message format.</param>
  /// <param name="args">Arguments.</param>
  /// <returns>Result.</returns>
  public static Result Warning( string format, params object[] args )
  {
    return new Result( EResultCode.Warning, string.Format( format, args ) );
  }
}