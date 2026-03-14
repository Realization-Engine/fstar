# FStarEquations — TDD Implementation Plan

## Overview

Translate all 45 equations from *The Multiplier and the Mirror* into a .NET 10 C# class library (`FStarEquations`) with full XUnit test coverage (`FStarEquationsTests`). The approach is **test-driven**: all tests are written first against skeleton classes (which throw `NotImplementedException`), confirmed to fail, and then the implementations are filled in to make them pass.

**Future goal (out of scope):** A Blazor visual test harness will consume this library.

---

## Solution Structure

```
fstar/
├── FStarEquations.slnx                          # .NET 10 XML solution file
├── src/
│   ├── FStarEquations/
│   │   ├── FStarEquations.csproj                 # net10.0 class library
│   │   ├── BaseModel.cs                          # Eq (1), (1a), (2), (3)
│   │   ├── VarianceAmplification.cs              # Eq (4), (4a), (5), (6)
│   │   ├── CreationEvaluation.cs                 # Eq (7), (7a)
│   │   ├── NegativeForce.cs                      # Eq (8), (9), (10)
│   │   ├── ForceDynamics.cs                      # Eq (11), (11a), (11b), (11c), (14), (14a)
│   │   ├── TacitKnowledge.cs                     # Eq (12), (12a), (12b), (13)
│   │   ├── DivergenceTrajectories.cs             # Eq (15a), (15b), (16), (16a), (32)
│   │   ├── OrganizationalDynamics.cs             # Eq (17), (18), (19), (20), (21), (22)
│   │   ├── Motivation.cs                         # Eq (23)
│   │   ├── Sovereignty.cs                        # Eq (24), (24a)
│   │   ├── ModelGrowth.cs                        # Eq (25)
│   │   └── ForceToModelTransfer.cs               # Eq (26), (26a), (27), (28), (29), (30), (31)
│   └── FStarEquationsTests/
│       ├── FStarEquationsTests.csproj             # net10.0 xunit project
│       ├── BaseModelTests.cs
│       ├── VarianceAmplificationTests.cs
│       ├── CreationEvaluationTests.cs
│       ├── NegativeForceTests.cs
│       ├── ForceDynamicsTests.cs
│       ├── TacitKnowledgeTests.cs
│       ├── DivergenceTrajectoriesTests.cs
│       ├── OrganizationalDynamicsTests.cs
│       ├── MotivationTests.cs
│       ├── SovereigntyTests.cs
│       ├── ModelGrowthTests.cs
│       └── ForceToModelTransferTests.cs
```

---

## TDD Phases

### Phase 1: Red (Current Task)

1. Create solution (`FStarEquations.slnx`) and both projects.
2. Write **skeleton classes** — all public methods with correct signatures, all bodies `throw new NotImplementedException();`.
3. Write **all XUnit test classes** covering every equation.
4. **Code validation gate** — `dotnet build /p:TreatWarningsAsErrors=true` must pass (compiles clean, zero warnings).
5. Run `dotnet test` — **all tests must fail** (with `NotImplementedException`).

### Phase 2: Green (Next Task)

6. Implement each equation method, running tests incrementally.
7. All tests pass.

### Phase 3: Refactor (Follow-Up)

8. Review for DRY, naming conventions, XML doc comments.
9. Final validation gate.

---

## Code Validation Gates

Every phase must pass these checks before proceeding:

| Check | Command | Criterion |
|-------|---------|-----------|
| **Clean build** | `dotnet build /p:TreatWarningsAsErrors=true` | 0 errors, 0 warnings |
| **Naming conventions** | Manual review | PascalCase for public members, camelCase for locals/params, no abbreviations except well-known (`Snr`, `Dt`) |
| **Nullable reference types** | `<Nullable>enable</Nullable>` in csproj | No nullable warnings |
| **Consistent parameterization** | Review | Every physical quantity is a parameter — no magic numbers in library code |
| **Test naming** | Convention: `MethodName_Scenario_ExpectedResult` | All tests follow this pattern |
| **Test isolation** | Each `[Fact]` or `[Theory]` is independent | No shared mutable state |

---

## Complete Equation-to-Method Mapping

All parameters are fully exposed — nothing is hard-coded in the library.

### BaseModel.cs — Equations (1), (1a), (2), (3)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (1) | `double ComputeOutput(double m, double[] forces, double[] weights)` | O = M × ∏ fᵢ^wᵢ (Cobb-Douglas) |
| (1a) | `bool ValidateLayerOrdering(double mEffSurface, double mEffMiddle, double mEffDeep)` | M_eff_surface ≫ M_eff_middle > M_eff_deep ≈ 1 |
| (2) | `double ComputeOutputAcrossDomains(double[] domainMultipliers, double[] domainForces)` | O = Σ Ms(d) × F(d) |
| (3) | `double SkillValueSensitivity(double dMsDIp, double avgForce)` | ∂Vs/∂Ip = (∂Ms/∂Ip) × F̄s |

### VarianceAmplification.cs — Equations (4), (4a), (5), (6)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (4) | `double OutputVariance(double m, double varianceF)` | Var(O) = M² · Var(F) |
| (4a) | `double OutputVarianceLowerBound(double m, double varianceF)` | Returns M²·Var(F); when M correlates with F, actual > this |
| (5) | `double AbsoluteOutputGap(double m, double forceHigh, double forceLow)` | ΔO = M(F_H − F_L) |
| (6) | `double MarketValue(double force, double thresholdLow, double thresholdHigh, double premiumHigh, double wageMid, double floorLow)` | Piecewise V(F): three tiers |

### CreationEvaluation.cs — Equations (7), (7a)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (7) | `double EvaluationThroughput(double budgetEval, double costEval)` | Throughput = Budget_eval / C_eval |
| (7a) | `double ReallocatedForce(double forceHigh, double fractionToEval)` | F_high redirected to evaluation |

### NegativeForce.cs — Equations (8), (9), (10)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (8) | `double DirectedForce(double[] weights, double[] forces)` | F_dir = Σ wᵢfᵢ, some fᵢ < 0 |
| (9) | `double Damage(double m, double forceNeg, double tau)` | D = M · |F_neg| · τ |
| (10) | `double EpistemicGap(double mPresentation, double mSubstance, double forceIndividual)` | Δ_epistemic ∝ Mp / (Ms · Fi) |

### ForceDynamics.cs — Equations (11), (11a), (11b), (11c), (14), (14a)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (11) | `double DfDt(double alpha, double struggle, double gamma, double engagement, double force, double beta, double replacement, double sigma, double mAbsorbed)` | dF/dt = αS + γEF − βR − σM_abs |
| (11a) | `double DfSurfaceDt(double betaSurface, double replacement)` | df_surf/dt = −β_s R |
| (11b) | `double DfMiddleDt(double alphaM, double struggle, double gammaM, double engagement, double forceMid, double betaM, double replacement)` | df_mid/dt = α_m S + γ_m E f_mid − β_m R |
| (11c) | `double DfDeepDt(double alphaD, double struggle, double betaD, double replacement)` | df_deep/dt = α_d S − β_d R |
| (14) | `double TippingPoint(double beta, double replacement, double sigma, double mAbsorbed, double gamma, double engagement)` | F* = (βR + σM_abs) / (γE) |
| (14a) | `bool IsHysteresisPresent(double dfDtDecay, double dfDtRecovery)` | |dF/dt|_decay > |dF/dt|_recovery |

### TacitKnowledge.cs — Equations (12), (12a), (12b), (13)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (12) | `double KnowledgeStockNext(double kCurrent, double delta, double transmission)` | K(t+1) = K(t)(1−δ) + T(t) |
| (12a) | `double Transmission(double phi, double sharedWork, double forceSenior)` | T = φ · W · F_senior |
| (12b) | `double SharedWork(double w0, double psi, double m)` | W = W₀ · e^(−ψM) |
| (13) | `bool IsPipelineBroken(double transmission, double delta, double kCurrent)` | T(t) < δK(t) |

### DivergenceTrajectories.cs — Equations (15a), (15b), (16), (16a), (32)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (15a) | `double DfHighDt(double alpha, double struggle0, double gamma, double m, double forceHigh)` | dF_H/dt = αS₀ + γMF_H |
| (15b) | `double DfLowDt(double alpha, double struggle0, double beta, double m, double kappa, double forceLow)` | dF_L/dt = max(αS₀ − βM, −κF_L) |
| (16) | `bool IsGapWidening(double dfHighDt, double dfLowDt)` | d(F_H − F_L)/dt > 0 |
| (16a) | `bool IsGapAccelerating(double d2fHighDt2, double d2fLowDt2)` | d²(F_H − F_L)/dt² > 0 |
| (32) | `double InitialForce(double forceMax, double struggleAvailable, double strugglePre, double rho)` | F_init = F_max · (S_avail / S_pre)^ρ |

### OrganizationalDynamics.cs — Equations (17), (18), (19), (20), (21), (22)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (17) | `double MarginalReturn(double m, double forceJ)` | ΔOⱼ = (M−1) · Fⱼ |
| (18) | `double AssessmentSnr(double varForceTrue, double mPresentation, double varEpsilon, double varEta)` | SNR formula; → 0 as Mp → ∞ |
| (19) | `double MeasuredForce(double forceTrue, double deltaGaming)` | F_meas = F_true + δ(Mp) |
| (20) | `double OrganizationalThroughput(double rateDecision, double m, double rateExecution)` | min(R_dec, M · R_exec) |
| (21) | `double IndecisionCost(double m, double rateExecution, double deltaTime)` | OC = M · R_exec · Δt |
| (22) | `double CompetitiveAdvantage(double m, double forceFirm, double forceCompetitor)` | A = M(F_firm − F_comp) |

### Motivation.cs — Equation (23)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (23) | `double MotivationDecay(double fMot0, double lambda, double accumulatedLoss)` | f_mot = f_mot(0) · e^(−λA(t)) |

### Sovereignty.cs — Equations (24), (24a)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (24) | `double NationalCapability(double[] forces, double m, double probabilityAccess)` | E[cap] = Σ Fᵢ · M · P(access) |
| (24a) | `bool IsSovereignResilient(double[] forces, double minViable)` | Σ Fᵢ ≥ min viable |

### ModelGrowth.cs — Equation (25)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (25) | `double MultiplierAtTime(double m0, double mu, double t)` | M(t) = M₀ · e^(μt) |

### ForceToModelTransfer.cs — Equations (26), (26a), (27), (28), (29), (30), (31)

| Eq. | Method Signature | Description |
|-----|-----------------|-------------|
| (26) | `double TransferRate(double[] eta, double[] forcesHigh, double tauExtract)` | dM_abs/dt = Σ ηᵢ · f_{i,high} · τ_ext |
| (26a) | `bool ValidateTransferEfficiencies(double etaSurface, double etaMiddle, double etaDeep)` | η_surf ≈ 1, 0.3 ≤ η_mid ≤ 0.6, η_deep ≈ 0 |
| (27) | `double AbsorptionCeiling(double m0, double[] eta, double[] forcesExplicit)` | lim M_abs = M₀ + Σ ηᵢ · f_{i,explicit} |
| (28) | `double TotalTimeAllocation(double tauCreate, double tauEval, double tauExtract)` | τ = τ_create + τ_eval + τ_extract |
| (29) | `bool IsKnowledgePreserved(double kTacit, double kModel)` | K_tacit ≠ K_model → returns false |
| (30) | `bool HasTippingPointRisen(double fStarPost, double fStarPre)` | F*_post > F*_pre |
| (31) | `double ModelQualityNext(double qualityCurrent, double avgForceAnnotators, Func<double, double, double> qualityFunction)` | Quality(M_{t+1}) = g(Quality(M_t), F̄_ann) |

---

## Test Strategy

### Test Types

| Type | Use For | Example |
|------|---------|---------|
| `[Fact]` | Single deterministic assertion | `ComputeOutput_WithKnownInputs_ReturnsExpectedValue` |
| `[Theory] + [InlineData]` | Same logic, multiple input sets | `OutputVariance_VariousMultipliers_ScalesAsSquare` |
| Property-based assertions | Inequalities and ordering | `IsGapWidening_WhenHighGrowsFaster_ReturnsTrue` |
| Edge cases | Zeros, negatives, extremes | `Damage_WithZeroForce_ReturnsZero` |
| Limit tests | Asymptotic behavior | `AssessmentSnr_AsMpGrowsLarge_ApproachesZero` |

### Naming Convention

```
MethodName_Scenario_ExpectedResult
```

Examples:
- `ComputeOutput_EqualWeights_ReturnsGeometricMean`
- `TippingPoint_HighReplacement_RaisesThreshold`
- `MotivationDecay_ZeroLoss_ReturnsInitialValue`

### Tolerances

Floating-point comparisons use `Assert.Equal(expected, actual, precision: 10)` (10 decimal places) for exact formulas, and `Assert.InRange` or relative tolerance for ODE (Ordinary Differential Equation — an equation that describes how a quantity changes over time, such as `dF/dt = ...`) integration tests.

---

## Parameterization Philosophy

> "Parameterize everything."

- **No magic numbers** in the library. Every coefficient, rate, threshold, and initial condition is a method parameter.
- **Default parameter sets** may live in a separate `Defaults` static class for convenience, but the core methods accept all values explicitly.
- **Tests use hand-calculated values** with clear comments showing the arithmetic.

---

## Equation Coverage Checklist

| Eq. | Class | Method | Tests Written | Tests Pass |
|-----|-------|--------|:---:|:---:|
| (1) | BaseModel | ComputeOutput | [ ] | [ ] |
| (1a) | BaseModel | ValidateLayerOrdering | [ ] | [ ] |
| (2) | BaseModel | ComputeOutputAcrossDomains | [ ] | [ ] |
| (3) | BaseModel | SkillValueSensitivity | [ ] | [ ] |
| (4) | VarianceAmplification | OutputVariance | [ ] | [ ] |
| (4a) | VarianceAmplification | OutputVarianceLowerBound | [ ] | [ ] |
| (5) | VarianceAmplification | AbsoluteOutputGap | [ ] | [ ] |
| (6) | VarianceAmplification | MarketValue | [ ] | [ ] |
| (7) | CreationEvaluation | EvaluationThroughput | [ ] | [ ] |
| (7a) | CreationEvaluation | ReallocatedForce | [ ] | [ ] |
| (8) | NegativeForce | DirectedForce | [ ] | [ ] |
| (9) | NegativeForce | Damage | [ ] | [ ] |
| (10) | NegativeForce | EpistemicGap | [ ] | [ ] |
| (11) | ForceDynamics | DfDt | [ ] | [ ] |
| (11a) | ForceDynamics | DfSurfaceDt | [ ] | [ ] |
| (11b) | ForceDynamics | DfMiddleDt | [ ] | [ ] |
| (11c) | ForceDynamics | DfDeepDt | [ ] | [ ] |
| (12) | TacitKnowledge | KnowledgeStockNext | [ ] | [ ] |
| (12a) | TacitKnowledge | Transmission | [ ] | [ ] |
| (12b) | TacitKnowledge | SharedWork | [ ] | [ ] |
| (13) | TacitKnowledge | IsPipelineBroken | [ ] | [ ] |
| (14) | ForceDynamics | TippingPoint | [ ] | [ ] |
| (14a) | ForceDynamics | IsHysteresisPresent | [ ] | [ ] |
| (15a) | DivergenceTrajectories | DfHighDt | [ ] | [ ] |
| (15b) | DivergenceTrajectories | DfLowDt | [ ] | [ ] |
| (16) | DivergenceTrajectories | IsGapWidening | [ ] | [ ] |
| (16a) | DivergenceTrajectories | IsGapAccelerating | [ ] | [ ] |
| (17) | OrganizationalDynamics | MarginalReturn | [ ] | [ ] |
| (18) | OrganizationalDynamics | AssessmentSnr | [ ] | [ ] |
| (19) | OrganizationalDynamics | MeasuredForce | [ ] | [ ] |
| (20) | OrganizationalDynamics | OrganizationalThroughput | [ ] | [ ] |
| (21) | OrganizationalDynamics | IndecisionCost | [ ] | [ ] |
| (22) | OrganizationalDynamics | CompetitiveAdvantage | [ ] | [ ] |
| (23) | Motivation | MotivationDecay | [ ] | [ ] |
| (24) | Sovereignty | NationalCapability | [ ] | [ ] |
| (24a) | Sovereignty | IsSovereignResilient | [ ] | [ ] |
| (25) | ModelGrowth | MultiplierAtTime | [ ] | [ ] |
| (26) | ForceToModelTransfer | TransferRate | [ ] | [ ] |
| (26a) | ForceToModelTransfer | ValidateTransferEfficiencies | [ ] | [ ] |
| (27) | ForceToModelTransfer | AbsorptionCeiling | [ ] | [ ] |
| (28) | ForceToModelTransfer | TotalTimeAllocation | [ ] | [ ] |
| (29) | ForceToModelTransfer | IsKnowledgePreserved | [ ] | [ ] |
| (30) | ForceToModelTransfer | HasTippingPointRisen | [ ] | [ ] |
| (31) | ForceToModelTransfer | ModelQualityNext | [ ] | [ ] |
| (32) | DivergenceTrajectories | InitialForce | [ ] | [ ] |

**Total: 45 equations, 45 methods, 12 classes, 12 test files.**

---

## Execution Order

```
Step 1:  dotnet new slnx → FStarEquations.slnx
Step 2:  dotnet new classlib → src/FStarEquations/
Step 3:  dotnet new xunit → src/FStarEquationsTests/
Step 4:  dotnet sln add both projects; add project reference
Step 5:  Write all 12 skeleton .cs files (NotImplementedException)
Step 6:  Write all 12 test .cs files
Step 7:  dotnet build /p:TreatWarningsAsErrors=true  → must pass
Step 8:  dotnet test  → all tests fail (RED)
Step 9:  ── Phase 1 complete ──
Step 10: Implement equations one class at a time (Phase 2 — GREEN)
Step 11: Refactor pass (Phase 3)
Step 12: Final validation gate
```
