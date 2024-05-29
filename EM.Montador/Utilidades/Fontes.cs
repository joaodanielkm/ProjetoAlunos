using iTextSharp5.text;

namespace EM.Montador.Utilidades;

public static class Fontes
{
    public static readonly Font FonteHelvetica16 = FontFactory.GetFont(FontFactory.HELVETICA, 16);
    public static readonly Font FonteHelvetica14 = FontFactory.GetFont(FontFactory.HELVETICA, 14);

    public static readonly Font FonteHelvetica18Negrito = FontFactory.GetFont(FontFactory.HELVETICA, 18, Font.BOLD);
    public static readonly Font FonteHelvetica17Negrito = FontFactory.GetFont(FontFactory.HELVETICA, 17, Font.BOLD);

    public static readonly Font FonteHelvetica14Italico = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.ITALIC);
}
