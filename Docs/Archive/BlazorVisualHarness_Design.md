# Blazor Visual Test Harness — Design Plan

## Overview

A Blazor WebAssembly standalone app that provides interactive visualizations for the 45 equations in *The Multiplier and the Mirror*. The app references the `FStarEquations` class library directly — all computation runs client-side in the browser.

---

## Section-by-Section Visualization Analysis

Each section of the source document is analyzed for: (a) which equations can produce standalone charts, (b) which cross-equation relationships warrant composite visualizations, and (c) what interactive controls (sliders, dropdowns, toggles) make the dynamics tangible.

---

### 1. Force is Not a Number — Eq (1), (1a)

**Standalone: Cobb-Douglas Sensitivity Surface**
- **Chart type:** 3D surface or heatmap
- **Axes:** Two force components (e.g., f_domain, f_judgment) on X and Y; Output O on Z/color
- **Fixed params (sliders):** M, remaining forces, all weights
- **Key insight to visualize:** When any force component approaches zero, output collapses — the multiplicative cliff. A heatmap makes the "zero kills everything" property visceral.

**Standalone: Layer Ordering Bar Chart — Eq (1a)**
- **Chart type:** Horizontal bar chart
- **Bars:** M_eff for surface, middle, deep layers
- **Sliders:** Adjustable M_eff values
- **Validation indicator:** Green/red badge showing whether `ValidateLayerOrdering` returns true
- **Key insight:** The hierarchy M_eff_surface >> M_eff_middle > M_eff_deep ~ 1 is always maintained in the framework.

---

### 2. The Variable Multiplier — Eq (2), (3)

**Standalone: Domain Output Breakdown — Eq (2)**
- **Chart type:** Stacked bar chart
- **Bars:** Each domain d, showing Ms(d) x F(d) contribution
- **Sliders:** Per-domain multiplier and force
- **Key insight:** Total output is dominated by domains where *both* M and F are high.

**Standalone: Skill Value Sensitivity — Eq (3)**
- **Chart type:** Line chart
- **X-axis:** Provider investment Ip (slider-driven)
- **Y-axis:** Market value shift dVs/dIp
- **Parameter:** Average force F-bar (slider)
- **Key insight:** The same provider investment has a larger effect on high-force skills.

---

### 3. The Variance Amplifier — Eq (4), (4a), (5)

**Standalone: Variance Amplification Curve — Eq (4)**
- **Chart type:** Line chart
- **X-axis:** Multiplier M (slider range 1–20)
- **Y-axis:** Var(O) = M^2 * Var(F)
- **Parameter:** Var(F) slider
- **Key insight:** The parabolic scaling — small increases in M produce large variance growth.

**Composite: Output Gap Divergence — Eq (4), (4a), (5)**
- **Chart type:** Dual-axis line chart
- **Lines:** Var(O) (eq 4), absolute gap DeltaO (eq 5), with (4a) shown as a shaded region above the (4) line
- **X-axis:** M (shared slider)
- **Parameters:** F_high, F_low (sliders)
- **Key insight:** (4a) shows the lower bound is *understated* — the true variance is worse.

---

### 4. The Barbell Effect — Eq (6)

**Standalone: Market Value Piecewise Function — Eq (6)**
- **Chart type:** Line chart with three colored segments
- **X-axis:** Force F (continuous range 0–1)
- **Y-axis:** Market value V(F)
- **Sliders:** Threshold_low, Threshold_high, Premium_high, Wage_mid, Floor_low
- **Key insight:** The hollowed middle — a flat plateau that drops off on the left while the right shoots up.

**Composite: Barbell + Variance — Eq (4), (5), (6)**
- **Chart type:** Combined view: market value curve (eq 6) with an overlay showing how M shifts the thresholds or amplifies the premium tier
- **Key insight:** As M grows, the premium tier's advantage grows quadratically while the middle stays flat.

---

### 5. Creation vs Evaluation — Eq (7), (7a)

**Standalone: Evaluation Bottleneck — Eq (7)**
- **Chart type:** Gauge or bar chart
- **Display:** Throughput = Budget / Cost, with sliders for both
- **Key insight:** As M drives creation cost to near-zero, evaluation cost becomes the binding constraint.

**Composite: Resource Allocation Trade-off — Eq (7a) + (28)**
- **Chart type:** Stacked area or pie chart
- **Segments:** Time on creation, evaluation, extraction (eq 28)
- **Slider:** Fraction of high-force time allocated to eval
- **Key insight:** High-force individuals are pulled three ways.

---

### 6. Negative Force and Epistemic Corruption — Eq (8), (9), (10)

**Standalone: Directed Force Composition — Eq (8)**
- **Chart type:** Waterfall chart
- **Bars:** Each weighted force component (positive bars up, negative bars down)
- **Sliders:** Individual force component values and weights
- **Key insight:** Negative components drag the total down additively.

**Standalone: Damage Scaling — Eq (9)**
- **Chart type:** Heatmap
- **Axes:** M on X, |F_neg| on Y, color = Damage D
- **Slider:** Tau (exposure time)
- **Key insight:** Damage scales linearly in both dimensions — a large M with even small negative force is destructive.

**Standalone: Epistemic Gap Surface — Eq (10)**
- **Chart type:** Heatmap or contour plot
- **Axes:** User force F_i on X, substance multiplier M_s on Y, color = epistemic gap
- **Fixed:** M_p (slider)
- **Key insight:** The gap is largest in the bottom-left corner (low F, low M_s) and smallest in the top-right (high F, high M_s). M_p uniformly inflates the gap.

---

### 7. The Atrophy Problem — Eq (11), (11a), (11b), (11c)

**This is the flagship simulation page** (the Tipping Point hub at /tipping-point is the conceptual centerpiece; this ODE simulator is the most technically rich interactive visualization).

**Composite: Force Dynamics ODE Simulator — Eq (11), (11a-c)**
- **Chart type:** Time-series line chart (animated or scrubbed)
- **Lines:** F_total(t), f_surface(t), f_middle(t), f_deep(t) — four lines showing layered decay
- **Sliders for all parameters:** alpha, beta_s, beta_m, beta_d, gamma_m, S(t), E(t), R(t), sigma, M_abs
- **Integration method:** Forward Euler with configurable dt
- **Key insight:** Surface decays fast, deep barely moves, middle is the battleground. Users can see how changing the balance between S (struggle), E (engagement), and R (reliance) flips the trajectory.

**Standalone: Layer Decay Comparison — Eq (11a), (11b), (11c)**
- **Chart type:** Three small multiples (sparklines), each showing one layer's trajectory
- **Shared time axis, independent Y axes**
- **Key insight:** The rates are dramatically different — surface is nearly vertical, deep is nearly flat.

---

### 8. Tacit Knowledge — Eq (12), (12a), (12b), (13)

**Composite: Knowledge Pipeline Simulator — Eq (12), (12a), (12b), (13)**
- **Chart type:** Time-series with threshold indicator
- **Lines:** K_tacit(t), T(t), delta*K(t) threshold line
- **Sliders:** delta, phi, W0, psi, M, F_senior
- **Alert indicator:** Red when T < delta*K (pipeline broken, eq 13)
- **Key insight:** As M grows (slider), shared work W decays exponentially (12b), transmission T drops, and the pipeline break threshold is crossed. Users can see the exact M value where the break occurs.

**Standalone: Shared Work Decay — Eq (12b)**
- **Chart type:** Exponential decay curve
- **X-axis:** M (multiplier)
- **Y-axis:** W (shared work)
- **Sliders:** W0, psi
- **Key insight:** The steepness of the decay — small increases in M from a low base eliminate the most shared work.

---

### 9. The Tipping Point — Eq (14), (14a) — CONCEPTUAL HUB

Equation (14) is the fulcrum of the entire framework. F\* is the threshold force level above which an engineer's trajectory rises and below which it declines — and F\* is not static. Every major dynamic in the paper either raises F\* or erodes the force that F\* measures:

- **Eq (25):** M grows over time → M_absorbed grows → F\* rises
- **Eq (30):** Successful F→M transfer explicitly raises F\*
- **Eq (23):** Motivation decay reduces E (engagement) → F\* rises (smaller denominator)
- **Eq (12b):** Shared work decay increases reliance R → F\* rises (larger numerator)
- **Eq (32):** Post-LLM cohorts enter below F\* by construction

This page is the conceptual hub of the harness. It gets its own primary navigation entry, separate from the Force Dynamics ODE simulator.

**Primary Visualization: The Moving Threshold — Eq (14) + population**
- **Chart type:** Number line / distribution overlay (animated)
- **Shows:** A population of engineers plotted as dots along a force axis, with F\* as a vertical dividing line
- **Color:** Dots above F\* are green (growth trajectory). Dots below are red (decay trajectory).
- **Animation:** As the user adjusts sliders (M_absorbed, R, sigma, gamma, E), F\* slides along the axis. Engineers whose dots are swept past the threshold flip from green to red in real time.
- **Population model:** Configurable distribution (normal or bimodal) with mean and spread sliders
- **Key insight:** The threshold moves *through* the population. Engineers don't get weaker — the bar gets higher. This is the visualization that makes a room go quiet.

**Secondary: What Moves F\* — Parameter Decomposition**
- **Chart type:** Horizontal stacked bar (tornado diagram)
- **Bars:** Each parameter's contribution to F\* — numerator terms (beta\*R, sigma\*M_abs) push it right, denominator terms (gamma\*E) push it left
- **Sliders:** All six parameters independently
- **Key insight:** Users see *which lever* has the most influence on F\* and in which direction.

**Tertiary: Phase Portrait — Eq (11) + (14)**
- **Chart type:** Phase diagram (F on X-axis, dF/dt on Y-axis)
- **Shows:** The curve of dF/dt vs F, with F\* marked as the zero-crossing
- **Shading:** Green region (dF/dt > 0) above F\*, red region (dF/dt < 0) below
- **Sliders:** All parameters from eq (11) and (14)
- **Key insight:** The two basins of attraction are visible as the curve crossing zero. Moving sliders shifts the crossing point.

**Quaternary: Hysteresis Diagram — Eq (14a)**
- **Chart type:** Asymmetric funnel on a number line
- **Display:** |dF/dt|_decay vs |dF/dt|_recovery at equal distances from F\*
- **Shows:** Steep descent arrow on the left, shallow ascent arrow on the right
- **Key insight:** The asymmetry — it's easier to fall than to climb back. F\* is a cliff, not a hill.

**Cross-links from this page:**
- Link to /force-dynamics (ODE simulator showing the trajectories that F\* separates)
- Link to /transfer (eq 30 — how transfer raises F\*)
- Link to /model-growth (eq 25 — how M growth raises F\*)
- Link to /divergence (eq 15a/15b — the trajectories above and below F\*)
- Link to /motivation (eq 23 — how motivation decay raises F\*)

---

### 10. The Accelerating Gap — Eq (15a), (15b), (16), (16a)

**Composite: Divergence Trajectories — Eq (15a), (15b), (16)**
- **Chart type:** Time-series with two diverging lines
- **Lines:** F_H(t) (compounding upward), F_L(t) (decaying toward floor)
- **Shaded area:** The gap F_H - F_L, widening over time
- **Sliders:** alpha, S0, gamma, beta, M, kappa, initial F_H and F_L
- **Key insight:** The gap doesn't just widen — it *accelerates* (16a). The shaded area's growth rate itself increases visually.

---

### 11. Cohort Discontinuity — Eq (32)

**Standalone: Cohort Force Ceiling — Eq (32)**
- **Chart type:** Line chart
- **X-axis:** Cohort year c (or S_available as proxy)
- **Y-axis:** F_initial
- **Sliders:** F_max, S_pre, rho
- **Key insight:** Each successive cohort enters with a lower force ceiling as S_available shrinks.

**Composite: Generational Step-Down — Eq (32) + (15a/15b)**
- **Chart type:** Multiple trajectory lines, each starting at a different F_initial (by cohort)
- **Shows:** Pre-LLM cohort trajectory vs post-LLM cohort trajectories, all on the same time axis
- **Key insight:** Even with the same dynamics, later cohorts cannot reach the heights of earlier ones.

---

### 12. Organizational Consequences — Eq (17), (18), (19), (20), (21), (22)

**Standalone: ROI by Force Level — Eq (17)**
- **Chart type:** Bar chart
- **Bars:** Multiple engineers at different force levels, showing marginal return (M-1)*Fj
- **Slider:** M
- **Key insight:** The return on giving AI tools is wildly unequal across force levels.

**Standalone: Assessment SNR Collapse — Eq (18)**
- **Chart type:** Line chart
- **X-axis:** M_p (presentation multiplier, log scale)
- **Y-axis:** SNR
- **Sliders:** Var(F_true), Var(epsilon), Var(eta)
- **Key insight:** SNR → 0 as M_p grows. The curve's approach to zero is the legibility crisis.

**Standalone: Goodhart Gaming — Eq (19)**
- **Chart type:** Side-by-side bar chart
- **Bars:** F_true vs F_measured for multiple individuals
- **Slider:** delta_gaming (which increases with M_p)
- **Key insight:** As M_p grows, measured force diverges from true force for everyone, but the gap varies.

**Standalone: Decision Bottleneck — Eq (20), (21)**
- **Chart type:** Line chart with constraint crossing
- **Lines:** R_decision (horizontal), M * R_execution (rising with M)
- **X-axis:** M
- **Y-axis:** Throughput
- **Annotation:** The crossing point where decision speed becomes binding
- **Second panel:** Opportunity cost (eq 21) rising linearly with M
- **Key insight:** As M grows, the cost of indecision explodes.

**Standalone: Competitive Advantage — Eq (22)**
- **Chart type:** Bar chart or gauge
- **Shows:** A = M * (F_firm - F_comp)
- **Sliders:** M, F_firm, F_comp
- **Key insight:** The advantage is entirely determined by force differential, amplified by M.

---

### 13. The Meaning Problem — Eq (23)

**Standalone: Motivation Decay Curve — Eq (23)**
- **Chart type:** Exponential decay curve
- **X-axis:** Accumulated autonomy loss A(t)
- **Y-axis:** f_mot(t)
- **Sliders:** f_mot(0), lambda
- **Key insight:** Motivation decays exponentially. The curve's steepness (lambda) determines how quickly craft satisfaction erodes.

**Composite: Motivation → Force Feedback — Eq (23) + (1)**
- **Chart type:** Dual-panel: motivation decay on left, total output O on right
- **Shows:** How declining f_mot enters the Cobb-Douglas product and drags down total F multiplicatively
- **Key insight:** Motivation doesn't subtract linearly — it *multiplies* through the product, pulling down everything.

---

### 14. Sovereignty — Eq (24), (24a)

**Standalone: National Capability Under Access Risk — Eq (24)**
- **Chart type:** Bar chart with risk discount
- **Bars:** Sum of F_i * M at full access, shaded to show P(access) discount
- **Slider:** P(access) from 0 to 1
- **Key insight:** The dramatic drop when P(access) falls below 1.

**Standalone: Sovereign Resilience Test — Eq (24a)**
- **Chart type:** Horizontal gauge or threshold bar
- **Shows:** Sum(F_i) vs minimum viable capability
- **Green/red indicator:** Pass/fail on sovereign resilience
- **Key insight:** Can the nation function without the multiplier?

---

### 15. Model Growth — Eq (25)

**Standalone: Exponential Multiplier Growth — Eq (25)**
- **Chart type:** Line chart (exponential)
- **X-axis:** Time t
- **Y-axis:** M(t)
- **Sliders:** M0, mu
- **Key insight:** The compounding nature of M — small mu produces dramatic long-term growth.

**Composite: M(t) Impact Cascade — Eq (25) + (4) + (14) + (21)**
- **Chart type:** Small multiples dashboard — 4 panels sharing the same time axis
- **Panel 1:** M(t) growth (eq 25)
- **Panel 2:** Var(O) = M(t)^2 * Var(F) (eq 4) — quadratic explosion
- **Panel 3:** F* = (beta*R + sigma*M_abs(t)) / (gamma*E) (eq 14) — tipping point rising
- **Panel 4:** OC = M(t) * R_exec * delta_t (eq 21) — indecision cost rising
- **Key insight:** A single growing M cascades through the entire framework, making every problem worse convexly.

---

### 16. F→M Transfer — Eq (26), (26a), (27), (28), (29), (30), (31)

**Standalone: Transfer Rate by Layer — Eq (26), (26a)**
- **Chart type:** Stacked bar chart
- **Bars:** Three layers (surface, middle, deep), each showing eta_i * f_i_high
- **Slider:** tau_extract
- **Key insight:** Surface dominates transfer. Deep barely registers.

**Standalone: Absorption Ceiling — Eq (27)**
- **Chart type:** Horizontal bar with ceiling marker
- **Shows:** Current M_absorbed approaching the ceiling M0 + sum(eta_i * f_explicit)
- **Key insight:** There's a hard limit — tacit knowledge doesn't transfer.

**Composite: Time Allocation Trade-off — Eq (28) + (7a)**
- **Chart type:** Ternary plot or stacked bar
- **Three segments:** Create, Evaluate, Extract
- **Slider:** Total available time, fraction to each
- **Key insight:** Every hour on one is an hour off the others.

**Composite: Data Quality Spiral — Eq (31) + (25)**
- **Chart type:** Time-series with feedback
- **Lines:** Model quality Q(t), average annotator force F-bar(t), M(t)
- **Shows:** When F-bar drops, model quality improvement stalls or reverses, which stalls M growth
- **Key insight:** The spiral — the most critical composite visualization after the ODE simulator.

**Standalone: Tipping Point Shift — Eq (30) + (14)**
- **Chart type:** Before/after comparison
- **Shows:** F* pre-transfer vs F* post-transfer on a number line, with a population distribution overlay
- **Key insight:** Successful transfer *raises* F*, causing engineers who were above it to fall below.

---

## Cross-Cutting Composite Dashboards

These are the "big picture" views that span multiple sections.

### Dashboard 1: The Cascade (7 Feedback Loops)

Reproduces the mermaid diagram from the document as an interactive flow:
- Each node is a live indicator (current value based on sliders)
- Each edge shows the equation number and current flow direction/magnitude
- Clicking a node opens its standalone chart
- **Equations involved:** (1), (4), (6), (7), (10), (11), (12), (14), (23), (26), (31), (32)

### Dashboard 2: Terminal Dynamics Phase Space

- **Chart type:** Animated phase-plane plot
- **Axes:** F(t) vs M(t)
- **Trajectories:** Virtuous, managed decline, collapse — three colored paths
- **Sliders:** All key parameters — the user can explore which parameter regimes lead to which trajectory
- **Equations involved:** (11), (25), (31)

### Dashboard 3: The Full Timeline

- **Chart type:** Multi-panel time-series (vertically stacked, shared X-axis = time)
- **Panels:** M(t), F_total(t), f_layers(t), K_tacit(t), Var(O)(t), F*(t), SNR(t)
- **Shows:** Everything evolving together over a 20-year horizon
- **Equations involved:** All ODE equations integrated simultaneously

---

## Visualization Technology Choices

**No third-party charting or UI component libraries.** All visualizations are custom-built.

| Component | Choice | Rationale |
|-----------|--------|-----------|
| **Charting** | Custom SVG/Canvas via JS interop, built as Blazor components in `FStar.UI` Razor class library | Full control over rendering, animation, and interaction. No dependency bloat. SVG for static/interactive charts, Canvas for animated simulations. |
| **Sliders/controls** | Native HTML `<input type="range">` with Blazor `@bind` and `@oninput` | Zero dependencies. Smooth real-time binding via `@oninput` for immediate feedback without full re-render. |
| **Layout** | Native CSS Grid/Flexbox in `Site.css` (app-level) | Single CSS file for the Blazor app. Component-scoped styles live in the Razor library via CSS isolation (`FStar.UI`). |
| **ODE integration** | Forward Euler in C# (in `FStarEquations` library, new `Integration` class) | Simple, transparent, fast enough for WASM real-time |

### Project Architecture

```
fstar/
├── FStarEquations.slnx
├── src/
│   ├── FStarEquations/                    # Equation library (existing)
│   ├── FStarEquationsTests/               # XUnit tests (existing)
│   ├── FStar.UI/                          # Blazor Razor class library (NEW)
│   │   ├── FStar.UI.csproj               # net10.0, Sdk: Microsoft.NET.Sdk.Razor
│   │   ├── Components/
│   │   │   ├── Charts/
│   │   │   │   ├── LineChart.razor        # SVG-based line chart component
│   │   │   │   ├── HeatMap.razor          # SVG/Canvas heatmap component
│   │   │   │   ├── BarChart.razor         # SVG bar chart component
│   │   │   │   ├── WaterfallChart.razor   # SVG waterfall chart component
│   │   │   │   ├── PhasePortrait.razor    # SVG phase diagram component
│   │   │   │   ├── NumberLine.razor       # SVG number line with markers
│   │   │   │   ├── TornadoDiagram.razor   # SVG tornado/butterfly chart
│   │   │   │   └── TimeSeriesAnimator.razor  # Canvas-based animated time-series
│   │   │   ├── Controls/
│   │   │   │   ├── ParameterSlider.razor  # Labeled slider with value display
│   │   │   │   ├── SliderPanel.razor      # Group of related sliders
│   │   │   │   └── ThresholdIndicator.razor  # Green/red pass/fail badge
│   │   │   └── Layout/
│   │   │       ├── ChartCard.razor        # Card wrapper with title and description
│   │   │       └── SplitPanel.razor       # Chart + controls side-by-side layout
│   │   ├── wwwroot/
│   │   │   └── FStar.UI.lib.module.js     # JS interop for Canvas rendering
│   │   │   # Note: CSS isolation files (*.razor.css) live alongside their .razor files, not here
│   │   └── _Imports.razor
│   └── FStarEquations.App/               # Blazor WASM standalone app (NEW)
│       ├── FStarEquations.App.csproj      # net10.0, references FStar.UI + FStarEquations
│       ├── Program.cs
│       ├── App.razor
│       ├── Pages/
│       │   ├── Home.razor
│       │   ├── TippingPoint.razor         # ★ Conceptual hub
│       │   ├── BaseModelPage.razor
│       │   ├── VariancePage.razor
│       │   ├── CreationEvaluationPage.razor
│       │   ├── NegativeForcePage.razor
│       │   ├── ForceDynamicsPage.razor
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
│               └── Site.css               # ALL app-level CSS lives here
```

### CSS Strategy

| Scope | Location | Mechanism |
|-------|----------|-----------|
| **App-level** (layout, page grid, nav, typography, theming) | `FStarEquations.App/wwwroot/css/Site.css` | Single file, loaded in `index.html` |
| **Component-level** (chart internals, slider styling, card borders) | `FStar.UI/Components/**/*.razor.css` | Blazor CSS isolation — automatically scoped, bundled into `FStar.UI.bundle.scp.css` |

No CSS-in-JS. No third-party CSS frameworks. Clean separation: the app controls *where* things go, the Razor library controls *how* things look.

---

## Page Structure (Proposed)

```
/ (Home)
├── /tipping-point  ★              Eq (14), (14a) — CONCEPTUAL HUB (primary nav)
├── /base-model                    Eq (1), (1a), (2), (3)
├── /variance                      Eq (4), (4a), (5), (6)
├── /creation-evaluation           Eq (7), (7a)
├── /negative-force                Eq (8), (9), (10)
├── /force-dynamics                Eq (11), (11a-c) — ODE simulator (flagship)
├── /tacit-knowledge               Eq (12), (12a), (12b), (13)
├── /divergence                    Eq (15a), (15b), (16), (16a), (32)
├── /organizational                Eq (17), (18), (19), (20), (21), (22)
├── /motivation                    Eq (23)
├── /sovereignty                   Eq (24), (24a)
├── /model-growth                  Eq (25)
├── /transfer                      Eq (26), (26a), (27), (28), (29), (30), (31)
├── /cascade                       Dashboard 1: Feedback loops
├── /terminal-dynamics             Dashboard 2: Phase space
└── /timeline                      Dashboard 3: Full simulation
```

16 pages total: 13 section pages (tipping point elevated to its own page) + 3 cross-cutting dashboards.

---

## Equation → Visualization Summary

| Eq. | Standalone Chart | Part of Composite | Chart Type |
|-----|:---:|:---:|------------|
| (1) | Y | Cascade, Timeline | Heatmap / 3D surface |
| (1a) | Y | — | Bar chart with validation badge |
| (2) | Y | — | Stacked bar |
| (3) | Y | — | Line chart |
| (4) | Y | Variance composite, M(t) cascade | Line chart (parabolic) |
| (4a) | — | Variance composite | Shaded region above (4) |
| (5) | — | Variance composite | Line chart |
| (6) | Y | Barbell + variance | Piecewise line chart |
| (7) | Y | Resource allocation | Gauge / bar |
| (7a) | — | Resource allocation, time trade-off | Pie / stacked area |
| (8) | Y | — | Waterfall chart |
| (9) | Y | — | Heatmap |
| (10) | Y | Cascade | Heatmap / contour |
| (11) | — | ODE simulator, Cascade, Timeline, Terminal | Time-series (animated) |
| (11a) | — | ODE simulator, layer comparison | Sparkline |
| (11b) | — | ODE simulator, layer comparison | Sparkline |
| (11c) | — | ODE simulator, layer comparison | Sparkline |
| (12) | — | Knowledge pipeline simulator | Time-series |
| (12a) | — | Knowledge pipeline simulator | (component of T) |
| (12b) | Y | Knowledge pipeline simulator | Exponential decay |
| (13) | — | Knowledge pipeline simulator | Threshold indicator |
| (14) | Y | Phase portrait, M(t) cascade, Timeline, **Hub page** | **Animated population threshold + tornado diagram + phase diagram** |
| (14a) | Y | Hub page phase portrait | Asymmetric funnel diagram |
| (15a) | — | Divergence trajectories, generational | Time-series |
| (15b) | — | Divergence trajectories, generational | Time-series |
| (16) | — | Divergence trajectories | Shaded gap area |
| (16a) | — | Divergence trajectories | (acceleration shown visually) |
| (17) | Y | — | Bar chart |
| (18) | Y | Timeline | Line chart (SNR collapse) |
| (19) | Y | — | Side-by-side bars |
| (20) | Y | M(t) cascade | Line chart with crossing |
| (21) | — | Decision bottleneck, M(t) cascade | Second panel |
| (22) | Y | — | Bar / gauge |
| (23) | Y | Motivation→Force composite, Cascade | Exponential decay |
| (24) | Y | — | Bar with discount |
| (24a) | Y | — | Threshold gauge |
| (25) | Y | M(t) cascade, Terminal, Timeline | Exponential growth |
| (26) | Y | Transfer composite | Stacked bar |
| (26a) | — | Transfer composite | (validation of eta ranges) |
| (27) | Y | — | Ceiling bar |
| (28) | — | Time allocation trade-off | Ternary / stacked |
| (29) | — | — | (conceptual — K_tacit != K_model) |
| (30) | Y | Tipping point shift | Before/after number line |
| (31) | — | Data quality spiral, Terminal, Timeline | Time-series with feedback |
| (32) | Y | Generational step-down | Line chart |

**Standalone charts: ~25.  Composite visualizations: ~12.  Dashboards: 3.  Hub page (Eq 14): 4 visualizations.**

---

## Implementation Phases (Future)

### Phase A: Scaffolding
- Create `FStar.UI` Razor class library project (`dotnet new razorclasslib`)
- Create `FStarEquations.App` Blazor WASM standalone project (`dotnet new blazorwasm`)
- Add both to `FStarEquations.slnx`
- Wire project references: App → FStar.UI (chart components); App → FStarEquations (equation methods). FStar.UI does NOT reference FStarEquations — it is a generic component library.
- Set up `MainLayout.razor`, `NavMenu.razor`, routing, `Site.css`
- Create empty page stubs for all 16 routes

### Phase B: Core Chart Components (FStar.UI)
- Build `LineChart.razor` (SVG) — the most reused component
- Build `BarChart.razor` (SVG)
- Build `ParameterSlider.razor` with `@oninput` real-time binding
- Build `ChartCard.razor` and `SplitPanel.razor` layout wrappers
- Build `ThresholdIndicator.razor`
- Write `FStar.UI.lib.module.js` for Canvas interop scaffolding

### Phase C: Hub Page + Standalone Charts
- Build the Tipping Point hub page first (/tipping-point) — `NumberLine.razor`, `TornadoDiagram.razor`, `PhasePortrait.razor`
- Then standalone charts for each section page, one chart type at a time:
  - `HeatMap.razor` (for eq 1, 9, 10)
  - `WaterfallChart.razor` (for eq 8)
  - Piecewise line chart variant (for eq 6)

### Phase D: Composite Visualizations
- `TimeSeriesAnimator.razor` (Canvas-based) for the ODE simulator (eq 11) and knowledge pipeline (eq 12)
- Add `Integration` class (Forward Euler) to `FStarEquations` library
- Wire up multi-equation composites: divergence trajectories, data quality spiral, M(t) cascade

### Phase E: Dashboards
- Build Cascade dashboard (interactive flow diagram, SVG)
- Build Terminal Dynamics dashboard (animated phase-plane, Canvas)
- Build Full Timeline dashboard (multi-panel stacked time-series)

### Phase F: Polish
- Responsive layout in `Site.css`
- Section-prose integration: each page includes relevant excerpt from the source document
- Keyboard accessibility for sliders
- Dark/light theme toggle (CSS custom properties in `Site.css`)
