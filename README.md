# Bryce Campain Resume Web App

A Blazor WebAssembly resume site I built to present my experience, practice modern .NET UI patterns, and experiment with deployment and hosting workflows. The project focuses on polishing my existing C#/.NET web skills while picking up new ones around Blazor, repo hygiene, Azure-hosted assets, and Cloudflare.

## Live Site & Resume
- Live: https://bryce.campain.me
- Repo: https://github.com/bcampain/brycecampain-resume
- PDF: `wwwroot/Bryce-Campain-Resume-Nov2025.pdf` (linked in the app)

## Tech Stack
- .NET 9, Blazor WebAssembly, C#
- Static assets and CSS custom styling
- Azure Blob Storage (animation config), Cloudflare Analytics, canonical SEO/meta tags

## Features
- Resume presented as an interactive single-page Blazor app
- Downloadable resume PDF
- “Fun” mode with animated cards; animation timings fetched from signed blob storage with local `wwwroot/config.json` fallback
- SEO-friendly meta tags, Open Graph, canonical URL
- Responsive layout with dark gradient styling

## Getting Started
Prerequisite: .NET 9 SDK

```bash
dotnet restore
dotnet watch run
# App serves on the URL shown in the console (configured as https://localhost:7253 in launchsettings)
```

## Blazor WASM Testing
- Run tests: `dotnet test`
- Run just the test project: `dotnet test BryceCampainResume.Tests/BryceCampainResume.Tests.csproj`

## Azure Functions API (/api)
- Functions: `GET /api/download-resume` (streams PDF), `GET /api/resume-metadata` (returns `{ "revisionText": "<metadata or Unknown>" }`)
- Shared blob access is via `ResumeStorageService`, pulling from `public/Bryce-Campain-Resume.pdf` with `revisionDate` metadata
- Local run: `func start --cors http://localhost:7253 --cors-credentials` (requires Azure Functions Core Tools; uses the public blob so no local storage emulator needed)
- Local HTTP base: `http://localhost:7071` (default Functions host)
- Once you've started up the func you can run the Blazor WASM app at `https://localhost:7253` and it should be able to hit the running func worker

## Configuration
- Animation config: fetched from a signed Azure Blob URL first, falling back to `wwwroot/config.json`. You can use that config as a template and save to blob storage.
- SEO/meta: adjust title, description, Open Graph, and canonical link in `wwwroot/index.html`.
- Resume PDF: upload latest version of resume PDF to `/public/Bryce-Campain-Resume.pdf` with `revisionDate` metadata set and the Download PDF Resume button revision will auto-update.

## Deployment
- Build/publish: `dotnet publish -c Release`
- Blazor WebAssembly outputs static files; deploy the publish output (or `wwwroot` for static hosting) to your target host. Works with Azure Static Web Apps/Static Sites and other static hosts. Cloudflare handles DNS/analytics for the live site.

## Learning Goals
- Strengthen Blazor and modern .NET client patterns
- Learn GitHub repo management best practices (previously used Azure Devops)
- Practice hosting and routing for static Blazor apps
- Use Azure-hosted assets (blob storage) plus Cloudflare for DNS/analytics

## Roadmap
- Expand Fun Mode interactions and animation polish
- Track resume downloads via analytics
- Add a career timeline component
- **Long Term:**
  - Generalize the app into a reusable resume template (remove personal-only references and make customization straightforward)
  - Make the resume content data-driven via external config (e.g., JSON) so sections can be supplied without code changes

## Contact
- LinkedIn: https://www.linkedin.com/in/brycecampain
- Email: bryce@campain.me
