using System;
using System.Collections.Generic;

namespace sahibindenWebApi.Models;

public partial class Admin
{
    public int Id { get; set; }

    public int? AdminId { get; set; }

    public string AdminMail { get; set; } = null!;

    public string AdminSifre { get; set; } = null!;

    public string? AdminAdi { get; set; }

    public string? AdminSoyadi { get; set; }

    public int AdminAdres { get; set; }
}
