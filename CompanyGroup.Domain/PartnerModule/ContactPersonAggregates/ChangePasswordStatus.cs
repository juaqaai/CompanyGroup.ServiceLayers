namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// passzív, ha jelszómódosítást követően az adminisztrátor kezdeményezte a jelszómódosítás megszüntetését
    /// aktív jelszómódosítást követően
    /// </summary>
    public enum ChangePasswordStatus { Passive = 0, Active = 1 }
}