# manager.ioc
Simple singleton manager that handles managers with 'Inversion of Control'

# SetterUpdaterEditor
This is a Editor targeting Monobehaviour and executing all setters.
I included this because my Inspector properties all have getters and setters that are used in code.
So I do updates in the setter method like for example if you change the max range of a unit the visuals get updated in real time because it is setter driven.
But changing those values in the unity inspector does not show the "real" change. With this Editor all setters of a script get triggered if the GUI updates.

NOTE: this does not change anything at the default inspector drawing but could conflict with another Editor targeting MonoBehaviours directly!

## Naming Convention for setters and getters
If you want to use this feature as well just stick to this naming convention:
```
property: lowercaseCharAtStart
setter: LowercaseCharAtStart // UppercaseCharacter at start but still the same name as the Property
```