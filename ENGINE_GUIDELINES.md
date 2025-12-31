# Engine Code Review & Design Guidelines

This project implements a **GameObject + Component architecture** with
**explicit Actions and Events**.  
All code changes should follow the rules below.

---

## 1. Core Concepts (Do Not Mix)

### GameAction

- Represents **intent** (“something is trying to happen”)
- Examples: `Chop`, `Slash`, `Damage`, `UseItem`
- May be ignored, modified, or forwarded
- **Handled by Components**

### GameEvent

- Represents a **fact** (“something already happened”)
- Examples: `DamageTaken`, `HealthDepleted`
- **Must never cause the same state change again**
- **Observed by Components**

### Rule:

> **Actions cause Events. Events never cause Actions.**

---

## 2. Component Responsibilities

### Components:

- Own behavior and reactions
- Handle `GameAction`s via event-style `Handle<T>()` methods in constructor
- Emit `GameEvent`s when state changes
- May observe `GameEvent`s for secondary effects

### GameObjects:

- Compose components only
- Must not orchestrate behavior
- Must not contain gameplay logic
- Must not wire events manually

If a `GameObject` subscribes to events, it is likely a design smell.

---

## 3. Event Usage Rules (Critical)

- A component must **not observe an event that represents the result of its own state change**

  - ❌ `Health` reacting to `DamageTaken`
  - ✅ `Health` emitting `DamageTaken`

- Events must not:
  - Apply damage
  - Modify health
  - Trigger the same fact again

Events are **notifications only**.

---

## 4. Damage & Combat Flow

Correct pipeline:

```

GameAction
↓
Material / Trait Components
↓
Damage (Action)
↓
Health
↓
DamageTaken (Event)
↓
Secondary Effects (VFX, UI, Status)

```

Only `Health` may reduce HP and emit health-related events.

---

## 5. Naming Conventions (Strict)

### State Components (nouns)

- `Health`
- `Mana`
- `Inventory`

### Trait Components (adjectives)

- `Wooden`
- `Stone`
- `Metal`
- `Flesh`

### Behavior Components (verb phrases)

- `DestroyOnDeath`
- `RemoveOnHealthDepletion`
- `BleedOnHit`
- `ShowDamageOnHit`

### Presentation Components (visual/audio)

- `Sprite`
- `Animation`
- `SoundSource`

Avoid suffixes like:

- `Component`
- `System`
- `Manager`

---

## 6. Lifetime & Events

- GameObjects clear all event listeners on `Terminate()`
- Components must not manage subscriptions manually
- No long-lived references from events

---

## 7. Code Review Checklist

When reviewing or generating code, ensure:

- [ ] Actions and Events are not mixed
- [ ] Components handle actions, not GameObjects
- [ ] Events describe facts, not commands
- [ ] No event causes the same state change again
- [ ] Naming follows the defined grammar
- [ ] Composition is preferred over inheritance

If unsure, ask:

> “Is this describing intent or a fact?”

---

## 8. Design Philosophy

- Favor clarity over abstraction
- Prefer composition over inheritance
- Avoid central “god systems”
- Behavior emerges from component interaction
- Code should read like a description of the entity

Example:

```csharp
Add(new Health(30));
Add(new Wooden());
Add(new RemoveOnDeath());
Add(new Sprite(treeTexture));
```

---

**If a change violates these rules, it should be refactored.**
