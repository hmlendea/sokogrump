# Privacy and Personal Data

SokoGrump is a standalone, cross-platform desktop puzzle game with no network connectivity, telemetry, user accounts, or external data processing. All data remains on the local device under the control of the person running the game.

**Information reviewed:** 2026-10-09

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how SokoGrump at https://github.com/hmlendea/sokogrump handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

SokoGrump is distributed as a self-contained desktop application. Users download and run it locally on their own devices. There is no central service operated by the project maintainers that receives data from the application.

The instance operator (the person running the game) controls their instance's configuration, local storage, logs, backups, access controls, retention, and request handling. The project does not control those functions.

No data is sent from a self-hosted instance to project maintainers or external services. There is no telemetry, no update checks, no crash reporting, no email integration, no authentication service, no reverse proxy, no object storage, and no monitoring integration.

## 📥 Data We Handle

### Data Provided to the Application

- **Language preference** — The user selects a display language (English or Romanian) in the settings menu. This is stored as a string in the local settings file.
- **Graphics settings** — The user may toggle fullscreen mode and select a resolution in the settings menu. These values are stored locally.
- **Audio settings** — Volume and mute preferences exist in the settings model but are not currently exposed in the UI.

No personal identifiers, account credentials, or sensitive data are requested.

### Data Generated or Collected by the Application

- **Game progress** — The last completed level number is saved automatically when a level is finished. This is an integer stored in the local settings file.
- **Settings file** — All user preferences (language, graphics, audio, last level) are serialised to an XML file at `%LOCALAPPDATA%/SokoGrump/Settings.xml` (Windows) or `~/.local/share/SokoGrump/Settings.xml` (Linux/macOS).
- **Logs directory** — A `Logs` directory is created at `%LOCALAPPDATA%/SokoGrump/Logs` (Windows) or `~/.local/share/SokoGrump/Logs` (Linux/macOS), but no logging implementation currently writes to it.

No telemetry, crash reports, usage analytics, or automatic data collection occurs.

### Data Received from Integrations

No personal data is received from integrations or third parties. The application has no built-in integrations.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- **Language selection** — Language preference
- **Graphics configuration** — Fullscreen toggle and resolution
- **Audio configuration** — Volume and mute (stored but not currently used in UI)
- **Progress tracking** — Last completed level number

## 🗄️ Storage, Retention, and Deletion

All data is stored locally on the device running the game:

| Data category | Storage location | Retention | Deletion |
|---------------|------------------|-----------|----------|
| Settings (language, graphics, audio, last level) | XML file at `ApplicationPaths.SettingsFile` under local application data (`%LOCALAPPDATA%/SokoGrump/Settings.xml` on Windows, `~/.local/share/SokoGrump/Settings.xml` on Linux/macOS) | Persisted until manually deleted by the user | User deletes the `Settings.xml` file or the entire `SokoGrump` directory in local application data |
| Logs (directory exists, no current writers) | `ApplicationPaths.LogsDirectory` under local application data | Not applicable (no data written) | User deletes the `Logs` directory |

The instance operator (the person running the game) controls storage, deletion, and backups. The project does not access, retain, or delete this data.

## 🔗 External Processing and Integrations

The application has no built-in external data transfer. No external services, recipients, or integrations process or receive data from SokoGrump.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | — | — | — |

## 🛡️ Data Protection and Security

- All data remains on the local device; no network transmission occurs.
- The settings file is a plain XML file in the user's local application data directory. File-system permissions control access.
- The instance operator is responsible for operating-system updates, access controls, backups, network exposure (the game does not open network ports), and log protection.
- No encryption is applied to the local settings file. Do not store secrets in the game settings.
- No absolute security is promised; this is a local single-user desktop application.

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/sokogrump/blob/main/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers at https://github.com/hmlendea/sokogrump/issues. For a self-hosted instance, contact the instance operator (the person running the game on their device), unless the project explicitly handles the request. Include the game version and operating system if relevant; do not send passwords, access tokens, or other secrets.