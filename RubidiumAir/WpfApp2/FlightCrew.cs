//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    
    public partial class FlightCrew
    {
        public int FlightID { get; set; }
        public int EmployeeID { get; set; }
        public string Role { get; set; }
    
        public virtual Employees Employees { get; set; }
        public virtual Flights Flights { get; set; }
    }
}
