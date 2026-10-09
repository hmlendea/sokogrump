# Contributing to SokoGrump

This document covers the guidelines and processes for contributing to this project, including how to report issues, suggest enhancements, submit code changes, and follow project standards.

## 📑 Table of Contents

- [How to Contribute](#how-to-contribute)
- [Security](#security)
- [License](#license)
- [Getting Help](#getting-help)

## 🤝 How to Contribute

### Reporting Issues

- Search existing issues first.
- Use the issue templates if available.
- Provide clear reproduction steps.
- Include environment details.

### Suggesting Enhancements

- Check the roadmap and existing discussions.
- Explain the use case and expected behavior.
- Consider implementation complexity.

### Code Contributions

#### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MonoGame content build tools (`dotnet-mgcb`) - required to rebuild game assets
- TrueType core fonts - required for font rendering on Linux (`fonts-freefont-ttf` or equivalent)

#### Development Setup

```bash
# Clone the repository
git clone https://github.com/hmlendea/sokogrump.git
cd sokogrump

# Install dependencies
dotnet restore
dotnet tool install --global dotnet-mgcb
```

#### Making Changes

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes.
4. Run tests: `dotnet test SokoGrump.slnx`
5. Commit with clear and descriptive messages.
6. Push to your fork.
7. Open a Pull Request.

#### Pull Request Guidelines

- Target the `master` branch.
- Keep PRs focused and atomic.
- Update documentation if applicable.
- Add tests for any new functionality.
- Ensure the CI checks pass.

### Code Style

Follow the project's coding standards:
- C# coding conventions as defined in the project's `.editorconfig` and instruction files
- Run `dotnet format` before committing.

### Testing

```bash
# Run all tests
dotnet test SokoGrump.slnx

# Run specific test project
dotnet test SokoGrump.UnitTests
```

### Documentation

- Update relevant docs for changes.
- Follow the documentation style guide.
- Preview changes locally if possible.



## 🔒 Security

Report security vulnerabilities per the [Security Policy](SECURITY.md).

## 📄 License

By contributing, you agree that your contributions will be licensed under the GPL v3.

## ❓ Getting Help

- [GitHub Issues](https://github.com/hmlendea/sokogrump/issues)
- Check existing discussions and documentation first.