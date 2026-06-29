# Security Policy

## Supported Versions

Only the latest released version is supported with security updates.

## Reporting a Vulnerability

If you discover a security vulnerability, please report it privately. Do **not** open a public
issue or pull request for a suspected vulnerability.

Use GitHub's private reporting on this repository: open the **Security** tab and choose
**Report a vulnerability**. If that channel is unavailable to you, contact the Panoramic Data
security team via https://www.panoramicdata.com to arrange a private disclosure.

Please include, where you can:

- A description of the vulnerability
- Steps to reproduce the issue
- Any relevant logs or screenshots

We aim to acknowledge reports within 48 hours and to provide a fix or mitigation within 7 business
days, although complex issues may take longer.

## Disclosure Policy

We follow a coordinated disclosure process. We ask that you:

1. Allow us reasonable time to investigate and address the issue
2. Avoid exploiting the vulnerability beyond what is necessary to demonstrate it
3. Do not disclose the issue publicly until we have released a fix

## Handling of Credentials

This library authenticates using an HPE (Hewlett Packard Enterprise) GreenLake **client_id** /
**client_secret** pair via the OAuth 2.0 `client_credentials` grant. Treat both as secrets:

- Do not commit them to source control. If a secret is ever committed, rotate it immediately and
  remove it from the repository history.
- Provide them at runtime via configuration, environment variables, user-secrets, or a vault.
- Access tokens are kept in memory and are not written to logs. If you enable verbose HTTP logging,
  the `client_secret` and `Authorization` header are masked.

Thank you for helping keep our software and users safe.
