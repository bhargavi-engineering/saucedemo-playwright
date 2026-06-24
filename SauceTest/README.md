# SauceTest — Playwright UI Test Automation

An end-to-end UI test automation framework for [SauceDemo](https://www.saucedemo.com), built with Playwright .NET and NUnit. Implements the Page Object Model (POM) pattern with full cross-browser support, trace capture on failure, and a clean separation between test infrastructure and test logic.

---

## Features

- Page Object Model (POM) with a shared `BasePage` for common UI elements
- Cross-browser support — Chromium, Firefox, and WebKit via `.runsettings` files
- Automatic trace capture on test failure (screenshots, snapshots, and sources)
- `BaseTest` handles all browser lifecycle, tracing setup, and teardown
- Multi-environment support — pass environment name via CLI or CI/CD to switch config automatically
- Separate `Models` for strongly-typed test data
- `RestApiTests` folder ready for API-level test coverage
- Global usings to keep test files clean

---

## Tech stack

| Layer | Technology |
|---|---|
| Language | C# (.NET 8.0) |
| Test Framework | NUnit |
| Browser Automation | Microsoft Playwright |
| Pattern | Page Object Model (POM) |
| Browser Engines | Chromium, Firefox, WebKit |
| Tracing | Playwright Trace Viewer |
| Environment Config | JSON config files + `ConfigurationHelper` (Static Singleton) |

---

## Project structure

```
SauceTest/
├── Data/                          # Test data files
├── Models/
│   ├── InventoryItem.cs           # Strongly-typed inventory item model
│   ├── Login.cs                   # Login credentials model
│   └── TestConfig.cs              # Environment config model (SiteUrl, ApiBaseUrl, DbConnection)
├── Pages/
│   ├── BasePage.cs                # Shared UI elements (header, hamburger menu, logout)
│   ├── CheckoutInformationPage.cs # Checkout information form page
│   ├── CheckoutOverviewPage.cs    # Checkout order summary page
│   ├── InventoryPage.cs           # Main product listing page
│   ├── LoginPage.cs               # Login page
│   └── ShoppingCartPage.cs        # Shopping cart page
├── runsettings/
│   ├── chromium.runsettings       # Chromium browser + default environment
│   ├── firefox.runsettings        # Firefox browser + default environment
│   └── webkit.runsettings         # WebKit (Safari) browser + default environment
├── TestConfig/
│   ├── config.local.json          # Local environment config (committed)
│   ├── config.staging.json        # Staging environment config (gitignored)
├── Tests/
│   ├── RestApiTests/              # API-level tests (coming soon)
│   └── UXTests/
│       ├── BaseTest.cs            # Browser setup, environment config, tracing, and teardown
│       └── CheckoutTests.cs       # End-to-end checkout test suite
├── Utilities/
│   └── ConfigurationHelper.cs     # Singleton — reads environment, loads matching config JSON
├── GlobalUsings.cs                # Global using statements
└── README.md
```

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) (for Playwright browser installation)

---

## Getting started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/SauceTest.git
cd SauceTest
```

### 2. Install the Playwright NUnit package

```bash
dotnet add package Microsoft.Playwright.NUnit
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Build the project

```bash
dotnet build
```

### 5. Install Playwright browsers

After building, install the browser binaries using the Playwright CLI:

```powershell
pwsh bin/Debug/net8.0/playwright.ps1 install
```

> **Note:** If `pwsh` is not recognised, install PowerShell from the [Microsoft docs](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows).

---

## Running the tests

### Run all tests (default browser)

```bash
dotnet test
```

### Run tests on a specific browser

Use the `.runsettings` files to target a specific browser engine:

```bash
# Chromium (Chrome / Edge)
dotnet test --settings:runsettings/chromium.runsettings

# Firefox
dotnet test --settings:runsettings/firefox.runsettings

# WebKit (Safari)
dotnet test --settings:runsettings/webkit.runsettings
```

### Run tests on all browsers in sequence

```powershell
# RunTests.ps1
dotnet test --settings:runsettings/chromium.runsettings
dotnet test --settings:runsettings/firefox.runsettings
dotnet test --settings:runsettings/webkit.runsettings
```

### Run a specific test class

```bash
dotnet test --filter "ClassName=CheckoutTests"
```

### Run a specific test method

```bash
dotnet test --filter "FullyQualifiedName~CheckoutTest_SingleRandomItem"
```

---

## Running tests in Visual Studio

1. Open `SauceTest.sln` in Visual Studio 2022
2. Go to **Test → Configure Run Settings → Select Solution Wide runsettings File**
3. Select the desired `.runsettings` file from the `runsettings/` folder (e.g. `chromium.runsettings`)
4. Open **Test Explorer** (`Ctrl+E, T`)
5. Click **Run All Tests** or right-click a specific test and select **Run**

---

## Environment configuration

Tests support multiple environments driven by a single `Environment` parameter. Based on the value passed, `ConfigurationHelper` automatically loads the matching JSON config file from the `TestConfig/` folder.

### Config files

| File | Environment | Committed |
|---|---|---|
| `config.local.json` | `local` | ✓ Yes |
| `config.staging.json` | `staging` | ✗ Gitignored — inject via CI |

Each config file contains:

```json
{
  "SiteUrl": "https://www.saucedemo.com",
  "ApiBaseUrl": "http://localhost:5000",
  "DbConnection": "Server=localhost;Database=MyLibraryDb;Trusted_Connection=True;"
}
```

### Passing environment via CLI

```bash
# Local (default — uses config.local.json)
dotnet test --settings:runsettings/chromium.runsettings

# Staging — override environment at runtime
dotnet test --settings:runsettings/chromium.runsettings \
  -- TestRunParameters.Parameter.Environment=staging


```

### Passing environment via CI/CD

**GitHub Actions:**
```yaml
- name: Run Tests
  run: |
    dotnet test --settings:runsettings/chromium.runsettings \
    -- TestRunParameters.Parameter.Environment=${{ vars.TEST_ENVIRONMENT }}
```


> **Security note:** Never commit `config.staging.json` or `config.production.json` with real credentials. Add them to `.gitignore` and inject sensitive values via CI/CD secrets.

---

## Browser configuration

Browser settings are controlled via `.runsettings` files in the `runsettings/` folder. Example `chromium.runsettings`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <Playwright>
    <BrowserName>chromium</BrowserName>
    <LaunchOptions>
      <Headless>false</Headless>
      <SlowMo>0</SlowMo>
    </LaunchOptions>
  </Playwright>
</RunSettings>
```

| Setting | Description |
|---|---|
| `BrowserName` | `chromium`, `firefox`, or `webkit` |
| `Headless` | `true` to run without UI (for CI), `false` to see the browser |
| `SlowMo` | Milliseconds to slow down each action (useful for debugging) |

---

## Trace viewer

On test failure, a Playwright trace is automatically saved to:

```
bin/Debug/net8.0/playwright-traces/
```

The trace file includes screenshots, DOM snapshots, and source code at every step.

### Viewing a trace

**Option 1 — Playwright CLI:**
```powershell
pwsh bin/Debug/net8.0/playwright.ps1 show-trace "playwright-traces/YourTest.zip"
```

**Option 2 — Browser (no install needed):**

Drag and drop the `.zip` file onto [trace.playwright.dev](https://trace.playwright.dev)

---

## Page object structure

All pages inherit from `BasePage` which provides shared elements available on every authenticated page:

```
BasePage                    — Page title, hamburger menu, logout
├── LoginPage               — Username, password, login button
├── InventoryPage           — Product listing, add to cart
├── InventoryItemPage       — Individual product detail
├── ShoppingCartPage        — Cart items, checkout button
├── CheckoutInformationPage — First name, last name, zip code
└── CheckoutOverviewPage    — Order summary, finish button
```

---

## Roadmap

- [x] Multi-environment support via `ConfigurationHelper` and `TestConfig` JSON files
- [x] Trace capture on test failure with sanitized filenames and timestamps
- [ ] Complete `RestApiTests` for the Library API backend
- [ ] Add tests for all SauceDemo user types (`locked_out_user`, `problem_user`, etc.)
- [ ] Integrate with GitHub Actions CI/CD pipeline
- [ ] Add test reporting (Allure or ExtentReports)
- [ ] Add visual regression tests

---

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m 'Add my feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Open a pull request

---
