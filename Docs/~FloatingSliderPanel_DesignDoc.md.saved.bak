# Floating Slider Panel — Design Document

## Problem

Every `ChartCard` embeds its own `SliderPanel` inside a `SplitPanel` (70/30 split). This creates three issues:

1. **Bulk** — The slider column consumes 30% of horizontal space even when the user is focused on the chart.
2. **Duplication** — Pages like `BaseModelPage` repeat identical sliders (M, forces, weights) across multiple cards. Changing one doesn't change the other.
3. **Context loss** — When a SliderPanel is collapsed, the user loses the ability to adjust parameters without scrolling back to that specific card and expanding it.

## Solution: `FloatingSliderPanel`

A **draggable, floating panel** that materializes when the in-card `SliderPanel` is collapsed. It hovers above the page content, can be repositioned by dragging its title bar, and broadcasts parameter changes to all charts on the page.

### Behavior

| State | What the user sees |
|-------|-------------------|
| SliderPanel **expanded** | Normal inline layout (current behavior). No floating panel. |
| SliderPanel **collapsed** | The `+` toggle remains. A compact floating panel appears at a default position (top-right of the main content area) containing the same sliders. |
| Floating panel **dragged** | User grabs the title bar → panel follows the pointer → drops at new position. Position persists until page navigation. |
| Floating panel **minimized** | Clicking the `−` button shrinks the panel to a small blue pill showing "⚙ {Title}". Click the pill to restore. |

### Architecture

```
┌──────────────────────────────────────────────────┐
│ Page (@code block)                               │
│                                                  │
│   double _m = 1.0;  ← single source of truth    │
│   bool _slidersCollapsed;                        │
│                                                  │
│   ┌─────────────────────────────────────────┐    │
│   │ ChartCard                               │    │
│   │  ┌────────────────────┬────────────┐    │    │
│   │  │ Chart (LineChart)  │ SliderPanel │    │    │
│   │  │                    │ (@bind-     │    │    │
│   │  │                    │ IsCollapsed │    │    │
│   │  │                    │ toggles     │    │    │
│   │  │                    │ floating)   │    │    │
│   │  └────────────────────┴────────────┘    │    │
│   └─────────────────────────────────────────┘    │
│                                                  │
│   ┌─────────────────────────────────────────┐    │
│   │ ChartCard 2 — uses same _m, no sliders  │    │
│   └─────────────────────────────────────────┘    │
│                                                  │
│   ┌───────────────────────────┐                  │
│   │ FloatingSliderPanel       │  position: fixed │
│   │ (@bind-Visible =          │  draggable       │
│   │  _slidersCollapsed)       │  z-index: 1000   │
│   │                           │                  │
│   │  ParameterSlider: M       │                  │
│   │  ParameterSlider: Force   │                  │
│   │  ...                      │                  │
│   └───────────────────────────┘                  │
│                                                  │
└──────────────────────────────────────────────────┘
```

### Component API

```razor
<FloatingSliderPanel @bind-Visible="_slidersCollapsed"
                     Title="Base Model Parameters"
                     DefaultY="100">
    <ParameterSlider Label="M" Min="0.5" Max="5" Step="0.1"
                     Value="@_m" ValueChanged="OnMChanged" />
    <!-- more sliders -->
</FloatingSliderPanel>
```

#### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Visible` | `bool` | `false` | Two-way bound via `@bind-Visible`. Controls visibility. |
| `Title` | `string` | `"Parameters"` | Title bar text. Also shown in the minimized pill. |
| `DefaultX` | `int` | `-1` | Initial left position (px). `-1` = auto-position to top-right (`window.innerWidth - 320`). |
| `DefaultY` | `int` | `80` | Initial top position (px from viewport top). |
| `ChildContent` | `RenderFragment` | — | The `ParameterSlider` instances. |

### Drag Implementation

**Pure Blazor + minimal JS interop** — no third-party libraries.

```
JS interop (FStar.UI.lib.module.js):
  - initDraggable(elementId, titleBarId, defaultX, defaultY)
      Attaches pointerdown/pointermove/pointerup listeners.
      Pointer Events unify mouse and touch — no separate touch handling needed.
  - destroyDraggable(elementId)
      Removes all listeners on dispose.

Blazor side:
  - OnAfterRenderAsync → calls initDraggable once (guarded by _jsInitialized flag)
  - Implements IAsyncDisposable → calls destroyDraggable
  - On minimize → sets _jsInitialized = false so drag re-initializes on restore
```

The JS handler:
1. `pointerdown` on title bar element → records offset from element rect, sets `dragging = true`
2. `pointermove` on `document` → updates `element.style.left/top`, clamped to viewport bounds
3. `pointerup` on `document` → sets `dragging = false`

### CSS

```css
.floating-slider-panel {
    position: fixed;
    z-index: 1000;
    width: 280px;
    max-height: 70vh;
    overflow-y: auto;
    background: var(--bg-card, #fff);
    border: 1px solid var(--border-color, #e0e0e0);
    border-radius: var(--radius, 0.5rem);
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.15);
}

.floating-title-bar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem 0.75rem;
    background: var(--bg-secondary, #f9fafb);
    border-bottom: 1px solid var(--border-color, #e0e0e0);
    cursor: grab;
    user-select: none;
    border-radius: var(--radius) var(--radius) 0 0;
}

.floating-title-bar:active { cursor: grabbing; }

.floating-slider-body {
    padding: 0.25rem 0.75rem 0.5rem;
}

.floating-btn {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 1.1rem;
    line-height: 1;
    color: var(--text-muted, #9ca3af);
    padding: 0.125rem 0.375rem;
    border-radius: 0.25rem;
    transition: background-color 0.15s, color 0.15s;
}

.floating-btn:hover {
    background-color: var(--bg-primary, #f3f4f6);
    color: var(--text-primary, #1f2937);
}

/* Minimized pill — fixed bottom-right */
.floating-slider-pill {
    position: fixed;
    z-index: 1000;
    bottom: 1.5rem;
    right: 1.5rem;
    padding: 0.5rem 1rem;
    background: var(--accent, #2563eb);
    color: white;
    border-radius: 999px;
    cursor: pointer;
    font-size: 0.8rem;
    font-weight: 600;
    box-shadow: 0 4px 16px rgba(37, 99, 235, 0.35);
    user-select: none;
    transition: transform 0.15s, box-shadow 0.15s;
}

.floating-slider-pill:hover {
    transform: scale(1.05);
    box-shadow: 0 6px 20px rgba(37, 99, 235, 0.45);
}
```

### SliderPanel Integration

`SliderPanel` was modified to support two-way binding via `@bind-IsCollapsed`:

```razor
@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool IsCollapsed { get; set; }
    [Parameter] public EventCallback<bool> IsCollapsedChanged { get; set; }

    private async Task ToggleCollapsed()
    {
        IsCollapsed = !IsCollapsed;
        await IsCollapsedChanged.InvokeAsync(IsCollapsed);
    }
}
```

Pages wire it — the same `_slidersCollapsed` bool drives both components:

```razor
<SliderPanel @bind-IsCollapsed="_slidersCollapsed">
    <!-- sliders here -->
</SliderPanel>

<FloatingSliderPanel @bind-Visible="_slidersCollapsed" Title="Parameters">
    <!-- same sliders here, bound to same backing fields -->
</FloatingSliderPanel>
```

### Implementation Plan (5 agents, 2 waves)

#### Wave F1 — Infrastructure ✅ COMPLETE

The FloatingSliderPanel component, JS interop, and SliderPanel upgrade have been built and verified with a working demo on BaseModelPage.

Files created:
- `src/FStar.UI/Components/Controls/FloatingSliderPanel.razor` ✅
- `src/FStar.UI/Components/Controls/FloatingSliderPanel.razor.css` ✅

Files modified:
- `src/FStar.UI/wwwroot/FStar.UI.lib.module.js` — added `initDraggable`, `destroyDraggable` ✅
- `src/FStar.UI/Components/Controls/SliderPanel.razor` — added `@bind-IsCollapsed` support ✅

Demo page wired:
- `src/FStarEquations.App/Pages/BaseModelPage.razor` — duplicate sliders removed from second card, floating panel added ✅

#### Wave F2 — Page Wiring (5 agents)

Each agent wires the FloatingSliderPanel to assigned pages, replacing duplicate SliderPanels with a single shared floating panel per page.

| Agent | Pages | Card Count |
|-------|-------|------------|
| 1 | VariancePage, CreationEvaluationPage | 8 |
| 2 | NegativeForcePage, ForceDynamicsPage, TacitKnowledgePage | 9 |
| 3 | TippingPoint, DivergencePage, MotivationPage, SovereigntyPage | 11 |
| 4 | OrganizationalPage, ModelGrowthPage | 8 |
| 5 | TransferPage, CascadeDashboard, TerminalDynamics, TimelineDashboard + FloatingSliderPanelTests.cs | ~18 + tests |

**Note:** BaseModelPage is already wired (Wave F1 demo). Not assigned to any Wave F2 agent.

**Per-page transformation:**

1. Identify all `ParameterSlider` instances on the page.
2. De-duplicate: merge identical slider parameters across cards into a single set.
3. Keep the first `SplitPanel` + `SliderPanel` as-is (for users who prefer inline). Add `@bind-IsCollapsed="_slidersCollapsed"`.
4. Remove duplicate `SliderPanel` instances from subsequent cards — those cards become full-width (remove the `SplitPanel` wrapper, chart renders directly inside `<ChildContent>`).
5. Add one `FloatingSliderPanel` per page at the bottom of the markup (before `@code`) with the unified slider set, bound via `@bind-Visible="_slidersCollapsed"`.
6. Add `private bool _slidersCollapsed;` to the `@code` block.

**Special cases:**
- Pages with only 1 ChartCard (e.g., SovereigntyPage): no dedup needed, but still add `@bind-IsCollapsed` + floating panel for the "drag anywhere" UX benefit.
- TimelineDashboard/CascadeDashboard: already use a single global SliderPanel at the top — just add `@bind-IsCollapsed` + floating panel.
- Pages where each card has *different* slider sets (e.g., ForceDynamicsPage with surface/middle/deep/ODE): unify all sliders into one floating panel. Cards that had independent SliderPanels lose them; the floating panel contains the superset.

### Validation Gate

```bash
dotnet build -p:TreatWarningsAsErrors=true
dotnet test src/FStarEquationsTests/FStarEquationsTests.csproj --verbosity quiet
dotnet test src/FStarEquations.App.Tests/FStarEquations.App.Tests.csproj --verbosity quiet
```

Expected: all existing tests pass + new FloatingSliderPanel tests pass.
