using Restaurant.Api.Domain.Tables;

namespace Restaurant.Api.Contracts.Tables;

public sealed record UpdateTableStatusRequest(TableStatus Status);
