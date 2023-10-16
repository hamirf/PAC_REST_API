namespace PAC_Map_Combiner_REST_API.Interfaces;

public interface IResult
{
  /// <summary>
  ///   Result code.
  /// </summary>
  EResultCode ResultCode { get; }

  /// <summary>
  ///   Description of the result.
  /// </summary>
  string Description { get; }

  /// <summary>
  ///   Check is result is NOT <see cref="EResultCode.Ok" />.
  /// </summary>
  /// <returns><c>true</c> if result is NOT <see cref="EResultCode.Ok" />.</returns>
  bool IsNotOK();

  /// <summary>
  ///   Check is result is other than <see cref="EResultCode.Ok" /> and other than <see cref="EResultCode.Warning" />.
  /// </summary>
  /// <returns>
  ///   <c>true</c> if result is other than <see cref="EResultCode.Ok" /> and other than
  ///   <see cref="EResultCode.Warning" />.
  /// </returns>
  bool IsNotOkOrWarning();

  /// <summary>
  ///   Join several result which need to be merged and chained together.
  /// </summary>
  /// <param name="other">Result to join.</param>
  /// <returns>Worse ResultCode with all description joined, separated with new line</returns>
  IResult Join( IResult other );
}

public interface IResult<T> : IResult
{
  /// <summary>
  ///   Value of the result.
  /// </summary>
  T Value { get; }
}