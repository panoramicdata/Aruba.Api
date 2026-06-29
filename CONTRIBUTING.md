# Contributing

Thank you for your interest in contributing to this project!

## How to Contribute

1. **Fork** the repository
2. **Create a branch** for your feature or fix (`git checkout -b feature/my-feature`)
3. **Make your changes** following the coding standards below
4. **Write or update tests** as appropriate
5. **Ensure the build passes** with zero errors, zero warnings, and zero messages
6. **Submit a Pull Request** against the `main` branch

## Coding Standards

- All public types and members should have XML documentation comments
- Use `System.Text.Json` — do not introduce `Newtonsoft.Json`
- Use Refit for HTTP client interfaces
- Use file-scoped namespaces
- Use the `required` keyword for DTO properties where appropriate
- Ensure `TreatWarningsAsErrors` remains enabled
- All code must compile with zero diagnostics

### Adding a new endpoint

1. Add (or extend) the relevant Refit interface under `Aruba.Api/Interfaces/<Area>/`.
   Group interfaces to mirror the HPE Aruba Networking Central API areas
   (`network-monitoring`, `network-notifications`, `network-reporting`,
   `network-services`, `network-troubleshooting`, `network-msp`).
2. Use `[Get]`, `[Post]`, `[Put]`, `[Patch]`, `[Delete]` with the path relative to
   the regional API gateway base address. Route parameters use Refit's `{name}`
   placeholder bound with `[AliasAs("...")]` where the URL segment differs from a
   legal C# identifier (e.g. `serial-number`).
3. Add or reuse a strongly-typed model under `Aruba.Api/Data/`. List endpoints
   return the shared `PagedResponse<T>` envelope (`items`, `count`, `total`, `next`).
4. Expose the interface as a property on `ArubaCentralClient`.
5. Add a unit test (mocked) and, where useful, an integration test (skipped when
   credentials are absent — integration tests live in a separate concern from unit tests).

## Testing

- Use xUnit v3 for all tests
- Use AwesomeAssertions for fluent assertions
- Unit tests must never be skipped (`failSkips: true` in `xunit.runner.json`)
- Integration tests may skip when credentials are unavailable, and are clearly marked
- Ensure all existing tests pass before submitting a PR

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
