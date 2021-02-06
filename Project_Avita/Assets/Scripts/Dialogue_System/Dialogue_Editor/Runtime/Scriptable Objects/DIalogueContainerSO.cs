using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
[System.Serializable]
public class DIalogueContainerSO : ScriptableObject
{
    
}

public class LanguageGeneric<T>
{
    public LanguageType LanguageType;
    public T LanguageGenericType;
}
