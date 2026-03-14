# Blazor Visual Test Harness — Implementation Plan

## Overview

Build a Blazor WebAssembly standalone app that provides interactive visualizations for all 45 equations from *The Multiplier and the Mirror*. The app consumes the existing `FStarEquations` class library directly — all computation runs client-side.

**Inputs:** Design document (`BlazorVisualHarness_Design.md`), existing `FStarEquations` library (12 classes, 45 methods, 110 passing tests).

**Outputs:** Three new projects (`FStar.UI` Razor class library, `FStarEquations.App` Blazor WASM app, `FStarEquations.App.Tests` BUnit test project) added to `FStarEquations.slnx`, plus one new class (`Integration`) in the existing `FStarEquations` library.

---

## Solution Structure (Final State)

```
fstar/
├── FStarEquations.slnx
├── src/
│   ├── FStarEquations/                        # Existing equation library
│   │   ├── FStarEquations.csproj
│   │   ├── (12 existing equation classes)
│   │   └── Integration.cs                     # NEW — Forward Euler ODE solver
│   ├── FStarEquationsTests/                   # Existing tests
│   │   ├── FStarEquationsTests.csproj
│   │   ├── (12 existing test classes)
│   │   └── IntegrationTests.cs                # NEW — tests for Euler solver
│   ├── FStar.UI/                              # NEW — Blazor Razor class library
│   │   ├── FStar.UI.csproj
│   │   ├── _Imports.razor
│   │   ├── Components/
│   │   │   ├── Charts/
│   │   │   │   ├── LineChart.razor
│   │   │   │   ├── LineChart.razor.css
│   │   │   │   ├── HeatMap.razor
│   │   │   │   ├── HeatMap.razor.css
│   │   │   │   ├── BarChart.razor
│   │   │   │   ├── BarChart.razor.css
│   │   │   │   ├── WaterfallChart.razor
│   │   │   │   ├── WaterfallChart.razor.css
│   │   │   │   ├── PhasePortrait.razor
│   │   │   │   ├── PhasePortrait.razor.css
│   │   │   │   ├── NumberLine.razor
│   │   │   │   ├── NumberLine.razor.css
│   │   │   │   ├── TornadoDiagram.razor
│   │   │   │   ├── TornadoDiagram.razor.css
│   │   │   │   └── TimeSeriesAnimator.razor
│   │   │   │       (Canvas-based — no CSS isolation, styled via JS)
│   │   │   ├── Controls/
│   │   │   │   ├── ParameterSlider.razor
│   │   │   │   ├── ParameterSlider.razor.css
│   │   │   │   ├── SliderPanel.razor
│   │   │   │   ├── SliderPanel.razor.css
│   │   │   │   ├── ThresholdIndicator.razor
│   │   │   │   └── ThresholdIndicator.razor.css
│   │   │   └── Layout/
│   │   │       ├── ChartCard.razor
│   │   │       ├── ChartCard.razor.css
│   │   │       ├── SplitPanel.razor
│   │   │       └── SplitPanel.razor.css
│   │   └── wwwroot/
│   │       └── FStar.UI.lib.module.js         # JS interop for Canvas rendering
│   ├── FStarEquations.App.Tests/             # NEW — BUnit tests for UI
│   │   ├── FStarEquations.App.Tests.csproj
│   │   ├── Components/
│   │   │   ├── Charts/
│   │   │   │   ├── LineChartTests.cs
│   │   │   │   ├── BarChartTests.cs
│   │   │   │   ├── HeatMapTests.cs
│   │   │   │   ├── NumberLineTests.cs
│   │   │   │   ├── TornadoDiagramTests.cs
│   │   │   │   ├── PhasePortraitTests.cs
│   │   │   │   ├── WaterfallChartTests.cs
│   │   │   │   └── TimeSeriesAnimatorTests.cs
│   │   │   ├── Controls/
│   │   │   │   ├── ParameterSliderTests.cs
│   │   │   │   ├── SliderPanelTests.cs
│   │   │   │   └── ThresholdIndicatorTests.cs
│   │   │   └── Layout/
│   │   │       ├── ChartCardTests.cs
│   │   │       └── SplitPanelTests.cs
│   │   └── Pages/
│   │       ├── PageRenderTests.cs              # All 17 pages render without throwing
│   │       ├── TippingPointTests.cs            # Hub page: 4 viz + cross-links
│   │       ├── EquationWiringTests.cs          # Each page calls correct equations
│   │       └── SliderInteractionTests.cs       # Slider changes trigger re-render
│   └── FStarEquations.App/                    # NEW — Blazor WASM standalone app
│       ├── FStarEquations.App.csproj
│       ├── Program.cs
│       ├── App.razor
│       ├── _Imports.razor
│       ├── Pages/
│       │   ├── Home.razor
│       │   ├── TippingPoint.razor             # ★ Conceptual hub
│       │   ├── BaseModelPage.razor
│       │   ├── VariancePage.razor
│       │   ├── CreationEvaluationPage.razor
│       │   ├── NegativeForcePage.razor
│       │   ├── ForceDynamicsPage.razor         # ODE simulator (flagship)
│       │   ├── TacitKnowledgePage.razor
│       │   ├── DivergencePage.razor
│       │   ├── OrganizationalPage.razor
│       │   ├── MotivationPage.razor
│       │   ├── SovereigntyPage.razor
│       │   ├── ModelGrowthPage.razor
│       │   ├── TransferPage.razor
│       │   ├── CascadeDashboard.razor
│       │   ├── TerminalDynamics.razor
│       │   └── TimelineDashboard.razor
│       ├── Layout/
│       │   ├── MainLayout.razor
│       │   └── NavMenu.razor
│       └── wwwroot/
│           ├── index.html
│           └── css/
│               └── Site.css
```

---

## Validation Gates

Every phase must pass before proceeding to the next.

| Check | Command | Criterion |
|-------|---------|-----------|
| **Clean build (all projects)** | `dotnet build -p:TreatWarningsAsErrors=true` | 0 errors, 0 warnings |
| **Equation tests pass** | `dotnet test --project src/FStarEquationsTests/` | 115+ tests pass, 0 failures |
| **BUnit tests pass** | `dotnet test --project src/FStarEquations.App.Tests/` | All component and page render tests pass, 0 failures |
| **App launches** | `dotnet run --project src/FStarEquations.App/` | Loads in browser, no console errors |
| **Nullable reference types** | `<Nullable>enable</Nullable>` in all `.csproj` files | No nullable warnings |
| **No third-party charting** | Manual review of all `.csproj` files | Only `Microsoft.AspNetCore.Components.WebAssembly` and `bunit` references (no Plotly, Chart.js, Radzen, etc.) |
| **CSS discipline** | Manual review | App CSS only in `Site.css`; component CSS only in `.razor.css` files within `FStar.UI` |
| **Naming conventions** | Manual review | PascalCase public members, `@page` routes match design doc |

---

## BUnit Test Strategy

Every chart component, control, layout wrapper, and page gets automated BUnit tests. BUnit renders Blazor components in-process (no browser required) and asserts on the rendered markup, component parameters, and interaction behavior.

### Test Tiers

| Tier | What It Proves | When Written |
|------|---------------|-------------|
| **T1: Component renders** | Component accepts parameters and produces markup without throwing | Same wave as the component (Waves 2–3) |
| **T2: Markup structure** | Rendered output contains expected SVG/HTML elements (e.g., `<polyline>`, `<rect>`, `<input type="range">`) | Same wave as the component |
| **T3: Page renders** | Each of the 17 pages renders without throwing when wired to real equation methods | Same wave as the page (Waves 3–5) |
| **T4: Equation wiring** | Each page's `@code` block calls the correct `FStarEquations` methods — verified by checking rendered chart data values match expected equation outputs for known inputs | Wave after the page is wired (Waves 4–5) |
| **T5: Slider interaction** | Changing a slider value triggers re-render and the chart data updates | Wave after standalone charts are wired (Wave 4+) |

### Test Categories by Component Type

**Chart Components** (`Components/Charts/*Tests.cs`):

| Component | T1: Renders | T2: Markup Structure |
|-----------|:-----------:|:--------------------:|
| `LineChart` | Renders with 1 series, renders with 3 series, renders with empty series | Contains `<svg>`, `<polyline>` per series, `<text>` for axis labels |
| `BarChart` | Renders vertical, renders horizontal, renders stacked | Contains `<svg>`, `<rect>` per bar, labels |
| `HeatMap` | Renders with 3×3 grid | Contains `<svg>`, `<rect>` per cell with fill colors |
| `NumberLine` | Renders with threshold and points | Contains `<svg>`, threshold `<line>`, `<circle>` per point |
| `TornadoDiagram` | Renders with 3 factors | Contains `<svg>`, left/right `<rect>` per factor |
| `PhasePortrait` | Renders with curve and zero-crossing | Contains `<svg>`, `<polyline>`, marker at zero-crossing |
| `WaterfallChart` | Renders with positive, negative, and total items | Contains `<svg>`, colored `<rect>` per item |
| `TimeSeriesAnimator` | Renders canvas element with correct dimensions | Contains `<canvas>` with width/height attributes |

**Control Components** (`Components/Controls/*Tests.cs`):

| Component | T1: Renders | T2: Markup Structure |
|-----------|:-----------:|:--------------------:|
| `ParameterSlider` | Renders with label, min, max, value | Contains `<input type="range">` with correct `min`/`max`/`step`/`value` attributes, label text |
| `SliderPanel` | Renders with child content | Contains child content within wrapper element |
| `ThresholdIndicator` | Renders passed (green), renders failed (red) | Contains label text, correct CSS class for pass/fail |

**Layout Components** (`Components/Layout/*Tests.cs`):

| Component | T1: Renders | T2: Markup Structure |
|-----------|:-----------:|:--------------------:|
| `ChartCard` | Renders with title and child content | Contains title text, description (optional), child content |
| `SplitPanel` | Renders with left and right fragments | Contains two child regions |

**Page Tests** (`Pages/*Tests.cs`):

| Test Class | What It Covers |
|-----------|---------------|
| `PageRenderTests` | One `[Theory]` with `[InlineData]` per page type — instantiates and renders each of the 17 pages, asserts no exception thrown. This is the **automated proof that all pages render.** |
| `TippingPointTests` | Hub page renders all 4 chart components (`NumberLine`, `TornadoDiagram`, `PhasePortrait`, `NumberLine` hysteresis), contains 5 cross-links |
| `EquationWiringTests` | For each section page: set slider parameters to known values, render, verify chart data points match expected equation outputs (e.g., render `/variance` with M=3, Var(F)=2 → verify the LineChart's first series contains a point at Y=18.0 since Var(O)=M²·Var(F)=9·2=18) |
| `SliderInteractionTests` | For representative pages: render, find the `<input type="range">`, change its value, verify the chart component re-renders with updated data |

### Test Naming Convention

Same as the equation library: `MethodName_Scenario_ExpectedResult`

Examples:
- `LineChart_SingleSeries_RendersOnePolyline`
- `ParameterSlider_ValueChanged_UpdatesInputElement`
- `VariancePage_Renders_WithoutThrowing`
- `TippingPointPage_BetaSliderChanged_FStarUpdates`

### Expected Test Counts

| Category | Estimated Tests |
|----------|:--------------:|
| Chart components (8 components × ~4 tests) | ~32 |
| Control components (3 components × ~3 tests) | ~9 |
| Layout components (2 components × ~2 tests) | ~4 |
| Page render tests (17 pages) | 17 |
| Tipping Point hub tests | ~8 |
| Equation wiring tests (16 content pages × ~2 tests) | ~32 |
| Slider interaction tests (~8 representative pages) | ~8 |
| **Total** | **~110** |

Combined with the equation library (115 tests), the final test count will be approximately **225 tests**.

---

## Phase A: Scaffolding

**Goal:** Both new projects compile, reference each other correctly, and the app loads a blank page in the browser. Existing tests still pass.

### Step A1: Create FStar.UI Razor Class Library

```bash
dotnet new razorclasslib -n FStar.UI -o src/FStar.UI -f net10.0
```

Configure `FStar.UI.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

- Remove any template-generated files (`ExampleJsInterop.cs`, `Component1.razor`, etc.)
- Create directory structure: `Components/Charts/`, `Components/Controls/`, `Components/Layout/`
- Create `wwwroot/FStar.UI.lib.module.js` with empty export scaffolding
- Create `_Imports.razor` with `@using Microsoft.AspNetCore.Components` and `@using Microsoft.JSInterop`

### Step A2: Create FStarEquations.App Blazor WASM Project

```bash
dotnet new blazorwasm -n FStarEquations.App -o src/FStarEquations.App -f net10.0 --empty
```

Configure `FStarEquations.App.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk.BlazorWebAssembly">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\FStar.UI\FStar.UI.csproj" />
    <ProjectReference Include="..\FStarEquations\FStarEquations.csproj" />
  </ItemGroup>
</Project>
```

**Critical:** `FStar.UI` does NOT reference `FStarEquations`. Only the app references both.

### Step A3: Add Projects to Solution

```bash
dotnet sln FStarEquations.slnx add src/FStar.UI/FStar.UI.csproj
dotnet sln FStarEquations.slnx add src/FStarEquations.App/FStarEquations.App.csproj
```

### Step A3b: Create BUnit Test Project

```bash
dotnet new xunit -n FStarEquations.App.Tests -o src/FStarEquations.App.Tests -f net10.0
dotnet sln FStarEquations.slnx add src/FStarEquations.App.Tests/FStarEquations.App.Tests.csproj
```

Configure `FStarEquations.App.Tests.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="bunit" Version="2.*" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\FStar.UI\FStar.UI.csproj" />
    <ProjectReference Include="..\FStarEquations\FStarEquations.csproj" />
    <ProjectReference Include="..\FStarEquations.App\FStarEquations.App.csproj" />
  </ItemGroup>
  <ItemGroup>
    <Using Include="Xunit" />
    <Using Include="Bunit" />
  </ItemGroup>
</Project>
```

Create directory structure: `Components/Charts/`, `Components/Controls/`, `Components/Layout/`, `Pages/`.

### Step A4: Set Up App Shell

Create the minimal Blazor app shell:

| File | Contents |
|------|----------|
| `Program.cs` | Standard WASM bootstrap (`WebAssemblyHostBuilder`) |
| `App.razor` | `<Router>` with `<RouteView>` and `<LayoutView>` |
| `_Imports.razor` | `@using` for `Microsoft.AspNetCore.Components.Web`, `Microsoft.AspNetCore.Components.Routing`, `FStar.UI.Components.Charts`, `FStar.UI.Components.Controls`, `FStar.UI.Components.Layout`, `FStarEquations` |
| `Layout/MainLayout.razor` | Sidebar nav + `@Body` content area |
| `Layout/NavMenu.razor` | Navigation links for all 16 pages (tipping-point first, then section pages, then dashboards) |
| `wwwroot/index.html` | Standard Blazor WASM host page, links `Site.css` and `FStar.UI.bundle.scp.css` |
| `wwwroot/css/Site.css` | Base reset, CSS custom properties for theming, grid layout for nav+content |

### Step A5: Create Empty Page Stubs

Create all 17 `.razor` page files with `@page` directive and a placeholder heading. Each page gets the correct route from the design doc:

| File | Route | Heading |
|------|-------|---------|
| `Home.razor` | `/` | F* Visual Test Harness |
| `TippingPoint.razor` | `/tipping-point` | The Tipping Point — Eq (14), (14a) |
| `BaseModelPage.razor` | `/base-model` | Force is Not a Number — Eq (1), (1a), (2), (3) |
| `VariancePage.razor` | `/variance` | The Variance Amplifier / Barbell Effect — Eq (4), (4a), (5), (6) |
| `CreationEvaluationPage.razor` | `/creation-evaluation` | Creation vs Evaluation — Eq (7), (7a) |
| `NegativeForcePage.razor` | `/negative-force` | Negative Force — Eq (8), (9), (10) |
| `ForceDynamicsPage.razor` | `/force-dynamics` | The Atrophy Problem — Eq (11), (11a-c) |
| `TacitKnowledgePage.razor` | `/tacit-knowledge` | Tacit Knowledge — Eq (12), (12a), (12b), (13) |
| `DivergencePage.razor` | `/divergence` | The Accelerating Gap — Eq (15a-b), (16), (16a), (32) |
| `OrganizationalPage.razor` | `/organizational` | Organizational Consequences — Eq (17)–(22) |
| `MotivationPage.razor` | `/motivation` | The Meaning Problem — Eq (23) |
| `SovereigntyPage.razor` | `/sovereignty` | Sovereignty — Eq (24), (24a) |
| `ModelGrowthPage.razor` | `/model-growth` | Model Growth — Eq (25) |
| `TransferPage.razor` | `/transfer` | F→M Transfer — Eq (26)–(31) |
| `CascadeDashboard.razor` | `/cascade` | The Cascade — Feedback Loops |
| `TerminalDynamics.razor` | `/terminal-dynamics` | Terminal Dynamics Phase Space |
| `TimelineDashboard.razor` | `/timeline` | The Full Timeline |

### Step A6: Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

- Build: 0 errors, 0 warnings (all 5 projects)
- Tests: 110 equation tests pass, 0 BUnit tests yet (project scaffolded but empty)
- App: loads in browser, nav links work, all 17 pages show their placeholder heading

---

## Phase B: Core Chart Components (FStar.UI)

**Goal:** Build the most-reused chart and control components. After this phase, every section page can start wiring up visualizations.

### Step B1: ParameterSlider Component

`Components/Controls/ParameterSlider.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Label` | `string` | Display name (e.g., "Multiplier M") |
| `Min` | `double` | Range minimum |
| `Max` | `double` | Range maximum |
| `Step` | `double` | Increment |
| `Value` | `double` | Current value (two-way bound) |
| `ValueChanged` | `EventCallback<double>` | Blazor two-way binding callback |

- Uses native `<input type="range">` with `@oninput` for immediate feedback
- Displays current value next to slider
- CSS isolation: `ParameterSlider.razor.css`

### Step B2: SliderPanel Component

`Components/Controls/SliderPanel.razor`

- Wraps a `RenderFragment` child content area
- Renders as a vertical stack of sliders with a collapsible header
- CSS isolation: `SliderPanel.razor.css`

### Step B3: ThresholdIndicator Component

`Components/Controls/ThresholdIndicator.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Label` | `string` | What's being checked |
| `Passed` | `bool` | Green (true) or red (false) |

- Renders a colored badge with label text
- CSS isolation: `ThresholdIndicator.razor.css`

### Step B4: ChartCard Component

`Components/Layout/ChartCard.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Title` | `string` | Chart title |
| `Description` | `string?` | Optional subtitle/description |
| `ChildContent` | `RenderFragment` | The chart component goes here |

- Card wrapper with border, title bar, and content area
- CSS isolation: `ChartCard.razor.css`

### Step B5: SplitPanel Component

`Components/Layout/SplitPanel.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Left` | `RenderFragment` | Chart content (takes ~70% width) |
| `Right` | `RenderFragment` | Controls/sliders (takes ~30% width) |

- CSS Grid: two-column layout, responsive (stacks on narrow screens)
- CSS isolation: `SplitPanel.razor.css`

### Step B6: LineChart Component (SVG)

`Components/Charts/LineChart.razor`

This is the most reused chart component (~15 visualizations). Build it first and thoroughly.

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width` | `int` | SVG viewport width (default 600) |
| `Height` | `int` | SVG viewport height (default 400) |
| `XLabel` | `string` | X-axis label |
| `YLabel` | `string` | Y-axis label |
| `Series` | `IReadOnlyList<ChartSeries>` | One or more data series |
| `XMin`, `XMax` | `double?` | Optional axis bounds (auto-scaled if null) |
| `YMin`, `YMax` | `double?` | Optional axis bounds (auto-scaled if null) |
| `ShowGrid` | `bool` | Show grid lines (default true) |

Supporting types (defined in `FStar.UI/Models/`):

```csharp
public record ChartSeries(
    string Label,
    IReadOnlyList<DataPoint> Points,
    string Color = "#2563eb",
    double StrokeWidth = 2.0,
    bool ShowDots = false);

public record DataPoint(double X, double Y);
```

SVG rendering approach:
- Compute axis scale from data range (or explicit bounds)
- Draw axes, tick marks, labels as SVG `<text>` and `<line>`
- Draw each series as an SVG `<polyline>` (not individual `<line>` elements)
- Optional: draw dots as `<circle>` elements at each data point
- Grid lines as dashed `<line>` elements

### Step B7: BarChart Component (SVG)

`Components/Charts/BarChart.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `XLabel`, `YLabel` | `string` | Axis labels |
| `Bars` | `IReadOnlyList<BarItem>` | Data items |
| `Orientation` | `BarOrientation` | Vertical (default) or Horizontal |

```csharp
public record BarItem(string Label, double Value, string Color = "#2563eb");
public enum BarOrientation { Vertical, Horizontal }
```

- Stacked variant: `StackedBars` parameter (`IReadOnlyList<BarStack>` where `BarStack` contains multiple `BarSegment` records)
- Side-by-side variant: `GroupedBars` parameter (multiple `BarItem` per category)

### Step B8: JS Interop Scaffolding

`wwwroot/FStar.UI.lib.module.js`

Initial exports:
```javascript
export function initCanvas(canvasId, width, height) { ... }
export function clearCanvas(canvasId) { ... }
export function drawLine(canvasId, x1, y1, x2, y2, color, width) { ... }
export function drawCircle(canvasId, cx, cy, r, color) { ... }
export function drawText(canvasId, text, x, y, font, color) { ... }
export function requestAnimationFrame(dotNetRef, methodName) { ... }
```

These are low-level primitives. The `TimeSeriesAnimator.razor` component (Phase D) will compose them.

### Step B9: Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

- Build: clean
- Tests: 115 equation + ~19 BUnit (134 total) pass
- App: loads, components render (verify with a temporary test page that shows a `LineChart` with sample data, a `BarChart`, sliders, and a `ChartCard` — remove after verification)

---

## Phase C: Hub Page + Standalone Charts

**Goal:** Build the Tipping Point hub page first, then implement all standalone chart visualizations for every section page. After this phase, every page shows its standalone charts wired to live equation computations via sliders.

### Step C1: Integration Class (FStarEquations Library)

Before building the ODE-dependent visualizations, add a Forward Euler integrator to the equation library.

`src/FStarEquations/Integration.cs`:

```csharp
namespace FStarEquations;

public static class Integration
{
    /// <summary>
    /// Forward Euler integration: y(t+dt) = y(t) + f(t, y) * dt
    /// </summary>
    public static double[] ForwardEuler(
        double y0,
        double tStart,
        double tEnd,
        double dt,
        Func<double, double, double> dydt)
    { ... }

    /// <summary>
    /// Multi-variable Forward Euler for coupled ODEs.
    /// </summary>
    public static double[][] ForwardEulerSystem(
        double[] y0,
        double tStart,
        double tEnd,
        double dt,
        Func<double, double[], double[]> dydt)
    { ... }
}
```

`src/FStarEquationsTests/IntegrationTests.cs`:
- `ForwardEuler_ConstantDerivative_LinearResult` — dy/dt = 1 → y = t
- `ForwardEuler_ExponentialGrowth_ApproximatesAnalytical` — dy/dt = y → y ≈ e^t (within Euler tolerance)
- `ForwardEulerSystem_TwoVariables_BothIntegrated` — coupled system test
- `ForwardEuler_ZeroDt_ThrowsArgumentException`
- `ForwardEuler_NegativeTimeSpan_ThrowsArgumentException`

### Step C2: Tipping Point Hub Page — 4 Visualizations

This is the conceptual centerpiece. Build all 4 visualizations described in the design doc.

**C2a: NumberLine Component** (`FStar.UI/Components/Charts/NumberLine.razor`)

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `Min`, `Max` | `double` | Number line range |
| `Threshold` | `double` | F* position (vertical divider) |
| `Points` | `IReadOnlyList<NumberLinePoint>` | Population dots |

```csharp
public record NumberLinePoint(double Value, string Color);
```

**C2b: TornadoDiagram Component** (`FStar.UI/Components/Charts/TornadoDiagram.razor`)

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `Factors` | `IReadOnlyList<TornadoFactor>` | Named contributions |

```csharp
public record TornadoFactor(string Label, double PositiveContribution, double NegativeContribution);
```

**C2c: PhasePortrait Component** (`FStar.UI/Components/Charts/PhasePortrait.razor`)

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `XLabel`, `YLabel` | `string` | Axis labels |
| `Curve` | `IReadOnlyList<DataPoint>` | The dF/dt vs F curve |
| `ZeroCrossing` | `double?` | F* position (optional vertical marker) |

**C2d: TippingPoint.razor Page Wiring**

Wire all 4 visualizations to `ForceDynamics.TippingPoint()` and `ForceDynamics.DfDt()`:

1. **Moving Threshold** — `NumberLine` with a slider-driven F* and a generated population (normal distribution, configurable mean/spread). Sliders: beta, R, sigma, M_abs, gamma, E, population mean, population spread.
2. **Parameter Decomposition** — `TornadoDiagram` showing beta*R, sigma*M_abs (positive), gamma*E (negative). Same sliders.
3. **Phase Portrait** — `PhasePortrait` plotting dF/dt (Y) vs F (X) over a range, with F* as zero-crossing marker. Sliders: alpha, S, gamma, E, beta, R, sigma, M_abs.
4. **Hysteresis Funnel** — `NumberLine` in asymmetric mode showing |dF/dt|_decay vs |dF/dt|_recovery arrows at equal distances from F*. Uses `ForceDynamics.IsHysteresisPresent()`.

Cross-links at page bottom: `/force-dynamics`, `/transfer`, `/model-growth`, `/divergence`, `/motivation`.

### Step C3: HeatMap Component

`FStar.UI/Components/Charts/HeatMap.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `XLabel`, `YLabel` | `string` | Axis labels |
| `Data` | `double[,]` | 2D grid of values |
| `XValues`, `YValues` | `double[]` | Axis tick values |
| `ColorScaleMin`, `ColorScaleMax` | `double?` | Color mapping bounds |
| `LowColor`, `HighColor` | `string` | Gradient endpoints |

SVG rendering: grid of `<rect>` elements with computed fill colors (linear interpolation between LowColor and HighColor).

Used by: Eq (1) sensitivity surface, Eq (9) damage scaling, Eq (10) epistemic gap surface.

### Step C4: WaterfallChart Component

`FStar.UI/Components/Charts/WaterfallChart.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | SVG viewport |
| `Items` | `IReadOnlyList<WaterfallItem>` | Steps in the waterfall |
| `YLabel` | `string` | Y-axis label |

```csharp
public record WaterfallItem(string Label, double Value, bool IsTotal = false);
```

Positive values draw upward (green), negative draw downward (red), totals draw from baseline (blue).

Used by: Eq (8) directed force composition.

### Step C5: Wire All Section Pages — Standalone Charts

Work through each page, wiring sliders to equation calls and chart components:

| Page | Standalone Charts | Components Used |
|------|-------------------|-----------------|
| `/base-model` | Cobb-Douglas heatmap, layer ordering bars | `HeatMap`, `BarChart`, `ThresholdIndicator` |
| `/variance` | Variance curve, market value piecewise | `LineChart` (×3) |
| `/creation-evaluation` | Evaluation bottleneck gauge | `BarChart` |
| `/negative-force` | Directed force waterfall, damage heatmap, epistemic gap heatmap | `WaterfallChart`, `HeatMap` (×2) |
| `/force-dynamics` | Layer decay sparklines | `LineChart` (×3 small multiples) |
| `/tacit-knowledge` | Shared work decay curve | `LineChart` |
| `/divergence` | Cohort force ceiling | `LineChart` |
| `/organizational` | ROI bars, SNR collapse, Goodhart bars, decision bottleneck, competitive advantage | `BarChart` (×3), `LineChart` (×2) |
| `/motivation` | Motivation decay curve | `LineChart` |
| `/sovereignty` | Capability bars, resilience gauge | `BarChart`, `ThresholdIndicator` |
| `/model-growth` | Exponential M(t) growth | `LineChart` |
| `/transfer` | Transfer rate bars, absorption ceiling bar, tipping point shift | `BarChart` (×2), `NumberLine` |

Each page follows the same pattern:
1. `@page "/route"` directive
2. `<PageTitle>` for browser tab
3. Section heading and brief description (from source document)
4. For each visualization: `<SplitPanel>` with chart on left, `<SliderPanel>` on right
5. `@code` block: slider-bound parameters, computed equation results, chart data generation

### Step C6: Home Page

`Home.razor` — overview page with:
- Title and brief description of the harness
- Navigation cards (one per section + hub + dashboards) showing section name, equations covered, and a brief tagline
- Tipping Point card visually emphasized (larger, highlighted border)

### Step C7: Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

- Build: clean (including new `Integration` class and tests)
- Tests: 115 equation + ~49 BUnit (164 total) pass
- App: all 17 pages render with working sliders and live charts
- Spot-check: adjusting a slider on `/tipping-point` updates the population visualization in real time

---

## Phase D: Composite Visualizations

**Goal:** Build the multi-equation composite charts that show cross-equation relationships and the ODE simulators that require time integration.

### Step D1: TimeSeriesAnimator Component (Canvas)

`FStar.UI/Components/Charts/TimeSeriesAnimator.razor`

| Parameter | Type | Purpose |
|-----------|------|---------|
| `Width`, `Height` | `int` | Canvas dimensions |
| `TimeSeries` | `IReadOnlyList<AnimatedSeries>` | Named time-series to plot |
| `Duration` | `double` | Total simulation time |
| `Dt` | `double` | Time step |
| `IsAnimating` | `bool` | Play/pause state |
| `OnFrame` | `EventCallback<double>` | Callback per animation frame (current time) |

```csharp
public record AnimatedSeries(string Label, double[] Values, string Color);
```

- Uses JS interop (`FStar.UI.lib.module.js`) for Canvas drawing
- `requestAnimationFrame` loop in JS, calling back to .NET for frame updates
- Play/pause/reset controls built into the component

### Step D2: ODE Simulator Composite — `/force-dynamics`

The flagship interactive simulation:

- Uses `Integration.ForwardEulerSystem()` to integrate Eq (11), (11a), (11b), (11c) simultaneously
- `TimeSeriesAnimator` shows 4 lines: F_total(t), f_surface(t), f_middle(t), f_deep(t)
- Sliders: alpha, beta_s, beta_m, beta_d, gamma_m, S(t), E(t), R(t), sigma, M_abs, dt
- Layer decay small multiples (from Phase C) shown below the main simulator
- Cross-link to `/tipping-point` (F* line overlaid on the time-series)

### Step D3: Knowledge Pipeline Composite — `/tacit-knowledge`

- Integrates Eq (12) over time using `Integration.ForwardEuler()`
- `TimeSeriesAnimator` shows: K_tacit(t), T(t), delta*K(t) threshold line
- `ThresholdIndicator` turns red when `TacitKnowledge.IsPipelineBroken()` is true
- Sliders: delta, phi, W0, psi, M, F_senior

### Step D4: Divergence Trajectories Composite — `/divergence`

- Integrates Eq (15a) and (15b) simultaneously using `Integration.ForwardEulerSystem()`
- `LineChart` with two diverging lines and a shaded gap area (SVG `<polygon>` between the two curves)
- Additional line series for Eq (16a) gap acceleration indicator
- Generational step-down: multiple trajectory lines from different `InitialForce()` (Eq 32) starting points

### Step D5: Remaining Composites

| Page | Composite | Equations Combined |
|------|-----------|-------------------|
| `/variance` | Output Gap Divergence | (4) + (4a) + (5) — dual-axis line chart |
| `/variance` | Barbell + Variance | (4) + (5) + (6) — market value with M overlay |
| `/creation-evaluation` | Resource Allocation | (7a) + (28) — stacked area |
| `/motivation` | Motivation → Force Feedback | (23) + (1) — dual panel |
| `/model-growth` | M(t) Impact Cascade | (25) + (4) + (14) + (21) — 4 small multiples |
| `/transfer` | Time Allocation Trade-off | (28) + (7a) — stacked bar |
| `/transfer` | Data Quality Spiral | (31) + (25) — time-series with feedback loop |
| `/transfer` | Tipping Point Shift | (30) + (14) — before/after number line |

### Step D6: Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

- Build: clean
- Tests: 115 equation + ~110 BUnit (225 total) pass
- App: all composites render with working ODE integration
- Spot-check: ODE simulator on `/force-dynamics` animates smoothly, pause/resume works

---

## Phase E: Cross-Cutting Dashboards

**Goal:** Build the 3 dashboard pages that span multiple sections of the framework.

### Step E1: Cascade Dashboard (`/cascade`)

Interactive flow diagram reproducing the mermaid diagram from the source document:

- SVG-based node-and-edge diagram
- Each node is a live indicator showing the current value (computed from slider state)
- Each edge shows the equation number and flow direction
- Clicking a node navigates to its standalone chart page
- Equations involved: (1), (4), (6), (7), (10), (11), (12), (14), (23), (26), (31), (32)
- Implementation: custom SVG layout (nodes positioned manually, edges as SVG `<path>` with arrowheads)

### Step E2: Terminal Dynamics Dashboard (`/terminal-dynamics`)

Animated phase-plane plot:

- `PhasePortrait` component extended for animation (or `TimeSeriesAnimator` in phase-plane mode)
- Axes: F(t) vs M(t)
- Three trajectory presets: Virtuous, Managed Decline, Collapse (each with different parameter sets)
- User can adjust all key parameters and see the trajectory change
- Equations: (11), (25), (31) integrated as a coupled system

### Step E3: Full Timeline Dashboard (`/timeline`)

Multi-panel stacked time-series (shared X-axis = time):

- 7 vertically stacked `LineChart` instances, all sharing the same time axis
- Panels: M(t), F_total(t), f_layers(t), K_tacit(t), Var(O)(t), F*(t), SNR(t)
- All ODE equations integrated simultaneously over a 20-year horizon
- Master time slider + per-panel parameter sliders
- This is the culmination of the entire harness — the "everything at once" view

### Step E4: Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

- Build: clean
- Tests: 115 equation + ~110 BUnit (225 total) pass
- App: all 3 dashboards render and interact correctly
- Spot-check: Cascade node clicks navigate to correct pages; Timeline 7-panel view scrolls and animates

---

## Phase F: Polish

**Goal:** Responsive layout, accessibility, theming, and final quality pass.

### Step F1: Responsive Layout

In `Site.css`:
- Breakpoints: desktop (>1200px), tablet (768–1200px), mobile (<768px)
- `SplitPanel` stacks vertically on tablet and below
- Nav sidebar collapses to hamburger menu on mobile
- Charts maintain aspect ratio via `viewBox` (SVG) or resize observer (Canvas)

### Step F2: Section Prose

Each page includes a relevant excerpt from the source document between the heading and the first visualization:
- 2–4 sentences per page, drawn from the corresponding section of *The Multiplier and the Mirror*
- Styled as a blockquote in `Site.css`

### Step F3: Keyboard Accessibility

- All sliders focusable and adjustable with arrow keys (native `<input type="range">` already supports this)
- Tab order follows visual order
- `aria-label` attributes on all SVG charts and Canvas elements
- Play/pause buttons on animated charts accessible via keyboard

### Step F4: Dark/Light Theme

In `Site.css`:
- CSS custom properties for all colors (background, foreground, accent, chart palette)
- `:root` defines light theme, `[data-theme="dark"]` defines dark theme
- Toggle switch in `MainLayout.razor` header
- Chart components read colors from CSS custom properties (passed as parameters from page code, or inherited from the SVG's CSS context)

### Step F5: Final Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

Full checklist:

| Check | Pass? |
|-------|:-----:|
| Build: 0 errors, 0 warnings | [ ] |
| All tests pass (115 equation + ~110 BUnit = 225 total) | [ ] |
| No third-party charting packages in any `.csproj` | [ ] |
| App CSS only in `Site.css` | [ ] |
| Component CSS only in `.razor.css` files | [ ] |
| All 17 pages render with working charts | [ ] |
| All sliders produce real-time updates | [ ] |
| ODE simulations animate smoothly | [ ] |
| Tipping point hub has all 4 visualizations + cross-links | [ ] |
| All 3 dashboards functional | [ ] |
| Responsive at all 3 breakpoints | [ ] |
| Dark/light theme toggle works | [ ] |
| Keyboard navigation works | [ ] |
| No console errors | [ ] |

---

## Phase G: Review — Best Practices, Internal Consistency, and Correctness

**Goal:** Systematic audit of the entire codebase for adherence to best practices, internal consistency across all files, and correctness of equation wiring. This phase catches the class of errors that slip through per-file work: naming drift, parameter mismatches, dead code, inconsistent patterns across agents' work, and incorrect equation-to-visualization mappings.

### Step G1: Structural Consistency Audit

Review all `.csproj` files, `_Imports.razor` files, and solution references for consistency:

| Check | Criterion |
|-------|-----------|
| Target framework | All projects target `net10.0` |
| Nullable | `<Nullable>enable</Nullable>` in every `.csproj` |
| ImplicitUsings | `<ImplicitUsings>enable</ImplicitUsings>` in every `.csproj` |
| No third-party charting | No Plotly, Chart.js, Radzen, MudBlazor, or similar in any `.csproj` |
| Project references | App → FStar.UI, App → FStarEquations. FStar.UI does NOT reference FStarEquations. App.Tests → FStar.UI, App.Tests → FStarEquations, App.Tests → App |
| Solution membership | All 5 projects in `FStarEquations.slnx` |
| `_Imports.razor` | Both `FStar.UI/_Imports.razor` and `FStarEquations.App/_Imports.razor` have correct and minimal `@using` directives |

### Step G2: Naming Convention Audit

Review all files for adherence to the naming conventions established in the TDD plan:

| Convention | Scope | Example |
|------------|-------|---------|
| PascalCase | Public members, component parameters, record properties | `ChartSeries`, `StrokeWidth`, `XLabel` |
| camelCase | Local variables, private fields, method parameters | `currentValue`, `seriesData` |
| `@page` routes | Kebab-case, match design doc exactly | `/tipping-point`, `/force-dynamics`, `/tacit-knowledge` |
| Page file names | PascalCase with `Page` suffix (section pages) or descriptive name (dashboards) | `TippingPoint.razor`, `CascadeDashboard.razor` |
| Component file names | PascalCase, match class name | `LineChart.razor`, `ParameterSlider.razor` |
| CSS class names | Kebab-case in `.razor.css` files | `.chart-container`, `.slider-label` |
| JS function names | camelCase in `FStar.UI.lib.module.js` | `initCanvas`, `drawLine` |

### Step G3: Equation Wiring Correctness

For every page, verify that the equation methods called in the `@code` block match the equations documented in the design doc for that page:

| Page | Expected Equations | Verify |
|------|-------------------|:------:|
| `/tipping-point` | 14, 14a, 11 (for phase portrait) | [ ] |
| `/base-model` | 1, 1a, 2, 3 | [ ] |
| `/variance` | 4, 4a, 5, 6 | [ ] |
| `/creation-evaluation` | 7, 7a, 28 | [ ] |
| `/negative-force` | 8, 9, 10 | [ ] |
| `/force-dynamics` | 11, 11a, 11b, 11c, 14 | [ ] |
| `/tacit-knowledge` | 12, 12a, 12b, 13 | [ ] |
| `/divergence` | 15a, 15b, 16, 16a, 32 | [ ] |
| `/organizational` | 17, 18, 19, 20, 21, 22 | [ ] |
| `/motivation` | 23, 1 | [ ] |
| `/sovereignty` | 24, 24a | [ ] |
| `/model-growth` | 25, 4, 14, 21 | [ ] |
| `/transfer` | 26, 26a, 27, 28, 29, 30, 31, 7a, 25, 14 | [ ] |
| `/cascade` | 1, 4, 6, 7, 10, 11, 12, 14, 23, 26, 31, 32 | [ ] |
| `/terminal-dynamics` | 11, 25, 31 | [ ] |
| `/timeline` | All ODE equations | [ ] |

### Step G4: Component API Consistency

Review all chart components for consistent parameter patterns:

| Pattern | Applies To | Check |
|---------|-----------|:-----:|
| `Width`/`Height` as `int` with sensible defaults | All chart components | [ ] |
| `XLabel`/`YLabel` as `string` | All axis-based charts | [ ] |
| Two-way binding via `Value`/`ValueChanged` | `ParameterSlider` | [ ] |
| `RenderFragment` child content pattern | `ChartCard`, `SliderPanel`, `SplitPanel` | [ ] |
| Color parameters as `string` (hex) with defaults | All chart components | [ ] |
| `IReadOnlyList<T>` for data inputs (not arrays, not `List<T>`) | All chart components except `HeatMap` (uses `double[,]` for grid data and `double[]` for axis values — 2D arrays are the natural fit) | [ ] |
| Null-safe optional parameters (`double?`, `string?`) | Optional bounds, labels | [ ] |

### Step G5: CSS Discipline Audit

| Check | Criterion |
|-------|-----------|
| App CSS location | ALL app-level CSS in `FStarEquations.App/wwwroot/css/Site.css` — no other `.css` files in the app project |
| Component CSS location | ALL component CSS in `FStar.UI/Components/**/*.razor.css` — Blazor CSS isolation only |
| No inline styles | No `style="..."` attributes in `.razor` files (use CSS classes instead) |
| No CSS-in-JS | No programmatic style manipulation in `FStar.UI.lib.module.js` beyond Canvas drawing |
| CSS custom properties | All colors, spacing, and font sizes use `var(--property-name)` for theming |
| `index.html` references | Links both `Site.css` and `FStar.UI.bundle.scp.css` |

### Step G6: Code Quality Audit

| Check | Criterion |
|-------|-----------|
| No magic numbers | All numeric constants in page `@code` blocks are either equation parameters (from sliders) or documented defaults |
| No dead code | No commented-out code, unused variables, or unreachable branches |
| No duplicate logic | Equation computations call `FStarEquations` library methods — no re-implementation in page code |
| Data generation | Chart data arrays are generated in computed methods, not inline in markup |
| Error handling | `Integration.ForwardEuler` validates inputs (dt > 0, tEnd > tStart). No silent failures. |
| Parameterization | Every slider has explicit min/max/step/default — no unbounded ranges |

### Step G7: Cross-Reference Audit

Verify internal consistency between the three planning documents and the implemented code:

| Check | Documents Compared |
|-------|-------------------|
| All 45 equations have a visualization | Design doc equation table ↔ implemented pages |
| Page routes match design doc | Design doc page structure ↔ `@page` directives |
| Component list matches design doc | Design doc project tree ↔ actual `FStar.UI/Components/` directory |
| Slider parameters match equation signatures | Page sliders ↔ `FStarEquations` method parameters |
| Cross-links on hub page point to correct routes | `TippingPoint.razor` links ↔ page route list |
| Nav menu has all 16 content entries | `NavMenu.razor` ↔ design doc page structure (Home is default route, not a nav link) |
| Design doc's chart types match implementations | Design doc per-equation chart type ↔ component used on page |

### Step G8: Build and Test Validation

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test --project src/FStarEquationsTests/
dotnet test --project src/FStarEquations.App.Tests/
dotnet run --project src/FStarEquations.App/
```

Final pass:

| Check | Pass? |
|-------|:-----:|
| Build: 0 errors, 0 warnings | [ ] |
| All tests pass (115 equation + ~110 BUnit = 225 total) | [ ] |
| G1: Structural consistency | [ ] |
| G2: Naming conventions | [ ] |
| G3: Equation wiring correctness (all 16 content pages) | [ ] |
| G4: Component API consistency | [ ] |
| G5: CSS discipline | [ ] |
| G6: Code quality (no magic numbers, dead code, duplication) | [ ] |
| G7: Cross-reference audit (3 docs ↔ code) | [ ] |
| Issues found and fixed | [ ] |

---

## Execution Order Summary

```
WAVE 1 ── Phase A ── 1 agent, sequential ──────────────────────────────
  A1: Create FStar.UI Razor class library
  A2: Create FStarEquations.App Blazor WASM project
  A3: Add both to solution
  A3b: Create FStarEquations.App.Tests BUnit test project
  A4: Set up app shell (Program, App, Layout, NavMenu, index.html, Site.css)
  A5: Create 17 empty page stubs
  A+: Create shared model types (ChartSeries, DataPoint, BarItem, etc.)
  A6: Validation gate
  ── All 5 projects compile, app loads, existing tests pass ───────────

WAVE 2 ── Phase B + T1/T2 ── 5 agents parallel ───────────────────────
  Agent 1: ParameterSlider + SliderPanel + ThresholdIndicator
           + BUnit tests (T1/T2)                                 [B1-B3]
  Agent 2: ChartCard + SplitPanel + BUnit tests (T1/T2)         [B4-B5]
  Agent 3: LineChart (SVG) + BUnit tests (T1/T2)                [B6]
  Agent 4: BarChart (SVG) + BUnit tests (T1/T2)                 [B7]
  Agent 5: JS interop scaffolding + Integration class + tests    [B8, C1]
  ── Validation gate (134 tests) ──────────────────────────────────────

WAVE 3 ── Phase C + T1/T2/T3 ── 5 agents parallel ────────────────────
  Agent 1: Hub page — NumberLine, TornadoDiagram, PhasePortrait
           components → TippingPoint.razor (4 visualizations)
           + BUnit tests for new components + TippingPointTests  [C2]
  Agent 2: HeatMap + WaterfallChart → BaseModelPage,
           NegativeForcePage + BUnit tests for new components    [C3-C4]
  Agent 3: VariancePage, CreationEvaluationPage,
           MotivationPage                                        [C5]
  Agent 4: OrganizationalPage, SovereigntyPage,
           ModelGrowthPage                                       [C5]
  Agent 5: ForceDynamicsPage, TacitKnowledgePage, DivergencePage,
           TransferPage, Home.razor + PageRenderTests (T3)       [C5-C6]
  ── Validation gate (164 tests) ──────────────────────────────────────

WAVE 4 ── Phase D + T4/T5 ── 5 agents parallel ───────────────────────
  Agent 1: TimeSeriesAnimator → ODE simulator → Knowledge
           pipeline (animated composites) + Animator tests       [D1-D3]
  Agent 2: Divergence trajectories + generational step-down      [D4]
  Agent 3: Variance, CreationEval composites
           + EquationWiringTests (T4)                            [D5]
  Agent 4: Motivation, ModelGrowth composites
           + SliderInteractionTests (T5)                         [D5]
  Agent 5: Transfer composites (×3)                              [D5]
  ── Validation gate (225 tests) ──────────────────────────────────────

WAVE 5 ── Phase E + F1-F2 ── 5 agents parallel ───────────────────────
  Agent 1: Cascade dashboard (interactive SVG flow diagram)      [E1]
  Agent 2: Terminal Dynamics dashboard (phase-plane animation)   [E2]
  Agent 3: Full Timeline dashboard (7-panel stacked time-series) [E3]
  Agent 4: Responsive layout (Site.css breakpoints)              [F1]
  Agent 5: Section prose excerpts on all section pages           [F2]
  ── Validation gate ──────────────────────────────────────────────────

WAVE 6 ── Phase F3-F5 ── 3 agents parallel ────────────────────────────
  Agent 1: Keyboard accessibility (aria-labels, tab order)       [F3]
  Agent 2: Dark/light theming (CSS custom properties, toggle)    [F4]
  Agent 3: Final validation (14-item checklist)                  [F5]
  ── Validation gate ──────────────────────────────────────────────────

WAVE 7 ── Phase G ── 5 agents parallel ────────────────────────────────
  Agent 1: Structural consistency + naming conventions           [G1-G2]
  Agent 2: Equation wiring correctness (all 16 content pages)            [G3]
  Agent 3: Component API consistency + CSS discipline             [G4-G5]
  Agent 4: Code quality audit (magic numbers, dead code, dupes)  [G6]
  Agent 5: Cross-reference audit (3 docs ↔ code) + final build  [G7-G8]
  ── All issues fixed, final build clean ──────────────────────────────
  ── DONE ─────────────────────────────────────────────────────────────
```

---

## Agent Execution Strategy

The work is organized into 7 sequential **waves**. Within each wave, up to 5 agents run in parallel. A wave's validation gate must pass before the next wave begins.

### Dependency Graph

```
Wave 1 (Phase A)           — sequential, 1 agent
    │
    ▼
Wave 2 (Phase B)           — 5 agents parallel
    │
    ▼
Wave 3 (Phase C)           — 5 agents parallel
    │
    ▼
Wave 4 (Phase D)           — 5 agents parallel
    │
    ▼
Wave 5 (Phase E + F1–F2)   — 5 agents parallel
    │
    ▼
Wave 6 (Phase F3–F5)       — 3 agents parallel
    │
    ▼
Wave 7 (Phase G)           — 5 agents parallel (review + fix)
```

---

### Wave 1: Scaffolding — 1 Agent, Sequential

**Precondition:** Existing solution builds and 110 tests pass.

A single agent executes all of Phase A in order:

| Step | Work |
|------|------|
| A1 | Create `FStar.UI` Razor class library |
| A2 | Create `FStarEquations.App` Blazor WASM project |
| A3 | Add both projects to `FStarEquations.slnx` |
| A3b | Create `FStarEquations.App.Tests` BUnit test project, add to solution |
| A4 | App shell: `Program.cs`, `App.razor`, `_Imports.razor`, `MainLayout.razor`, `NavMenu.razor`, `index.html`, `Site.css` |
| A5 | Create 17 empty page stubs with `@page` directives |
| A+ | Create `FStar.UI/Models/` with all shared record types (`ChartSeries`, `DataPoint`, `BarItem`, `BarOrientation`, `BarStack`, `BarSegment`, `WaterfallItem`, `TornadoFactor`, `NumberLinePoint`, `AnimatedSeries`) |
| A+ | Create `FStar.UI/Components/Charts/`, `Controls/`, `Layout/` directories |
| A6 | Validation gate: build clean, 110 tests pass, app loads blank shell in browser |

**Why single agent:** Every subsequent agent needs the project structure, `.csproj` files, solution references, and shared model types to exist. This is fast (~5 minutes) and eliminates all file-contention risk.

**Output:** All three new projects compile, app loads with 17 navigable placeholder pages.

---

### Wave 2: Core Components — 5 Agents Parallel

**Precondition:** Wave 1 complete. All shared model types exist in `FStar.UI/Models/`.

Each agent writes to its own files — no file contention.

| Agent | Files Created | Steps |
|-------|---------------|-------|
| **Agent 1** — Controls | `ParameterSlider.razor` + `.razor.css`, `SliderPanel.razor` + `.razor.css`, `ThresholdIndicator.razor` + `.razor.css`, `App.Tests/Components/Controls/ParameterSliderTests.cs`, `SliderPanelTests.cs`, `ThresholdIndicatorTests.cs` | B1, B2, B3 + T1/T2 |
| **Agent 2** — Layout | `ChartCard.razor` + `.razor.css`, `SplitPanel.razor` + `.razor.css`, `App.Tests/Components/Layout/ChartCardTests.cs`, `SplitPanelTests.cs` | B4, B5 + T1/T2 |
| **Agent 3** — LineChart | `LineChart.razor` + `.razor.css`, `App.Tests/Components/Charts/LineChartTests.cs` | B6 + T1/T2 |
| **Agent 4** — BarChart | `BarChart.razor` + `.razor.css`, `App.Tests/Components/Charts/BarChartTests.cs` | B7 + T1/T2 |
| **Agent 5** — JS Interop + Integration | `FStar.UI.lib.module.js`, `Integration.cs`, `IntegrationTests.cs` | B8, C1 |

**File contention analysis:** Zero. Each agent writes to completely separate files (including separate test files). No agent modifies `.csproj` or `_Imports.razor` (those were set up in Wave 1).

**Validation gate (after all 5 complete):** Build clean, 110 + 5 Integration tests pass + ~19 BUnit tests pass (134 total), app loads.

---

### Wave 3: Hub Page + Standalone Charts — 5 Agents Parallel

**Precondition:** Wave 2 complete. All core chart/control/layout components available.

| Agent | Pages Owned | Charts Wired | New Components Built | BUnit Tests Written |
|-------|-------------|-------------|---------------------|---------------------|
| **Agent 1** — Tipping Point Hub | `TippingPoint.razor` | Moving Threshold, Parameter Decomposition, Phase Portrait, Hysteresis Funnel | `NumberLine.razor` + `.razor.css`, `TornadoDiagram.razor` + `.razor.css`, `PhasePortrait.razor` + `.razor.css` | `NumberLineTests.cs`, `TornadoDiagramTests.cs`, `PhasePortraitTests.cs`, `TippingPointTests.cs` |
| **Agent 2** — HeatMap pages | `BaseModelPage.razor`, `NegativeForcePage.razor` | Cobb-Douglas heatmap, layer ordering bars, directed force waterfall, damage heatmap, epistemic gap heatmap | `HeatMap.razor` + `.razor.css`, `WaterfallChart.razor` + `.razor.css` | `HeatMapTests.cs`, `WaterfallChartTests.cs` |
| **Agent 3** — LineChart-heavy pages | `VariancePage.razor`, `CreationEvaluationPage.razor`, `MotivationPage.razor` | Variance curve, market value piecewise, evaluation bottleneck, motivation decay | (none — uses existing components) | — |
| **Agent 4** — BarChart-heavy pages | `OrganizationalPage.razor`, `SovereigntyPage.razor`, `ModelGrowthPage.razor` | ROI bars, SNR collapse, Goodhart bars, decision bottleneck, competitive advantage, capability bars, resilience gauge, M(t) growth | (none — uses existing components) | — |
| **Agent 5** — Transfer + remaining | `ForceDynamicsPage.razor` (sparklines only), `TacitKnowledgePage.razor` (shared work decay only), `DivergencePage.razor` (cohort ceiling only), `TransferPage.razor`, `Home.razor` | Layer decay sparklines, shared work decay, cohort force ceiling, transfer rate bars, absorption ceiling, Home nav cards | (none — uses existing components) | `PageRenderTests.cs` (T3 — all 17 pages) |

**File contention analysis:** Zero. Each agent owns a disjoint set of `.razor` page files and test files. Agents 1 and 2 also create new chart components, but in separate files from each other.

**Why this split:**
- Agent 1 has the most complex page (4 visualizations + 3 new components) but only 1 page
- Agent 2 builds 2 new chart components and wires 2 pages (5 total charts)
- Agents 3–5 wire existing components into pages — higher page count but simpler per-page work
- Agent 5 only wires the *standalone* charts for pages that will later get composites in Wave 4
- Agent 5 also writes `PageRenderTests.cs` (T3) since it touches the most pages and can verify all 17

**Validation gate:** Build clean, 115 equation + ~49 BUnit tests pass (164 total), all 17 pages render (16 with standalone charts, Home with nav cards), sliders update charts in real time.

---

### Wave 4: Composite Visualizations — 5 Agents Parallel

**Precondition:** Wave 3 complete. All standalone charts working on all pages.

| Agent | Pages Modified | Composites Added | BUnit Tests Written |
|-------|---------------|-----------------|---------------------|
| **Agent 1** — Animated composites | `ForceDynamicsPage.razor`, `TacitKnowledgePage.razor` | **Builds `TimeSeriesAnimator.razor`** (Canvas), then ODE simulator (Eq 11, 11a-c), then Knowledge pipeline (Eq 12, 12a, 13) | `TimeSeriesAnimatorTests.cs` (T1/T2) |
| **Agent 2** — Divergence | `DivergencePage.razor` | Divergence trajectories composite (Eq 15a, 15b, 16, 16a), Generational step-down (Eq 32 + 15a/15b) | — |
| **Agent 3** — Variance + Creation | `VariancePage.razor`, `CreationEvaluationPage.razor` | Output Gap Divergence (Eq 4+4a+5), Barbell+Variance (Eq 4+5+6), Resource Allocation (Eq 7a+28) | `EquationWiringTests.cs` (T4 — all 16 content pages) |
| **Agent 4** — Motivation + Model Growth | `MotivationPage.razor`, `ModelGrowthPage.razor` | Motivation→Force Feedback (Eq 23+1), M(t) Impact Cascade (Eq 25+4+14+21) | `SliderInteractionTests.cs` (T5 — ~8 representative pages) |
| **Agent 5** — Transfer composites | `TransferPage.razor` | Time Allocation Trade-off (Eq 28+7a), Data Quality Spiral (Eq 31+25), Tipping Point Shift (Eq 30+14) | — |

**File contention analysis:** Zero. Each agent modifies a disjoint set of page files and test files. Agent 1 also creates `TimeSeriesAnimator.razor` (a new file no one else touches).

**Critical path:** Agent 1 is the longest path — it builds the `TimeSeriesAnimator` component AND wires two animated composites. Agents 2–5 use only static SVG charts and finish faster. However, Agent 1's work is self-contained (its two pages are the only consumers of `TimeSeriesAnimator` in this wave), so there is no blocking dependency.

**Validation gate:** Build clean, 115 equation + ~110 BUnit tests pass (225 total), all composites render, ODE simulator animates with play/pause/reset.

---

### Wave 5: Dashboards + Early Polish — 5 Agents Parallel

**Precondition:** Wave 4 complete. All composites working.

| Agent | Work | Phase |
|-------|------|-------|
| **Agent 1** — Cascade Dashboard | `CascadeDashboard.razor`: SVG node-and-edge flow diagram, live indicator nodes, click-to-navigate edges | E1 |
| **Agent 2** — Terminal Dynamics Dashboard | `TerminalDynamics.razor`: animated phase-plane plot (F vs M), three trajectory presets, `PhasePortrait` or `TimeSeriesAnimator` in phase-plane mode | E2 |
| **Agent 3** — Full Timeline Dashboard | `TimelineDashboard.razor`: 7 vertically stacked `LineChart` instances, shared time axis, master slider, 20-year ODE integration | E3 |
| **Agent 4** — Responsive Layout | `Site.css` updates: breakpoints, `SplitPanel` stacking, nav collapse, chart `viewBox` aspect ratio | F1 |
| **Agent 5** — Section Prose | All 13 section pages + hub: add blockquote excerpts from source document between heading and first chart | F2 |

**File contention analysis:** Agents 1–3 each own one dashboard page. Agent 4 owns `Site.css` exclusively. Agent 5 modifies section pages — but Agents 1–3 are writing *dashboard* pages (different files), so no overlap. Agent 5 does NOT modify dashboard pages.

**Why overlap E and F:** Dashboards and early polish are independent. Responsive CSS (F1) doesn't change page logic. Prose excerpts (F2) are additive (inserting a `<blockquote>` above the first `<SplitPanel>`). Starting F1–F2 here saves an entire wave.

**Validation gate:** Build clean, 115 equation + ~110 BUnit tests pass (225 total), all 3 dashboards functional, responsive at desktop/tablet/mobile, prose visible on section pages.

---

### Wave 6: Final Polish — 3 Agents Parallel

**Precondition:** Wave 5 complete. All pages, composites, and dashboards functional.

| Agent | Work | Phase |
|-------|------|-------|
| **Agent 1** — Accessibility | `aria-label` on all SVG/Canvas elements, tab order review, keyboard focus for play/pause buttons | F3 |
| **Agent 2** — Theming | CSS custom properties in `Site.css` (`:root` light, `[data-theme="dark"]` dark), toggle in `MainLayout.razor`, pass theme colors to chart components | F4 |
| **Agent 3** — Final Validation | Full checklist pass (14 items), cross-browser spot-check, console error audit | F5 |

**File contention analysis:** Agent 1 touches `.razor` files (adding `aria-label`). Agent 2 touches `Site.css` and `MainLayout.razor`. Agent 3 is read-only (validation). No overlap between Agents 1 and 2 — Agent 1 modifies page/chart `.razor` files, Agent 2 modifies `Site.css` and `MainLayout.razor` only.

**Why 3 agents, not 5:** Only 3 tasks remain. The work is small enough that adding agents would create coordination overhead without speedup.

---

### Wave 7: Review — Best Practices, Consistency, Correctness — 5 Agents Parallel

**Precondition:** Wave 6 complete. All features implemented, all polish applied.

This wave is the quality gate. Each agent performs a read-heavy audit, then fixes any issues found. Agents may modify any file to fix issues, but coordinate through the disjoint audit scopes — each agent owns a specific class of issue.

| Agent | Audit Scope | Steps | Fixes Owned |
|-------|-------------|-------|-------------|
| **Agent 1** — Structure + Naming | `.csproj` files, `_Imports.razor`, solution file, all public member names, route strings, file names | G1, G2 | Rename mismatches, fix `.csproj` settings |
| **Agent 2** — Equation Wiring | All 16 content page `@code` blocks — verify every equation call matches design doc | G3 | Fix wrong method calls, add missing equations, remove incorrect ones |
| **Agent 3** — Component API + CSS | All `FStar.UI` component parameter signatures, all `.razor.css` and `Site.css` files | G4, G5 | Normalize parameter types, fix CSS violations |
| **Agent 4** — Code Quality | All page files and component files — magic numbers, dead code, duplicate logic, error handling | G6 | Extract constants, delete dead code, replace duplicate logic with library calls |
| **Agent 5** — Cross-Reference + Final Build | All 3 planning docs vs implemented code, then `dotnet build` + `dotnet test` | G7, G8 | Fix any doc/code mismatches, run final build |

**File contention analysis:** Low risk. Each agent's *fix scope* is disjoint by category:
- Agent 1 fixes naming/structural issues (`.csproj`, file renames)
- Agent 2 fixes `@code` blocks in pages (equation calls only)
- Agent 3 fixes component `.razor` parameter signatures and `.razor.css` files
- Agent 4 fixes page markup and logic (non-equation code)
- Agent 5 is primarily read-only; its build/test step runs last

**If an agent finds an issue that crosses into another agent's scope:** Flag it in its output rather than fixing it. The orchestrator resolves cross-cutting issues after all 5 agents report.

**Validation gate:** All G1–G8 checklists pass. Final build clean (0 errors, 0 warnings). All 225+ tests pass (115 equation + ~110 BUnit). All issues found have been fixed and verified.

---

### Agent Execution Summary

```
Wave 1 ─── 1 agent  ─── Phase A scaffolding + shared models
              │
Wave 2 ─── 5 agents ─── Controls, Layout, LineChart, BarChart, JS+Integration
              │
Wave 3 ─── 5 agents ─── Hub page, HeatMap pages, LineChart pages, BarChart pages, Transfer+Home
              │
Wave 4 ─── 5 agents ─── Animated composites, Divergence, Variance+Creation, Motivation+Growth, Transfer composites
              │
Wave 5 ─── 5 agents ─── Cascade, Terminal, Timeline, Responsive CSS, Section prose
              │
Wave 6 ─── 3 agents ─── Accessibility, Theming, Final validation
              │
Wave 7 ─── 5 agents ─── Structure+Naming, Equation wiring, API+CSS, Code quality, Cross-reference+Build
```

**Total: 7 waves, 29 agent-slots used (of 35 possible), 0 file contention points.**

### File Ownership Matrix

Every file is owned by exactly one agent within each wave. This eliminates merge conflicts entirely.

| Wave | Agent 1 | Agent 2 | Agent 3 | Agent 4 | Agent 5 |
|------|---------|---------|---------|---------|---------|
| **1** | *All scaffolding (incl. BUnit test project)* | — | — | — | — |
| **2** | `ParameterSlider`, `SliderPanel`, `ThresholdIndicator` + `*Tests.cs` | `ChartCard`, `SplitPanel` + `*Tests.cs` | `LineChart` + `LineChartTests.cs` | `BarChart` + `BarChartTests.cs` | `FStar.UI.lib.module.js`, `Integration.cs`, `IntegrationTests.cs` |
| **3** | `TippingPoint.razor`, `NumberLine`, `TornadoDiagram`, `PhasePortrait` + `*Tests.cs`, `TippingPointTests.cs` | `BaseModelPage`, `NegativeForcePage`, `HeatMap`, `WaterfallChart` + `*Tests.cs` | `VariancePage`, `CreationEvalPage`, `MotivationPage` | `OrgPage`, `SovereigntyPage`, `ModelGrowthPage` | `ForceDynPage`, `TacitPage`, `DivergencePage`, `TransferPage`, `Home`, `PageRenderTests.cs` |
| **4** | `TimeSeriesAnimator` + `Tests.cs`, `ForceDynPage`, `TacitPage` | `DivergencePage` | `VariancePage`, `CreationEvalPage`, `EquationWiringTests.cs` | `MotivationPage`, `ModelGrowthPage`, `SliderInteractionTests.cs` | `TransferPage` |
| **5** | `CascadeDashboard` | `TerminalDynamics` | `TimelineDashboard` | `Site.css` | Section pages (prose only) |
| **6** | Chart `.razor` files (aria) | `Site.css`, `MainLayout` | *(read-only)* | — | — |
| **7** | `.csproj`, `_Imports`, renames | Page `@code` (eq calls) | Component params, `.razor.css` | Page markup/logic | *(read-only → build)* |

**Note on Wave 3 → Wave 4 page re-ownership:** Some pages change agent ownership between waves (e.g., `ForceDynamicsPage` is wired by Agent 5 in Wave 3 with standalone sparklines, then modified by Agent 1 in Wave 4 with the ODE simulator composite). This is safe because waves are sequential — no two agents ever touch the same file at the same time.

**Note on Wave 7 fix coordination:** Wave 7 agents audit and fix within their defined scope. Cross-cutting issues are flagged for the orchestrator to resolve after all agents report.

---

## Equation Coverage by Phase

| Phase | Equations Visualized | Running Total |
|-------|---------------------|:-------------:|
| C2 (Hub) | 14, 14a | 2 |
| C5 (Standalones) | 1, 1a, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12b, 17, 18, 19, 20, 22, 23, 24, 24a, 25, 26, 26a, 27, 32 | 27 |
| D2–D5 (Composites) | 4a, 7a, 11, 11a, 11b, 11c, 12, 12a, 13, 15a, 15b, 16, 16a, 21, 28, 29, 30, 31 | 45 |
| E1–E3 (Dashboards) | (All equations via integrated views) | 45 |

**All 45 equations are visualized by the end of Phase D. Dashboards provide integrated cross-equation views.**
