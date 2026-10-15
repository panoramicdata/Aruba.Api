# Security Policy

## Supported Versions

Only the latest released version is supported with security updates.

## Reporting a Vulnerability

If you discover a security vulnerability, please report it responsibly.

**Do not open a public GitHub issue.**

Instead, use GitHub's private vulnerability reporting: open this repository's **Security** tab
and choose **Report a vulnerability**, or go straight to
https://github.com/panoramicdata/Aruba.Api/security/advisories/new

Please include:

- A description of the vulnerability
- Steps to reproduce the issue
- Any relevant logs or screenshots

We will acknowledge receipt within 48 hours and aim to provide a fix or mitigation within 7 business days.

## Disclosure Policy

We follow a coordinated disclosure process. We ask that you:

1. Allow us reasonable time to investigate and address the issue
2. Avoid exploiting the vulnerability beyond what is necessary to demonstrate it
3. Do not disclose the issue publicly until we have released a fix

Thank you for helping keep our software and users safe.

## Notes on hard-coded network endpoints

This repository intentionally contains hard-coded network endpoints, and they are reviewed:

- **API and SSO endpoints** (`https://us2.api.central.arubanetworks.com`, `https://eu1.api.central.arubanetworks.com`, …, `https://sso.common.cloud.hpe.com/…`) in `Aruba.Api/ArubaCentralClusters.cs` and `Aruba.Api/ArubaCentralClientOptions.cs` are the official, public HPE Aruba Networking Central regional gateway and HPE GreenLake SSO hosts, as documented by HPE. They are DNS names (not raw IP literals), and all traffic uses **TLS**. These are the only endpoints the client talks to, and customers can override `BaseAddress` and `TokenEndpoint` with their own values.
- **`8.8.8.8` (Google Public DNS)** appears only in the **documentation examples** in `README.md` and as a value accepted by the generic `TroubleshootingCommandRequest.Host` field. It is not used by the library at runtime: the client never issues a ping itself. When a caller runs an Aruba AP "ping" troubleshooting command, the **on-premise access point** performs the ping to a host that the caller chooses; `8.8.8.8` in the README is a neutral, well-known example host (publicly reachable DNS) that is safe to ping and does not reveal anything about the caller or the tenant. Using it in a troubleshooting example is equivalent to HPE's own Aruba Central UI, which offers "8.8.8.8" as a common ping target.
- **Test credentials** are never committed: `Aruba.Api.Test/usersecrets.example.json` is a template with placeholder values (`your-new-central-client-id` / `your-new-central-client-secret`), and real credentials are read from .NET user-secrets or environment variables at test time.

If any of these endpoints ever change, or a new raw IP literal is introduced, review it against
the guidance above before merging.
