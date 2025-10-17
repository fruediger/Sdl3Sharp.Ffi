using System;

namespace Sdl3Sharp.Ffi;

public sealed class StatusException(Status status, string? message = default) : Exception(message)
{
	public Status Status => status;
}
