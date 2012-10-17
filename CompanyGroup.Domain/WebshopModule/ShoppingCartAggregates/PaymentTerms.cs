/// <summary>
/// fizetési opciók (nincs megadva, kézpénzes fizetés, átutalás, előre utalás, fizetés átvételkor)
/// </summary>
public enum PaymentTerms { None = 0, Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4 }