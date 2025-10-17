# SDL3# - Sdl3Sharp.Ffi

These are just some managed bindings around [libffi](https://github.com/libffi/libffi) that [SDL3#](https://github.com/fruediger/Sdl3Sharp) and adjacent projects use internally.

---

Might be useful for other projects including yours, might be not. Use at your own risk. Nothing is really (well) documented, as for now, this meant to be used internally by [SDL3#](https://github.com/fruediger/Sdl3Sharp) only.

If you're looking for how you can call C-style variadic parameter functions from your managed .NET code, this could be the repository for you, as it's the sole reason for why this project exists in the first place. But from there, you're on your own. I don't offer support nor give any kinds of guarantees for this project outside of it's usage for [SDL3#](https://github.com/fruediger/Sdl3Sharp).

Thank you for your understanding.

---

## Third-party licensing

The original [libffi](https://github.com/libffi/libffi), the [mirror-fork of the original libffi repository](https://github.com/fruediger/libffi) from which the native binaries are built, and the built NuGet packages that contain those native binaries are all under the same license that can be viewed here: <https://github.com/libffi/libffi/blob/master/LICENSE>
