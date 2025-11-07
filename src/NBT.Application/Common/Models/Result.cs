namespace NBT.Application.Common.Models;

/// <summary>
/// Represents the result of an operation.
/// </summary>
/// <typeparam name="T">The type of the result data.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; private set; }

    /// <summary>
    /// Gets the result data.
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// Gets the error message if the operation failed.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    public List<string> Errors { get; private set; } = new();

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="data">The result data.</param>
    /// <returns>Success result.</returns>
    public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="error">Error message.</param>
    /// <returns>Failure result.</returns>
    public static Result<T> Failure(string error) => new() { Succeeded = false, Error = error };

    /// <summary>
    /// Creates a failure result with multiple errors.
    /// </summary>
    /// <param name="errors">List of error messages.</param>
    /// <returns>Failure result.</returns>
    public static Result<T> Failure(List<string> errors) => new() { Succeeded = false, Errors = errors };
}

/// <summary>
/// Represents the result of an operation without data.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; private set; }

    /// <summary>
    /// Gets the error message if the operation failed.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    public List<string> Errors { get; private set; } = new();

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>Success result.</returns>
    public static Result Success() => new() { Succeeded = true };

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="error">Error message.</param>
    /// <returns>Failure result.</returns>
    public static Result Failure(string error) => new() { Succeeded = false, Error = error };

    /// <summary>
    /// Creates a failure result with multiple errors.
    /// </summary>
    /// <param name="errors">List of error messages.</param>
    /// <returns>Failure result.</returns>
    public static Result Failure(List<string> errors) => new() { Succeeded = false, Errors = errors };
}
