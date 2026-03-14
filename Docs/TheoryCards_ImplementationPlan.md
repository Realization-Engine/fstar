# Theory Cards Implementation Plan

Adds a **Theory** tab to every chart card in the Blazor Visual Harness.
Each chart card becomes a two-tab container: **Chart** | **Theory**.
The Theory tab displays the relevant equations (rendered via KaTeX) and prose context from *The Multiplier and the Mirror*.

---

## Prerequisites (Complete)

- [x] KaTeX 0.16.21 self-hosted (`wwwroot/lib/katex/`) — JS, CSS, 20 WOFF2 fonts
- [x] `window.renderKatex()` global JS interop function in `index.html`
- [x] `KatexBlock.razor` component — wraps a `<span>` and calls `renderKatex` via `OnAfterRenderAsync`
- [x] Proof-of-concept page (`/katex-test`) validates rendering of display + inline LaTeX
- [x] Build passes with 0 warnings / 0 errors
- [x] `ChartCard.razor` extended with `TheoryContent` RenderFragment + tab bar (backward-compatible)
- [x] `ChartCard.razor.css` extended with tab + theory content styles
- [x] 8 ChartCard BUnit tests pass (3 existing + 5 new tab behavior tests)
- [x] 85 total BUnit tests pass

---

## Verified Card Counts (53 total ChartCard instances)

| Page | Cards | Notes |
|------|-------|-------|
| BaseModelPage | 2 | |
| VariancePage | 5 | |
| CreationEvaluationPage | 3 | |
| NegativeForcePage | 3 | |
| ForceDynamicsPage | 4 | |
| TacitKnowledgePage | 2 | |
| TippingPoint | 4 | |
| DivergencePage | 3 | |
| OrganizationalPage | 6 | |
| MotivationPage | 2 | |
| SovereigntyPage | 2 | |
| ModelGrowthPage | 6 | 2 top-level + 4 nested sub-cards (theory on top-level only) |
| TransferPage | 7 | Includes Knowledge Preservation (Eq 29) |
| CascadeDashboard | 0 | Uses raw SVG — needs wrapper or separate approach |
| TerminalDynamics | 3 | |
| TimelineDashboard | 7 | |

**Theory blocks to write**: 45 section page + 10 dashboard = **55 total**
(ModelGrowthPage's 4 nested sub-cards skipped — too small, outer card covers the theory)

---

## Architecture

### Component Changes

#### 1. Extend `ChartCard` with tab support

**File**: `src/FStar.UI/Components/Layout/ChartCard.razor`

Add a `TheoryContent` RenderFragment parameter. When provided, the card renders a tab bar; when absent, it renders exactly as today (backward-compatible).

```razor
<div class="chart-card">
    <div class="chart-card-header">
        <h3 class="chart-card-title">@Title</h3>
        @if (!string.IsNullOrWhiteSpace(Description))
        {
            <p class="chart-card-description">@Description</p>
        }
    </div>
    @if (TheoryContent is not null)
    {
        <div class="chart-card-tabs">
            <button class="tab @(_activeTab == Tab.Chart ? "active" : "")"
                    @onclick="@(() => _activeTab = Tab.Chart)">Chart</button>
            <button class="tab @(_activeTab == Tab.Theory ? "active" : "")"
                    @onclick="@(() => _activeTab = Tab.Theory)">Theory</button>
        </div>
    }
    <div class="chart-card-content">
        @if (_activeTab == Tab.Chart || TheoryContent is null)
        {
            @ChildContent
        }
        else
        {
            <div class="theory-content">
                @TheoryContent
            </div>
        }
    </div>
</div>

@code {
    private enum Tab { Chart, Theory }
    private Tab _activeTab = Tab.Chart;

    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string? Description { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? TheoryContent { get; set; }
}
```

#### 2. `KatexBlock.razor` (already created)

**File**: `src/FStar.UI/Components/Layout/KatexBlock.razor`

Parameters:
- `Latex` (string, required) — the LaTeX expression
- `DisplayMode` (bool, default `true`) — block vs inline rendering

#### 3. Tab + Theory CSS

**File**: `src/FStar.UI/Components/Layout/ChartCard.razor.css` (extend existing)

New rules for `.chart-card-tabs`, `.tab`, `.tab.active`, `.theory-content`.

---

### Theory Content Structure

Each Theory tab contains a consistent structure:

```razor
<TheoryContent>
    <div class="theory-section">
        <h4>Equation</h4>
        <KatexBlock Latex="O = M \times F \quad \text{where} \quad F = \prod_{i} f_i^{w_i} \qquad (1)" />

        <h4>In Plain Language</h4>
        <p>Your productive output equals the LLM's power times the product of your
           capabilities, each raised to its importance for the task at hand.</p>

        <h4>The Mirror</h4>
        <p>You cannot place a question before the mirror that exhibits precision
           you don't possess. The reflection is faithful along the substance channel.</p>

        <h4>Connections</h4>
        <ul>
            <li>Force layers decay at different rates → <strong>Eq (11a-c)</strong></li>
            <li>Variance amplified by M² → <strong>Eq (4)</strong></li>
        </ul>
    </div>
</TheoryContent>
```

---

## Theory Content Mapping

### 41 Chart Cards across 13 Section Pages

Each entry below specifies the chart card Title, the equation(s), and the theory content source from the concept doc.

#### BaseModelPage (2 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 1 | Cobb-Douglas HeatMap | Eq 1, 1a | §Force is Not a Number: Cobb-Douglas multiplicative model; any zero component collapses force; mirror reflects substance channel faithfully |
| 2 | Layer Ordering | Eq 1a | §Layered Structure: surface/middle/deep hierarchy; M_eff varies by layer; surface transfers fully, deep barely at all |

#### VariancePage (5 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 3 | Variance Curve | Eq 4 | §Variance Amplifier: Var(O) = M² × Var(F); divergence engine, not leveler |
| 4 | Output Gap | Eq 5 | §Variance Amplifier: ΔO = M(F_H - F_L); pre-existing 3x gap becomes 9x at M=3 |
| 5 | Market Value Piecewise | Eq 6 | §Barbell Effect: judgment gains premium, middle commoditized, new orchestration tier |
| 6 | Output Gap Divergence | Eq 4, 4a, 5 | §Variance Amplifier: lower bound when M correlates with F; actual divergence worse |
| 7 | Barbell + Variance | Eq 4, 5, 6 | §Barbell Effect + §Variance Amplifier: durability gradient from Eq 1a determines which skills commoditize |

#### CreationEvaluationPage (3 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 8 | Evaluation Bottleneck | Eq 7 | §Creation Becomes Free: creation cost → ε, evaluation unchanged or higher; M_p polishes defects |
| 9 | Reallocation Effect | Eq 7a | §Creation Becomes Free: best creators must become evaluators; organizational paradox |
| 10 | Resource Allocation | Eq 7a, 28 | §Three-Way Resource Competition: τ_create + τ_eval + τ_extract; no slack |

#### NegativeForcePage (3 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 11 | Directed Force Waterfall | Eq 8 | §When Force Goes Negative: additive form permits negative force; some f_i < 0 |
| 12 | Damage HeatMap | Eq 9 | §When Force Goes Negative: D = M × |F_neg| × τ; blast radius scales with M; mirror reflects mistakes with equal fluency |
| 13 | Epistemic Gap HeatMap | Eq 10 | §Epistemic Corruption: Δ_epistemic ∝ M_p/(M_s·F_i); Narcissus metaphor; virtual depth |

#### ForceDynamicsPage (4 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 14 | Surface Layer dF/dt | Eq 11a | §Layered Decay: surface erodes fast under LLM dependence; loss is benign since LLM substitutes fully |
| 15 | Middle Layer dF/dt | Eq 11b | §Layered Decay: critical battleground; γ_m·E·f_mid determines whether judgment compounds or atrophies |
| 16 | Deep Layer dF/dt | Eq 11c | §Layered Decay: barely changes short-run; hardest to rebuild; built through years of direct experience |
| 17 | ODE Simulator | Eq 11, 11a-c | §The Atrophy Problem: full force dynamics; αS = struggle, γEF = deliberate use, βR = passive reliance, σM = de-investment; dancer vs Narcissus |

#### TacitKnowledgePage (2 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 18 | Shared Work Decay | Eq 12b | §Tacit Knowledge: W(t) declines exponentially with M; first increments eliminate most delegable tasks |
| 19 | Knowledge Pipeline | Eq 12, 12a, 13 | §Tacit Knowledge: K_tacit stock = survive attrition + transmission; pipeline breaks when T(t) < δK; won't notice for years |

#### TippingPoint (4 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 20 | Moving Threshold | Eq 14 | §Tipping Point: F* = (βR + σM_abs)/(γE); above = studio mirror, below = Narcissus pool; same object, different function |
| 21 | Parameter Decomposition | Eq 14 | §Tipping Point: sensitivity of F* to each parameter; transfer raises threshold |
| 22 | Phase Portrait | Eq 11, 14 | §Tipping Point: two stable equilibria; dF/dt sign determines trajectory |
| 23 | Hysteresis Funnel | Eq 14a | §Hysteresis: |dF/dt|_decay > |dF/dt|_recovery; F* is a cliff, not a hill; re-learning lacks novelty |

#### DivergencePage (3 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 24 | Cohort Force Ceiling | Eq 32 | §Cohort Discontinuity: F_initial bounded by available struggle; stunted development, not atrophy; step function at cohort boundary |
| 25 | Divergence Trajectories | Eq 15a-b, 16, 16a | §Accelerating Gap: high-F compounds, low-F decays; gap widens *and accelerates*; Matthew Effect |
| 26 | Generational Step-Down | Eq 32, 15a | §Cohort Discontinuity + §Accelerating Gap: each successive cohort enters with lower ceiling; within- and between-cohort divergence |

#### OrganizationalPage (variable cards — treat as aggregate)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 27 | Marginal Return | Eq 17 | §ROI Paradox: ΔO_j = (M-1)F_j; rational allocation concentrates on strongest |
| 28 | Assessment SNR | Eq 18 | §Legibility Crisis: SNR→0 as M_p→∞; presentation collapses assessment signal |
| 29 | Measured Force | Eq 19 | §Goodhart's Trap: gaming scales with M_p; metric fails when it matters most |
| 30 | Org Throughput | Eq 20 | §Decision Bottleneck: min(R_decision, M×R_execution); decision speed becomes binding |
| 31 | Indecision Cost | Eq 21 | §Decision Bottleneck: OC = M×R_exec×Δt; every hour wasted burns M× more |
| 32 | Competitive Advantage | Eq 22 | §Competitive Moats: A = M(F_firm - F_comp); advantage reduces to force density |

#### MotivationPage (2 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 33 | Motivation Decay Curve | Eq 23 | §The Meaning Problem: f_mot decays exponentially with autonomy loss; enters Cobb-Douglas multiplicatively; highest-force most sensitive |
| 34 | Motivation → Force Feedback | Eq 23, 1 | §The Meaning Problem: declining f_mot reduces F, reduces γEF, shifts balance toward atrophy; feedback loop |

#### SovereigntyPage (2 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 35 | National Capability | Eq 24 | §Sovereignty: E[cap] = ΣF_i × M × P(access); dependency on foreign M; mirror metaphor for nations |
| 36 | Sovereign Resilience | Eq 24a | §Sovereignty: ΣF_i ≥ min viable without M; resilience test; category error of expecting AI to close the gap |

#### ModelGrowthPage (2 cards + 4 sub-cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 37 | Exponential M(t) Growth | Eq 25 | §The Multiplier is Growing: M(t) = M_0·e^μt; most dynamics convex in M; problems compound faster as tech improves |
| 38 | M(t) Impact Cascade | Eq 25, 4, 14, 21 | §The Multiplier is Growing: Var scales M², F* rises, OC scales M; data quality spiral may be self-limiting |

#### TransferPage (6 cards)

| # | Card Title | Equations | Theory Source |
|---|-----------|-----------|---------------|
| 39 | Transfer Rate | Eq 26 | §Transfer Function: dM_abs/dt = Ση_i·f_i·τ_extract; mirror is recording, not just reflecting |
| 40 | Transfer Efficiency | Eq 26a | §Transfer Function: η_surface≈1, η_middle≈0.3-0.6, η_deep≈0; deep knowledge resists encoding |
| 41 | Absorption Ceiling | Eq 27 | §Transfer Function: ceiling = M_0 + Ση_i·f_explicit; explicit transfers, tacit doesn't |
| 42 | Time Allocation Trade-off | Eq 28, 7a | §Three-Way Resource Competition: three demands on same scarce people; no slack |
| 43 | Data Quality Spiral | Eq 31, 25 | §Data Quality Spiral: Quality(M_t+1) = g(Quality(M_t), F̄_annotators); strongest argument against exponential M growth |
| 44 | Tipping Point Shift | Eq 30, 14 | §Paradox of Successful Transfer: F*_post > F*_pre; better transfer undermines conditions for maintaining F |

### 3 Dashboard Pages (11 cards — cross-referential)

Dashboard cards are composites of multiple equations. Each gets a Theory tab summarizing the cross-cutting dynamics:

| Page | Cards | Theory |
|------|-------|--------|
| CascadeDashboard | SVG flow (1 card-equivalent) | §The Cascade: 7 reinforcing loops; multiple positive feedback mechanisms, few natural brakes |
| TerminalDynamics | 3 cards (Phase Portrait, F(t), M(t)) | §Terminal Dynamics: virtuous regime, managed decline, collapse spiral; coupled F-M system |
| TimelineDashboard | 7 cards (M, F, layers, K, Var, F*, SNR) | §The Multiplier is Growing: all dynamics evolve as M grows; convexity makes problems compound |

---

## Execution Strategy

### Wave Structure: 3 Waves

#### Wave T1: Infrastructure (1 agent)

**Agent T1-1**: Modify ChartCard + CSS

Files owned:
- `src/FStar.UI/Components/Layout/ChartCard.razor` — add tab bar + TheoryContent parameter
- `src/FStar.UI/Components/Layout/ChartCard.razor.css` — tab styles, theory content styles
- `src/FStarEquations.App.Tests/Components/Layout/ChartCardTests.cs` — add tests for tab behavior:
  - ChartCard without TheoryContent renders no tabs (backward-compatible)
  - ChartCard with TheoryContent renders tab bar with Chart/Theory buttons
  - Clicking Theory tab switches content
  - Clicking Chart tab restores chart content

**Validation gate**: `dotnet build -p:TreatWarningsAsErrors=true && dotnet test`

#### Wave T2: Theory Content — Section Pages (5 parallel agents)

Each agent wires `<TheoryContent>` blocks into its assigned pages, using `KatexBlock` for equations and HTML for prose. Theory text is extracted from the concept doc sections mapped above.

**Agent T2-1**: BaseModelPage + VariancePage + CreationEvaluationPage (10 cards)
- Cards 1-10

**Agent T2-2**: NegativeForcePage + ForceDynamicsPage (7 cards)
- Cards 11-17

**Agent T2-3**: TacitKnowledgePage + TippingPoint + DivergencePage (9 cards)
- Cards 18-26

**Agent T2-4**: OrganizationalPage + MotivationPage + SovereigntyPage (8 cards)
- Cards 27-36

**Agent T2-5**: ModelGrowthPage + TransferPage (8 cards)
- Cards 37-44

**Validation gate**: `dotnet build -p:TreatWarningsAsErrors=true && dotnet test`

#### Wave T3: Dashboards + Final Polish (2 agents)

**Agent T3-1**: Dashboard Theory Tabs
- CascadeDashboard, TerminalDynamics, TimelineDashboard
- Wrap SVG/composite sections with theory-tab-capable containers

**Agent T3-2**: BUnit Tests + Visual Validation
- Write `TheoryCardTests.cs` — tests that verify TheoryContent is present on key pages
- Test KatexBlock renders `<span>` elements (can't verify JS rendering in BUnit)
- Verify tab switching via bUnit markup assertions
- Clean up KatexTest.razor (keep or remove — user's choice)

**Validation gate**: Full build + test + visual spot-check via preview

---

## Files Modified (Summary)

| Category | Files | Count |
|----------|-------|-------|
| Component | `ChartCard.razor`, `ChartCard.razor.css` | 2 |
| Component (new) | `KatexBlock.razor` (already created) | 0 |
| JS interop | `index.html` (already modified) | 0 |
| Static assets | `wwwroot/lib/katex/` (already created) | 0 |
| Section pages | 13 page .razor files | 13 |
| Dashboard pages | 3 page .razor files | 3 |
| Tests | `ChartCardTests.cs` (extend), `TheoryCardTests.cs` (new) | 2 |
| **Total** | | **20** |

---

## CSS Design

### Tab Bar

```css
.chart-card-tabs {
    display: flex;
    border-bottom: 2px solid var(--border-color);
    padding: 0 var(--spacing-md);
}

.tab {
    padding: var(--spacing-sm) var(--spacing-md);
    border: none;
    background: transparent;
    cursor: pointer;
    font-weight: 500;
    color: var(--text-muted);
    border-bottom: 2px solid transparent;
    margin-bottom: -2px;
    transition: color 0.15s, border-color 0.15s;
}

.tab:hover {
    color: var(--text-primary);
}

.tab.active {
    color: var(--accent);
    border-bottom-color: var(--accent);
}
```

### Theory Content

```css
.theory-content {
    padding: var(--spacing-lg);
    line-height: 1.7;
    max-width: 72ch;
}

.theory-content h4 {
    margin-top: var(--spacing-md);
    margin-bottom: var(--spacing-xs);
    color: var(--text-primary);
    font-size: var(--font-size-sm);
    text-transform: uppercase;
    letter-spacing: 0.05em;
}

.theory-content p {
    color: var(--text-secondary);
    margin-bottom: var(--spacing-sm);
}

.theory-content ul {
    padding-left: var(--spacing-lg);
    color: var(--text-secondary);
}
```

---

## Equation Rendering Notes

- KaTeX is self-hosted at `wwwroot/lib/katex/` (276 KB JS + 23 KB CSS + ~200 KB fonts)
- No CDN dependency — works offline
- `KatexBlock` calls `window.renderKatex()` via `OnAfterRenderAsync(firstRender)`
- Re-renders on parameter change via `OnParametersSetAsync`
- Display mode equations are centered block elements
- Inline equations flow within paragraph text
- All `\text{}` commands render correctly (e.g., `\text{Var}`, `\text{absorbed}`)
- Fractions, products, subscripts, superscripts all verified working

---

## Estimated Scale

- **~44 theory blocks** to write across 13 section pages
- **~11 theory blocks** for 3 dashboard pages
- **~55 total theory blocks**
- Average theory block: 1 KatexBlock equation + 2-3 paragraphs of prose + 2-4 connection bullets
- **~8 new BUnit tests** for tab behavior
