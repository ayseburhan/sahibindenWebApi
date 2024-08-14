using System;
using System.Collections.Generic;

namespace sahibindenWebApi.Models;

public partial class Kullanici
{
    public int Id { get; set; }

    public int KullaniciId { get; set; }
    public string KullaniciMail { get; set; } = null!;

    public string KullaniciSifre { get; set; } = null!;

    public string KullaniciAdi { get; set; } = null!;

    public string KullaniciSoyadi { get; set; } = null!;

    public string KullaniciAdres { get; set; } = null!;

    public int KullaniciYetki { get; set; }
    public string RefreshToken { get; internal set; }
    public DateTime RefreshTokenExpiryTime { get; internal set; }
}
