namespace E_Commerce.Application.Common
{
    public sealed record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(
            string code = "General.Failure",
            string description = "A failure occurred.")
            => new(code, description, ErrorType.Failure);

        public static Error Validation(
            string code = "General.Validation",
            string description = "One or more validation errors occurred.")
            => new(code, description, ErrorType.Validation);

        public static Error NotFound(
            string code = "General.NotFound",
            string description = "The requested resource was not found.")
            => new(code, description, ErrorType.NotFound);

        public static Error Conflict(
            string code = "General.Conflict",
            string description = "A conflict occurred.")
            => new(code, description, ErrorType.Conflict);

        public static Error Unauthorized(
            string code = "General.Unauthorized",
            string description = "Authentication is required.")
            => new(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(
            string code = "General.Forbidden",
            string description = "You do not have permission to perform this action.")
            => new(code, description, ErrorType.Forbidden);

        public static Error Invalid(
            string code = "General.Invalid",
            string description = "The request is invalid.")
            => new(code, description, ErrorType.Invalid);
    }
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        Invalid = 6,
    }
}