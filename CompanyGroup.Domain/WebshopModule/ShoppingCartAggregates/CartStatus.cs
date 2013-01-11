/// <summary>
/// kosár státusz (törölt, új létrehozás, tárolt, feladva rendelésre, vár a feladásra, )
/// </summary>
public enum CartStatus { Deleted = 0, Created = 1, Stored = 2, Posted = 3, WaitingForAutoPost = 4 }