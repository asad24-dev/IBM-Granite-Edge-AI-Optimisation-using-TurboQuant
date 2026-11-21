# APP-8GB-HM-Q2-006 code evidence

This note records the source-level connection between the experimental 2K
profile and the two relevant application paths. It does not replace the
physical-device evidence in the matching raw-results folder.

| Concern | Source evidence |
| --- | --- |
| Model-specific context choice | `shared/GraniteEdgeAI.ModelHardwareCompatibility.Core/Application/Presentation/CompatibilityEngine.cs` defines the exact Q2_K model SHA-256 and returns `2048` for that hash; other models retain the default `4096`. |
| Compatibility estimate | The same `GgufContextCeilingForModel(...)` result is placed into the compatibility request in `CompatibilityEngine.cs`, so the estimator receives the model-specific context ceiling. |
| Runtime admission and launch composition | `IBM Granite with TurboQuant (Intel)/Features/ModelHardwareCompatibility/Infrastructure/GgufOptimizationProductionAuthority.cs` calls the same context-bound function, uses it in runtime admission, and passes the evaluated context to the current launch configuration. |
| Safety policy | The experimental path retains runtime admission; it does not bypass the app's memory gate. |

The supplied screenshots independently show that the setup screen displayed
2,048 tokens. They do not independently prove the runtime context value; that
link is established here by source review.
