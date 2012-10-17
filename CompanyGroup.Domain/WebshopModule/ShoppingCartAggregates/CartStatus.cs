/// <summary>
/// kosár státusz (törölt, új létrehozás, tárolt, feladva rendelésre, feladva finance ajánlatra, )
/// </summary>
public enum CartStatus { Deleted = 0, Created = 1, Stored = 2, Posted = 3, FinanceOfferPosted = 4, WaitingForAutoPost = 5, WaitingForOfferAutoPost = 6 }