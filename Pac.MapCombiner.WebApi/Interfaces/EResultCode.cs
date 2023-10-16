namespace PAC_Map_Combiner_REST_API.Interfaces;

public enum EResultCode
{
  /// <summary>
  ///   Initial state.
  /// </summary>
  Unknown = 0,

  /// <summary>
  ///   Command is eligible to be executed.
  /// </summary>
  Ok = 1,

  /// <summary>
  ///   Used for command is eligible to be executed, however some circumstances not meet expectations.
  /// </summary>
  Warning = 100,

  /// <summary>
  ///   Used for cannot execute command due to known reason.
  /// </summary>
  Fail = 200,

  /// <summary>
  ///   Used for cannot execute command due to unknown reason.
  /// </summary>
  Error = 300
}