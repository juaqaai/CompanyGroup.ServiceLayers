
/// <summary>
/// kiadas allapota:     0 none,    1 sold,   2 deducted (eladva),   3 picked (kivéve),  4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)
/// </summary>
public enum StatusIssue { None = 0, Sold = 1, Deducted = 2, Picked = 3, ReservPhysical = 4, ReservOrdered = 5, OnOrder = 6, Quotation = 7 }