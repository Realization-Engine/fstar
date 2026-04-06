# How to Read the Equations

*A reading guide and symbol glossary for* **The Multiplier, Mirror and The Tipping Point**

---

## Why This Guide Exists

The equations in *The Multiplier and the Mirror* use notation drawn from calculus, statistics, and production economics. If you last encountered these symbols in a college course you have since forgotten, or if you never took that course at all, the notation can feel like a wall between you and the argument.

The wall is thinner than it looks. The paper uses approximately 35 distinct symbols and conventions. Most map to concepts you already reason about in everyday life: rates of change, averages, products, thresholds. This guide gives you the vocabulary to read every equation in the paper without needing to recall (or learn) the underlying mathematics from scratch.

The Appendix of the main paper provides "In plain language" glosses for each equation. This guide complements those glosses by covering the *symbols themselves*, the recurring building blocks that appear across many equations, so that you can read any equation on sight rather than looking up each one individually.

---

## 1. The Symbol Vocabulary

Mathematical notation is a language. Like any language, fluency comes from vocabulary. The vocabulary here is small: roughly 35 symbols, most of which map to something you already understand.

### 1.1 The Framework's Own Variables

These are the paper's named quantities. They are not standard mathematical symbols; they are defined by the framework.

| Symbol | Reads As | You Already Know It As |
|--------|----------|------------------------|
| $O$ | "output" | Value-weighted productive work; business value actually delivered |
| $M$ | "the multiplier" | How much more productive an engineer becomes with the LLM |
| $\mathbf{M}_{\text{mirror}}$ | "Mirror" (bold, subscripted) | The full structured LLM-mediated reflective system; not a single number |
| $M_s(d)$ | "substance multiplier for domain d" | How much the LLM amplifies *real* capability in a specific area |
| $M_p$ | "presentation multiplier" | How polished and professional the LLM makes output *look*, regardless of substance |
| $F$ | "Force" | Composite human capability; the thing the multiplier acts on |
| $F^*$ | "F-star" or "the tipping point" | The threshold level of Force where growth and decay exactly balance |
| $f_i$ | "f sub i" or "the i-th component" | A single capability component (domain expertise, judgment, taste, etc.) |
| $w_i$ | "w sub i" or "weight i" | How much component $i$ matters for the task at hand |
| $K_{\text{tacit}}$ | "tacit knowledge stock" | The organization's accumulated deep knowledge |
| $T(t)$ | "transmission at time t" | How much tacit knowledge is passed from seniors to juniors in a period |
| $W(t)$ | "shared work at time t" | Volume of work seniors and juniors do together |
| $S(t)$ | "struggle at time t" | Productive, effortful problem-solving that builds Force |
| $E(t)$ | "engagement at time t" | Deliberate, active use of the LLM as a thinking partner |
| $R(t)$ | "reliance at time t" | Passive reliance on the LLM; accepting output without evaluation |
| $D$ | "damage" | Total destructive output from a negative-Force individual |

### 1.2 Greek Letters

Greek letters are used as *coefficients* (explained in Section 2 below) and *parameters*: fixed or slowly changing numbers that control how fast things grow, shrink, or transfer. Think of them as the "settings" or "dials" on the system.

| Symbol | Name | Role in the Framework | You Already Know It As |
|--------|------|----------------------|------------------------|
| $\alpha$ | alpha | Learning rate from struggle | How fast you learn from hard work |
| $\beta$ | beta | Decay rate from passive reliance | How fast skills rust when you stop using them |
| $\gamma$ | gamma | Growth rate from deliberate LLM engagement | How fast you improve by actively sparring with the tool |
| $\sigma$ | sigma | Rate of organizational de-investment in human capability | How fast the company stops investing in people once the model "handles it" |
| $\delta$ | delta | Natural knowledge decay fraction (retirements, turnover) | Annual attrition of institutional knowledge |
| $\phi$ | phi | Knowledge transmission efficiency | How good the organization is at passing knowledge from seniors to juniors |
| $\psi$ | psi | Sensitivity of shared work to the multiplier | How quickly the LLM eliminates the tasks juniors used to learn from |
| $\eta$ | eta | Transfer efficiency (Force into the model) | What fraction of human expertise can be encoded into model weights |
| $\lambda$ | lambda | Sensitivity to autonomy loss | How strongly a person reacts to losing creative control |
| $\mu$ | mu | Multiplier growth rate | How fast the LLM itself is improving over time |
| $\kappa$ | kappa | Proportional decay constant | A floor on how fast Force can decline (prevents nonsensical negatives) |
| $\rho$ | rho | Elasticity exponent for cohort Force ceiling | How sharply reduced struggle lowers the ceiling on what a cohort can achieve |
| $\tau$ | tau | Time (specifically, time allocated or elapsed) | Hours spent; duration |
| $\pi_p$, $\pi_s$ | pi (subscripted) | Projection functions from Mirror | The operation that extracts the presentation or substance channel from Mirror |
| $\varepsilon$ | epsilon | A quantity approaching zero | "Negligibly small"; effectively nothing |

### 1.3 Mathematical Operators and Notation

These are the standard mathematical symbols that appear throughout. Each one has a plain-English reading.

| Symbol | Reads As | You Already Know It As |
|--------|----------|------------------------|
| $\times$ | "times" or "multiplied by" | Multiplication; scaling one quantity by another |
| $\prod$ | "the product of" | Multiply all the listed terms together; like computing a combined score from several ratings |
| $\sum$ | "the sum of" | Add all the listed terms together; a running total |
| $=$ | "equals" | The left side and the right side are the same thing |
| $\neq$ | "is not equal to" | These two things are categorically different |
| $>$ | "is greater than" | The left side is larger |
| $\geq$ | "is greater than or equal to" | The left side is at least as large |
| $\gg$ | "is much greater than" | The left side is not just larger but dramatically larger |
| $<$ | "is less than" | The left side is smaller |
| $\ll$ | "is much less than" | The left side is dramatically smaller |
| $\approx$ | "is approximately equal to" | Close enough for practical purposes |
| $\propto$ | "is proportional to" | When one doubles, the other doubles; they scale together |
| $\to$ | "approaches" or "maps to" | Trends toward a value, or transforms into |
| $\implies$ | "which implies" | If the left side is true, the right side follows |
| $\in$ | "in" or "belonging to" | Membership; "for each person in the workforce" |
| $\mid$ (in context of piecewise) | "if" or "when" | Different rules apply depending on which condition holds |

### 1.4 Calculus Notation

These symbols describe *rates of change*. They are the framework's way of talking about speed and acceleration.

| Symbol | Reads As | You Already Know It As |
|--------|----------|------------------------|
| $\dfrac{dF}{dt}$ | "the rate of change of F over time" | The speedometer: how fast Force is changing right now |
| $\dfrac{d^2F}{dt^2}$ | "the acceleration of F" | How fast the speedometer itself is changing; are things speeding up or slowing down? |
| $\dfrac{\partial V}{\partial I}$ | "the partial derivative of V with respect to I" | If you change *only* I, holding everything else fixed, how much does V change? Like asking "if I turn up just the bass knob, how does the sound change?" |
| $\lim_{t \to \infty}$ | "the limit as t goes to infinity" | The value something settles toward if you wait long enough |
| $\max(\ldots)$ | "whichever is larger" | Pick the bigger of the two options |
| $\min(\ldots)$ | "whichever is smaller" | Pick the smaller; the bottleneck |

### 1.5 Statistical Notation

| Symbol | Reads As | You Already Know It As |
|--------|----------|------------------------|
| $\text{Var}(X)$ | "the variance of X" | How spread out the values are; the width of the distribution |
| $\bar{X}$ | "X-bar" or "the average of X" | The mean; add them up and divide by how many there are |
| $E[X]$ | "the expected value of X" | The average outcome you would see over many trials |
| $P(\text{event})$ | "the probability of the event" | The likelihood, from 0 (impossible) to 1 (certain) |

### 1.6 Exponents, Subscripts, and Superscripts

| Convention | Reads As | Example |
|-----------|----------|---------|
| $X^2$ | "X squared" | $M^2$ means "the multiplier, squared": $M \times M$ |
| $f_i^{w_i}$ | "f-sub-i raised to the power w-sub-i" | The capability component, raised to its weight; controls sensitivity |
| $e^{x}$ | "e to the x" | Exponential growth or decay; the natural "compound interest" function |
| $e^{-x}$ | "e to the negative x" | Exponential *decay*; like radioactive half-life or a depreciating asset |
| $X_{\text{subscript}}$ | "X sub subscript" | Labels which version: $F_H$ is "Force of the high-force individual" |
| $X^{\text{superscript}}$ | "X superscript" | Labels which layer or type: $M_{\text{eff}}^{\text{surface}}$ is "effective M for the surface layer" |
| $|X|$ | "the absolute value of X" | The magnitude without the sign; "how big, ignoring direction" |

### 1.7 Special Conventions in This Paper

| Convention | Meaning |
|-----------|---------|
| $F^*$ (asterisk) | A *critical* or *equilibrium* value; the tipping point where forces balance |
| Bold $\mathbf{M}_{\text{mirror}}$ | A structured object (not a single number); has internal dimensions |
| Piecewise $\begin{cases} \ldots \end{cases}$ | Different formulas apply depending on which condition is true; like an if/else |
| $(t)$ after a variable | This quantity changes over time; $F(t)$ means "Force at time t" |
| $(d)$ after a variable | This quantity varies by domain; $M_s(d)$ means "substance multiplier in domain d" |
| $(c)$ after a variable | This quantity varies by cohort (entry year); $F_{\text{initial}}(c)$ means "initial Force of cohort c" |

---

## 2. What Is a Coefficient?

A *coefficient* is a fixed number that sits in front of a variable and controls how strongly that variable contributes to the result. It is the "dial setting" on a term in an equation.

### 2.1 The Concept

Consider a simple example from everyday life. If you earn \$50 per hour, the equation for your daily earnings is:

$$\text{Earnings} = 50 \times \text{hours worked}$$

The number 50 is a coefficient. It does not change hour to hour. It tells you *how much each hour contributes* to the total. If you got a raise to \$60/hour, the coefficient changes, and every hour now contributes more.

In the framework's equations, the Greek letters $\alpha$, $\beta$, $\gamma$, $\sigma$, $\delta$, $\phi$, $\psi$, $\eta$, $\lambda$, $\mu$, $\kappa$, and $\rho$ all serve as coefficients. They are the "dial settings" that determine how strongly each dynamic operates.

### 2.2 Why Coefficients Matter

Consider Eq. 11, the central dynamics equation:

$$\frac{dF}{dt} = \alpha \cdot S + \gamma \cdot E \cdot F - \beta \cdot R - \sigma \cdot M_{\text{absorbed}}$$

This equation has four terms, each controlled by its own coefficient:

| Term | Coefficient | What the coefficient controls |
|------|------------|------------------------------|
| $\alpha \cdot S$ | $\alpha$ | How much Force you gain per unit of struggle. A high $\alpha$ means struggle is highly effective at building capability. |
| $\gamma \cdot E \cdot F$ | $\gamma$ | How much Force you gain per unit of deliberate LLM engagement, *per unit of existing Force*. A high $\gamma$ means active engagement is a powerful growth channel. |
| $\beta \cdot R$ | $\beta$ | How much Force you lose per unit of passive reliance. A high $\beta$ means passive reliance is corrosive. |
| $\sigma \cdot M_{\text{absorbed}}$ | $\sigma$ | How much Force you lose per unit of model absorption, because the organization de-invests. A high $\sigma$ means the organization is quick to withdraw human investment once the model appears capable. |

The *signs* matter. The first two terms are positive (they add to $dF/dt$, meaning they grow Force). The last two are negative (they subtract from $dF/dt$, meaning they erode Force). The coefficients determine how strong each pressure is, and the balance between them determines whether Force grows or decays.

### 2.3 Coefficients vs. Variables

The distinction is important:

- **Variables** change during the analysis. $F(t)$, $M(t)$, $S(t)$, $R(t)$ all evolve over time. They are the things you are tracking.
- **Coefficients** are treated as fixed for a given context. $\alpha$, $\beta$, $\gamma$ do not change moment to moment. They characterize the *environment*: how effective learning is, how corrosive passive reliance is, how responsive the organization is. They might differ between organizations, between individuals, or between eras, but within a given analysis they are constants.

This is why the tipping point equation (Eq. 14) is so revealing:

$$F^* = \frac{\beta \cdot R + \sigma \cdot M_{\text{absorbed}}}{\gamma \cdot E}$$

The tipping point is entirely determined by the coefficients and the behavioral variables ($R$, $E$, $M_{\text{absorbed}}$). It tells you: given *this* learning environment (these coefficient values) and *this* pattern of use (these behavioral values), here is the threshold. Change the coefficients (build a better learning culture, reduce passive reliance) and the threshold moves.

### 2.4 Coefficients with Subscripts

Some coefficients carry subscripts to indicate that the same type of dial operates differently in different contexts:

| Notation | Meaning |
|----------|---------|
| $\alpha_m$, $\alpha_d$ | Learning-from-struggle rate for the middle layer and deep layer, respectively |
| $\beta_s$, $\beta_m$, $\beta_d$ | Passive-reliance decay rate for the surface, middle, and deep layers |
| $\gamma_m$ | Deliberate-engagement growth rate for the middle layer |
| $\eta_{\text{surface}}$, $\eta_{\text{middle}}$, $\eta_{\text{deep}}$ | Transfer efficiency for each Force layer |

The subscript tells you *where* the dial applies. The decay rate for the surface layer ($\beta_s$) is much larger than the decay rate for the deep layer ($\beta_d$), which is written as $\beta_d \ll \beta_s$: surface skills rust quickly; deep intuition erodes slowly.

---

## 3. How to Read the Equation Types

The paper uses four main equation forms. Once you recognize the form, reading the content becomes straightforward.

### 3.1 Production Functions: "Output equals..."

**Form:** $O = M \times F$

**How to read it:** "Output equals the multiplier times Force." This is the simplest form: one quantity is the product of two others. If either factor doubles, output doubles. If either goes to zero, output goes to zero.

**The Cobb-Douglas extension** (Eq. 1) expands $F$ into a product of components:

$$F = \prod_i f_i^{w_i}$$

Read the $\prod$ as "multiply together": take each component $f_i$, raise it to the power $w_i$, then multiply all the results. The key insight is that this is a *product*, not a sum. In a sum, a zero term is merely absent. In a product, a zero term kills the entire result, just as a single broken link breaks a chain regardless of how strong the other links are.

**Pattern to recognize:** When you see $X = A \times B \times C$, you are looking at a system where all factors must be present. Any factor near zero collapses the whole thing.

### 3.2 Differential Equations: "The rate of change is..."

**Form:** $\dfrac{dF}{dt} = \text{(things that increase F)} - \text{(things that decrease F)}$

**How to read it:** "The speed at which Force is changing equals the sum of all growth pressures minus all decay pressures." Think of it as a bathtub: $dF/dt$ is whether the water level is rising or falling. The positive terms are faucets (inflow). The negative terms are drains (outflow). If faucets exceed drains, the level rises. If drains exceed faucets, it falls. If they exactly balance, the level holds steady, and that steady level is the *equilibrium* ($F^*$).

The critical feature of Eq. 11 is that one of the growth terms, $\gamma \cdot E \cdot F$, contains $F$ itself. This makes it a *compounding* term: the more Force you have, the faster it grows, like compound interest. This is what creates the tipping point. Below a certain level of $F$, the compounding term is too small to overcome the drains. Above it, the compounding term dominates and growth accelerates.

**Second derivatives** ($d^2F/dt^2$, Eq. 16a) tell you whether the rate of change is itself increasing or decreasing. If $dF/dt$ is the speedometer, $d^2F/dt^2$ is whether you are pressing the accelerator or the brake.

**Partial derivatives** ($\partial V / \partial I$, Eq. 3) isolate one input. "If I change *only* the provider's investment, holding everything else constant, how does skill value change?" It is like turning one dial on a mixing board while leaving all others fixed.

### 3.3 Statistical Equations: "The spread is..."

**Form:** $\text{Var}(O) = M^2 \times \text{Var}(F)$

**How to read it:** "The spread in output equals the multiplier squared times the spread in Force." This is a standard statistical property: when you multiply a distribution by a constant, the variance (the spread) scales by the *square* of that constant, not the constant itself.

Why the square? Variance measures squared deviations from the mean. Multiplying every value by $M$ multiplies every deviation by $M$, and squaring a deviation that has been multiplied by $M$ gives $M^2$ times the original squared deviation.

**What this means practically:** If your team has a moderate spread in capability and you introduce a 3x multiplier, the spread in output does not triple. It increases ninefold ($3^2 = 9$). A 5x multiplier produces a twenty-fivefold increase in spread. The multiplier is a divergence engine.

### 3.4 Threshold and Piecewise Equations: "It depends on which side you're on"

**Form:**
$$V(F) \to \begin{cases} \text{Rule A} & \text{if condition 1} \\ \text{Rule B} & \text{if condition 2} \\ \text{Rule C} & \text{if condition 3} \end{cases}$$

**How to read it:** "Different formulas apply depending on which condition is met." This is an if/else structure. The conditions are mutually exclusive; exactly one applies for any given input. In Eq. 6, the three conditions sort people into three labor-market tiers based on their Force level relative to a threshold.

**The tipping point** (Eq. 14) is a special case: a single value $F^*$ that divides all possible trajectories into two basins. Above $F^*$, one set of dynamics dominates (growth compounds). Below $F^*$, the opposite set dominates (decay compounds). The equation itself is derived by finding the balance point where growth and decay exactly cancel, then recognizing that any displacement from that balance point pushes you further away from it rather than back toward it. This is what makes $F^*$ a *tipping* point rather than a *resting* point.

### 3.5 Exponential Functions: "Compound growth or compound decay"

Two exponential forms recur throughout:

**Growth:** $M(t) = M_0 \cdot e^{\mu t}$

Read: "The multiplier at time $t$ equals its starting value times $e$ raised to the growth rate times time." This is compound growth. The value at each moment grows in proportion to its current size, like money earning interest on interest. The larger it gets, the faster it grows in absolute terms. $M_0$ is where it starts; $\mu$ controls how fast.

**Decay:** $f_{\text{mot}}(t) = f_{\text{mot}}(0) \cdot e^{-\lambda \cdot A(t)}$

Read: "Motivation at time $t$ equals initial motivation times $e$ raised to negative-lambda-times-accumulated-autonomy-loss." The negative sign in the exponent makes this shrink over time instead of grow. Like a radioactive substance losing half its mass at regular intervals, the quantity falls toward zero but never quite reaches it. The rate of fall ($\lambda$) and the accumulated exposure ($A(t)$) together determine how much is left.

**The key intuition:** $e^{+\text{something}}$ curves upward, growing faster and faster. $e^{-\text{something}}$ curves downward, shrinking but never reaching zero. Both appear throughout the framework because the dynamics it describes are compounding: growth compounds, decay compounds, and which one dominates depends on the tipping point.

---

## 4. Workshop

The companion document **[F* Equations Workshop](F%20Star%20Equations%20Workshop.md)** walks through every equation in the framework with concrete numbers. It uses a recurring cast of three engineers (Ada, Ben, Cora) to show how the same equation produces radically different results depending on the inputs. Each of the 32 equations (plus sub-equations) gets a worked numerical example with step-by-step arithmetic, comparison scenarios, and plain-language interpretation of the results.

Start there once you are comfortable with the symbol vocabulary (Sections 1-3 above).


---

## 5. Equation Dependency Map

The equations build on each other. This map shows which ones you need to understand before moving to the next.

**Layer 1: Foundations** (read these first)

- **Eq. 1** (Base model: $O = M \times F$) establishes the core relationship
- **Eq. 1a** (Substitution by layer) explains why the LLM helps differently at different depths

**Layer 2: Structure** (builds on Layer 1)

- **Eq. 2** (Variable multiplier by domain) decomposes $M$ across domains
- **Eq. 8** (Additive Force, negative components) introduces the alternative model where Force can go negative

**Layer 3: Consequences** (builds on Layers 1-2)

- **Eqs. 4, 4a, 5** (Variance amplification) follow directly from Eq. 1
- **Eq. 6** (Barbell effect) follows from Eqs. 4-5 and 1a
- **Eq. 7, 7a** (Evaluation bottleneck) follow from the creation-cost collapse implied by high $M$
- **Eq. 9** (Damage) follows from Eq. 8
- **Eq. 10** (Epistemic gap) introduces the $M_p$ / $M_s$ distinction from Mirror

**Layer 4: Dynamics** (builds on Layers 1-3)

- **Eq. 11** (Force dynamics) is the central engine; depends on Eq. 1 for the meaning of $F$
- **Eqs. 11a-c** (Layered decay) decompose Eq. 11 by Force layer, using Eq. 1a
- **Eqs. 12, 12a, 12b** (Tacit knowledge) build a parallel dynamics model for organizations
- **Eq. 13** (Pipeline break) is the threshold condition from Eq. 12

**Layer 5: The Tipping Point** (builds on Layer 4)

- **Eq. 14** (Tipping point $F^*$) is derived from setting Eq. 11 to zero and solving
- **Eq. 14a** (Hysteresis) modifies Eq. 14 with asymmetric recovery
- **Eqs. 15a, 15b** (Diverging trajectories) follow from Eqs. 11 and 14
- **Eqs. 16, 16a** (Accelerating gap) follow from Eqs. 15a and 15b

**Layer 6: Organizational and Systemic** (builds on Layers 1-5)

- **Eqs. 17-21** (ROI paradox, legibility crisis, Goodhart, decision bottleneck) apply the framework to organizational contexts
- **Eq. 22** (Competitive moats) extends to inter-firm competition
- **Eq. 23** (Motivation decay) feeds back into Eq. 1 and Eq. 11

**Layer 7: Transfer and Sovereignty** (builds on everything)

- **Eqs. 24, 24a** (Sovereignty) extend Eq. 1 to nations
- **Eq. 25** ($M$ growth) makes the multiplier dynamic
- **Eqs. 26-31** (F-to-M transfer) close the loop between $F$ and $M$
- **Eq. 32** (Cohort discontinuity) introduces generational dynamics

---

## 6. Reading Strategies

### Start with the Base Model

Every equation in the paper is a descendant of $O = M \times F$ (Eq. 1). If you understand that one equation, you have the seed from which everything else grows. Each subsequent equation asks: "What happens to this simple relationship when we relax an assumption?"

- "What if $M$ varies by domain?" gives you Eq. 2.
- "What if $F$ changes over time?" gives you Eq. 11.
- "What if $F$ can go negative?" gives you Eq. 8.
- "What if there is a level of $F$ where the dynamics flip?" gives you Eq. 14.

### Read the "Captures" Line First

Each equation in the Appendix ends with a "Captures:" summary. Read that line before reading the equation itself. It tells you *what the equation is saying* in one sentence, which orients you before you parse the symbols.

### Follow the Signs

In any equation with plus and minus terms (especially Eq. 11), the *signs* tell you the story. Positive terms are forces pushing upward (growth, building, compounding). Negative terms are forces pushing downward (decay, erosion, loss). The equation is a tug-of-war, and the signs tell you who is pulling in which direction.

### Watch for Multiplication vs. Addition

The paper is precise about when it uses products ($\prod$, $\times$) and when it uses sums ($\sum$, $+$). Products mean "all factors must be present; a zero kills the whole thing." Sums mean "contributions are independent; strength in one area compensates for weakness in another." The structural difference between Eq. 1 (multiplicative, within a domain) and Eq. 2 (additive, across domains) is not a mathematical detail. It is the core of the argument about how Force works.

### Use the Worked Examples as Templates

When an equation confuses you, plug in concrete numbers. The worked examples in Section 4 above are templates. Pick values that are easy to compute with (0.5, 1, 2, 3) and trace through the equation by hand. The abstract symbols become concrete quickly.

---

## 7. Notation Glossary (Quick Reference)

For quick lookup during reading. Sorted alphabetically within categories.

### Roman Letters

| Symbol | Equation(s) | Meaning |
|--------|-------------|---------|
| $A$ | 22 | Competitive advantage |
| $A(t)$ | 23 | Accumulated autonomy loss |
| $C_{\text{evaluate}}$ | 7 | Per-unit evaluation cost |
| $D$ | 9 | Total damage |
| $E$ or $E(t)$ | 11 | Deliberate LLM engagement |
| $F$ | 1, throughout | Composite human capability (Force) |
| $F^*$ | 14 | Tipping point (critical threshold) |
| $F_H$, $F_L$ | 15a, 15b | High-force individual, low-force individual |
| $f_i$ | 1 | Individual capability component |
| $f_{\text{mot}}$ | 23 | Intrinsic motivation component |
| $I_p$ | 3 | Provider investment |
| $K_{\text{tacit}}$ | 12 | Organizational tacit knowledge stock |
| $M$ | 1, throughout | LLM amplification factor (multiplier) |
| $\mathbf{M}_{\text{mirror}}$ | Mirror section | Mirror as structured object |
| $M_p$ | 10, 18, 19 | Presentation projection |
| $M_s(d)$ | 2, 10 | Substance projection for domain $d$ |
| $M_{\text{absorbed}}$ | 11, 14 | Model capability from F-to-M transfer |
| $O$ | 1, 2 | Output (value-weighted productive work) |
| $R$ or $R(t)$ | 11 | Passive reliance on LLM |
| $S$ or $S(t)$ | 11 | Productive struggle |
| $T(t)$ | 12 | Tacit knowledge transmitted per period |
| $V(F)$ | 6 | Market value as function of Force |
| $W(t)$ | 12a, 12b | Shared work between seniors and juniors |
| $w_i$ | 1 | Weight of capability component $i$ |

### Greek Letters

| Symbol | Equation(s) | Meaning |
|--------|-------------|---------|
| $\alpha$ | 11 | Learning rate from struggle |
| $\beta$ | 11 | Decay rate from passive reliance |
| $\gamma$ | 11 | Growth rate from deliberate engagement |
| $\delta$ | 12 | Natural knowledge decay fraction |
| $\varepsilon$ | 6 | Quantity approaching zero |
| $\eta$ | 26 | Transfer efficiency (Force into model) |
| $\kappa$ | 15b | Proportional decay constant |
| $\lambda$ | 23 | Sensitivity to autonomy loss |
| $\mu$ | 25 | Multiplier growth rate |
| $\pi_p$, $\pi_s$ | Mirror section | Projection functions (presentation, substance) |
| $\rho$ | 32 | Elasticity exponent for cohort Force ceiling |
| $\sigma$ | 11 | Rate of organizational de-investment |
| $\phi$ | 12a | Knowledge transmission efficiency |
| $\psi$ | 12b | Sensitivity of shared work to $M$ |
| $\tau$ | 9, 28 | Time (allocated or elapsed) |
