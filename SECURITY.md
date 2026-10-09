# Security Policy

SokoGrump is a standalone, cross-platform desktop puzzle game with no network connectivity, user accounts, or remote data processing. This policy covers the local application and its packaged dependencies.

## 📑 Table of Contents

- [Supported Versions](#supported-versions)
- [Reporting a Vulnerability](#reporting-a-vulnerability)
- [Scope](#scope)
- [Disclosure Policy](#disclosure-policy)
- [Safe Harbour](#safe-harbour)
- [Recognition](#recognition)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | FlatHub | ✅ |
| Latest version | GitHub Releases | ✅ |
| Latest version | Unofficial third-party distribution channels | ❌ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/sokogrump/security/advisories)
- Contact the maintainers directly at https://github.com/hmlendea/sokogrump/issues

## 📌 Scope

The subsequent report categories are in scope for this repository:
- Local application code and packaged game assets in the SokoGrump project
- Dependencies included in the SokoGrump and SokoGrump.UnitTests projects (MonoGame, NuciXNA, NuciDAL)
- Game logic, board mapping, save/load behaviour, and settings persistence

The subsequent categories are out of scope unless explicitly stated to the contrary:
- The release.sh deployment helper script, which delegates to an external upstream script
- Third-party distribution channels (FlatHub, Snap Store, AppImage) and their packaging infrastructure
- The NuciXNA and MonoGame frameworks beyond the versions pinned in the project file
- Build tooling and development dependencies

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.

## 🧾 Safe Harbour

If your research is conducted in good faith, confined to authorised scope, and disclosed responsibly, the maintainers will not pursue action for policy-compliant activity.

## 🙏 Recognition

We appreciate responsible disclosure. Reporters who desire public attribution may be acknowledged in release notes, advisories, or a dedicated acknowledgements section.