using System;
using System.Collections.Generic;

namespace sahibindenWebApi.Models;

public partial class Favori
{
    public int Id { get; set; }

    public int KullaniciId { get; set; }

    public int UrunId { get; set; }
}
