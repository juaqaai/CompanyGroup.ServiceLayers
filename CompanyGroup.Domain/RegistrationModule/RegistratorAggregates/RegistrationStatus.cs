using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    
    /// <summary>
    /// kosár státusz (törölt, új létrehozás, feladva, feladásra vár, )
    /// </summary>
    public enum RegistrationStatus { Deleted = 0, Created = 1, Posted = 2, WaitingForAutoPost = 3 }
}
