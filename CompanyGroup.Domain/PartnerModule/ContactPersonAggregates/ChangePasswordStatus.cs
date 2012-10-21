namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// passzív, ha jelszómódosítást követően az adminisztrátor kezdeményezte a jelszómódosítás megszüntetését,
    /// aktív jelszómódosítást követően, 
    /// Failed, ha nem sikerült a jelszómódosítás
    /// </summary>
    public enum ChangePasswordStatus { Passive = 0, Active = 1, Failed = 2 }
}